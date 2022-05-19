using System.Data;
using System.Linq;
using AutoMapper;
using Loan2022.Application.Extensions;
using Loan2022.Application.Interfaces.CacheRepositories;
using Loan2022.Application.Interfaces.Repositories;
using Loan2022.Application.Interfaces.Services;
using Loan2022.Domain.Entities;
using Loan2022.Result;
using Loan2022.ViewModels.Bank;
using System.Linq.Dynamic.Core;
using Loan2022.Application.Enums;
using Loan2022.Infrastructure.DbContexts;
using Loan2022.ViewModels.MediasCustomer;
using Microsoft.EntityFrameworkCore;

namespace Loan2022.Service;

public class MediaCustomerService : IMediaCustomerService
{
    private readonly IMediaCustomerRepository _mediaCustomerRepository;
    private readonly IMediaRepository _mediaRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private IMapper _mapper;
    private readonly ApplicationDbContext _applicationDbContext;

    public MediaCustomerService(
        IMediaCustomerRepository mediaCustomerRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IMediaRepository mediaRepository,
        ApplicationDbContext applicationDbContext,
        ICustomerRepository customerRepository
    )
    {
        _mediaCustomerRepository = mediaCustomerRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _mediaRepository = mediaRepository;
        _applicationDbContext = applicationDbContext;
        _customerRepository = customerRepository;
    }

    public async Task<PaginatedResult<MediaCustomerDto>> GetAll(MediaCustomerInputDto input)
    {
        var query = _mediaCustomerRepository.MediaCustomers
            .Select(x => new MediaCustomerDto
            {
                Id = x.Id,
                Type = x.Type,
                CustomerId = x.CustomerId,
                MediaId = x.MediaId
            });
        var pageAndSorted = await query
            .OrderBy(input.Sorting)
            .ToPaginatedListAsync(input.PageNumber, input.PageSize);
        return pageAndSorted;
    }

    public async Task CreateOrUpdate(MediaCustomerDto input)
    {
        if (input.Id > 0)
        {
            await Update(input);
        }
        else
        {
            await Create(input);
        }
    }

    private async Task Create(MediaCustomerDto input)
    {
        var data = _mapper.Map<MediaCustomer>(input);
        await _mediaCustomerRepository.InsertAsync(data);
        await _unitOfWork.Commit();
    }

    private async Task Update(MediaCustomerDto input)
    {
        var bank = await _mediaCustomerRepository.GetByIdAsync(input.Id);
        _mapper.Map(input, bank);
        await _mediaCustomerRepository.UpdateAsync(bank);
        await _unitOfWork.Commit();
    }

    public async Task<MediaCustomerDto> GetById(long id)
    {
        var data = await _mediaCustomerRepository.GetByIdAsync(id);
        var result = _mapper.Map<MediaCustomerDto>(data);
        return result;
    }

    public async Task Delete(long id)
    {
        var data = await _mediaCustomerRepository.GetByIdAsync(id);
        await _mediaCustomerRepository.DeleteAsync(data);
        await _unitOfWork.Commit();
    }


    public async Task<string?> GetAvatarCustomer(long id)
    {
        var avatar = await _mediaCustomerRepository.MediaCustomers
            .Include(x => x.Media)
            .Where(x => x.CustomerId == id && x.Type == MediaType.Avatar.ToString())
            .Select(x => x.Media.Path).FirstOrDefaultAsync();
        return avatar;
    }

    public async Task<List<MediaDto>> GetMediasCustomer(long id)
    {
        var medias = await _mediaCustomerRepository.MediaCustomers
            .Include(x => x.Media)
            .Where(x => x.CustomerId == id)
            .Select(x => new MediaDto()
            {
                Extension = x.Media.Extension,
                Name = x.Media.Name,
                Path = x.Media.Path,
                Id = x.Media.Id,
                Type = x.Type
            }).ToListAsync();
        return medias;
    }

    public async Task<string> GetSignatureContractCustomer(long id)
    {
        var signatureId = await _customerRepository.Customers.Include(x => x.Contract)
            .Where(x => x.Id == id).Select(x => x.Contract.DigitalSignature).FirstOrDefaultAsync();
        if (!(signatureId > 0)) return "";
        var result = await _mediaRepository.GetByIdAsync(signatureId ?? 1);
        return result.Name;
    }


    public async Task CreateIdentityCustomerMedia(Dictionary<string, MediaDto> files, long customerId)
    {
        using (var transaction =
               await _applicationDbContext.Database.BeginTransactionAsync(IsolationLevel.ReadUncommitted))
        {
            try
            {
                foreach (var item in files)
                {
                    var media = _mapper.Map<Media>(item.Value);
                    await _mediaRepository.InsertAsync(media);
                    await _unitOfWork.Commit();
                    var exist = await _mediaCustomerRepository.MediaCustomers.FirstOrDefaultAsync(x =>
                        x.Type == item.Key && x.CustomerId == customerId);
                    if (exist == null)
                    {
                        var mediaCustomer = new MediaCustomer()
                        {
                            CustomerId = customerId,
                            MediaId = media.Id,
                            Type = item.Key
                        };
                        await _mediaCustomerRepository.InsertAsync(mediaCustomer);
                    }
                    else
                    {
                        exist.MediaId = media.Id;
                        await _mediaCustomerRepository.UpdateAsync(exist);
                    }

                    await _unitOfWork.Commit();
                }

                var customer = await _customerRepository.GetByIdAsync(customerId);
                customer.CurrentStepVerify = 1;
                await _customerRepository.UpdateAsync(customer);
                await _unitOfWork.Commit();
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw e;
            }
        }
    }
}