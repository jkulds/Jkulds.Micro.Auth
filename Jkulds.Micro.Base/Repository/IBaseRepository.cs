namespace Jkulds.Micro.Base.Repository;

public interface IBaseRepository<T>
{
    public Task<T> AddAsync(T model);
    public Task AddRangeAsync(ICollection<T> models);
    public Task<T> UpdateAsync(T model);
    public Task UpdateRangeAsync(List<T> models);
    public Task<T> DeleteById(Guid id);
    public Task<ICollection<T>?> GetByIdsAsync(ICollection<Guid> ids);
    public Task<T?> GetByIdAsync(Guid id);
}