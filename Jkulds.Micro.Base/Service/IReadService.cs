using Jkulds.Micro.Base.Dto;

namespace Jkulds.Micro.Base.Service;

public interface IReadService<T> where T: DtoBase
{
    public Task<List<T>> GetByIdsAsync(List<T> dtoList);
    public Task<T> GetByIdAsync(List<T> dto);
}