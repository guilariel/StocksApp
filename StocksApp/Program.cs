using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQAndGenericRepository.RabbitMq;
using StocksApp.Application.UseCases.RepositoryUseCases;
using StocksApp.Domain.Repositorys;
using StocksApp.Infrastructure.ExternalServices;
using StocksApp.Infrastructure.Utilities;
using StocksApp.Application.UseCases.DbUseCases;
using Microsoft.AspNetCore.Cors; // Asegúrate de incluir este espacio de nombres

var builder = WebApplication.CreateBuilder(args);

//en otro proyecto
//hacer servicio de compras 
//tiene que tener las acciones y el precio y tengo que poder hacer compras con las copias de las acciones del precio
//los precios se actualizan con las bases de datos de precios
//

builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

builder.Services.Configure<RabbitMQOptions>(
    builder.Configuration.GetSection("RabbitMQ")
);
builder.Services.Configure<ConnectionStringsOptions>(
    builder.Configuration.GetSection("ConnectionStrings")
);

var connectionString = builder.Configuration.GetConnectionString("DataBase");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);
// Add services to the container.
builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(connectionString));
builder.Services.AddHangfireServer();

// Registrar el connectionString como singleton si lo necesitas en otros lugares
//builder.Services.AddSingleton<string>(connectionString);
builder.Services.AddSingleton<ICompanyRepository, CompanyRepository>();

builder.Services.AddTransient<AddCompanyHandler>();
builder.Services.AddTransient<DeleteCompanyHandler>();
builder.Services.AddTransient<GetAllCompaniesHandler>();
builder.Services.AddTransient<GetCompanyBySymbolHandler>();
builder.Services.AddTransient<GetQuoteBySymbolHandler>();
builder.Services.AddTransient<GetPriceHandler>();
builder.Services.AddTransient<UpdateCompanys>();
builder.Services.AddTransient<ChangeCurrencyHandler>();

builder.Services.Configure<RabbitMQOptions>(
    builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<RabbitMQOptions>>().Value);

builder.Services.AddTransient<RabbitMessageService>();
builder.Services.AddTransient<ActualizeDbByRabbitHandler>();

// Registrar clientes HTTP
builder.Services.AddHttpClient<FinnhubSymbolClient>();
builder.Services.AddHttpClient<FinnhubQuoteClient>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString)
);
// Configurar Entity Framework y PostgreSQL
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly)
);
builder.Services.AddTransient<CrudPossession>();
builder.Services.AddTransient<CrudPrices>();
builder.Services.AddTransient<CrudStocks>();
builder.Services.AddTransient<CrudUsers>();

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

builder.Services.AddTransient<UpdateDbs>();

// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Controllers y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{//1212121e*
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Stocks API",
        Description = "API para manejar compañías, stocks y precios"
    });
});

var app = builder.Build();

app.UseHangfireDashboard();

// Aplicar la política de CORS
app.UseCors();

// Job recurrente cada 5 minutos
RecurringJob.AddOrUpdate("UpdateStocks", (UpdateCompanys update) => update.UpdateAsync(), "*/5 * * * *");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();


/*$projects = @(
 "C:\Users\UniEz\source\repos\StocksApp\StocksApp\StocksApp.csproj",
 "C:\Users\UniEz\source\repos\StocksApp\SellStocks\SellStocks.csproj",
 "C:\Users\UniEz\source\repos\StocksApp\PurchaseStocks\PurchaseStocks.csproj",
 "C:\Users\UniEz\source\repos\StocksApp\LogIn\LogIn.csproj"
)

foreach ($p in $projects) {
    dotnet publish $p -c Release
}*/