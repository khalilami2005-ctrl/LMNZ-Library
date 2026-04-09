using Microsoft.EntityFrameworkCore;
using LibraryAPI.Data;
using LibraryAPI.DTOs;
using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;

namespace LibraryAPI.Services;

public class LoanService : ILoanService
{
    private readonly AppDbContext _context;

    public LoanService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<LoanResponseDto>> GetAllAsync()
    {
        return await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.User)
            .Select(l => new LoanResponseDto
            {
                Id = l.Id,
                BookId = l.BookId,
                BookTitle = l.Book!.Title,
                UserId = l.UserId,
                UserName = l.User!.Name,
                BorrowDate = l.BorrowDate,
                ReturnDate = l.ReturnDate,
                IsActive = l.IsActive
            })
            .ToListAsync();
    }

    public async Task<List<LoanResponseDto>> GetActiveAsync()
    {
        return await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.User)
            .Where(l => l.IsActive)
            .Select(l => new LoanResponseDto
            {
                Id = l.Id,
                BookId = l.BookId,
                BookTitle = l.Book!.Title,
                UserId = l.UserId,
                UserName = l.User!.Name,
                BorrowDate = l.BorrowDate,
                ReturnDate = l.ReturnDate,
                IsActive = l.IsActive
            })
            .ToListAsync();
    }

    public async Task<(bool Success, string Message, LoanResponseDto? Loan)> BorrowBookAsync(BorrowBookDto dto)
    {
        // 1. Check that the book exists
        var book = await _context.Books.FindAsync(dto.BookId);
        if (book == null)
            return (false, "Book not found.", null);

        // 2. Check that the user exists
        var user = await _context.Users.FindAsync(dto.UserId);
        if (user == null)
            return (false, "User not found.", null);

        // 3. Check that the book is available
        if (!book.IsAvailable)
            return (false, "This book is already borrowed.", null);

        // 4. Double check via the Loans table
        var isAlreadyBorrowed = await _context.Loans
            .AnyAsync(l => l.BookId == dto.BookId && l.IsActive);
        if (isAlreadyBorrowed)
            return (false, "This book is already on loan.", null);

        // 5. Create the loan
        var loan = new Loan
        {
            BookId = dto.BookId,
            UserId = dto.UserId,
            BorrowDate = DateTime.Now,
            IsActive = true
        };

        // 6. Update book availability
        book.IsAvailable = false;

        _context.Loans.Add(loan);
        await _context.SaveChangesAsync();

        var response = new LoanResponseDto
        {
            Id = loan.Id,
            BookId = loan.BookId,
            BookTitle = book.Title,
            UserId = loan.UserId,
            UserName = user.Name,
            BorrowDate = loan.BorrowDate,
            ReturnDate = loan.ReturnDate,
            IsActive = loan.IsActive
        };

        return (true, "Book borrowed successfully.", response);
    }

    public async Task<(bool Success, string Message, LoanResponseDto? Loan)> ReturnBookAsync(int bookId)
    {
        // 1. Find the active loan for this book
        var loan = await _context.Loans
            .Include(l => l.Book)
            .Include(l => l.User)
            .FirstOrDefaultAsync(l => l.BookId == bookId && l.IsActive);

        if (loan == null)
            return (false, "No active loan found for this book.", null);

        // 2. Mark the loan as completed
        loan.ReturnDate = DateTime.Now;
        loan.IsActive = false;

        // 3. Set the book as available again
        if (loan.Book != null)
            loan.Book.IsAvailable = true;

        await _context.SaveChangesAsync();

        var response = new LoanResponseDto
        {
            Id = loan.Id,
            BookId = loan.BookId,
            BookTitle = loan.Book?.Title ?? "Unknown",
            UserId = loan.UserId,
            UserName = loan.User?.Name ?? "Unknown",
            BorrowDate = loan.BorrowDate,
            ReturnDate = loan.ReturnDate,
            IsActive = loan.IsActive
        };

        return (true, "Book returned successfully.", response);
    }
}
