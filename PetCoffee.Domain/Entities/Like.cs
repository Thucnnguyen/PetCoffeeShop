using LockerService.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;
[Table("Like")]
public class Like : BaseAuditableEntity
{
	[Key]
	public long Id { get; set; }
	public long AccountId { get; set; }
	public long PostId { get; set; }
	public Account Account { get; set; }
	public Post Post { get; set; }
}
