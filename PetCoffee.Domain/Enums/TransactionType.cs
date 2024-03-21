
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
}
