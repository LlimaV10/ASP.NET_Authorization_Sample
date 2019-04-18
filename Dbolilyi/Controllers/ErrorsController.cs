using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Dbolilyi.Controllers
{
    public class ErrorsController : Controller
    {
		[HttpGet]
		public IActionResult Index(int? code)
		{
			ViewData["code"] = code;
			ViewData["IsAuthenticated"] = User.Identity.IsAuthenticated;
			ViewData["LoggedUser"] = User.Identity.Name;
			return View();
		}
	}
}