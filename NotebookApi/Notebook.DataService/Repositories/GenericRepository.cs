using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Notebook.DataService.Data;
using Notebook.DataService.IRepositories;

namespace Notebook.DataService.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _dbContext;
    internal DbSet<T> _dbSet;
    protected readonly ILogger _logger;

    public GenericRepository(ApplicationDbContext dbContext, ILogger logger)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
        _logger = logger;
    }

    public virtual async Task<IEnumerable<T>> GetAll()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetById(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<bool> Create(T entity)
    {
        await _dbSet.AddAsync(entity);
        return true;
    }

    public virtual Task<bool> Upsert(T entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(Guid id, string userId)
    {
        throw new NotImplementedException();
    }
}