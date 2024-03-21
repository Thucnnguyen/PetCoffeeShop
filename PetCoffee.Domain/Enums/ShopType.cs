using System.ComponentModel;

namespace PetCoffee.Domain.Enums;
public enum ShopType
{

    [Description("Cà phế mèo")]
    Cat = 0,

    [Description("Cà phế chó")]
    Dog = 1,

    [Description("Cà phế mèo và chó")]
    CatAndDog = 2,
}
