using Coffee.Journal.Application.Contracts.Models;

namespace Coffee.Journal.Application.Contracts.UseCases;

public interface IBaseUseCase<TBaseRequest, TBaseResponse> 
    where TBaseRequest : BaseRequest
    where TBaseResponse : BaseResponse
{
    Task<Guid> AddAsync(TBaseRequest request);

    Task<IReadOnlyList<TBaseResponse>> GetAllAsync();

    Task<TBaseResponse?> GetByIdAsync(Guid id);

    Task<bool> UpdateAsync(Guid id, TBaseRequest request);

    Task<bool> DeleteAsync(Guid id);
}
