using MediatR;
using System.Text.Json.Serialization;

namespace PetCoffee.Application.Features.Reservation.Commands;

public class UpdateProductOfBookingCommand : IRequest<bool>
{
	[JsonIgnore]
	public long OrderId { get; set; }

	public List<ProductForBooking> Products { get; set; }

}

public class ProductForBooking
{
    public long ProductId { get; set; }
    public int Quantity { get; set; }
}
