using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApplication2.DataBaseConnection;

namespace WebApplication2.Services;

public class DbContextFactory : IDbContextFactory
{
    private readonly IServiceProvider _serviceProvider;

    public DbContextFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public DictionaryContext CreateContext()
    {
        // Создаем новый Scope и запрашиваем DictionaryContext
        var scope = _serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<DictionaryContext>();
    }
}