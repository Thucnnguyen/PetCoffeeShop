﻿

using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Service;
using System.Security.Claims;

namespace PetCoffee.Infrastructure.Services;

public class CurrentPrincipleService : ICurrentPrincipalService
{
	private readonly IHttpContextAccessor _accessor;
	public CurrentPrincipleService(IHttpContextAccessor httpContextAccessor)
	{
		_accessor = httpContextAccessor;
	}

	// get curent userID
	public string? CurrentPrincipal
	{
		get
		{
			var identity = _accessor?.HttpContext?.User.Identity as ClaimsIdentity;

			if (identity == null || !identity.IsAuthenticated) return null;

			var claims = identity.Claims;

			var id = claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value ?? null;

			return id;
		}
	}

	public long? CurrentSubjectId => CurrentPrincipal != null ? long.Parse(CurrentPrincipal) : null;
}
