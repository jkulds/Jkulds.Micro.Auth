using Jkulds.Micro.Base.Entity;
using Microsoft.EntityFrameworkCore;

namespace Jkulds.Micro.Base.Repository;

public class BaseRepository<T, TContext> : IBaseRepository<T> where T : EntityBase where TContext: DbContext
{
    private readonly TContext _context;
    private readonly DbSet<T> _dbSet;

    protected BaseRepository(TContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<T> AddAsync(T model)
    {
        model.CreatedAt = DateTime.UtcNow;

        await _dbSet.AddAsync(model);
        await _context.SaveChangesAsync();
        
        return await Task.FromResult(model);
    }

    public async Task AddRangeAsync(ICollection<T> models)
    {
        foreach (var model in models)
        {
            model.CreatedAt = DateTime.UtcNow;
        }

        await _dbSet.AddRangeAsync(models);
        await _context.SaveChangesAsync();

        await Task.CompletedTask;
    }

    public async Task<T> UpdateAsync(T model)
    {
        model.UpdatedAt = DateTime.UtcNow;
        
        _dbSet.Entry(model).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return await Task.FromResult(model);
    }

    public async Task UpdateRangeAsync(List<T> models)
    {
        foreach (var model in models)
        {
            model.UpdatedAt = DateTime.UtcNow;
            _dbSet.Entry(model).State = EntityState.Modified;
        }
        
        await _context.SaveChangesAsync();

        await Task.CompletedTask;
    }

    public async Task<T> DeleteById(Guid id)
    {
        var model = await _dbSet.FirstOrDefaultAsync(x => x.Id == id);

        ArgumentNullException.ThrowIfNull(model);
        
        _dbSet.Remove(model);
        await _context.SaveChangesAsync();

        return model;
    }

    public async Task<ICollection<T>?> GetByIdsAsync(ICollection<Guid> ids)
    {
        var set = ids.ToHashSet();
        
        var result = await _dbSet.Where(x => set.Contains(x.Id)).ToListAsync();

        return result;
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        var result = await GetByIdsAsync(new List<Guid> { id }); 
        return result?.FirstOrDefault();
    }
}