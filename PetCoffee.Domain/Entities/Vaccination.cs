
using EntityFrameworkCore.Projectables;
using PetCoffee.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PetCoffee.Domain.Entities;
public class Vaccination
{
	[Key]
	public long Id { get; set; }	
	public DateTime VaccinationDate { get; set; }
	public DateTime ExpireTime { get; set; }
	public VaccinationType VaccinationType { get; set; }
	public string? PhotoEvidence { get; set; }

	public long PetId { get; set; }
	public Pet Pet { get; set; }

	[Projectable]
	public bool IsVerified => PhotoEvidence != null;
}
