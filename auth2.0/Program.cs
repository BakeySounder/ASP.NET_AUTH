using auth2._0;
using Microsoft.Data.Sqlite;

internal class Program
{
    static DB db = null;
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        db = new DB();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.Map("/Login", Login);

        app.Map("/Register", Register);

        app.Run();
    }

    private static void Login(IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            context.Response.ContentType = "text/html; charset=utf-8";

            var form = context.Request.Form;
            string flogin = form["login"];
            string fpass = form["password"];

            int result = db.CheckUserInDB(flogin, fpass);
            switch (result)
            {
                case 1:
                    await context.Response.WriteAsync($"<div><p>Correct auth</p></div>");
                    break;
                case 0:
                    await context.Response.WriteAsync($"<div><p>Incorrect password</p></div>");
                    break;
                case -1:
                    await context.Response.WriteAsync($"<div><p>UserLogin NotFound</p></div>");
                    break;
            }
            await next();

        });
    }
    private static void Register(IApplicationBuilder app)
    {
        Console.WriteLine("Register");
        app.Use(async (context, next) =>
        {
            context.Response.ContentType = "text/html; charset=utf-8";


            var form = context.Request.Form;
            string flogin = form["login"];
            string fpass = form["password"];
            string femail = form["email"];

            int result = db.CheckUserInDB(flogin);
            if (result == 0 || result == 1)
            {
                await context.Response.WriteAsync($"<div><p>That user already exists</p></div>");
            }
            else
            {
                if (db.AddUserInDB(flogin, fpass, femail)) await context.Response.WriteAsync($"<div><p>Correct register</p></div>");
                else await context.Response.WriteAsync($"<div><p>Something went wrong</p></div>");

            }
            await next();
           

        });
    }

}