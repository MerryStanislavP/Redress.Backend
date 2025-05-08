using Microsoft.EntityFrameworkCore;
using Redress.Backend.Application.Interfaces;
using Redress.Backend.Domain.Entities;
using Redress.Backend.Infrastructure.Persistence;

namespace Redress.Backend.Infrastructure.Persistence
{
    public class RedressDbContext : DbContext, IRedressDbContext
    {
        public RedressDbContext(DbContextOptions<RedressDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Listing> Listings { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<ListingImage> ListingImages { get; set; }
        public DbSet<ProfileImage> ProfileImages { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Bid> Bids { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(RedressDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
