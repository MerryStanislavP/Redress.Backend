using FluentValidation;
using Redress.Backend.Application.Services.ListingArea.Categories;

public class GetCategoryByIdQueryValidator : AbstractValidator<GetCategoryByIdQuery>
{
    public GetCategoryByIdQueryValidator()
    {
        // Перевірка, що ListingId не пустий
        RuleFor(x => x.ListingId)
            .NotEmpty().WithMessage("ListingId є обов'язковим.");

        // Перевірка, що UserId не пустий
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId є обов'язковим.");
    }
}