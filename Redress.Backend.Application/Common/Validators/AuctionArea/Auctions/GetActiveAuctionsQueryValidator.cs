using FluentValidation;
using Redress.Backend.Application.Services.AuctionArea.Auctions;

public class GetActiveAuctionsQueryValidator : AbstractValidator<GetActiveAuctionsQuery>
{
    public GetActiveAuctionsQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();

        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100");
    }
}
