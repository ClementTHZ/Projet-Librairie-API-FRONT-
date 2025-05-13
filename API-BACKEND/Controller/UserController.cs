using System.Text;
using System.Data;

public class UserController
{
    public async static Task GetAllUsers(HttpContext httpContext)
    {
        var response = new StringBuilder();
        var users = UserServices.GetAllUsers();
        var usersArray = new User[users.Rows.Count];
        for (int i = 0; i < users.Rows.Count; i++)
        {
            var row = users.Rows[i];
            var user = new User
            {
                Id = Convert.ToInt32(row["id"]),
                FirstName = row["firstname"].ToString(),
                LastName = row["lastname"].ToString(),
                Age = row["age"].ToString()
            };
            usersArray[i] = user;
            response.AppendLine($"{user.Id} - {user.FirstName} {user.LastName}");
        }
        httpContext.Response.ContentType = "text/plain";
        await httpContext.Response.WriteAsync(response.ToString());
    }
    public async static Task GetUserById(int id, HttpContext httpContext)
    {
        var currUser = UserServices.GetUserById(id);
        var response = new StringBuilder();
        foreach (DataRow row in currUser.Rows)
        {
            var user = new User
            {
                Id = Convert.ToInt32(row["id"]),
                FirstName = row["firstname"].ToString(),
                LastName = row["lastName"].ToString(),
                Age = row["age"].ToString()
            };
            response.AppendLine($"{user.Id} - {user.FirstName} {user.LastName} / {user.Age}ans");
        }
        httpContext.Response.ContentType = "plain/text";
        await httpContext.Response.WriteAsync(response.ToString());
    }
    public async static Task CreatedUser(HttpContext httpContext)
    {
        var user = await httpContext.Request.ReadFromJsonAsync<User>();
        if (user != null)
        {
            UserServices.CreateUser(user.FirstName, user.LastName, user.Age);
            httpContext.Response.StatusCode = 201;
            await httpContext.Response.WriteAsync("✅ User has been created !");
        }
    }
    public async static Task DeleteUser(int id, HttpContext httpContext)
    {
        string userFirstName = "";
        string userLastName = "";
        var user = UserServices.GetUserById(id);
        if (user != null)
        {
            foreach (DataRow row in user.Rows)
            {
                userFirstName = row["firstname"].ToString();
                userLastName = row["lastname"].ToString();
            }
        }
        UserServices.DeleteUser(id);
        httpContext.Response.ContentType = "plain/text";
        await httpContext.Response.WriteAsync($"✅ User {userFirstName} {userLastName} has been deleted !");
    }
}