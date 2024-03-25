using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Areas.Models;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.Product.Models;
using PetCoffee.Domain.Entities;
using PetCoffee.Domain.Enums;

namespace PetCoffee.Application.Features.Reservation.Models;

public class ReservationResponse : BaseAuditableEntityResponse
{
	public long Id { get; set; }
	public decimal TotalPrice { get; set; }
	public OrderStatus Status { get; set; }
	public decimal Discount { get; set; }
	public DateTimeOffset StartTime { get; set; }
	public DateTimeOffset EndTime { get; set; }
	public string? Note { get; set; }
	public string Code { get; set; }
	public string? Rate { get; set; }
	public string? Comment { get; set; }

	public long? AreaId { get; set; }
	public AreaResponse AreaResponse { get; set; }

	public int BookingSeat { get; set; }
	public IList<ProductForReservationResponse> Products { get; set; }
	public AccountForReservation? AccountForReservation { get; set; }

	public PetCoffeeShopResponse PetCoffeeShopResponse { get; set; }
	public decimal AmountRefund { get; set; }
}

public class AccountForReservation
{
	public string? FullName { get; set; }
	public string PhoneNumber { get; set; }
	public string Email { get; set; }
	public string? Address { get; set; }

}
