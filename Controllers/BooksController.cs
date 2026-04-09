using Microsoft.AspNetCore.Mvc;
using LibraryAPI.DTOs;
using LibraryAPI.Services.Interfaces;

namespace LibraryAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    // GET: api/books
    [HttpGet]
    public async Task<ActionResult<List<BookResponseDto>>> GetAll()
    {
        var books = await _bookService.GetAllAsync();
        return Ok(books);
    }

    // GET: api/books/5
    [HttpGet("{id}")]
    public async Task<ActionResult<BookResponseDto>> GetById(int id)
    {
        var book = await _bookService.GetByIdAsync(id);
        if (book == null)
            return NotFound(new { message = "Book not found." });
        return Ok(book);
    }

    // POST: api/books
    [HttpPost]
    public async Task<ActionResult<BookResponseDto>> Create(CreateBookDto dto)
    {
        var book = await _bookService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    // PUT: api/books/5
    [HttpPut("{id}")]
    public async Task<ActionResult<BookResponseDto>> Update(int id, UpdateBookDto dto)
    {
        var book = await _bookService.UpdateAsync(id, dto);
        if (book == null)
            return NotFound(new { message = "Book not found." });
        return Ok(book);
    }

    // DELETE: api/books/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _bookService.DeleteAsync(id);
            if (!result)
                return NotFound(new { message = "Book not found." });
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}