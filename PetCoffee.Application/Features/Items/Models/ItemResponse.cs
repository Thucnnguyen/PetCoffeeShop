

namespace PetCoffee.Application.Features.Items.Models
{
    public class ItemResponse
    {
        public long ItemId { get; set; }
		public string? Icon { get; set; }
		public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }

    }
}
