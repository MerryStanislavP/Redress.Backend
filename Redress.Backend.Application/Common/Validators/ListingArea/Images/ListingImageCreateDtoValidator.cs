using FluentValidation;
using Redress.Backend.Contracts.DTOs.CreateDTOs;

public class ListingImageCreateDtoValidator : AbstractValidator<ListingImageCreateDto>
{
    public ListingImageCreateDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(255).WithMessage("Name must be at most 255 characters.");

        RuleFor(x => x.Url)
            .NotEmpty().WithMessage("Image URL is required.")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .WithMessage("Image URL must be a valid absolute URI.");

        RuleFor(x => x.ListingId)
            .NotEmpty().WithMessage("ListingId is required.");
    }
}