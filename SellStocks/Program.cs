using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using PurchaseDll;
using PurchaseStocks.Application.Handlers;
using RabbitMQAndGenericRepository.RabbitMq;
using RabbitMQAndGenericRepository.Repositorio;
using SellDll;

var builder = WebApplication.CreateBuilder(args);
//setx ConnectionString_SellStocksDb "Host=localhost;Port=5432;Database=SellStocksDataBase;Username=postgres;Password=2325"
var connectionString =
    Environment.GetEnvironmentVariable("ConnectionString_SellStocksDb") ??
    builder.Configuration.GetConnectionString("ConnectionString_SellStocksDb");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("No se encontró ninguna cadena de conexión configurada.");
}


builder.Services.AddDbContext<DbContext, GenericDbContext>(options =>
    options.UseNpgsql(connectionString)
);

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly)
);

builder.Services.AddTransient<DeleteInPossessionHandler>();
builder.Services.AddScoped<StockRepository>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<PriceRepository>();

builder.Services.Configure<RabbitMQOptions>(
    builder.Configuration.GetSection("RabbitMQ")
);
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<RabbitMQOptions>>().Value);
builder.Services.AddTransient<RabbitMessageService>();

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

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors();

app.MapControllers();

app.Run();
