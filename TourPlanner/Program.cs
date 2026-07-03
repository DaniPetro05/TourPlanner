using Microsoft.EntityFrameworkCore;
using TourPlanner.Data;
using TourPlanner.Interfaces;
using TourPlanner.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TourPlanner;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddScoped<ITourService, TourService>();

        builder.Services.AddScoped<ITourLogService, TourLogService>();

        builder.Services.AddScoped<IOpenRouteServiceClient, OpenRouteServiceClient>();

        builder.Services.AddScoped<JWTService>();

        builder.Services.AddControllers();
        
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

        builder.Services.AddHttpClient<OpenRouteServiceClient>();

        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
                };
            });
        
        builder.Services.AddHttpContextAccessor();

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        
        app.UseCors();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}