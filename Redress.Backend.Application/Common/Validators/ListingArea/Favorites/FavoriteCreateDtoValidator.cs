using FluentValidation;
using Redress.Backend.Contracts.DTOs.CreateDTO;

public class FavoriteCreateDtoValidator : AbstractValidator<FavoriteCreateDto>
{
    public FavoriteCreateDtoValidator()
    {
        RuleFor(x => x.ProfileId)
            .NotEmpty().WithMessage("ProfileId is required.");

        RuleFor(x => x.ListingId)
            .NotEmpty().WithMessage("ListingId is required.");
    }
}
