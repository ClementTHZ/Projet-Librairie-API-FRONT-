using System.Data;
using System.Text;


public class EmpruntController
{
    public async static Task GetAllEmprunts(HttpContext httpContext)
    {
        var emprunts = EmpruntServices.GetAllEmprunts();
        var response = new StringBuilder();
        foreach (DataRow row in emprunts.Rows)
        {
            var userName = row["userfirstname"].ToString();
            var bookTitle = row["booktitle"].ToString();
            var emprunt = new Emprunt
            {
                Id = Convert.ToInt32(row["id"]),
                UserId = Convert.ToInt32(row["userid"]),
                BookId = Convert.ToInt32(row["bookid"]),
                Created_At = Convert.ToDateTime(row["created_at"])
            };
            response.AppendLine($"{emprunt.Id} - {userName} à emprunter {bookTitle} le {emprunt.Created_At}");
        }
        httpContext.Response.ContentType = "plain/text";
        await httpContext.Response.WriteAsync(response.ToString());
    }
    public async static Task GetEmpruntByUserId(int userId, HttpContext httpContext)
    {
        var response = new StringBuilder();

        var currUser = UserServices.GetUserById(userId);
        foreach (DataRow row in currUser.Rows)
        {
            var user = new User { FirstName = row["firstname"].ToString() };
            response.AppendLine($"Voici les emprunts de {user.FirstName}:\n");
        }

        var empruntsTable = EmpruntServices.GetAllEmpruntsByUserId(userId);
        foreach (DataRow row in empruntsTable.Rows)
        {
            var bookTitle = row["booktitle"].ToString();
            var emprunt = new Emprunt
            {
                Id = Convert.ToInt32(row["id"]),
                Created_At = Convert.ToDateTime(row["created_at"])
            };
            response.AppendLine($"{emprunt.Id} - {bookTitle} | Date de l'emprunt: {emprunt.Created_At}");
        }
        httpContext.Response.ContentType = "plain/text";
        await httpContext.Response.WriteAsync(response.ToString());
    }
    public async static Task CreatedEmprunt(HttpContext httpContext)
    {
        var emprunt = await httpContext.Request.ReadFromJsonAsync<Emprunt>();
        if (emprunt != null) EmpruntServices.CreateEmprunt((int)emprunt.UserId, (int)emprunt.BookId);
        httpContext.Response.ContentType = "text";
        await httpContext.Response.WriteAsync("✅ Emprunt has been created !");
    }
    public async static Task DeletedEmprunt(int id, HttpContext httpContext)
    {
        EmpruntServices.DeleteEmprunt(id);
        httpContext.Response.ContentType = "plain/text";
        await httpContext.Response.WriteAsync("✅ Emprunt has been return");
    }
}