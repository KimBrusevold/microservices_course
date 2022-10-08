using Catalog.API.Data;
using Catalog.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICatalogContext, CatalogContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();


app.Run();
