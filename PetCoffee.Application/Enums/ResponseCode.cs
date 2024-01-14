using System.ComponentModel;

namespace PetCoffee.Application.Common.Enums;

public enum ResponseCode
{
    [Description("Có lỗi xảy ra")] CommonError = 1,

	[Description("Lỗi định dạng dữ liệu")] ValidationError = 2,

	[Description("Tài khoản không được xác thực")] Unauthorized = 3,

	[Description("Tài khoản không được phép truy cập tài nguyên này")] Forbidden = 4,

	//Auth
	[Description("Email đã tồn tại")] AccountIsExisted = 5,

}