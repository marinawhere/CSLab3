using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Services;

namespace WebApplication2.Controllers;

[Route("api/[controller]")] // Устанавливает базовый маршрут для всех методов контроллера (например, /api/dictionary).
[ApiController] // Указывает, что этот класс является контроллером API.
public class DictionaryController : ControllerBase
{
    private readonly IDictionaryService _dictionaryService;

    // Конструктор контроллера принимает экземпляр DictionaryService через DI.
    public DictionaryController(IDictionaryService service)
    {
        _dictionaryService = service; // Сохраняет ссылку на сервис для использования в методах.
    }

    // Получить слово по его ID
    
    [HttpGet("{id}")] // HTTP GET запрос с параметром {id} в маршруте (например, /api/dictionary/1).
    public async Task<IActionResult> GetWord(int id)
    {
        // Асинхронно запрашивает слово с указанным ID из словаря.
        var result = await _dictionaryService.GetWordAsync(id);
        if (result == null) 
            return Ok($"Слово с id = {id} нет в словаре"); // Возвращает сообщение, если слово не найдено.
        return Ok(result); // Возвращает найденное слово в формате JSON.
    }


    // Добавить новое слово
    [HttpPost("add")] // HTTP POST запрос для добавления слова, маршрут: /api/dictionary/add.
    public async Task<IActionResult> AddWordFromQuery(
        [FromQuery] string englishWord, // Получает слово на английском из строки запроса.
        [FromQuery] string translation, // Получает перевод слова из строки запроса.
        [FromQuery] int memorizationLevel // Получает уровень запоминания из строки запроса.
    )
    {
        // Проверяет, что слово и перевод не пустые.
        if (string.IsNullOrEmpty(englishWord) || string.IsNullOrEmpty(translation))
        {
            return BadRequest(new { message = "Неправильно введены данные" }); // Возвращает ошибку 400, если данные некорректны.
        }

        // Проверяет, что уровень запоминания находится в допустимом диапазоне.
        if (memorizationLevel < 1 || memorizationLevel > 3)
        {
            return BadRequest(new { message = "Уровень знания должен быть от 1 до 3" }); // Возвращает ошибку 400, если уровень некорректен.
        }

        // Создает новый объект Word с введенными данными.
        var word = new Word
        {
            EnglishWord = englishWord,
            Translation = translation,
            MemorizationLevel = memorizationLevel
        };

        // Асинхронно добавляет новое слово через сервис.
        var result = await _dictionaryService.AddNewWordAsync(word);
        if (result is false) 
            return Ok($"Слово {word.EnglishWord} уже в словаре"); // Возвращает сообщение, если слово уже существует.

        // Возвращает статус 201 (Created) и добавленное слово, если оно успешно добавлено.
        return CreatedAtAction(nameof(GetWord), new { id = word.Id }, word);
    }

    // Провести тестирование
    [HttpGet("test")] // HTTP GET запрос для получения случайного слова, маршрут: /api/dictionary/test.
    public async Task<IActionResult> TestKnowledge()
    {
        // Асинхронно получает случайное слово из словаря.
        var result = await _dictionaryService.GetRandomWordAsync();
        if (result == null) 
            return Ok("Словарь пуст"); // Возвращает сообщение, если словарь пуст.

        // Возвращает случайное слово для тестирования.
        return Ok($"Переведите слово {result.EnglishWord}");
    }
}