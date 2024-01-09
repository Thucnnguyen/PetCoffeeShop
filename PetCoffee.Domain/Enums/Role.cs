using System.ComponentModel;

namespace PetCoffee.Domain.Enums;

public enum Role
{
	[Description("Quản trị viên")]
	Admin = 0,

	[Description("Nhân viên cafe")]
	staff = 1,

	[Description("Chủ cafe")]
	Manager = 2,

	[Description("Khách hàng")]
	Customer = 3
}
