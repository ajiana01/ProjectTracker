using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectTracker.Application.Interfaces;
using ProjectTracker.Application.Tasks.Commands;
using ProjectTracker.Application.Tasks.Queries;
using ProjectTracker.Infrastructure.Data;
using ProjectTracker.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Dependency Injection
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql("Host=localhost;Port=5433;Database=projecttrackerdb;Username=postgres;Password=SuperSecret123"));

//setup identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

//setup jwt auth
var jwtSecret = "SuperSecretKeyThatIsLongEnoughForJwt123!"; // in prod save on .env / appsettings.json
builder.Services.AddAuthentication( options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    
}).AddJwtBearer( options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "ProjectTrackerAPI",
        ValidAudience = "ProjectTrackerClient",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
    };
});

builder.Services.AddAuthorization();


// Dependency injection
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddMediatR( cfg => cfg.RegisterServicesFromAssembly(typeof(CreateTaskCommand).Assembly));

var app = builder.Build();

// hack: automatic create file db projecttracker.db when apps run first time
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseAuthentication();
app.UseAuthorization();

// ENDPOINT definition

//endpoint auth
app.MapPost("/api/auth/register", async (string email, string password, UserManager<IdentityUser> userManager) =>
{
    var user = new IdentityUser { UserName = email, Email = email};
    var result = await userManager.CreateAsync(user, password);
    return result.Succeeded ? Results.Ok("User created") : Results.BadRequest(result.Errors);
});

app.MapPost("/api/auth/login", async (string email, string password, UserManager<IdentityUser> userManager) =>
{
    var user = await userManager.FindByEmailAsync(email);
    if (user != null && await userManager.CheckPasswordAsync(user, password))
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "ProjectTrackerAPI",
            audience: "ProjectTrackerClient",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return Results.Ok(new {token = new JwtSecurityTokenHandler().WriteToken(token)});
    }
    return Results.Unauthorized();
});


// endpoint post tasks with auth required
app.MapPost("/api/tasks", async (CreateTaskCommand command, ClaimsPrincipal user, IMediator mediator) =>
{
    var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue(JwtRegisteredClaimNames.Sub);
    var secureCommand = command with { UserId = userId!};
    var taskId = await mediator.Send(secureCommand);
    return Results.Ok(taskId);
}).RequireAuthorization();

app.MapGet("/api/tasks", async (ClaimsPrincipal user,IMediator mediator) =>
{
    var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue(JwtRegisteredClaimNames.Sub);
    var tasks = await mediator.Send(new GetAllTaskQuery(userId!));
    return Results.Ok(tasks);
}).RequireAuthorization();

app.Run();


// expose program class for testing
public partial class Program {}

