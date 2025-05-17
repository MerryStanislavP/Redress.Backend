using FluentValidation;
using Redress.Backend.Application.Services.ListingArea.Listings;

public class CreateListingCommandValidator : AbstractValidator<CreateListingCommand>
{
    public CreateListingCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId обов'язковий.");

        RuleFor(x => x.Listing)
            .NotNull().WithMessage("Нема даних оголошення");

        When(x => x.Listing != null, () =>
        {
            RuleFor(x => x.Listing.Title)
                .NotEmpty().WithMessage("Заголовок обов'язковий.")
                .MaximumLength(100);

            RuleFor(x => x.Listing.Description)
                .NotEmpty().WithMessage("Опис обов'язковий.")
                .MaximumLength(1000);

            RuleFor(x => x.Listing.Price)
                .GreaterThan(0).WithMessage("Ціна повинна бути більше 0.");

            RuleFor(x => x.Listing.CategoryId)
                .NotEqual(Guid.Empty).WithMessage("Категорія обов'язкова.");

            RuleFor(x => x.Listing.Latitude)
                .InclusiveBetween(-90, 90).When(x => x.Listing.Latitude.HasValue);

            RuleFor(x => x.Listing.Longitude)
                .InclusiveBetween(-180, 180).When(x => x.Listing.Longitude.HasValue);
        });
    }
}