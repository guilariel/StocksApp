using ActualizeDataBaseWithRabbitMQ.Infrastructure;
using LogIn.Application.UseCases;
using LogIn.Domain.Hashing;
using LogInDll;
using LogInLibrary;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer; // Agregar esta línea
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RabbitMQAndGenericRepository.RabbitMq;
using System.Security.Claims;
using System.Text;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var key = builder.Configuration["Jwt:Key"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });
builder.Services.AddSingleton(new JwtService(key));

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = "MyApp",
                ValidAudience = "MyAppUsers",

                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(key)
                ),

                NameClaimType = ClaimTypes.Name,
                RoleClaimType = ClaimTypes.Role
            };
        })
    .AddGoogle("Google", options =>
    {
        options.ClientId = "...";
        options.ClientSecret = "...";
        options.CallbackPath = "/auth/google/callback";
    });


var connectionString = builder.Configuration.GetConnectionString("DataBase");
//builder.Services.AddDbContext<LogInDbContext>(options =>
//    options.UseNpgsql(connectionString));
//builder.Services.AddMediatR(cfg =>
//    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly)
//);
builder.Services.Configure<RabbitMQOptions>(
    builder.Configuration.GetSection("RabbitMQ")
);
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<RabbitMQOptions>>().Value);
builder.Services.AddTransient<RabbitMessageService>();
builder.Services.AddTransient<UserRepository>();
builder.Services.AddTransient<UserFundsRepository>();

builder.Services.AddTransient<AddUserHandler>();
builder.Services.AddTransient<AddFundsHandler>();
builder.Services.AddTransient<SellFundsHandler>();
builder.Services.AddTransient<ICurrentUserService, CurrentUserService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/auth/google", async (HttpContext ctx) =>
{
    var props = new AuthenticationProperties { RedirectUri = "/auth/google/callback" };
    await ctx.ChallengeAsync("Google", props);
});

app.MapGet("/auth/google/callback", async (HttpContext ctx, UserManager<IdentityUser> userManager) =>
{
    var result = await ctx.AuthenticateAsync("Google");
    if (!result.Succeeded) 
        return Results.Unauthorized();
    var email = result.Principal.FindFirstValue(ClaimTypes.Email);
    if (email == null) 
        return Results.BadRequest("Email not provided");
    var user = await userManager.FindByEmailAsync(email);
    if (user == null)
    {
        user = new IdentityUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };
        await userManager.CreateAsync(user);
    }
    var token = JwtService.GenerateToken(user.Id, key);
    return Results.Ok(new { token });
});

app.MapGet("/api/secure", () => "OK").RequireAuthorization("ApiPolicy");

app.MapControllers();

app.Run();