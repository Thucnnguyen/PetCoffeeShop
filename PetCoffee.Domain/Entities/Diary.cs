﻿using PetCoffee.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace PetCoffee.Domain.Entities;

public class Diary : BaseAuditableEntity
{
	[Key]
	public long Id { get; set; }
	public string? Content { get; set; }
	public string? Image {  get; set; }
	public bool IsPublic { get; set; }
	public DiaryType DiaryType { get; set; }

	public long PetId { get; set; }
	public Pet Pet { get; set; }
}
