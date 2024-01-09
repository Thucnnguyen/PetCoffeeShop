
using Microsoft.EntityFrameworkCore;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Infrastructure.Persistence.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<Like> Like => Set<Like>();
    public DbSet<Notification> Notification => Set<Notification>();
    public DbSet<Order> Order => Set<Order>();
    public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
    public DbSet<Pet> Pets => Set<Pet>();
    public DbSet<PetCafeShop> Shops => Set<PetCafeShop>();
    public DbSet<Post> Posts => Set<Post>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Setting> Settings => Set<Setting>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Wallet> Wallets => Set<Wallet>();
}
