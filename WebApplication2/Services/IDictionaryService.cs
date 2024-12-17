using WebApplication2.Models;

namespace WebApplication2.Services;

public interface IDictionaryService
{
    Task<bool> AddNewWordAsync(Word word);
    
    Task<Word?> GetWordAsync(string englishWord);
    
    Task<Word?> GetRandomWordAsync();

    Task<bool> DeleteWordAsync(string englishWord);
}