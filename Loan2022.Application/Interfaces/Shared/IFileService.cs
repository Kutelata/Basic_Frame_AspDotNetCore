using Microsoft.AspNetCore.Http;

namespace Loan2022.Application.Interfaces.Shared;

public interface IFileService
{
    Task<bool> SaveFormFile(IFormFile formFile, string filePath, string fileName);
    Task DeleteFile(string path, string fileName);
}