using FluentValidation;
using Redress.Backend.Application.Services.ListingArea.Images;

public class UploadListingImageCommandValidator : AbstractValidator<UploadListingImageCommand>
{
    public UploadListingImageCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.Image)
            .NotNull().WithMessage("Image data is required.")
            .SetValidator(new ListingImageCreateDtoValidator());
    }
}