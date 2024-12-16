using WebApplication2.Models;

namespace WebApplication2.Services;

public interface IDictionaryService
{
    Task<bool> AddNewWordAsync(Word word);
    
    
    Task<Word?> GetWordAsync(int id);

    
    Task<Word?> GetRandomWordAsync();
}