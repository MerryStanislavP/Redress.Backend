using FluentValidation;
using Redress.Backend.Application.Services.ListingArea.Listings;

public class GetListingsByCategoryQueryValidator : AbstractValidator<GetListingsByCategoryQuery>
{
    public GetListingsByCategoryQueryValidator()
    {
        // Перевірка UserId — обов'язковий
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId є обов'язковим.");

        // Перевірка CategoryId — обов'язковий
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId є обов'язковим.");

        // Сторінка має бути >= 1
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Номер сторінки повинен бути більше нуля.");

        // Розмір сторінки має бути в межах [1; 100] — не дамо юзеру витягнути всю базу одним махом
        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize повинен бути від 1 до 100.");
    }
}