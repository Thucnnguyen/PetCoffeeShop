using System.ComponentModel.DataAnnotations;

namespace PetCoffee.Domain.Entities
{
	public class ReservationProduct
	{
		[Key]
        public long Id { get; set; }
        public long ReservationId { get; set; }
		public Reservation Reservation { get; set; }

		public long ProductId { get; set; }
		public Product Product { get; set; }

		public int TotalProduct { get; set; }
		public decimal ProductPrice { get; set; }

	}
}
