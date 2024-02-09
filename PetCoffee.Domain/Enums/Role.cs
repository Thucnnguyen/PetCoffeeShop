using System.ComponentModel;

namespace PetCoffee.Domain.Enums;

public enum Role
{
	[Description("Quản trị viên")]
	Admin = 0,

	[Description("Nhân viên cafe")]
	Staff = 1,

	[Description("Chủ cafe")]
	Manager = 2,

	[Description("Khách hàng")]
	Customer = 3
}
