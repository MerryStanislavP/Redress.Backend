using Microsoft.AspNetCore.Http;
using Redress.Backend.Application.Interfaces;

namespace Redress.Backend.Infrastructure.Integration.FileStorage
{
    public class FileService : IFileService
    {
        private readonly string _baseDirectory;

        public FileService(string baseDirectory)
        {
            _baseDirectory = baseDirectory;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string directory)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty", nameof(file));

            // Create directory if it doesn't exist
            var fullDirectory = Path.Combine(_baseDirectory, directory);
            Directory.CreateDirectory(fullDirectory);

            // Generate unique filename
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(fullDirectory, fileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return relative URL
            return $"/{directory}/{fileName}";
        }

        public async Task DeleteFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return;

            var fullPath = Path.Combine(_baseDirectory, filePath.TrimStart('/'));
            if (File.Exists(fullPath))
            {
                await Task.Run(() => File.Delete(fullPath));
            }
        }
    }
}
