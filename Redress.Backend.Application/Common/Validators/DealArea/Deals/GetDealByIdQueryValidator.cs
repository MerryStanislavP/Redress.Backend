using FluentValidation;
using Redress.Backend.Application.Services.DealArea.Deals;

public class GetDealByIdQueryValidator : AbstractValidator<GetDealByIdQuery>
{
    public GetDealByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id сделки обязателен.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId обязателен.");
    }
}