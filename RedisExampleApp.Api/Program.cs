using Microsoft.EntityFrameworkCore;
using RedisExampleApp.Api.Model;
using RedisExampleApp.Api.Repositories;
using RedisExampleApp.Api.Repository;
using RedisExampleApp.Api.Services;
using RedisExampleApp.Caching;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//cach kismi oldugu icin bu iptal
//builder.Services.AddScoped<IProductRepository, ProductRepository>();
//bunun yerine
builder.Services.AddScoped<IProductRepository>(sp =>
{
    var appDbContext = sp.GetRequiredService<AppDbContext>();
    var productRepository = new ProductRepository(appDbContext);
    var redisService = sp.GetRequiredService<RedisService>();
    return new ProductRepositoryWithCach(productRepository, redisService);
});

builder.Services.AddScoped<IProductService, ProductService>();


builder.Services.AddDbContext<AppDbContext>(options=>
{
    options.UseInMemoryDatabase("database");
});


builder.Services.AddSingleton<RedisService>(sp =>
{
    return new RedisService(builder.Configuration["CacheOptions:Url"]);
});

/*
 *  HER SERERSINDE db.GetDb(0); KULLANMAMAK ICIN YAPILABILIR
    builder.Services.AddSingleton<IDatabase>(sp =>
    {
        var db = sp.GetRequiredService<RedisService>();
        return db.GetDb(0);
    });
*/
var app = builder.Build();

//default seedleri alanilmek icin
//inMemory de calistigimizi icin yaptik
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}


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
