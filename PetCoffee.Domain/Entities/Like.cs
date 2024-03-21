using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;
[Table("Like")]
public class Like : BaseAuditableEntity
{
    public long PostId { get; set; }
    public Post Post { get; set; }
}
