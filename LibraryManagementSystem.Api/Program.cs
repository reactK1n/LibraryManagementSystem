using LibraryManagementSystem.Api.Extensions;
using LibraryManagementSystem.Application.Extensions;
using LibraryManagementSystem.Application.Utilities;
using LibraryManagementSystem.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Register database context
builder.Services.AddLibraryMSDbContext(builder.Configuration, builder.Environment);

builder.Services.AddServiceRegistrations();
builder.Services.AddApplicationSevicesInjection();

// Configure Identity options
builder.Services.AddIdentityConfig();
// Configure Authentication
builder.Services.AddAuthenticationConfig(builder.Configuration);

// Swagger configuration for authorization
builder.Services.AddSwaggerConfig();

builder.Services.AddSingleton<LoggerHandler>();
builder.Services.AddTransient<GlobalExceptionHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseDeveloperExceptionPage();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<GlobalExceptionHandler>();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

await app.UseSeedInitializer();//seed permission
app.MapControllers();
app.Run();
