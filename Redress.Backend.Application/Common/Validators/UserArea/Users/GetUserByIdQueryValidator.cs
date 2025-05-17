using FluentValidation;
using Redress.Backend.Application.Services.UserArea.Users;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("User ID to retrieve is required");
        RuleFor(x => x.UserId).NotEmpty().WithMessage("Requesting user ID is required");
    }
}
