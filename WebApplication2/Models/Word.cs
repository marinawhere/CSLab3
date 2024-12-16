namespace WebApplication2.Models;

public class Word
{
    public int Id { get; set; }
    public string EnglishWord { get; set; } = string.Empty;
    public string Translation { get; set; } = string.Empty;
    public int MemorizationLevel { get; set; } // 1 - плохо, 3 - отлично
}