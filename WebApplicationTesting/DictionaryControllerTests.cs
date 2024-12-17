using Microsoft.AspNetCore.Mvc;
using Moq;
using WebApplication2.Controllers;
using WebApplication2.Models;
using WebApplication2.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace WebApplicationTesting;

public class DictionaryControllerTests
{
    private readonly Mock<IDictionaryService> _mockService; // Mock-сервис
    private readonly DictionaryController _controller;     // Тестируемый контроллер

    public DictionaryControllerTests()
    {
        _mockService = new Mock<IDictionaryService>();
        _controller = new DictionaryController(_mockService.Object);
    }

    [Fact]
    public async Task GetWord_WordExists_ReturnsOkWithWord()
    {
        // Arrange
        var testWord = new Word { EnglishWord = "dog", Translation = "собака", MemorizationLevel = 1 };
        _mockService.Setup(s => s.GetWordAsync("dog")).ReturnsAsync(testWord);

        // Act
        var result = await _controller.GetWord("dog");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result); // Проверяем, что возвращается статус 200
        var returnedWord = Assert.IsType<Word>(okResult.Value); // Проверяем, что в теле ответа содержится объект Word
        Assert.Equal(testWord, returnedWord); // Сравниваем ожидаемый и фактический результат
    }

    [Fact]
    public async Task GetWord_WordDoesNotExist_ReturnsOkWithMessage()
    {
        // Arrange
        _mockService.Setup(s => s.GetWordAsync("cat")).ReturnsAsync((Word)null);

        // Act
        var result = await _controller.GetWord("cat");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result); // Проверяем, что возвращается статус 200
        Assert.Equal("Слово 'cat' нет в словаре", okResult.Value); // Проверяем, что возвращается правильное сообщение
    }

    [Fact]
    public async Task AddWord_ValidInput_AddsWordAndReturnsCreated()
    {
        // Arrange
        var testWord = new Word { EnglishWord = "cat", Translation = "кошка", MemorizationLevel = 2 };
        _mockService.Setup(s => s.AddNewWordAsync(It.IsAny<Word>())).ReturnsAsync(true);

        // Act
        var result = await _controller.AddWordFromQuery("cat", "кошка", 2);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result); // Проверяем, что возвращается статус 201
        var addedWord = Assert.IsType<Word>(createdResult.Value); // Проверяем, что в теле ответа содержится объект Word
        Assert.Equal(testWord.EnglishWord, addedWord.EnglishWord); // Сравниваем добавленное слово
    }

    [Fact]
    public async Task DeleteWord_WordExists_ReturnsOkWithMessage()
    {
        // Arrange
        _mockService.Setup(s => s.DeleteWordAsync("house")).ReturnsAsync(true);

        // Act
        var result = await _controller.DeleteWord("house");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result); // Проверяем, что возвращается статус 200
    }
    
    [Fact]
    public async Task TestKnowledge_ValidWord_ReturnsRandomWord()
    {
        // Arrange
        var testWord = new Word { EnglishWord = "house", Translation = "дом", MemorizationLevel = 1 };
        _mockService.Setup(s => s.GetRandomWordAsync()).ReturnsAsync(testWord);

        // Act
        var result = await _controller.TestKnowledge();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result); // Проверяем, что возвращается статус 200
        Assert.Equal($"Переведите слово {testWord.EnglishWord}", okResult.Value); // Проверяем сообщение с тестовым словом
    }

    [Fact]
    public async Task TestKnowledge_NoWordsInDictionary_ReturnsEmptyMessage()
    {
        // Arrange
        _mockService.Setup(s => s.GetRandomWordAsync()).ReturnsAsync((Word)null);

        // Act
        var result = await _controller.TestKnowledge();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result); // Проверяем, что возвращается статус 200
        Assert.Equal("Словарь пуст", okResult.Value); // Проверяем правильное сообщение
    }
}
