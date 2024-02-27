using System.ComponentModel;

namespace PetCoffee.Application.Common.Enums;

public enum ResponseCode
{
    //Area

    [Description("Area không tồn tại")] AreaNotExist,

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

    //shop
    [Description("Cửa hàng Không tồn tại")] ShopNotExisted,
	//TaxCode
	[Description("Mã Số thuế không tồn tại")] TaxCodeNotExisted,
	//pet
	[Description("Pet Không tồn tại")] PetNotExisted,
	//Post
	[Description("Bài đăng Không tồn tại")] PostNotExisted,
	//Comment
	[Description("Bình luận Không tồn tại")] CommentNotExisted,
	//follow
	[Description("Theo dõi Không tồn tại")] FollowNotExisted,
	//moment
	[Description("Khoảnh khắc Không tồn tại")] MomentNotExisted,
	//event
	[Description("Sự kiện Không tồn tại")] EventNotExisted,


}