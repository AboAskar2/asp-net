var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.UseDefaultFiles();
app.UseStaticFiles();
app.MapPost("/index", (HttpContext context, LoginRequest request) =>
{
    if (request.Username == "admin" && request.Password == "123")
    {
        context.Response.Cookies.Append("username", request.Username);

        return Results.Ok(new
        {
            Status = "Success",
            Message = "Login successful"
        });
    }

    return Results.BadRequest(new
    {
        Status = "Error",
        Message = "Invalid username or password"
    });
});
app.MapGet("/profile", (HttpContext context) =>
{
    var username = context.Request.Cookies["username"];
    if (string.IsNullOrEmpty(username))
    {
        return Results.BadRequest(new
        {
            Status = "Error",
            Message = "User not logged in"
        });

    }

    return Results.Ok(new
    {
        Status = "Success",
        Message = $"Welcome {username}"
    });

});

app.MapGet("/logout", (HttpContext context) =>
{
    context.Response.Cookies.Delete("username", new CookieOptions
    {
        Path = "/",
        Secure = false,
        SameSite = SameSiteMode.Lax
    });
    return Results.Ok("Logged out successfully");
});

app.Run();

public class LoginRequest
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}
