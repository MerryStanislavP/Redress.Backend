using FluentValidation;
using Redress.Backend.Application.Services.AuctionArea.Auctions;

public class StartAuctionCommandValidator : AbstractValidator<StartAuctionCommand>
{
    public StartAuctionCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();

        RuleFor(x => x.Auction).NotNull();

        RuleFor(x => x.Auction.ListingId).NotEmpty();

        RuleFor(x => x.Auction.StartPrice)
            .GreaterThan(0).WithMessage("Start price must be greater than 0");

        RuleFor(x => x.Auction.MinStep)
            .GreaterThan(0).WithMessage("Minimum step must be greater than 0");

        RuleFor(x => x.Auction.EndAt)
            .NotNull().WithMessage("End date is required")
            .GreaterThan(DateTime.UtcNow).WithMessage("End date must be in the future");
    }
}
