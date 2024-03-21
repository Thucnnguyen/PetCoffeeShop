using System.ComponentModel;

namespace PetCoffee.Domain.Enums;

public enum PostStatus
{
    [Description("kích hoạt")]
    Active = 1,

    [Description("không hoạt động")]
    Intactive = 0,
}
