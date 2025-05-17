using FluentValidation;
using Redress.Backend.Application.Services.DealArea.Deals;

public class DeleteDealCommandValidator : AbstractValidator<DeleteDealCommand>
{
    public DeleteDealCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Deal ID is required");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");
    }
}
