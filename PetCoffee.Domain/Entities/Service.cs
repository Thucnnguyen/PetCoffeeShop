using EntityFrameworkCore.Projectables;
using LockerService.Domain.Entities;
using PetCoffee.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;
[Table("Service")]
public class Service : BaseAuditableEntity
{
	[Key]
	public long Id { get; set; }

	public string Namne {  get; set; }
	public string? Description { get; set; }
	public decimal Price { get; set; }
	public decimal Rate { get; set; }
	//public string Type {  get; set; }  
	public string? Image {  get; set; }
	public ServiceStatus Status { get; set; }
	public long? PetCafeShopId { get; set; }
	public PetCafeShop? PetCafeShop { get; set; }
	[Projectable]
	public bool IsActive => ServiceStatus.Active.Equals(Status);
}
