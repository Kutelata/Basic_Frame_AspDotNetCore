using Loan2022.Application.Interfaces.Shared;

namespace Loan2022.Framework.Services;

public class FileService: IFileService
{
    public async Task<bool> SaveFormFile(IFormFile formFile, string filePath, string fileName)
    {
        try
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            await using (var stream = File.Create($"{filePath}/{fileName}"))
            {
                await formFile.CopyToAsync(stream);
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task DeleteFile(string path, string fileName)
    {
        try
        {
            if (File.Exists(Path.Combine(path, fileName)))
            {
                File.Delete(Path.Combine(path, fileName)); 
            }
        }
        catch (Exception e)
        {
            throw e;
        }
    }
}