using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using PurchaseStocks.Application.Handlers;
using PurchaseStocks.Application.UseCases;
using PurchaseStocks.Infrastructure;
using RabbitMQAndGenericRepository.RabbitMq;

var builder = WebApplication.CreateBuilder(args);
//setx ConnectionString_SellStocksDb "Host=localhost;Port=5432;Database=SellStocksDataBase;Username=postgres;Password=2325"
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("No se encontró ninguna cadena de conexión configurada.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);
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
builder.Services.AddTransient<CrudPossession>();
builder.Services.AddTransient<CrudPrices>();
builder.Services.AddTransient<CrudStocks>();
builder.Services.AddTransient<CrudUsers>();

builder.Services.AddTransient<AddPossessionHandler>();
builder.Services.AddTransient<GetAllInPossessionsHandler>();
builder.Services.AddTransient<GetInPossessionHandler>();
builder.Services.AddTransient<GetPossessionByNameHandler>();

builder.Services.AddTransient<GetOnePriceHandler>();
builder.Services.AddTransient<GetAllPricesHandler>();

builder.Services.AddTransient<GetAllStocksHandler>();
builder.Services.AddTransient<GetStockByIdHandler>();
builder.Services.AddTransient<GetStockByNameHandler>();
builder.Services.AddTransient<GetStockPriceByNameHandler>();

builder.Services.AddTransient<GetAllUsersHandler>();
builder.Services.AddTransient<GetUserByIdHandler>();
builder.Services.AddTransient<GetUserByNameHandler>();

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
