using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using PurchaseStocks.Application.Handlers;
using PurchaseDll;
using RabbitMQAndGenericRepository.RabbitMq;
using RabbitMQAndGenericRepository.Repositorio;
using ActualizeDataBaseWithRabbitMQ.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
//setx ConnectionString_SellStocksDb "Host=localhost;Port=5432;Database=SellStocksDataBase;Username=postgres;Password=2325"
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("No se encontró ninguna cadena de conexión configurada.");
}

//builder.Services.AddDbContext<PurchaseStocksDbContext>(options =>
//    options.UseNpgsql(connectionString)
//);
//  UseSqlServer, UseSqlite, etc., según tu base de datos

//  Registramos MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly)
);
builder.Services.Configure<RabbitMQOptions>(
    builder.Configuration.GetSection("RabbitMQ")
);
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<RabbitMQOptions>>().Value);
builder.Services.AddTransient<RabbitMessageService>();

builder.Services.AddTransient<AddPossessionHandler>();
builder.Services.AddScoped<StockRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<PriceRepository>();


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
