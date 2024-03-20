
using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Persistence.Context;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class PackagePromotionRespository : BaseRepository<PackagePromotion>, IPackagePromotionRespository
{
	private readonly ApplicationDbContext _dbContext;
	public PackagePromotionRespository(ApplicationDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}
}
