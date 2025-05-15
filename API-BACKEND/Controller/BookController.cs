using System.Data;
using System.Text;
using Microsoft.AspNetCore.Mvc;

public class BookController : ControllerBase
{

    public async static Task GetAllBooks(HttpContext httpContext)
    {
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
        }
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsJsonAsync<Book[]>(booksArray);
    }
    public async static Task GetBookById(int id, HttpContext httpContext)
    {
        var currBook = BookServices.GetBookById(id);
        var book = new Book();
        foreach (DataRow row in currBook.Rows)
        {
            var newBook = new Book
            {
                Id = Convert.ToInt32(row["id"]),
                Title = row["title"].ToString(),
                Description = row["description"].ToString(),
                Author = row["author"].ToString(),
                Quantity = row["quantity"].ToString(),
            };
            book = newBook;
        }
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsJsonAsync<Book>(book);
    }
    public async static Task GetAllBooksOnStock(HttpContext httpContext)
    {
        var books = BookServices.GetAllBooksOnStock();
        var booksArray = new List<Book>();
        foreach (DataRow row in books.Rows)
        {
            var book = new Book
            {
                Id = Convert.ToInt32(row["id"]),
                Title = row["title"].ToString(),
                Author = row["author"].ToString(),
                Quantity = row["quantity"].ToString()
            };
            booksArray.Add(book);
        }
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsJsonAsync<List<Book>>(booksArray);
    }
    public async static Task CreatedBook(HttpContext httpContext)
    {
        var book = await httpContext.Request.ReadFromJsonAsync<Book>();
        if (book != null && book.Title != null && book.Description != null && book.Author != null)
        {
            BookServices.CreateBook(book.Title, book.Description, book.Author);
            httpContext.Response.StatusCode = 201; // Created
            // await httpContext.Response.WriteAsync("✅ Book created successfully");
        }

        /* Méthode avec données reçu depuis un formulaire :
        var form = await httpContext.Request.ReadFormAsync();

        var title = form["title"];
        var description = form["description"];
        var author = form["author"];

        if (form != null)
        {
            BookServices.CreateBook(title, description, author);
            httpContext.Response.StatusCode = 201; // Created
            httpContext.Response.Redirect("http://localhost:5500/bookList.html");
        }
        */
    }
    public async static Task DeleteBook(int id, HttpContext httpContext)
    {
        BookServices.DeleteBook(id);
        httpContext.Response.StatusCode = 200; // OK
        await httpContext.Response.WriteAsync($"Book with ID {id} deleted successfully");
    }
    public async static Task AddBook(int id, HttpContext httpContext)
    {
        BookServices.AddBook(id);
        httpContext.Response.StatusCode = 200; // OK
        // var book = await httpContext.Request.ReadFromJsonAsync<Book>();
        // if (book != null) BookServices.AddBook(id);
        // httpContext.Response.ContentType = "application/json";
        // await httpContext.Response.WriteAsJsonAsync<Book>(book);
    }
}