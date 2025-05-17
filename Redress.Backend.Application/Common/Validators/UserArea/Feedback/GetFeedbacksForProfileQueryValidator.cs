using FluentValidation;
using Redress.Backend.Application.Services.UserArea.Feedback;

public class GetFeedbacksForProfileQueryValidator : AbstractValidator<GetFeedbacksForProfileQuery>
{
    public GetFeedbacksForProfileQueryValidator()
    {
        RuleFor(x => x.ProfileId)
            .NotEmpty().WithMessage("Потрібно вказати ID профілю.");

        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Номер сторінки має бути більшим за 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Розмір сторінки має бути від 1 до 100.");
    }
}
