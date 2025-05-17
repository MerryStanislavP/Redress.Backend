using FluentValidation;
using Redress.Backend.Application.Services.UserArea.Feedback;

public class LeaveFeedbackCommandValidator : AbstractValidator<LeaveFeedbackCommand>
{
    public LeaveFeedbackCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Обов’язково вкажіть ID користувача.");

        RuleFor(x => x.Feedback)
            .NotNull().WithMessage("Потрібно передати об’єкт відгуку.");

        RuleFor(x => x.Feedback.Rating)
            .InclusiveBetween(1, 5).WithMessage("Оцінка має бути від 1 до 5.");

        RuleFor(x => x.Feedback.Comment)
            .MaximumLength(1000).WithMessage("Коментар не може перевищувати 1000 символів.");

        RuleFor(x => x.Feedback.DealId)
            .NotEmpty().WithMessage("Потрібно вказати ID угоди.");
    }
}
