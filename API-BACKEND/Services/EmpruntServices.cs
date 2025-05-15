using System.Data;
using Microsoft.Data.Sqlite;

public class EmpruntServices
{
    public static DataTable GetAllEmprunts()
    {
        var emprunts = GetDataTable(@"
            SELECT emprunts.id, emprunts.userid, emprunts.bookid, emprunts.created_at, users.firstname as userfirstname, books.title as booktitle
            FROM emprunts 
            INNER JOIN users ON emprunts.userid = users.id 
            INNER JOIN books ON emprunts.bookid = books.id 
            ORDER BY firstname ASC");
        return emprunts;
    }
    public static DataTable GetEmpruntById(int id)
    {
        var emprunt = GetDataTable(@"
            SELECT emprunts.id, emprunts.userid, emprunts.bookid, emprunts.created_at, users.firstname as userfirstname, books.title as booktitle
            FROM emprunts 
            INNER JOIN users ON emprunts.userid = users.id 
            INNER JOIN books ON emprunts.bookid = books.id 
            WHERE emprunts.id = @id", new Dictionary<string, object> { ["@id"] = id });
        return emprunt;
    }
    public static DataTable GetAllEmpruntsByUserId(int userId)
    {
        var emprunt = GetDataTable(@"
            SELECT emprunts.id, emprunts.created_at,  users.id as userid, books.id as bookid
            FROM emprunts
            INNER JOIN users ON emprunts.userid = users.id
            INNER JOIN books ON emprunts.bookid = books.id
            WHERE users.id = @id", new Dictionary<string, object> { ["@id"] = userId });
        return emprunt;
    }

    public static void CreateEmprunt(int userId, int bookId)
    {
        try
        {
            ExecuteNonQuery(
                "INSERT INTO emprunts (userid, bookid, created_at) VALUES (@userid, @bookid, @created_at);" +
                "UPDATE books SET quantity = quantity - 1 WHERE id = @id",
                new Dictionary<string, object>
                {
                    ["@id"] = bookId,
                    ["@userid"] = userId,
                    ["@bookid"] = bookId,
                    ["@created_at"] = DateTime.Now
                });
            System.Console.WriteLine();
            System.Console.WriteLine("✅ Emprunt crée avec succès !");
        }
        catch (System.Exception err)
        {
            System.Console.WriteLine(err.Message);
            throw;
        }

    }
    public static void DeleteEmprunt(int id)
    {
        try
        {
            var table = GetDataTable("SELECT bookId FROM emprunts WHERE id = @id", new Dictionary<string, object> { ["@id"] = id });
            var bookId = table.Rows[0]["bookId"];
            // if (table.Rows.Count == 0) throw new Exception("il n'ya d'emprunt avec l'id");
            // var row = table.Rows[0];
            // var bookId = row != null ? (int)row["bookId"] : -1;
            // if (bookId == -1) throw new Exception("");
            ExecuteNonQuery("UPDATE books SET quantity = quantity + 1 WHERE id = @id", new Dictionary<string, object> { ["@id"] = bookId });
            ExecuteNonQuery("DELETE FROM emprunts WHERE id = @id", new Dictionary<string, object> { ["@id"] = id });
            System.Console.WriteLine();
            System.Console.WriteLine("✅ Le livre à été rendu par l'utilisateur");

        }
        catch (System.Exception err)
        {
            System.Console.WriteLine(err.Message);
            throw;
        }
    }

    private static void ExecuteNonQuery(string sql, Dictionary<string, object> parameters)
    {
        string dbfile = @"DataSource=C:\Users\ClémentTHOREZ\Documents\C#\Projet-C#\Sqlite\SQLITE-Gestionnaire-bibliothèque\db\gestionnaire-bibliotheque.db";
        using (var db = new SqliteConnection(dbfile))
        {
            db.Open();
            using (var commande = db.CreateCommand())
            {
                commande.CommandText = sql;
                foreach (var param in parameters)
                {
                    var value = param.Value ?? DBNull.Value;
                    commande.Parameters.AddWithValue(param.Key, value);
                }
                commande.ExecuteNonQuery();
            }
        }
    }
    private static DataTable GetDataTable(string sql, Dictionary<string, object>? parameters = null)
    {
        string dbfile = @"DataSource=C:\Users\ClémentTHOREZ\Documents\C#\Projet-C#\Sqlite\SQLITE-Gestionnaire-bibliothèque\db\gestionnaire-bibliotheque.db";
        using (var db = new SqliteConnection(dbfile))
        {
            db.Open();
            using (var commande = db.CreateCommand())
            {
                commande.CommandText = sql;
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        var value = param.Value ?? DBNull.Value;
                        commande.Parameters.AddWithValue(param.Key, value);
                    }
                }

                using (var reader = commande.ExecuteReader())
                {
                    var table = new DataTable();
                    table.Load(reader);
                    return table;
                }
            }
        }
    }

    /* Méthode qui retourne une valeur (GetValue) --> TODO A faire 
    private static object GetValue(string sql, string col, Dictionary<string, object>? parameters = null) // TODO Finir la méthode et l'appliquer
    {
        string dbfile = @"DataSource=C:\Users\ClémentTHOREZ\Documents\C#\Projet-C#\SQLITE-Gestionnaire-bibliothèque\db\gestionnaire-bibliotheque.db";
        using (var db = new SqliteConnection(dbfile))
        {
            db.Open();
            using (var commande = db.CreateCommand())
            {
                commande.CommandText = sql;
                if (parameters != null)
                {
                    foreach (var param in parameters) commande.Parameters.AddWithValue(param.Key, param.Value);
                }
                using (var reader = commande.ExecuteReader())
                {
                    var table = new DataTable();
                    table.Load(reader);
                    var value = table.Rows[0][$"{col}"];
                    System.Console.WriteLine("value :" + value);
                    return value;
                }

            }
        }
    }*/
}