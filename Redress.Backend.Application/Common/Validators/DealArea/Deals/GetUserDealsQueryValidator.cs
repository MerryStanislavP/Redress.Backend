using FluentValidation;
using Redress.Backend.Application.Services.DealArea.Deals;

public class GetUserDealsQueryValidator : AbstractValidator<GetUserDealsQuery>
{
    public GetUserDealsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId обязателен.");

        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Номер страницы должен быть больше нуля.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize должен быть от 1 до 100.");
    }
}