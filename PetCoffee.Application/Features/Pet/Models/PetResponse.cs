﻿using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Pet.Models;

public class PetResponse : BaseAuditableEntityResponse
{
    public long Id { get; set; }
    public string Name { get; set; }
    public int? BirthYear { get; set; }
    public double? Weight { get; set; }

    public string? Avatar { get; set; }
    public string? Backgrounds { get; set; }
    public string? Description { get; set; }
    public PetStatus PetStatus { get; set; }
    public PetType PetType { get; set; }
    public PetGender Gender { get; set; }
    public bool Spayed { get; set; } = false;
    public AreaResponseForPet Area { get; set; }
    public long PetCoffeeShopId { get; set; }
}
