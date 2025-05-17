using FluentValidation;
using Redress.Backend.Application.Services.ListingArea.Listings;

public class GetListingByIdQueryValidator : AbstractValidator<GetListingByIdQuery>
{
    public GetListingByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Listing ID обязателен");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID обязателен");
    }
}