using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sample2.Models
{
	public class PasswordEditModel
	{
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Старый пароль")]
		public string OldPassword { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[Display(Name = "Пароль")]
		public string Password { get; set; }

		[Required]
		[Compare("Password", ErrorMessage = "Пароли не совпадают")]
		[DataType(DataType.Password)]
		[Display(Name = "Подтвердить пароль")]
		public string PasswordConfirm { get; set; }
	}
}
