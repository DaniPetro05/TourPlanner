namespace TourPlanner;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        
        var allowedOrigins = builder.Configuration.GetValue<string>("allowedOrigins")!.Split(",");
        
        builder.Services.AddCors(options => 
            {
                options.AddDefaultPolicy(policy => 
                {
                    policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
                });
            });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        
        app.UseCors();

        app.UseAuthorization();

        /*var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };*/
        var user = new User()
        {
            id = 1,
            username = "Test",
            password = "",
            email = "test@example.com"
        };

        app.MapPost("/user", (HttpContext httpContext) => Results.Ok(user))
            .WithName("UserPostRequest");
        app.MapGet("/user", () => Results.Ok(user));
        app.Run();
    }
}