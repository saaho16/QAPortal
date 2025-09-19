namespace QAPortal.Data.Repositories;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> GetAllAsync();
    Task<T?> GetByIdAsync(int Id);
    Task<T?> InsertAsync(T Entity);
    Task<T?> UpdateAsync(T Entity);
    Task DeleteAsync(int Id);
    Task SaveAsync();
}