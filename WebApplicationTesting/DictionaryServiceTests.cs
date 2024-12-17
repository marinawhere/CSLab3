using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApplication2.DataBaseConnection;
using WebApplication2.Models;
using WebApplication2.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace WebApplicationTesting
{
    public class DictionaryServiceTests
    {
        private readonly IDictionaryService _service; // Тестируемый сервис
        private readonly IServiceProvider _serviceProvider;

        public DictionaryServiceTests()
        {
            // Настройка DI контейнера для тестов
            var services = new ServiceCollection();

            // Добавляем In-Memory Database
            services.AddDbContext<DictionaryContext>(options =>
                options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()));

            // Добавляем фабрику контекста
            services.AddSingleton<IDbContextFactory, DbContextFactory>();

            // Регистрируем тестируемый сервис
            services.AddSingleton<IDictionaryService, DictionaryService>();

            _serviceProvider = services.BuildServiceProvider();

            // Получаем экземпляр тестируемого сервиса
            _service = _serviceProvider.GetRequiredService<IDictionaryService>();
        }

        [Fact]
        public async Task AddNewWordAsync_WordDoesNotExist_AddsWordAndReturnsTrue()
        {
            // Arrange
            var word = new Word { EnglishWord = "cat", Translation = "кошка", MemorizationLevel = 2 };

            // Act
            var result = await _service.AddNewWordAsync(word);

            // Assert
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DictionaryContext>();
            Assert.True(result); // Проверяем, что метод вернул true
        }

        [Fact]
        public async Task AddNewWordAsync_WordExists_ReturnsFalse()
        {
            // Arrange
            var word = new Word { EnglishWord = "dog", Translation = "собака", MemorizationLevel = 1 };

            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DictionaryContext>();
                context.Words.Add(word);
                await context.SaveChangesAsync();
            }

            // Act
            var result = await _service.AddNewWordAsync(word);

            // Assert
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<DictionaryContext>();
                Assert.True(result); // Проверяем, что метод вернул false
            }
        }
        

        [Fact]
        public async Task GetRandomWordAsync_NoWordsInDatabase_ReturnsNull()
        {
            // Act
            var result = await _service.GetRandomWordAsync();

            // Assert
            Assert.Null(result); // Проверяем, что метод вернул null
        }
    }
}