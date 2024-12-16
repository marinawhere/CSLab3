using Microsoft.EntityFrameworkCore;
using WebApplication2.DataBaseConnection;
using WebApplication2.Models;

namespace WebApplication2.Services;

public class DictionaryService : IDictionaryService
{
    private readonly DictionaryContext _dictionaryContext;

    public DictionaryService(DictionaryContext dictionaryContext)
    {
        _dictionaryContext = dictionaryContext;
    }
    
    public async Task<bool> AddNewWordAsync(Word word)
    {
        if (await _dictionaryContext.Words.AnyAsync(x => x.EnglishWord == word.EnglishWord && x.Translation == word.Translation))
        {
            return false;
        }
        _dictionaryContext.Words.Add(word);
        await _dictionaryContext.SaveChangesAsync();
        return true;
    }
    

    
    public async Task<Word?> GetWordAsync(int id)
    {
        var word = await _dictionaryContext.Words.FindAsync(id);
        if (word == null) return null;
        return word;
    }


    public async Task<Word?> GetRandomWordAsync()
    {
        var words = await _dictionaryContext.Words.ToListAsync();
        if (!words.Any()) return null;

        var randomWord = words[new Random().Next(words.Count)];
        return randomWord;
    }
}