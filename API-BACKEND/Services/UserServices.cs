using Microsoft.Data.Sqlite;
using System.Data;

public class UserServices
{
    public static DataTable GetAllUsers()
    {
        var users = GetDataTable("SELECT * FROM users");
        return users;
    }

    public static DataTable GetUserById(int id)
    {
        var user = GetDataTable("SELECT * FROM users WHERE id = @id", new Dictionary<string, object> { ["@id"] = id });
        return user;
    }

    public static void CreateUser(string firstName, string lastName, string age)
    {
        ExecuteNonQuery("INSERT INTO users (firstname, lastname, age) VALUES (@firstname, @lastname, @age)", new Dictionary<string, object> { ["@firstname"] = firstName, ["@lastname"] = lastName, ["@age"] = age });
        try
        {
            System.Console.WriteLine("✅ L'utilisateur à été crée avec succès");
        }
        catch (System.Exception err)
        {
            System.Console.WriteLine(err.Message);
            throw;
        }
    }

    public static void DeleteUser(int id)
    {
        ExecuteNonQuery("DELETE FROM users WHERE id = @id", new Dictionary<string, object> { ["@id"] = id });
        try
        {
            System.Console.WriteLine("✅ L'utilisateur à été supprimé avec succès");
        }
        catch (System.Exception err)
        {
            System.Console.WriteLine(err.Message);
            throw;
        }
    }

    private static void ExecuteNonQuery(string sql, Dictionary<string, object> parameters)
    // --> Pour écrire dans la base de donnée
    {
        string dbfile = @"DataSource=C:\Users\ClémentTHOREZ\Documents\C#\Projet-C#\Sqlite\SQLITE-Gestionnaire-bibliothèque\db\gestionnaire-bibliotheque.db";
        using (var db = new SqliteConnection(dbfile))
        {
            db.Open();
            using (var command = db.CreateCommand())
            {
                command.CommandText = sql;
                foreach (var parameter in parameters)
                {
                    var value = parameter.Value ?? DBNull.Value;
                    command.Parameters.AddWithValue(parameter.Key, value);
                }
                command.ExecuteNonQuery();
            }
        }
    }

    private static DataTable GetDataTable(string sql, Dictionary<string, object>? parameters = null)
    // --> Pour Lire dans la base de donnée
    {
        string dbfile = @"DataSource=C:\Users\ClémentTHOREZ\Documents\C#\Projet-C#\Sqlite\SQLITE-Gestionnaire-bibliothèque\db\gestionnaire-bibliotheque.db";
        using (var db = new SqliteConnection(dbfile))
        {
            db.Open();
            using (var command = db.CreateCommand())
            {
                command.CommandText = sql;
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        var value = parameter.Value ?? DBNull.Value;
                        command.Parameters.AddWithValue(parameter.Key, value);
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