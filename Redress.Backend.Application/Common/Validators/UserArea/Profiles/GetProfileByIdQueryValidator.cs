using FluentValidation;
using Redress.Backend.Application.Services.UserArea.Profiles;

public class GetProfileByIdQueryValidator : AbstractValidator<GetProfileByIdQuery>
{
    public GetProfileByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Profile Id обязателен.");
    }
}