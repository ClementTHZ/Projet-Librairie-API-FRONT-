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
        var emprunts = EmpruntServices.GetAllEmpruntsByUserId(userId);
        var empruntsList = new List<Emprunt>();
        foreach (DataRow row in emprunts.Rows)
        {
            var emprunt = new Emprunt
            {
                Id = Convert.ToInt32(row["id"]),
                UserId = Convert.ToInt32(row["userid"]),
                BookId = Convert.ToInt32(row["bookid"]),
                Created_At = Convert.ToDateTime(row["created_at"])
            };
            empruntsList.Add(emprunt);
        }
        httpContext.Response.ContentType = "application/json";
        await httpContext.Response.WriteAsJsonAsync<List<Emprunt>>(empruntsList);
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