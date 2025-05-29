using Microsoft.AspNetCore.Http;


namespace Redress.Backend.Application.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string directory);
        Task DeleteFileAsync(string filePath);
        string GetFileUrl(string filePath);
    }
}
