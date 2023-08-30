using Jkulds.Micro.Base.Dto;

namespace Jkulds.Micro.Base.Service;

public interface IUpdateService<T> where T: DtoBase
{
    public Task<List<T>> UpdateRangeAsync(List<T> dtoList);
    public Task<T> UpdateAsync(List<T> dto);
}