using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Infrastructure.Persistence.Repository
{
	public class PromotionRepository : BaseRepository<Promotion>, IPromotionRepository
	{
		private readonly ApplicationDbContext _dbContext;

		public PromotionRepository(ApplicationDbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
	}
	
}
