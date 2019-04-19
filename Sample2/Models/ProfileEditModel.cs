using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sample2.Models
{
	public class ProfileEditModel
	{
		[Required]
		[Display(Name = "UserName")]
		public string UserName { get; set; }

		[Required]
		[Display(Name = "Email")]
		public string Email { get; set; }
	}
}
