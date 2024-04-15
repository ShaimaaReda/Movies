namespace Core.IRepository
{
    public interface IGenericRepository<T> where T : class
    {
        Task Create(T model);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(Guid id);
        Task UpdateAsync(T Movie);
        Task DeleteAsync(Guid id);
    }
}
