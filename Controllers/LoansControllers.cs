using Microsoft.AspNetCore.Mvc;
using LibraryAPI.DTOs;
using LibraryAPI.Services.Interfaces;

namespace LibraryAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoansController : ControllerBase
{
    private readonly ILoanService _loanService;

    public LoansController(ILoanService loanService)
    {
        _loanService = loanService;
    }

    // GET: api/loans
    [HttpGet]
    public async Task<ActionResult<List<LoanResponseDto>>> GetAll()
    {
        var loans = await _loanService.GetAllAsync();
        return Ok(loans);
    }

    // GET: api/loans/active
    [HttpGet("active")]
    public async Task<ActionResult<List<LoanResponseDto>>> GetActive()
    {
        var loans = await _loanService.GetActiveAsync();
        return Ok(loans);
    }

    // POST: api/loans/borrow
    [HttpPost("borrow")]
    public async Task<IActionResult> BorrowBook(BorrowBookDto dto)
    {
        var (success, message, loan) = await _loanService.BorrowBookAsync(dto);

        if (!success)
            return BadRequest(new { message });

        return Ok(new { message, loan });
    }

    // POST: api/loans/return/5
    [HttpPost("return/{bookId}")]
    public async Task<IActionResult> ReturnBook(int bookId)
    {
        var (success, message, loan) = await _loanService.ReturnBookAsync(bookId);

        if (!success)
            return NotFound(new { message });

        return Ok(new { message, loan });
    }
}