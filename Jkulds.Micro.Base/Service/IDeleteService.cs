using Jkulds.Micro.Base.Dto;

namespace Jkulds.Micro.Base.Service;

public interface IDeleteService<T> where T: DtoBase
{
    public Task<Guid> DeleteByIdAsync(Guid id);
}