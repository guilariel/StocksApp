using Hangfire;
using StocksApp.Application.UseCases;
using StocksApp.Domain.Repositorys;
using StocksApp.Infrastructure.ExternalServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHangfire(config => config.
    UseSqlServerStorage("Host=localhost;Port=5432;Database=StocksDateBase;Username=ariel;Password=2325"));
builder.Services.AddHangfireServer();
builder.Services.AddSingleton<ICompanyRepository, CompanyRepository>();
builder.Services.AddTransient<AddCompanyHandler>();
builder.Services.AddTransient<DeleteCompanyHandler>();
builder.Services.AddTransient<GetAllCompaniesHandler>();
builder.Services.AddTransient<GetCompanyBySymbolHandler>();
builder.Services.AddTransient<GetQuoteBySymbolHandler>();
builder.Services.AddTransient<GetPriceHandler>();
builder.Services.AddTransient<UpdateCompanys>();
builder.Services.AddTransient<ChangeCurrencyHandler>();
builder.Services.AddHttpClient<FinnhubSymbolClient>();
builder.Services.AddHttpClient<FinnhubQuoteClient>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseHangfireDashboard();

RecurringJob.AddOrUpdate("UpdateStocks", (UpdateCompanys update) => update.UpdateAsync(), "*/5 * * * *");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
