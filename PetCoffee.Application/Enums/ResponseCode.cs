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
	[Description("Tài khoản Chưa kích hoạt")] AccountNotActived = 6,
	[Description("Tài khoản đã kích hoạt")] AccountIsActived = 7,
	[Description("Tài khoản không tồn tại")] AccountNotExist = 8,
	[Description("Tài khoản không có quyền")] PermissionDenied = 9,

	[Description("Email đã tồn tại")] AccountIsExisted = 10,
	[Description("Tài Khoản hoặc mật khẩu sai")] LoginFailed = 11,
	[Description("Mật khẩu hiện tại bạn nhập sai vui lòng kiểm tra lại")] PassNotValid = 12,
	//PostCategory
	[Description("Tên đã tồn tại")] PostCategoryIsExisted = 13,
	[Description("Category Không tồn tại")] PostCategoryNotExisted = 14,

	//Post

	//shop
	[Description("Cửa hàng Không tồn tại")] ShopNotExisted = 15,
	//TaxCode
	[Description("Mã Số thuế không tồn tại")] TaxCodeNotExisted = 16,
	//pet
	[Description("Pet Không tồn tại")] PetNotExisted = 17,

}