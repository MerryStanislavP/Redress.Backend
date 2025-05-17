using FluentValidation;
using Redress.Backend.Application.Services.DealArea.Deals;

public class GetDealsByStatusQueryValidator : AbstractValidator<GetDealsByStatusQuery>
{
    public GetDealsByStatusQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid deal status");

        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100");
    }
}
