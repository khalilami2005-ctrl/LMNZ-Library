using LibraryAPI.DTOs;

namespace LibraryAPI.Services.Interfaces;

public interface ILoanService
{
    Task<List<LoanResponseDto>> GetAllAsync();
    Task<List<LoanResponseDto>> GetActiveAsync();
    Task<(bool Success, string Message, LoanResponseDto? Loan)> BorrowBookAsync(BorrowBookDto dto);
    Task<(bool Success, string Message, LoanResponseDto? Loan)> ReturnBookAsync(int bookId);
}
