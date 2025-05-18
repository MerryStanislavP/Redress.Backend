using FluentValidation;
using Redress.Backend.Application.Services.ListingArea.Categories;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        // Перевірка UserId
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId є обов'язковим.");

        // Перевірка, що DTO передано
        RuleFor(x => x.Category)
            .NotNull().WithMessage("Дані категорії є обов'язковими.");

        // Перевірка Name
        RuleFor(x => x.Category.Name)
            .NotEmpty().WithMessage("Назва категорії є обов'язковою.")
            .MaximumLength(100).WithMessage("Назва категорії повинна містити не більше 100 символів.");

        // Перевірка статі
        RuleFor(x => x.Category.Sex)
            .IsInEnum().WithMessage("Некоректне значення статі.");
    }
}