namespace PetCoffee.Domain.Entities
{
	public class InvoiceProduct
	{

		public long InvoiceId { get; set; }
		public Invoice Invoice { get; set; }

		public long ProductId { get; set; }
		public Product Product { get; set; }

		public int TotalProduct { get; set; }




	}
}
