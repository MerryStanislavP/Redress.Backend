using FluentValidation;
using Redress.Backend.Application.Services.UserArea.Profiles;

public class GetUserProfileQueryValidator : AbstractValidator<GetUserProfileQuery>
{
    public GetUserProfileQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId обязателен.");
    }
}