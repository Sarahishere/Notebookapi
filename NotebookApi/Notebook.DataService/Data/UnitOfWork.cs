using Microsoft.Extensions.Logging;
using Notebook.DataService.IConfiguration;
using Notebook.DataService.IRepositories;
using Notebook.DataService.Repositories;

namespace Notebook.DataService.Data;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    public IUserRepository Users { get; }
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger _logger;

    public UnitOfWork(ApplicationDbContext dbContext,ILoggerFactory loggerFactory)
    {
        _dbContext = dbContext;
        _logger = loggerFactory.CreateLogger("db_logs");
        Users = new UserRepository(_dbContext,_logger);
    }
    public async Task CompleteAsync()
    {
       await _dbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}