using Loan2022.Result;
using Loan2022.ViewModels.Bank;
using Loan2022.ViewModels.MediasCustomer;

namespace Loan2022.Application.Interfaces.Services;

public interface IMediaCustomerService
{
    Task<PaginatedResult<MediaCustomerDto>> GetAll(MediaCustomerInputDto input);
    Task CreateOrUpdate(MediaCustomerDto input);
    Task<MediaCustomerDto> GetById(long id);
    Task Delete(long id);
    Task<string?> GetAvatarCustomer(long id);
    Task<List<MediaDto>> GetMediasCustomer(long id);
    Task<string> GetSignatureContractCustomer(long id);
    Task CreateIdentityCustomerMedia(Dictionary<string, MediaDto> files, long customerId);
}