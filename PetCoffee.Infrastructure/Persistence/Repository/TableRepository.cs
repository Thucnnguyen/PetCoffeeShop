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
    public class TableRepository : BaseRepository<Table>, ITableRepository
    {

        private readonly ApplicationDbContext _dbContext;

        public TableRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }

    

}
