using FluentValidation;
using Redress.Backend.Application.Services.DealArea.Deals;

public class GetDealsByProfileIdQueryValidator : AbstractValidator<GetDealsByProfileIdQuery>
{
    public GetDealsByProfileIdQueryValidator()
    {
        RuleFor(x => x.ProfileId)
            .NotEmpty().WithMessage("ProfileId is required");

        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100");
    }
}
