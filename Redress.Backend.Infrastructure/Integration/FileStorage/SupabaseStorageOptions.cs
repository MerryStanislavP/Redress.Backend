namespace Redress.Backend.Infrastructure.Integration.FileStorage
{
    public class SupabaseStorageOptions
    {
        public string Url { get; set; }
        public string Key { get; set; }
        public string Bucket { get; set; } = "wwwroot";
    }
} 