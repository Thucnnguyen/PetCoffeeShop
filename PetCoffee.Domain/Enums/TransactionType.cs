﻿
using System.ComponentModel;

namespace PetCoffee.Domain.Enums;

public enum TransactionType
{
	[Description("Thanh toán đơn hàng")]
	Checkout = 0,

	[Description("Đặt chỗ đơn hàng")]
	Reserve = 1,

	[Description("Nạp tiền vào ví")]
	TopUp = 2,

	[Description("Tặng quà cho thú cưng")]
	Donate = 3,
	[Description("Mua Quà Tặng")]
	BuyItem = 4,

	[Description("Hoàn tiền đặt chỗ")]
	Refund = 5,
	[Description("Thêm đồ ăn thức uống")]
	AddProducts = 6,
	[Description("Mua gói")]
	Package = 7,
	[Description("Hủy đặt đồ uống")]
	RemoveProducts = 8,
}

