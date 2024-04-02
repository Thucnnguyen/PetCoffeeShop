
namespace PetCoffee.Application.Common.Constantsl;

public class EmailConstant
{
	public static string EmailSubject = "Mã Xác Minh Của Bạn";
	public static string InactiveEmailSubject = "Tài khoản của bạn đã bị vô hiệu hóa";
	public static string EmailForm = "Chào bạn {0},\r\n\r\nCảm ơn bạn đã chọn dịch vụ của chúng tôi! Để hoàn tất quá trình đăng ký và tăng cường bảo mật cho tài khoản của bạn, chúng tôi yêu cầu bạn xác minh địa chỉ email.\r\n\r\nMã xác minh của bạn là: {1}\r\n\r\nVui lòng nhập mã này trên trang web của chúng tôi để hoàn tất quá trình xác minh. Nếu bạn không yêu cầu mã này hoặc có bất kỳ lo ngại nào, vui lòng liên hệ ngay với đội hỗ trợ của chúng tôi.\r\n\r\nCảm ơn bạn đã hợp tác.\r\n\r\nTrân trọng,\r\nPet Coffee Platform";
	public static string EmailOTPForm = "Chào bạn {0},\r\n\r\nMã xác minh để đổi mật khẩu của bạn là: {1}";
	public static string AcceptShop = "Chào bạn {0}, quán của bạn đã được kích hoạt bạn có thể vào app chung tôi để tiếp tục trải nghiệm";
	public static string RejectShop = "Chào bạn {0}, yêu cầu tạo quán của bạn đã bị từ chối vui lòng xem xét lại!";
	public static string InActiveShop = "Chào bạn {0}, Quán của bạn đã vô hiệu hóa nếu có thắc mắc gì hãy liên hệ với chúng tôi!";
	public static string InactiveCustomerAccount = "<!DOCTYPE html>\r\n<html>\r\n<head>\r\n    <style>\r\n        body {\r\n            font-family: Arial, sans-serif;\r\n            margin: 0;\r\n            padding: 0;\r\n            background-color: #f6f6f6;\r\n        }\r\n\r\n        .email-container {\r\n            width: 100%;\r\n            max-width: 600px;\r\n            margin: 0 auto;\r\n            padding: 20px;\r\n            background-color: #ffffff;\r\n            box-shadow: 0px 0px 10px 0px rgba(0,0,0,0.1);\r\n        }\r\n\r\n        .email-container h1 {\r\n            color: #333333;\r\n            margin-bottom: 15px;\r\n        }\r\n\r\n        .email-container p {\r\n            color: #666666;\r\n            line-height: 1.5;\r\n        }\r\n    </style>\r\n</head>\r\n<body>\r\n    <div class=\"email-container\">\r\n        <h1>Xin chào {0},</h1>\r\n        <p>Tài khoản của bạn đã bị vô hiệu hóa. Nếu bạn có bất kỳ ý kiến hoặc câu hỏi nào, vui lòng liên hệ với chúng tôi để được xem xét.</p>\r\n    </div>\r\n</body>\r\n</html>";
}
