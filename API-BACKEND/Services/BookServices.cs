using System.Data;
using Microsoft.Data.Sqlite;

public class BookServices
{
    public static DataTable GetAllBooks()
    {
        var books = GetDataTable("SELECT * FROM books");
        return books;
    }
    public static DataTable GetAllBooksOnStock() // TODO A FAIRE
    {
        var books = GetDataTable("SELECT * FROM books WHERE quantity > 0");
        return books;
    }
    public static DataTable GetBookById(int id)
    {
        var book = GetDataTable("SELECT * FROM books WHERE id = @id", new Dictionary<string, object> { ["@id"] = id });
        return book;
    }

    public static string CreateBook(string title, string description, string author, string picture)
    {
        try
        {
            var table = GetDataTable("INSERT INTO books (title, description, author, quantity, picture) VALUES (@title, @description, @author, 0, @picture); SELECT id FROM books ORDER BY id DESC LIMIT 1", new Dictionary<string, object> { ["@title"] = title, ["@description"] = description, ["@author"] = author, ["@picture"] = picture });
            System.Console.WriteLine("✅ Le livre à été crée avec succès");
            return $"Annonce crée avec l'ID: {table.Rows[0]["id"]}";
        }
        catch (System.Exception err)
        {
            System.Console.WriteLine(err.Message);
            throw;
        }

    }
    public static void AddBook(int id)
    {
        try
        {
            ExecuteQuery("UPDATE books SET quantity = quantity + 1 WHERE id = @id", new Dictionary<string, object> { ["@id"] = id });
            var books = GetDataTable("SELECT * FROM books WHERE id = @id", new Dictionary<string, object> { ["@id"] = id });
            foreach (DataRow book in books.Rows)
            {
                System.Console.WriteLine($"✅ Nouveau stock : {book["quantity"]} ");
            }
        }
        catch (System.Exception err)
        {
            System.Console.WriteLine(err.Message);
            throw;
        }
    }
    public static void DeleteBook(int id)
    {
        ExecuteQuery("DELETE FROM books WHERE id = @id", new Dictionary<string, object> { ["@id"] = id });
        try
        {
            System.Console.WriteLine("✅ Le livre à été supprimé avec succès");
        }
        catch (System.Exception err)
        {
            System.Console.WriteLine(err.Message);
            throw;
        }
    }


    private static void ExecuteQuery(string sql, Dictionary<string, object> parameters)
    {
        var dbfile = @"DataSource=C:\Users\ClémentTHOREZ\Documents\C#\Projet-C#\Sqlite\SQLITE-Gestionnaire-bibliothèque\db\gestionnaire-bibliotheque.db";
        using (var dbConnection = new SqliteConnection(dbfile))
        {
            dbConnection.Open();
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = sql;
                foreach (var param in parameters)
                {
                    var value = param.Value ?? DBNull.Value;
                    command.Parameters.AddWithValue(param.Key, value);
                }
                command.ExecuteNonQuery();
            }
        }
    }
    private static DataTable GetDataTable(string sql, Dictionary<string, object>? parameters = null)
    {
        var dbfile = @"DataSource=C:\Users\ClémentTHOREZ\Documents\C#\Projet-C#\Sqlite\SQLITE-Gestionnaire-bibliothèque\db\gestionnaire-bibliotheque.db";
        using (var dbConnection = new SqliteConnection(dbfile))
        {
            dbConnection.Open();
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = sql;
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        var value = param.Value ?? DBNull.Value;
                        command.Parameters.AddWithValue(param.Key, value);
                    }
                }
                using (var reader = command.ExecuteReader())
                {
                    var table = new DataTable();
                    table.Load(reader);
                    return table;
                }
            }
        }
    }
}