using FluentValidation;
using Microsoft.AspNetCore.Http;
using Redress.Backend.Application.Services.ListingArea.Images;

namespace Redress.Backend.Application.Common.Validators.ListingArea.Images
{
    public class UploadListingImageCommandValidator : AbstractValidator<UploadListingImageCommand>
    {
        public UploadListingImageCommandValidator()
        {
            RuleFor(x => x.Image)
                .NotNull()
                .WithMessage("Image file is required")
                .Must(BeAValidImage)
                .WithMessage("Invalid file type. Only JPEG, PNG and GIF images are allowed")
                .Must(BeAValidSize)
                .WithMessage("File size exceeds the maximum limit of 5MB");

            RuleFor(x => x.ListingId)
                .NotEmpty()
                .WithMessage("Listing ID is required");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("User ID is required");
        }

        private bool BeAValidImage(IFormFile file)
        {
            if (file == null) return false;

            var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif" };
            return allowedTypes.Contains(file.ContentType.ToLower());
        }

        private bool BeAValidSize(IFormFile file)
        {
            if (file == null) return false;

            const int maxFileSize = 5 * 1024 * 1024; // 5MB in bytes
            return file.Length <= maxFileSize;
        }
    }
}