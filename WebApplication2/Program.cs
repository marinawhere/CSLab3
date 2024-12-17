using Microsoft.EntityFrameworkCore;
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

// Регистрация DictionaryService как Singleton
builder.Services.AddSingleton<IDbContextFactory, DbContextFactory>();
builder.Services.AddSingleton<IDictionaryService, DictionaryService>();

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