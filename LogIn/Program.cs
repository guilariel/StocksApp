using LogIn.Application.UseCases;
using LogIn.Domain.Hashing;
using LogIn.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer; // Agregar esta línea
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RabbitMQAndGenericRepository.RabbitMq;
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

var connectionString = builder.Configuration.GetConnectionString("DataBase");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly)
);
builder.Services.Configure<RabbitMQOptions>(
    builder.Configuration.GetSection("RabbitMQ")
);
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<RabbitMQOptions>>().Value);
builder.Services.AddTransient<RabbitMessageService>();
builder.Services.AddTransient<CrudUsers>();
builder.Services.AddTransient<GetAllUsersHandler>();
builder.Services.AddTransient<GetUserByIdHandler>();
builder.Services.AddTransient<GetUserByNameHandler>();
builder.Services.AddTransient<AddUserHandler>();
builder.Services.AddTransient<UserLogInHandler>();

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

app.MapControllers();

app.Run();
