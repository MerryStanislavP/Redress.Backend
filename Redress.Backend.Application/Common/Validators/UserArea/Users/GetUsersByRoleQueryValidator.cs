using FluentValidation;
using Redress.Backend.Application.Services.UserArea.Users;

public class GetUsersByRoleQueryValidator : AbstractValidator<GetUsersByRoleQuery>
{
    public GetUsersByRoleQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("Requesting user ID is required");

        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100");

        RuleFor(x => x.Role)
            .IsInEnum().WithMessage("Invalid role");
    }
}
