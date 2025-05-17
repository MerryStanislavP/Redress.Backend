using FluentValidation;
using Redress.Backend.Application.Services.ListingArea.Listings;

public class GetListingsByPriceRangeQueryValidator : AbstractValidator<GetListingsByPriceRangeQuery>
{
    public GetListingsByPriceRangeQueryValidator()
    {
        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Мінімальна ціна не может бути менше 0");

        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(x => x.MinPrice)
            .WithMessage("Максимальна ціна повинна бути більше або дорівнювати мінімальній");

        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Номер сторінки має бути більшим за 0");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Розмір сторінки має бути від 1 до 100");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID Обов'язковий");
    }
}