using Microsoft.EntityFrameworkCore;
using WebApplication2.DataBaseConnection;
using WebApplication2.Models;
using WebApplication2.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace WebApplicationTesting;

public class DictionaryServiceTests
{
    private readonly DictionaryContext _context; // Контекст базы данных
    private readonly DictionaryService _service; // Тестируемый сервис

    public DictionaryServiceTests()
    {
        // Настройка In-Memory Database для тестов
        var options = new DbContextOptionsBuilder<DictionaryContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Уникальная база для каждого теста
            .Options;

        _context = new DictionaryContext(options);
        _service = new DictionaryService(_context);
    }

    [Fact]
    public async Task AddNewWordAsync_WordDoesNotExist_AddsWordAndReturnsTrue()
    {
        // Arrange
        var word = new Word { EnglishWord = "cat", Translation = "кошка", MemorizationLevel = 2 };

        // Act
        var result = await _service.AddNewWordAsync(word);

        // Assert
        Assert.True(result); // Проверяем, что метод вернул true
        Assert.Equal(1, _context.Words.Count()); // Проверяем, что слово добавлено в базу
        Assert.Equal("cat", _context.Words.First().EnglishWord); // Проверяем, что данные корректны
    }

    [Fact]
    public async Task AddNewWordAsync_WordExists_ReturnsFalse()
    {
        // Arrange
        var word = new Word { EnglishWord = "dog", Translation = "собака", MemorizationLevel = 1 };
        _context.Words.Add(word);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.AddNewWordAsync(word);

        // Assert
        Assert.False(result); // Проверяем, что метод вернул false
        Assert.Equal(1, _context.Words.Count()); // Проверяем, что в базе одно слово
    }
    
    [Fact]
    public async Task GetRandomWordAsync_WordsExist_ReturnsRandomWord()
    {
        // Arrange
        var words = new List<Word>
        {
            new Word { EnglishWord = "house", Translation = "дом", MemorizationLevel = 1 },
            new Word { EnglishWord = "car", Translation = "машина", MemorizationLevel = 2 }
        };
        _context.Words.AddRange(words);
        await _context.SaveChangesAsync();

        // Act
        var result = await _service.GetRandomWordAsync();

        // Assert
        Assert.NotNull(result); // Проверяем, что случайное слово найдено
        Assert.Contains(result, words); // Проверяем, что слово взято из списка
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