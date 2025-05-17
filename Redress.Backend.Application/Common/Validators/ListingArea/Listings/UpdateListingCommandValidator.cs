using FluentValidation;
using Redress.Backend.Application.Services.ListingArea.Listings;

public class UpdateListingCommandValidator : AbstractValidator<UpdateListingCommand>
{
    public UpdateListingCommandValidator()
    {
        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("Id оголошення не може бути порожнім");

        RuleFor(c => c.UserId)
            .NotEmpty().WithMessage("Id користувача не може бути порожнім");

        RuleFor(c => c.UpdateDto)
            .NotNull().WithMessage("Дані для оновлення не можуть бути порожніми");

        When(c => c.UpdateDto != null, () =>
        {
            RuleFor(c => c.UpdateDto.Title)
                .MaximumLength(100).WithMessage("Заголовок не може перевищувати 100 символів");

            RuleFor(c => c.UpdateDto.Price)
                .GreaterThan(0).When(d => d.UpdateDto.Price.HasValue)
                .WithMessage("Ціна повинна бути більшою за 0");

            RuleFor(c => c.UpdateDto.Description)
                .MaximumLength(1000).When(d => d.UpdateDto.Description != null)
                .WithMessage("Опис не повинен перевищувати 1000 символів");

            RuleFor(c => c.UpdateDto.CategoryId)
                .NotEqual(Guid.Empty).When(d => d.UpdateDto.CategoryId.HasValue)
                .WithMessage("Категорія вказана некоректно");
        });
    }
}
