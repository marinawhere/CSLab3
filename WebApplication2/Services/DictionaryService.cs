using Microsoft.EntityFrameworkCore;
using WebApplication2.DataBaseConnection;
using WebApplication2.Models;

namespace WebApplication2.Services;

public class DictionaryService : IDictionaryService
{
    private readonly IDbContextFactory _dbContextFactory;

    public DictionaryService(IDbContextFactory dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<bool> AddNewWordAsync(Word word)
    {
        using (var context = _dbContextFactory.CreateContext())
        {
            if (await context.Words.AnyAsync(x => x.EnglishWord == word.EnglishWord && x.Translation == word.Translation))
            {
                return false;
            }
            context.Words.Add(word);
            await context.SaveChangesAsync();
            return true;
        }
    }

    
    public async Task<Word?> GetWordAsync(string englishWord)
    {
        using (var context = _dbContextFactory.CreateContext())
        {
            return await context.Words
                .FirstOrDefaultAsync(x => x.EnglishWord == englishWord); // Ищем слово по английскому слову.
        }
    }


    public async Task<Word?> GetRandomWordAsync()
    {
        using (var context = _dbContextFactory.CreateContext())
        {
            var words = await context.Words.ToListAsync();
            if (!words.Any()) return null;
            return words[new Random().Next(words.Count)];
        }
    }
    
    
    public async Task<bool> DeleteWordAsync(string englishWord)
    {
        using (var context = _dbContextFactory.CreateContext())
        {
            var word = await context.Words
                .FirstOrDefaultAsync(x => x.EnglishWord == englishWord); // Ищем слово по английскому слову.

            if (word == null)
            {
                return false; // Если слово не найдено, возвращаем false.
            }

            context.Words.Remove(word); // Удаляем слово.
            await context.SaveChangesAsync(); // Сохраняем изменения.
            return true; // Возвращаем true, если удаление успешно.
        }
    }

}