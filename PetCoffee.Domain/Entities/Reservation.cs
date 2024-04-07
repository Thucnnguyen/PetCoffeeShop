using PetCoffee.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace PetCoffee.Domain.Entities;
[Table("Order")]
public class Reservation : BaseAuditableEntity
{
	[Key]
	public long Id { get; set; }
	public decimal TotalPrice { get; set; }
	public OrderStatus Status { get; set; }
	public decimal? Discount { get; set; }
	public DateTimeOffset StartTime { get; set; }
	public DateTimeOffset EndTime { get; set; }
	public string? Note { get; set; }
	public string Code { get; set; }
	public long? Rate { get; set; }
	public string? Comment { get; set; }

	public int BookingSeat { get; set; }

	public long AreaId { get; set; }
	public Area Area { get; set; }

	public bool IsTotallyRefund { get; set; } = false;

    [InverseProperty(nameof(Transaction.Reservation))]
	public IList<Transaction> Transactions { get; set; } = new List<Transaction>();

    [InverseProperty(nameof(ReservationProduct.Reservation))]
	public IList<ReservationProduct> ReservationProducts { get; set; } = new List<ReservationProduct>();




}
