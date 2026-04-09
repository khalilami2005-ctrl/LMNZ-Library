using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.DTOs;

public class BorrowBookDto
{
    [Required(ErrorMessage = "Book ID is required.")]
    public int BookId { get; set; }

    [Required(ErrorMessage = "User ID is required.")]
    public int UserId { get; set; }
}

public class LoanResponseDto
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public DateTime BorrowDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public bool IsActive { get; set; }
}
