using LibraryAPI.DTOs;

namespace LibraryAPI.Services.Interfaces;

public interface IBookService
{
    Task<List<BookResponseDto>> GetAllAsync();
    Task<BookResponseDto?> GetByIdAsync(int id);
    Task<BookResponseDto> CreateAsync(CreateBookDto dto);
    Task<BookResponseDto?> UpdateAsync(int id, UpdateBookDto dto);
    Task<bool> DeleteAsync(int id);
}
