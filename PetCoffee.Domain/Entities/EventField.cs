﻿

using System.ComponentModel.DataAnnotations;

namespace PetCoffee.Domain.Entities;

public class EventField
{
	[Key]
	public long Id { get; set; }
	public string Question { get; set; }
	public string Type { get; set; }
	public bool IsOptional { get; set; }
	public string? Answer { get; set; }

	public long EventId { get; set; }
	public Event Event { get; set; }

}
