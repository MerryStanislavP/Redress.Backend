﻿using Microsoft.AspNetCore.Http;
using Redress.Backend.Application.Interfaces;
using Supabase;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Redress.Backend.Infrastructure.Integration.FileStorage
{
    public class FileService : IFileService
    {
        private readonly Supabase.Client _supabase;
        private readonly string _bucket;

        public FileService(Supabase.Client supabase, IOptions<SupabaseStorageOptions> options)
        {
            _supabase = supabase;
            _bucket = options.Value.Bucket;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string directory)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty", nameof(file));

            // Generate unique filename
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = $"{directory}/{fileName}";

            using var stream = file.OpenReadStream();
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            try
            {
                var response = await _supabase.Storage
                    .From(_bucket)
                    .Upload(fileBytes, filePath, new Supabase.Storage.FileOptions
                    {
                        ContentType = file.ContentType,
                        Upsert = true
                    });

                if (response == null)
                    throw new Exception("Failed to upload file to Supabase Storage");

                // В БД сохраняем только относительный путь
                return filePath;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to upload file to Supabase: {ex.Message}", ex);
            }
        }

        public async Task DeleteFileAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return;

            var path = filePath.TrimStart('/');
            try
            {
                var response = await _supabase.Storage
                    .From(_bucket)
                    .Remove(new List<string> { path });

                if (response == null)
                    throw new Exception("Failed to delete file from Supabase Storage");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete file from Supabase: {ex.Message}", ex);
            }
        }

        public async Task<string> GetFileUrlAsync(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return string.Empty;

            var path = filePath.TrimStart('/');
            int oneYearSeconds = 31536000;

            var signedUrl = await _supabase.Storage
                .From(_bucket)
                .CreateSignedUrl(path, oneYearSeconds);

            return signedUrl ?? string.Empty;
        }

    }
}
