using System.ComponentModel;


namespace PetCoffee.Domain.Enums;

public enum AccountLevel
{
    [Description("Bình thường")]
    Normal = 0,
    [Description("Bạc")]
    Silver = 1,
    [Description("Vàng")]
    Gold = 2,
    [Description("Bạch kim")]
    Platinum = 3,
    [Description("Kim cương")]
    Diamond = 4,
}
