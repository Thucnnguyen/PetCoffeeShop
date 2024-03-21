
using System.ComponentModel;

namespace PetCoffee.Domain.Enums;

public enum PaymentType
{
	[Description("Thanh toán đơn hàng")]
	Checkout = 0,

	[Description("Đặt chỗ đơn hàng")]
	Reserve = 1,

	[Description("Nạp tiền vào ví")]
	Deposit = 2,

	[Description("Tặng quà cho thú cưng")]
	Refund = 3,
}
