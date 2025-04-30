using Redress.Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Redress.Backend.Domain;

namespace Redress.Backend.Application.Interfaces
{
    public interface IRedressDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Profile> Profiles { get; }
        DbSet<Category> Categories { get; }
        DbSet<Listing> Listings { get; }
        DbSet<Favorite> Favorites { get; }
        DbSet<ListingImage> ListingImages { get; }
        DbSet<ProfileImage> ProfileImages { get; }
        DbSet<Deal> Deals { get; }
        DbSet<Feedback> Feedbacks { get; }
        DbSet<Auction> Auctions { get; }
        DbSet<Bid> Bids { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
