using FluentValidation;
using Redress.Backend.Application.Services.UserArea.Images;

public class UploadProfileImageCommandValidator : AbstractValidator<UploadProfileImageCommand>
{
    public UploadProfileImageCommandValidator()
    {
        RuleFor(x => x.ProfileImage)
            .NotNull().WithMessage("Profile image data is required.");

        RuleFor(x => x.ProfileImage.ProfileId)
            .NotEmpty().WithMessage("Profile ID is required.");

        RuleFor(x => x.ProfileImage.Name)
            .NotEmpty().WithMessage("Image name is required.")
            .MaximumLength(255).WithMessage("Image name must be at most 255 characters.");

        RuleFor(x => x.ProfileImage.Url)
            .NotEmpty().WithMessage("Image URL is required.")
            .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .WithMessage("Image URL is not valid.");
    }
}
