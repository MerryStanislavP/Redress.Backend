using FluentValidation;
using Redress.Backend.Application.Services.DealArea.Deals;

public class UpdateDealStatusCommandValidator : AbstractValidator<UpdateDealStatusCommand>
{
    public UpdateDealStatusCommandValidator()
    {
        RuleFor(x => x.DealId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();

        RuleFor(x => x.UpdateDto).NotNull();

        RuleFor(x => x.UpdateDto.Status)
            .IsInEnum().WithMessage("Invalid deal status");
    }
}
