using Microsoft.EntityFrameworkCore;
using LibraryAPI.Data;
using LibraryAPI.DTOs;
using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;

namespace LibraryAPI.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserResponseDto>> GetAllAsync()
    {
        return await _context.Users
            .Select(u => new UserResponseDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email
            })
            .ToListAsync();
    }

    public async Task<UserWithLoansResponseDto?> GetByIdAsync(int id)
    {
        var user = await _context.Users
            .Include(u => u.Loans)
                .ThenInclude(l => l.Book)
            .FirstOrDefaultAsync(u => u.Id == id);

        if (user == null) return null;

        return new UserWithLoansResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Loans = user.Loans.Select(l => new LoanResponseDto
            {
                Id = l.Id,
                BookId = l.BookId,
                BookTitle = l.Book?.Title ?? "Unknown",
                UserId = l.UserId,
                UserName = user.Name,
                BorrowDate = l.BorrowDate,
                ReturnDate = l.ReturnDate,
                IsActive = l.IsActive
            }).ToList()
        };
    }

    public async Task<UserResponseDto> CreateAsync(CreateUserDto dto)
    {
        // Check email uniqueness
        var emailExists = await _context.Users.AnyAsync(u => u.Email == dto.Email);
        if (emailExists)
            throw new InvalidOperationException("A user with this email already exists.");

        var user = new User
        {
            Name = dto.Name,
            Email = dto.Email
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }

    public async Task<UserResponseDto?> UpdateAsync(int id, UpdateUserDto dto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return null;

        // Check email uniqueness (excluding current user)
        var emailExists = await _context.Users.AnyAsync(u => u.Email == dto.Email && u.Id != id);
        if (emailExists)
            throw new InvalidOperationException("Another user with this email already exists.");

        user.Name = dto.Name;
        user.Email = dto.Email;

        await _context.SaveChangesAsync();

        return new UserResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        // Check if user has active loans
        var hasActiveLoans = await _context.Loans.AnyAsync(l => l.UserId == id && l.IsActive);
        if (hasActiveLoans)
            throw new InvalidOperationException("Cannot delete a user with active loans.");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
