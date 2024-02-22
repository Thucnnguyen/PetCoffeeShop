﻿
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Persistence.Interceptors;

namespace PetCoffee.Infrastructure.Persistence.Context;

public class ApplicationDbContext : DbContext
{
	private readonly AuditableEntitySaveChangesInterceptor _saveChangesInterceptor;

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, 
		AuditableEntitySaveChangesInterceptor saveChangesInterceptor) : base(options)
	{
		_saveChangesInterceptor = saveChangesInterceptor;
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		optionsBuilder.AddInterceptors(_saveChangesInterceptor);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<JoinEvent>()
			.HasKey(e => new { e.EventId, e.CreatedById });
		modelBuilder.Entity<FollowPetCfShop>()
			.HasKey(e => new { e.ShopId, e.CreatedById });
		modelBuilder.Entity<Like>()
			.HasKey(e => new { e.CreatedById, e.PostId });
		modelBuilder.Entity<PostCategory>()
			.HasKey(e => new { e.PostId, e.CategoryId });
		modelBuilder.Entity<PostPetCoffeeShop>()
			.HasKey(e => new { e.PostId, e.ShopId });
	}

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<Account> Accounts => Set<Account>();
	public DbSet<Category> Categories => Set<Category>();
	public DbSet<Comment> Comments => Set<Comment>();
	public DbSet<Moment> Diaries => Set<Moment>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<EventField> EventFields => Set<EventField>();
    public DbSet<JoinEvent> FollowEvents => Set<JoinEvent>();
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
	public DbSet<PostPetCoffeeShop> PostPetCoffeeShops => Set<PostPetCoffeeShop>();
	public DbSet<FollowPetCfShop> FollowPetCfShops => Set<FollowPetCfShop>();


}
