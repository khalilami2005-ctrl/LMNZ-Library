using Spectre.Console;
using LibraryCLI.Services;
using LibraryCLI.Models;

namespace LibraryCLI;

class Program
{
    private static ApiClient _api = new ApiClient("http://localhost:5056");

    static async Task Main(string[] args)
    {
        Console.Title = "Library CLI - SYSADMIN TERMINAL";

        while (true)
        {
            Console.Clear();
            DrawHeader();
            
            // Empty space before menu
            AnsiConsole.WriteLine();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[blue]>> SELECTIONNEZ UNE OPERATION :[/]")
                    .HighlightStyle("black on deepskyblue1")
                    .PageSize(10)
                    .AddChoices(new[] {
                        "SYS.BOOKS", 
                        "SYS.USERS", 
                        "SYS.LOANS", 
                        "EXIT" 
                    }));

            switch (choice)
            {
                case "SYS.BOOKS":
                    await ManageBooks();
                    break;
                case "SYS.USERS":
                    await ManageUsers();
                    break;
                case "SYS.LOANS":
                    await ManageLoans();
                    break;
                case "EXIT":
                    AnsiConsole.WriteLine();
                    AnsiConsole.MarkupLine("[bold red]>> SESSION TERMINATED.[/]");
                    return;
            }
        }
    }

    static void DrawHeader()
    {
        string ascii = """
██╗     ███╗   ███╗███╗   ██╗███████╗
██║     ████╗ ████║████╗  ██║╚══███╔╝
██║     ██╔████╔██║██╔██╗ ██║  ███╔╝ 
██║     ██║╚██╔╝██║██║╚██╗██║ ███╔╝  
███████╗██║ ╚═╝ ██║██║ ╚████║███████╗
╚══════╝╚═╝     ╚═╝╚═╝  ╚═══╝╚══════╝

██╗     ██╗██████╗ ██████╗  █████╗ ██████╗ ██╗   ██╗
██║     ██║██╔══██╗██╔══██╗██╔══██╗██╔══██╗╚██╗ ██╔╝
██║     ██║██████╔╝██████╔╝███████║██████╔╝ ╚████╔╝ 
██║     ██║██╔══██╗██╔══██╗██╔══██║██╔══██╗  ╚██╔╝  
███████╗██║██████╔╝██║  ██║██║  ██║██║  ██║   ██║   made by Lmnz
╚══════╝╚═╝╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═╝   ╚═╝
""";
        // Space before ASCII art
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[bold deepskyblue1]{Markup.Escape(ascii)}[/]");
        
        // Space after ASCII art
        AnsiConsole.WriteLine(); 
        AnsiConsole.MarkupLine("[bold grey]GitHub:[/] [deepskyblue1]khalilami2005-ctrl[/]");
        
        AnsiConsole.WriteLine(); 
        AnsiConsole.Write(new Rule("[blue]SECURE ADMINISTRATION TERMINAL[/]").RuleStyle("deepskyblue1").LeftJustified());
        AnsiConsole.WriteLine();
    }

    // --- BOOKS ---
    static async Task ManageBooks()
    {
        while (true)
        {
            Console.Clear();
            DrawHeader();
            AnsiConsole.MarkupLine("[bold underline deepskyblue1]>> MODULE : BOOKS[/]");
            AnsiConsole.WriteLine(); // Space added

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .HighlightStyle("black on deepskyblue1")
                    .AddChoices("LIST_BOOKS", "ADD_BOOK", "UPDATE_BOOK", "DELETE_BOOK", "RETURN_TO_MAIN"));

            switch (choice)
            {
                case "LIST_BOOKS":
                    await ListBooks();
                    break;
                case "ADD_BOOK":
                    await AddBook();
                    break;
                case "UPDATE_BOOK":
                    await UpdateBook();
                    break;
                case "DELETE_BOOK":
                    await DeleteBook();
                    break;
                case "RETURN_TO_MAIN":
                    return;
            }
        }
    }

    static async Task ListBooks()
    {
        var books = await _api.GetBooksAsync();
        AnsiConsole.WriteLine();

        if (books == null || !books.Any())
        {
            AnsiConsole.MarkupLine("[grey]>> NO RECORDS FOUND OR API UNREACHABLE.[/]");
            PromptAnyKey();
            return;
        }

        var table = new Table().BorderColor(Color.DeepSkyBlue1);
        table.AddColumn("[blue]ID[/]").AddColumn("[blue]TITLE[/]").AddColumn("[blue]AUTHOR[/]").AddColumn("[blue]STATUS[/]");

        foreach (var b in books)
        {
            string status = b.IsAvailable ? "[blue]AVAILABLE[/]" : "[red]BORROWED[/]";
            table.AddRow(b.Id.ToString(), b.Title, b.Author, status);
        }

        AnsiConsole.Write(table);
        PromptAnyKey();
    }

    static async Task AddBook()
    {
        AnsiConsole.WriteLine();
        var title = AnsiConsole.Ask<string>("[blue]>> INPUT TITLE:[/]");
        var author = AnsiConsole.Ask<string>("[blue]>> INPUT AUTHOR:[/]");

        AnsiConsole.WriteLine();
        var created = await _api.CreateBookAsync(new CreateBookDto { Title = title, Author = author });
        if (created != null)
            AnsiConsole.MarkupLine("[bold deepskyblue1]>> RECORD ADDED SUCCESSFULLY.[/]");
        else
            AnsiConsole.MarkupLine("[bold red]>> ERROR: FAILED TO ADD RECORD.[/]");
        
        PromptAnyKey();
    }

    static async Task UpdateBook()
    {
        AnsiConsole.WriteLine();
        var id = AnsiConsole.Ask<int>("[blue]>> INPUT TARGET ID:[/]");
        var title = AnsiConsole.Ask<string>("[blue]>> INPUT NEW TITLE:[/]");
        var author = AnsiConsole.Ask<string>("[blue]>> INPUT NEW AUTHOR:[/]");

        AnsiConsole.WriteLine();
        var updated = await _api.UpdateBookAsync(id, new UpdateBookDto { Title = title, Author = author });
        if (updated != null)
            AnsiConsole.MarkupLine("[bold deepskyblue1]>> RECORD UPDATED SUCCESSFULLY.[/]");
        else
            AnsiConsole.MarkupLine("[bold red]>> ERROR: OVERWRITE FAILED (NOT FOUND).[/]");
        
        PromptAnyKey();
    }

    static async Task DeleteBook()
    {
        AnsiConsole.WriteLine();
        var id = AnsiConsole.Ask<int>("[blue]>> INPUT TARGET ID TO PURGE:[/]");
        var confirm = AnsiConsole.Confirm("[red]>> CONFIRM PURGE ACTION?[/]");
        if (!confirm) return;

        AnsiConsole.WriteLine();
        var success = await _api.DeleteBookAsync(id);
        if (success)
            AnsiConsole.MarkupLine("[bold deepskyblue1]>> RECORD PURGED SUCCESSFULLY.[/]");
        else
            AnsiConsole.MarkupLine("[bold red]>> ERROR: PURGE FAILED.[/]");
        PromptAnyKey();
    }


    // --- USERS ---
    static async Task ManageUsers()
    {
        while (true)
        {
            Console.Clear();
            DrawHeader();
            AnsiConsole.MarkupLine("[bold underline deepskyblue1]>> MODULE : USERS[/]");
            AnsiConsole.WriteLine();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .HighlightStyle("black on deepskyblue1")
                    .AddChoices("LIST_USERS", "ADD_USER", "UPDATE_USER", "PURGE_USER", "RETURN_TO_MAIN"));

            switch (choice)
            {
                case "LIST_USERS":
                    await ListUsers();
                    break;
                case "ADD_USER":
                    await AddUser();
                    break;
                case "UPDATE_USER":
                    await UpdateUser();
                    break;
                case "PURGE_USER":
                    await DeleteUser();
                    break;
                case "RETURN_TO_MAIN":
                    return;
            }
        }
    }

    static async Task ListUsers()
    {
        AnsiConsole.WriteLine();
        var users = await _api.GetUsersAsync();
        if (users == null || !users.Any())
        {
            AnsiConsole.MarkupLine("[grey]>> NO RECORDS FOUND.[/]");
            PromptAnyKey();
            return;
        }

        var table = new Table().BorderColor(Color.DeepSkyBlue1);
        table.AddColumn("[blue]ID[/]").AddColumn("[blue]NAME[/]").AddColumn("[blue]EMAIL[/]");

        foreach (var u in users)
        {
            table.AddRow(u.Id.ToString(), u.Name, u.Email);
        }

        AnsiConsole.Write(table);
        PromptAnyKey();
    }

    static async Task AddUser()
    {
        AnsiConsole.WriteLine();
        var name = AnsiConsole.Ask<string>("[blue]>> INPUT NAME:[/]");
        var email = AnsiConsole.Ask<string>("[blue]>> INPUT EMAIL:[/]");

        AnsiConsole.WriteLine();
        var created = await _api.CreateUserAsync(new CreateUserDto { Name = name, Email = email });
        if (created != null)
            AnsiConsole.MarkupLine("[bold deepskyblue1]>> USER ADDED.[/]");
        else
            AnsiConsole.MarkupLine("[bold red]>> ERROR: ADDITION FAILED.[/]");
        
        PromptAnyKey();
    }

    static async Task UpdateUser()
    {
        AnsiConsole.WriteLine();
        var id = AnsiConsole.Ask<int>("[blue]>> INPUT TARGET ID:[/]");
        var name = AnsiConsole.Ask<string>("[blue]>> INPUT NEW NAME:[/]");
        var email = AnsiConsole.Ask<string>("[blue]>> INPUT NEW EMAIL:[/]");

        AnsiConsole.WriteLine();
        var updated = await _api.UpdateUserAsync(id, new UpdateUserDto { Name = name, Email = email });
        if (updated != null)
            AnsiConsole.MarkupLine("[bold deepskyblue1]>> USER DATA UPDATED.[/]");
        else
            AnsiConsole.MarkupLine("[bold red]>> ERROR: UPDATE FAILED.[/]");
        
        PromptAnyKey();
    }

    static async Task DeleteUser()
    {
        AnsiConsole.WriteLine();
        var id = AnsiConsole.Ask<int>("[blue]>> INPUT TARGET ID TO PURGE:[/]");
        if (!AnsiConsole.Confirm("[red]>> CONFIRM USER PURGE?[/]")) return;

        AnsiConsole.WriteLine();
        var success = await _api.DeleteUserAsync(id);
        if (success)
            AnsiConsole.MarkupLine("[bold deepskyblue1]>> USER RECORD PURGED.[/]");
        else
            AnsiConsole.MarkupLine("[bold red]>> ERROR: PURGE FAILED.[/]");
        PromptAnyKey();
    }

    // --- LOANS ---
    static async Task ManageLoans()
    {
        while (true)
        {
            Console.Clear();
            DrawHeader();
            AnsiConsole.MarkupLine("[bold underline deepskyblue1]>> MODULE : LOANS[/]");
            AnsiConsole.WriteLine();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .HighlightStyle("black on deepskyblue1")
                    .AddChoices("BORROW_BOOK", "RETURN_BOOK", "VIEW_ACTIVE_LOANS", "RETURN_TO_MAIN"));

            switch (choice)
            {
                case "BORROW_BOOK":
                    await BorrowBook();
                    break;
                case "RETURN_BOOK":
                    await ReturnBook();
                    break;
                case "VIEW_ACTIVE_LOANS":
                    await ListActiveLoans();
                    break;
                case "RETURN_TO_MAIN":
                    return;
            }
        }
    }

    static async Task ListActiveLoans()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[grey]>> FETCHING ACTIVE LOANS DATABANK...[/]");
        var loans = await _api.GetActiveLoansAsync();

        if (loans == null || !loans.Any())
        {
            AnsiConsole.MarkupLine("[grey]>> NO ACTIVE LOANS REGISTERED.[/]");
        }
        else
        {
            var table = new Table().BorderColor(Color.DeepSkyBlue1);
            table.AddColumn("[blue]LOAN_ID[/]").AddColumn("[blue]BOOK[/]").AddColumn("[blue]USER[/]").AddColumn("[blue]TIMESTAMP[/]");

            foreach (var l in loans)
            {
                table.AddRow(l.Id.ToString(), $"[deepskyblue1]{l.BookTitle}[/]", $"[blue]{l.UserName}[/]", l.BorrowDate.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            AnsiConsole.Write(table);
        }
        PromptAnyKey();
    }

    static async Task BorrowBook()
    {
        AnsiConsole.WriteLine();
        var bookId = AnsiConsole.Ask<int>("[blue]>> INPUT BOOK_ID:[/]");
        var userId = AnsiConsole.Ask<int>("[blue]>> INPUT USER_ID:[/]");

        AnsiConsole.WriteLine();
        var success = await _api.BorrowBookAsync(new BorrowBookDto { BookId = bookId, UserId = userId });
        if (success)
            AnsiConsole.MarkupLine("[bold deepskyblue1]>> LOAN TRANSACTION APPROVED.[/]");
        else
            AnsiConsole.MarkupLine("[bold red]>> ERROR: LOAN TRANSACTION DENIED.[/]");
        PromptAnyKey();
    }

    static async Task ReturnBook()
    {
        AnsiConsole.WriteLine();
        var bookId = AnsiConsole.Ask<int>("[blue]>> INPUT BOOK_ID TO RETURN:[/]");
        
        AnsiConsole.WriteLine();
        var success = await _api.ReturnBookAsync(bookId);
        
        if (success)
            AnsiConsole.MarkupLine("[bold deepskyblue1]>> RETURN TRANSACTION APPROVED.[/]");
        else
            AnsiConsole.MarkupLine("[bold red]>> ERROR: RETURN TRANSACTION DENIED (INVALID ID OR NO ACTIVE LOAN).[/]");
        PromptAnyKey();
    }

    static void PromptAnyKey()
    {
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[grey]>> PRESS ANY KEY TO CONTINUE...[/]");
        Console.ReadKey(true);
    }
}
