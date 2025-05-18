using FluentValidation;
using Redress.Backend.Application.Services.UserArea.Profiles;

public class DeleteProfileImageCommandValidator : AbstractValidator<DeleteProfileImageCommand>
{
    public DeleteProfileImageCommandValidator()
    {
        RuleFor(x => x.ProfileId)
            .NotEmpty().WithMessage("Profile ID is required.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");
    }
}
