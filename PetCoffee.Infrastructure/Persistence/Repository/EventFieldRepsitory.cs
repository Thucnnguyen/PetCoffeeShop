﻿using PetCoffee.Application.Persistence.Repository;
using PetCoffee.Domain.Entities;
using PetCoffee.Infrastructure.Persistence.Context;

namespace PetCoffee.Infrastructure.Persistence.Repository;

public class EventFieldRepsitory : BaseRepository<EventField>, IEventFieldRepsitory
{
	private readonly ApplicationDbContext _dbContext;
	public EventFieldRepsitory(ApplicationDbContext dbContext) : base(dbContext)
	{
		_dbContext = dbContext;
	}
}
