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

    //Vaccination
    [Description("Vaccination Không tồn tại")] VaccinationNotExisted,

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
	[Description("Sự kiện Không thể thay đổi vì đã quá thời gian bắt đầu")] EventCannotChanged,
	//submitEvent
	[Description("Gửi form tham gia event không hợp lệ vui lòng xem lại")] SubmittingEventNotCorrectForm,
	[Description("Bạn đã tham gia event rồi")] SubmittingEventIsExist,
	//EventField
	[Description("Trường này không tồn tại")] EventFieldIsNotExist,

	//firebaseToken
	[Description("Token của firbase không đúng")] FirebaseTokenNotValid,

}