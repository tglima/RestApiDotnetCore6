using WebApi.Helpers;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<DbSQLiteContext>();

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

// ensure database and tables exist
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<DbSQLiteContext>();
    await context.Init();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
