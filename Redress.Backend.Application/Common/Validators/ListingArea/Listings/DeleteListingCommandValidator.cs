using FluentValidation;
using Redress.Backend.Application.Services.ListingArea.Listings;

public class DeleteListingCommandValidator : AbstractValidator<DeleteListingCommand>
{
    public DeleteListingCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Ідентифікатор оголошення обов'язковий.");

        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Ідентифікатор користувача обов'язковий.");
    }
}