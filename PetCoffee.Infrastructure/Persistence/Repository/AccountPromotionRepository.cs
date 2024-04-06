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
	public class AccountPromotionRepository : BaseRepository<AccountPromotion>, IAccountPromotionRepository
	{
		private readonly ApplicationDbContext _dbContext;

		public AccountPromotionRepository(ApplicationDbContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}
	}


}
