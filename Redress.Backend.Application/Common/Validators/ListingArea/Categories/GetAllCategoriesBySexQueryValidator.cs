using FluentValidation;
using Redress.Backend.Application.Services.ListingArea.Categories;

public class GetAllCategoriesBySexQueryValidator : AbstractValidator<GetAllCategoriesBySexQuery>
{
    public GetAllCategoriesBySexQueryValidator()
    {
        // Перевірка UserId
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId є обов'язковим.");

        // Перевірка, що Sex не виходить за межі enum (якщо enum без 0)
        RuleFor(x => x.Sex)
            .IsInEnum().WithMessage("Некоректне значення статі.");
    }
}