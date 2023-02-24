using Notebook.DataService.IRepositories;

namespace Notebook.DataService.IConfiguration;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    Task  CompleteAsync();
}