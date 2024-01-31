
using Microsoft.EntityFrameworkCore;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Infrastructure.Persistence.Context;

public class ApplicationDbContext : DbContext
{
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<FollowEvent>()
			.HasKey(e => new { e.EventId, e.CreatedById });
		modelBuilder.Entity<Like>()
			.HasKey(e => new { e.CreatedById, e.PostId });
		modelBuilder.Entity<PostCategory>()
			.HasKey(e => new { e.PostId, e.CategoryId });


	}

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<Account> Accounts => Set<Account>();
	public DbSet<Category> Categories => Set<Category>();
	public DbSet<Comment> Comments => Set<Comment>();
	public DbSet<Diary> Diaries => Set<Diary>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<EventField> EventFields => Set<EventField>();
    public DbSet<FollowEvent> FollowEvents => Set<FollowEvent>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<Like> Like => Set<Like>();
    public DbSet<Notification> Notification => Set<Notification>();
    public DbSet<Reservation> Order => Set<Reservation>();
    public DbSet<Pet> Pets => Set<Pet>();
    public DbSet<PetCoffeeShop> Shops => Set<PetCoffeeShop>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<PostCategory> PostCategories => Set<PostCategory>();
    public DbSet<Report> Reports => Set<Report>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Wallet> Wallets => Set<Wallet>();


}
