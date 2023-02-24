namespace Notebook.DataService.IRepositories;

public interface IGenericRepository<T>  where T : class
{
    Task<IEnumerable<T>> GetAll();
    Task<T> GetById(Guid id);
    Task<bool> Create(T entity);
    Task<bool> Upsert(T entity);
    Task<bool> Delete(Guid id, string userId);
}
