

namespace PetCoffee.Domain.Entities
{
	public class AccountPromotion
	{
	
		public long PromotionId { get; set; }
		public long AccountId { get; set; }

		public Promotion Promotion { get; set; }	
		public Account Account	{ get; set; }
	}
}
