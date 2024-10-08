using System.ComponentModel;

namespace PetCoffee.Application.Common.Enums;

public enum ResponseCode
{
	//Area
	[Description("Tầng không tồn tại")] AreaNotExist,
	[Description("Tầng không đủ chỗ ngồi để đặt")] AreaInsufficientSeating,
	[Description("Tầng đã tồn tại trong quán")] AreaIsExist,
	[Description("Phải có tầng trước")] NotHasPreviousArea,

	//Comment
	[Description("Comment không tồn tại")] CommentNotExist,
	//Item

	[Description("Quà tặng đã tồn tại")] ItemNameIsExisted,
	[Description("Quà tặng không tồn tại")] ItemNotExist,
	[Description("Quà tặng này không thể mua được nữa!")] CannotBuyItem,

	//Auth

	[Description("Có lỗi xảy ra")] CommonError,
	[Description("Lỗi định dạng dữ liệu")] ValidationError,
	[Description("Tài khoản không được xác thực")] Unauthorized,
	[Description("Tài khoản không được phép truy cập tài nguyên này")] Forbidden,
	[Description("Mã đã hết hạn")] OptExpired,
	[Description("Tài khoản Chưa kích hoạt")] AccountNotActived,
	[Description("Tài khoản đã kích hoạt")] AccountIsActived,
	[Description("Tài khoản không tồn tại")] AccountNotExist,
	[Description("Tài khoản không có quyền")] PermissionDenied,

	[Description("Email đã tồn tại")] AccountIsExisted,
	[Description("Tài Khoản hoặc mật khẩu sai")] LoginFailed,
	[Description("Tài Khoản bị vô hiệu hóa")] AccountIsInactive,
	[Description("Mật khẩu hiện tại bạn nhập sai vui lòng kiểm tra lại")] PassNotValid,
	//PostCategory
	[Description("Tên đã tồn tại")] PostCategoryIsExisted,
	[Description("Category Không tồn tại")] PostCategoryNotExisted,

	//shop
	[Description("Cửa hàng Không tồn tại")] ShopNotExisted,
	[Description("Bạn đang có một yêu cầu tạo của hàng rồi")] HasShopRequest,
	[Description("Cửa hàng đã hết thời gian sử dụng vui lòng mua thêm gói để sử dụng")] ShopIsExpired,
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
	[Description("Sự kiện Không thể Xóa vì đã có người tham gia")] EventCannotDeleted,
	//submitEvent
	[Description("Gửi form tham gia event không hợp lệ vui lòng xem lại")] SubmittingEventNotCorrectForm,
	[Description("Form tham gia của bạn không tồn tại")] SubmittingEventNotExist,
	[Description("Bạn đã tham gia event rồi")] SubmittingEventIsExist,
	[Description("Sự kiện đã đủ người tham gia")] EventEnoughPaticipant,
	[Description("Sự kiện đã hủy không thể tham gia được")] EventIsClosed,
	//EventField
	[Description("Trường này không tồn tại")] EventFieldIsNotExist,

	//firebaseToken
	[Description("Token của firbase không đúng")] FirebaseTokenNotValid,

	//Notification
	[Description("Không tìm thấy thông báo")] NotificationErrorNotFound,
	[Description("Trạng thái hiện tại của Thông báo không cho phép thực hiện thao tác này")] NotificationErrorInvalidStatus,
	//Transaction
	[Description("Không tìm thấy giao dịch")] TransactionNotFound,

	//wallet
	[Description("Trong ví không đủ tiền để thực hiện, vui lòng nạp thêm")] NotEnoughBalance,
	[Description("Quà tặng trong ví của bạn không đủ vui lòng kiểm trả lại")] ItemInWalletNotEnough,
	[Description("Ví Không tồn tại")] WalletNotAvailable,

	//Report
	[Description("Báo cáo không tồn tại")] ReportNotExisted,
	[Description("Bạn không nên báo cáo bình luận hoặc bài viết của mình")] NotReportYourself,


	//Reservation

	[Description("Đơn hàng không tồn tại")] ReservationNotExist,
	[Description("Đơn hàng không tồn tại hoặc đã huỷ")] ReservationNotExistOrIsRefunded,

	//Table

	[Description("Table không tồn tại")] TableNotExist,

	//PackagePromotion

	[Description("Gói không tồn tại")] PackageNotExist,
	[Description("Thời hạn của gói đã tồn tại nha")] PackageisExisted,
	[Description("Tên của gói đã tồn tại nha")] PackageNameIsExisted,


	//Product

	[Description("Sản phẩm không tồn tại")] ProductNotExist,

	[Description("Đơn hàng đã quá hạn")] ExpiredReservation,

	[Description("Sản phẩm không tồn tại trong đơn hàng")] ProductNotExistInReservation,


	//Promotion
	[Description("Khuyến mãi không tồn tại")] PromotionNotExisted,
	[Description("Bạn đã sử dụng Khuyến mãi rồi")] PromotionWasUsed,



}