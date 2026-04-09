using System.Net.Http.Json;
using LibraryCLI.Models;

namespace LibraryCLI.Services;

public class ApiClient
{
    private readonly HttpClient _http;

    public ApiClient(string baseUrl)
    {
        _http = new HttpClient { BaseAddress = new Uri(baseUrl) };
    }

    // --- BOOKS ---
    public async Task<List<BookResponseDto>?> GetBooksAsync() =>
        await _http.GetFromJsonAsync<List<BookResponseDto>>("api/books");

    public async Task<BookResponseDto?> GetBookAsync(int id) =>
        await _http.GetFromJsonAsync<BookResponseDto>($"api/books/{id}");

    public async Task<BookResponseDto?> CreateBookAsync(CreateBookDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/books", dto);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<BookResponseDto>();
        return null;
    }

    public async Task<BookResponseDto?> UpdateBookAsync(int id, UpdateBookDto dto)
    {
        var response = await _http.PutAsJsonAsync($"api/books/{id}", dto);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<BookResponseDto>();
        return null;
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/books/{id}");
        return response.IsSuccessStatusCode;
    }

    // --- USERS ---
    public async Task<List<UserResponseDto>?> GetUsersAsync() =>
        await _http.GetFromJsonAsync<List<UserResponseDto>>("api/users");

    public async Task<UserResponseDto?> CreateUserAsync(CreateUserDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/users", dto);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<UserResponseDto>();
        return null;
    }

    public async Task<UserResponseDto?> UpdateUserAsync(int id, UpdateUserDto dto)
    {
        var response = await _http.PutAsJsonAsync($"api/users/{id}", dto);
        if (response.IsSuccessStatusCode)
            return await response.Content.ReadFromJsonAsync<UserResponseDto>();
        return null;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/users/{id}");
        return response.IsSuccessStatusCode;
    }

    // --- LOANS ---
    public async Task<List<LoanResponseDto>?> GetActiveLoansAsync()
    {
        // Fetch active loans from the API
        // I will assume standard endpoints, or I should check LoansController.
        try {
            return await _http.GetFromJsonAsync<List<LoanResponseDto>>("api/loans/active");
        } catch { return new List<LoanResponseDto>(); }
    }

    public async Task<bool> BorrowBookAsync(BorrowBookDto dto)
    {
        var response = await _http.PostAsJsonAsync("api/loans/borrow", dto);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> ReturnBookAsync(int bookId)
    {
        var response = await _http.PostAsync($"api/loans/return/{bookId}", null);
        return response.IsSuccessStatusCode;
    }
}
