using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Domain.Enums;

public enum PostStatus
{
	[Description("Bản nháp")]
	Draft = 0,
	[Description("Công khai")]
	Published = 1,
}
