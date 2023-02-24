using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Notebook.DataService.Data;
using Notebook.DataService.IRepositories;
using Notebook.Entities.DbSet;

namespace Notebook.DataService.Repositories;

public class UserRepository : GenericRepository<User>,IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext, ILogger logger) : base(dbContext, logger)
    {
    }

    public override async Task<IEnumerable<User>> GetAll()
    {
        try
        {
            return await _dbSet.Where(x => x.Status == 1)
                .AsNoTracking()
                .ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e,"{Repo} GetAll method has encountered error",typeof(UserRepository));
            throw;
        }
    }
}