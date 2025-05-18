using FluentValidation;
using Redress.Backend.Application.Services.UserArea.Feedback;

public class GetFeedbackByIdQueryValidator : AbstractValidator<GetFeedbackByIdQuery>
{
    public GetFeedbackByIdQueryValidator()
    {
        // Id відгуку – обов'язковий
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id відгуку є обов'язковим.");
    }
}