using Jkulds.Micro.Base.Dto;
using Jkulds.Micro.Base.Repository;

namespace Jkulds.Micro.Base.Service;

// TODO доделать
public class ServiceBase<T, TRepository> : ICreateService<T>, IReadService<T>, IUpdateService<T>, IDeleteService<T> where T: DtoBase
{
    public Task<List<T>> CreateRangeAsync(List<T> dtoList)
    {
        throw new NotImplementedException();
    }

    public Task<T> CreateAsync(List<T> dto)
    {
        throw new NotImplementedException();
    }

    public Task<List<T>> GetByIdsAsync(List<T> dtoList)
    {
        throw new NotImplementedException();
    }

    public Task<T> GetByIdAsync(List<T> dto)
    {
        throw new NotImplementedException();
    }

    public Task<List<T>> UpdateRangeAsync(List<T> dtoList)
    {
        throw new NotImplementedException();
    }

    public Task<T> UpdateAsync(List<T> dto)
    {
        throw new NotImplementedException();
    }

    public Task<Guid> DeleteByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}