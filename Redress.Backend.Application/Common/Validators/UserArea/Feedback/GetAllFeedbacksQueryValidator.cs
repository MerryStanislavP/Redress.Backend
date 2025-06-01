using FluentValidation;
using Redress.Backend.Application.Services.UserArea.Feedback;

namespace Redress.Backend.Application.Common.Validators.UserArea.Feedback
{
    public class GetAllFeedbacksQueryValidator : AbstractValidator<GetAllFeedbacksQuery>
    {
        public GetAllFeedbacksQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");

            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page number must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
        }
    }
} 