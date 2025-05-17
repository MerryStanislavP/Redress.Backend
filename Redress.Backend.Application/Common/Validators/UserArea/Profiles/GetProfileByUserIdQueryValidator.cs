using FluentValidation;
using Redress.Backend.Application.Services.UserArea.Profiles;

public class GetProfileByUserIdQueryValidator : AbstractValidator<GetProfileByUserIdQuery>
{
    public GetProfileByUserIdQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required.");
    }
}
