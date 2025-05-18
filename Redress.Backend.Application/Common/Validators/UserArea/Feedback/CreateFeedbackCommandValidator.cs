using FluentValidation;
using Redress.Backend.Application.Services.UserArea.Feedback;

public class CreateFeedbackCommandValidator : AbstractValidator<CreateFeedbackCommand>
{
    public CreateFeedbackCommandValidator()
    {
        // DealId обов'язковий
        RuleFor(x => x.Feedback.DealId)
            .NotEmpty().WithMessage("DealId є обов'язковим.");

        // Оцінка має бути в діапазоні від 1 до 5
        RuleFor(x => x.Feedback.Rating)
            .InclusiveBetween(1, 5).WithMessage("Оцінка повинна бути від 1 до 5.");

        // Коментар обов'язковий та не довший за 1000 символів
        RuleFor(x => x.Feedback.Comment)
            .NotEmpty().WithMessage("Коментар є обов'язковим.")
            .MaximumLength(1000).WithMessage("Коментар не повинен перевищувати 1000 символів.");

        // UserId обов'язковий
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId є обов'язковим.");
    }
}