using Microsoft.EntityFrameworkCore;
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Persistence.Context;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
	private readonly ApplicationDbContext _dbContext;

	public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}
}
