using System.ComponentModel;

namespace PetCoffee.Application.Common.Enums;

public enum ResponseCode
{
	//Auth

	[Description("Có lỗi xảy ra")] CommonError = 1,
	[Description("Lỗi định dạng dữ liệu")] ValidationError = 2,
	[Description("Tài khoản không được xác thực")] Unauthorized = 3,
	[Description("Tài khoản không được phép truy cập tài nguyên này")] Forbidden = 4,
	[Description("Mã đã hết hạn")] OptExpired = 5,
	[Description("Tài khoản đã kích hoạt")] AccountIsActived = 6,
	[Description("Tài khoản không tồn tại")] AccountNotExist = 7,

	[Description("Email đã tồn tại")] AccountIsExisted = 8,
	[Description("Tài Khoản hoặc mật khẩu sai")] LoginFailed = 9,

	//PostCategory
	[Description("Tên đã tồn tại")] PostCategoryIsExisted = 10,
	[Description("Category Không tồn tại")] PostCategoryNotExisted = 11,

	//Post

	//shop
	[Description("Cửa hàng Không tồn tại")] ShopNotExisted = 12,
	//pet
	[Description("Pet Không tồn tại")] PetNotExisted = 13,

}