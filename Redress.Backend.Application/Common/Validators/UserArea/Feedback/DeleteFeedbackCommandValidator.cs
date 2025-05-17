using FluentValidation;
using Redress.Backend.Application.Services.UserArea.Feedback;

public class DeleteFeedbackCommandValidator : AbstractValidator<DeleteFeedbackCommand>
{
    public DeleteFeedbackCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Потрібно вказати ID відгуку.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Потрібно вказати ID користувача.");
    }
}
