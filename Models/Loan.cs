namespace LibraryAPI.Models;

public class Loan
{
    public int Id { get; set; }
    
    // Foreign keys
    public int UserId { get; set; }
    public int BookId { get; set; }
    
    public DateTime BorrowDate { get; set; } = DateTime.Now;
    public DateTime? ReturnDate { get; set; } // Nullable — can be empty (null) if not yet returned
    
    public bool IsActive { get; set; } = true;

    // Navigation properties (for EF Core)
    public User? User { get; set; }
    public Book? Book { get; set; }
}