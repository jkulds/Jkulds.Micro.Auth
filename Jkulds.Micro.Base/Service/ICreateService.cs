using Jkulds.Micro.Base.Dto;

namespace Jkulds.Micro.Base.Service;

public interface ICreateService<T> where T: DtoBase
{
    public Task<List<T>> CreateRangeAsync(List<T> dtoList);
    public Task<T> CreateAsync(List<T> dto);
}