using System.ComponentModel;

namespace PetCoffee.Application.Common.Enums;

public enum ResponseCode
{
	//Auth

	[Description("Có lỗi xảy ra")] CommonError,
	[Description("Lỗi định dạng dữ liệu")] ValidationError,
	[Description("Tài khoản không được xác thực")] Unauthorized,
	[Description("Tài khoản không được phép truy cập tài nguyên này")] Forbidden,
	[Description("Mã đã hết hạn")] OptExpired,
	[Description("Tài khoản Chưa kích hoạt")] AccountNotActived,
	[Description("Tài khoản đã kích hoạt")] AccountIsActived ,
	[Description("Tài khoản không tồn tại")] AccountNotExist,
	[Description("Tài khoản không có quyền")] PermissionDenied,

	[Description("Email đã tồn tại")] AccountIsExisted,
	[Description("Tài Khoản hoặc mật khẩu sai")] LoginFailed,
	[Description("Mật khẩu hiện tại bạn nhập sai vui lòng kiểm tra lại")] PassNotValid,
	//PostCategory
	[Description("Tên đã tồn tại")] PostCategoryIsExisted,
	[Description("Category Không tồn tại")] PostCategoryNotExisted,

    //Post
    [Description("Post không tồn tại")] PostNotExist,
    //shop
    [Description("Cửa hàng Không tồn tại")] ShopNotExisted,
	//TaxCode
	[Description("Mã Số thuế không tồn tại")] TaxCodeNotExisted,
	//pet
	[Description("Pet Không tồn tại")] PetNotExisted,

}