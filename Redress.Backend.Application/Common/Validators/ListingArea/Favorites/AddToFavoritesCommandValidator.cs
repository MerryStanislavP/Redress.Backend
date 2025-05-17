using FluentValidation;
using Redress.Backend.Application.Services.ListingArea.Favorites;

public class AddToFavoritesCommandValidator : AbstractValidator<AddToFavoritesCommand>
{
    public AddToFavoritesCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.Favorite)
            .NotNull().WithMessage("Favorite payload is required.")
            .SetValidator(new FavoriteCreateDtoValidator());
    }
}