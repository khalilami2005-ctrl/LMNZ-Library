using System.ComponentModel.DataAnnotations;

namespace LibraryAPI.Models;

public class Book
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Author is required.")]
    [MaxLength(100, ErrorMessage = "Author name cannot exceed 100 characters.")]
    public string Author { get; set; } = string.Empty;

    public bool IsAvailable { get; set; } = true;
}