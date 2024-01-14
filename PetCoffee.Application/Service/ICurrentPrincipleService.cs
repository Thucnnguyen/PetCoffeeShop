﻿
using System.Security.Claims;

namespace PetCoffee.Application.Service;

public interface ICurrentPrincipleService
{
	public string? CurrentPrincipal { get; }

	public long? CurrentSubjectId { get; }

}
