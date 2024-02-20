using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Domain.Enums;

public enum PostStatus
{
	[Description("kích hoạt")]
	Active = 1,

	[Description("không hoạt động")]
	Intactive = 0,
}
