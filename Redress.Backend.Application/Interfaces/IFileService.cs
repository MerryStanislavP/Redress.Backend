using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Redress.Backend.Application.Interfaces
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string directory);
        Task DeleteFileAsync(string filePath);
        Task<string> GetFileUrlAsync(string filePath);
    }
}
