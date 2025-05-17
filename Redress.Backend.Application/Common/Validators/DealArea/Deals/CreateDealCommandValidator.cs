using FluentValidation;
using Redress.Backend.Application.Services.DealArea.Deals;

public class CreateDealCommandValidator : AbstractValidator<CreateDealCommand>
{
    public CreateDealCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required");

        RuleFor(x => x.Deal).NotNull().WithMessage("Deal data is required");

        RuleFor(x => x.Deal.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.Deal.ProfileId)
            .NotEmpty().WithMessage("ProfileId is required");

        RuleFor(x => x.Deal.ListingId)
            .NotEmpty().WithMessage("ListingId is required");

        RuleFor(x => x.Deal.Status)
            .IsInEnum().WithMessage("Invalid deal status");

        RuleFor(x => x.Deal.ListingType)
            .IsInEnum().WithMessage("Invalid listing type");
    }
}
