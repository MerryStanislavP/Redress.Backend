using FluentValidation;
using Redress.Backend.Application.Services.ListingArea.Images;

public class DeleteListingImageCommandValidator : AbstractValidator<DeleteListingImageCommand>
{
    public DeleteListingImageCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Image ID is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");
    }
}