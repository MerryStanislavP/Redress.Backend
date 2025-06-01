using FluentValidation;
using Redress.Backend.Application.Services.ListingArea.Listings;

namespace Redress.Backend.Application.Common.Validators.ListingArea.Listings
{
    public class GetAllListingsQueryValidator : AbstractValidator<GetAllListingsQuery>
    {
        public GetAllListingsQueryValidator()
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