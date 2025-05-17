using FluentValidation;
using Redress.Backend.Application.Services.UserArea.Users;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Target user ID is required");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("Requesting user ID is required");
    }
}
