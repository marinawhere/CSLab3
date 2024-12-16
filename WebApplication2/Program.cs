using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using WebApplication2.DataBaseConnection;
using WebApplication2.Services;

var builder = WebApplication.CreateBuilder(args);

// Добавляем настройки Entity Framework и подключение к MySQL
builder.Services.AddDbContext<DictionaryContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 23)) // Укажите версию вашего MySQL сервера
    )
);

builder.Services.AddScoped<IDictionaryService, DictionaryService>();

// Добавляем контроллеры
builder.Services.AddControllers();

var app = builder.Build();

// Конфигурация HTTP-запросов
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();