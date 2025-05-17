using FluentValidation;
using Redress.Backend.Application.Services.ListingArea.Favorites;

public class DeleteFavoriteCommandValidator : AbstractValidator<DeleteFavoriteCommand>
{
    public DeleteFavoriteCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.ProfileId)
            .NotEmpty().WithMessage("ProfileId is required.");

        RuleFor(x => x.ListingId)
            .NotEmpty().WithMessage("ListingId is required.");
    }
}