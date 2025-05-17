using FluentValidation;
using Redress.Backend.Application.Services.UserArea.Profiles;

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Profile ID is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");

        RuleFor(x => x.UpdateDto)
            .NotNull().WithMessage("Update DTO is required.");

        RuleFor(x => x.UpdateDto.Bio)
            .MaximumLength(1000).WithMessage("Bio can't be longer than 1000 characters.");
    }
}
