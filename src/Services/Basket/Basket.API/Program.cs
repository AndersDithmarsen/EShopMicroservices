using BuildingBlocks.Exceptions.Handler;
using Marten;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
// E.g. Add custom exception handler as a service into dependency injection

var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));  // ValidationBehavior first
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));     // LoggingBehavior next 
});
//builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("Database")!);
    options.Schema.For<ShoppingCart>().Identity(x => x.UserName); // Set userName is unique id
}).UseLightweightSessions();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

// Use scrutor to decorate repository object to use cached repository
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "Basket";
});

var app = builder.Build();

// Configure the HTTP request pipeline
// E.g. Configure application to use our custom exception handler

app.MapCarter();

app.UseExceptionHandler(options => { });

app.Run();
