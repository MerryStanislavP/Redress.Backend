using FluentValidation;
using Redress.Backend.Application.Services.UserArea.Feedback;

public class GetProfileFeedbacksQueryValidator : AbstractValidator<GetProfileFeedbacksQuery>
{
    public GetProfileFeedbacksQueryValidator()
    {
        // ProfileId – обов'язковий
        RuleFor(x => x.ProfileId)
            .NotEmpty().WithMessage("ProfileId є обов'язковим.");

        // UserId – обов'язковий
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId є обов'язковим.");

        // Сторінка ≥ 1
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Номер сторінки повинен бути більше нуля.");

        // PageSize у межах 1–100
        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize повинен бути від 1 до 100.");
    }
}