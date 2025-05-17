using FluentValidation;
using Redress.Backend.Application.Services.ListingArea.Listings;

public class GetListingsBySexQueryValidator : AbstractValidator<GetListingsBySexQuery>
{
    public GetListingsBySexQueryValidator()
    {
        RuleFor(x => x.Sex)
            .IsInEnum().WithMessage("Неприпустиме значення пола");

        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Номер сторінки має бути більшим за 0");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Розмір сторінки має бути від 1 до 100");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID Обов'язковий");
    }
}