using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Domain.Enums;

public enum AccountStatus
{
	[Description("Đang hoạt động")]
	Active = 0,

	[Description("Ngưng hoạt động")]
	Inactive = 1,

	[Description("Mới khởi tạo")]
	Verifying = 2
}
