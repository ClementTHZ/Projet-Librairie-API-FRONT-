var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(); // Ajoute les services CORS pour gérer les requêtes cross-origin

var app = builder.Build();
app.UseCors(policy => policy
.AllowAnyOrigin() // autorise toutes les origines (ex: http://localhost:3000, http://mon-site.fr, etc.)
.AllowAnyMethod() // autorise toutes les méthodes HTTP (GET, POST, PUT, etc.)
.AllowAnyHeader() // autorise tous les en-têtes (headers)
);

app.MapGet("/books", BookController.GetAllBooks);
app.MapGet("/books/available", BookController.GetAllBooksOnStock);
app.MapPost("/books", BookController.CreatedBook);
app.MapGet("/books/{id}", (int id, HttpContext httpContext) => BookController.GetBookById(id, httpContext));
app.MapDelete("/books/{id}", (int id, HttpContext httpContext) => BookController.DeleteBook(id, httpContext));
app.MapPut("/books/{id}", (int id, HttpContext httpContext) => BookController.AddBook(id, httpContext));

app.MapGet("/users", UserController.GetAllUsers);
app.MapPost("/users", UserController.CreatedUser);
app.MapGet("/users/{id}", (int id, HttpContext httpContext) => UserController.GetUserById(id, httpContext));
app.MapDelete("/users/{id}", (int id, HttpContext httpContext) => UserController.DeleteUser(id, httpContext));

app.MapGet("/emprunts", EmpruntController.GetAllEmprunts);
app.MapPost("/emprunts", EmpruntController.CreatedEmprunt);
app.MapGet("/emprunts/{userId}", (int userId, HttpContext httpContext) => EmpruntController.GetEmpruntByUserId(userId, httpContext));
app.MapDelete("/emprunts/{id}", (int id, HttpContext httpContext) => EmpruntController.DeletedEmprunt(id, httpContext));

app.Run(); // Démarre l'application

