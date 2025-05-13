using System.Data;
using System.Text;
using Microsoft.AspNetCore.Mvc;

public class BookController : ControllerBase
{

    public async static Task GetAllBooks(HttpContext httpContext)
    {
        var response = new StringBuilder();
        var books = BookServices.GetAllBooks();
        var booksArray = new Book[books.Rows.Count];
        for (int i = 0; i < books.Rows.Count; i++)
        {
            var row = books.Rows[i];
            var book = new Book
            {
                Id = Convert.ToInt32(row["id"]),
                Title = row["title"].ToString(),
                Description = row["description"].ToString(),
                Author = row["author"].ToString(),
            };
            booksArray[i] = book;
            response.AppendLine($"{book.Id} - {book.Title} / Autheur : {book.Author}");
        }
        httpContext.Response.ContentType = "text";
        await httpContext.Response.WriteAsync(response.ToString());
    }
    public async static Task GetBookById(int id, HttpContext httpContext)
    {
        var currBook = BookServices.GetBookById(id);
        var response = new StringBuilder();
        foreach (DataRow row in currBook.Rows)
        {
            var book = new Book
            {
                Id = Convert.ToInt32(row["id"]),
                Title = row["title"].ToString(),
                Description = row["description"].ToString(),
                Author = row["author"].ToString(),
                Quantity = row["quantity"].ToString(),
            };
            response.AppendLine($"{book.Title} / Autheur : {book.Author}");
            response.AppendLine($"Quantité: {book.Quantity}");
        }
        httpContext.Response.ContentType = "text";
        await httpContext.Response.WriteAsync(response.ToString());
    }
    public async static Task GetAllBooksOnStock(HttpContext httpContext)
    {
        var response = new StringBuilder();

        var books = BookServices.GetAllBooksOnStock();
        foreach (DataRow row in books.Rows)
        {
            var book = new Book
            {
                Id = Convert.ToInt32(row["id"]),
                Title = row["title"].ToString(),
                Author = row["author"].ToString(),
                Quantity = row["quantity"].ToString()
            };
            response.AppendLine($"{book.Id} - {book.Title} / autheur: {book.Author} (Quantité: {book.Quantity})");
        }
        httpContext.Response.ContentType = "plain/text";
        await httpContext.Response.WriteAsync(response.ToString());
    }
    public async static Task CreatedBook(HttpContext httpContext)
    {
        {
            var book = await httpContext.Request.ReadFromJsonAsync<Book>();
            if (book != null)
            {
                BookServices.CreateBook(book.Title, book.Description, book.Author);
                httpContext.Response.StatusCode = 201; // Created
                await httpContext.Response.WriteAsync("✅ Book created successfully");
            }
        }
    }
    public async static Task DeleteBook(int id, HttpContext httpContext)
    {
        BookServices.DeleteBook(id);
        httpContext.Response.StatusCode = 200; // OK
        await httpContext.Response.WriteAsync($"Book with ID {id} deleted successfully");
    }
    public async static Task AddBook(int id, HttpContext httpContext)
    {
        var book = await httpContext.Request.ReadFromJsonAsync<Book>();
        if (book != null) BookServices.AddBook(id, Convert.ToInt32(book.Quantity));
        httpContext.Response.ContentType = "plain/text";
        await httpContext.Response.WriteAsync("✅ Quantity update with succes !");
    }
}