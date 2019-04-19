using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Sample2.Controllers
{
    public class ErrorController : Controller
    {
		[HttpGet]
		public IActionResult Index(int? code)
		{
			ViewData["code"] = code;
			return View();
		}
	}
}