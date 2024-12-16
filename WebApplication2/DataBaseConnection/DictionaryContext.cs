using WebApplication2.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.DataBaseConnection;


// Создаем соединение с бд
public class DictionaryContext : DbContext
{
    public DictionaryContext(DbContextOptions<DictionaryContext> options) : base(options) { }

    public DbSet<Word> Words { get; set; }
}