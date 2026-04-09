using Microsoft.EntityFrameworkCore;
using LibraryAPI.Data;
using LibraryAPI.DTOs;
using LibraryAPI.Models;
using LibraryAPI.Services.Interfaces;

namespace LibraryAPI.Services;

public class BookService : IBookService
{
    private readonly AppDbContext _context;

    public BookService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<BookResponseDto>> GetAllAsync()
    {
        return await _context.Books
            .Select(b => new BookResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                IsAvailable = b.IsAvailable
            })
            .ToListAsync();
    }

    public async Task<BookResponseDto?> GetByIdAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return null;

        return new BookResponseDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            IsAvailable = book.IsAvailable
        };
    }

    public async Task<BookResponseDto> CreateAsync(CreateBookDto dto)
    {
        var book = new Book
        {
            Title = dto.Title,
            Author = dto.Author,
            IsAvailable = true
        };

        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return new BookResponseDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            IsAvailable = book.IsAvailable
        };
    }

    public async Task<BookResponseDto?> UpdateAsync(int id, UpdateBookDto dto)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return null;

        book.Title = dto.Title;
        book.Author = dto.Author;

        await _context.SaveChangesAsync();

        return new BookResponseDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            IsAvailable = book.IsAvailable
        };
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null) return false;

        // Check if the book has active loans
        var hasActiveLoans = await _context.Loans.AnyAsync(l => l.BookId == id && l.IsActive);
        if (hasActiveLoans)
            throw new InvalidOperationException("Cannot delete a book that is currently borrowed.");

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return true;
    }
}
