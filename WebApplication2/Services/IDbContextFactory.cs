using WebApplication2.DataBaseConnection;

namespace WebApplication2.Services;

public interface IDbContextFactory
{
    DictionaryContext CreateContext();
}