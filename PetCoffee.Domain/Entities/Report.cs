
using PetCoffee.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCoffee.Domain.Entities;

[Table("Report")]
public class Report : BaseAuditableEntity
{
	[Key]
	public long Id { get; set; }

	public long? CommentId { get; set; }
	public Comment? Comment { get; set; }
	
	public long? PostID { get; set; } 
	public Post? Post { get; set; }

	public string? Reason { get; set; }
	public ReportStatus Status { get; set; }
	public ReportCategory ReportCategory { get; set; }

}
