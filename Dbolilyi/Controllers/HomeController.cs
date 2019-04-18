using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dbolilyi.Controllers
{
    public class HomeController : Controller
    {
		//[Authorize]
		public IActionResult Index()
        {
			   //return Content(User.Identity.Name);//return View();
			   ViewData["IsAuthenticated"] = User.Identity.IsAuthenticated;
			ViewData["LoggedUser"] = User.Identity.Name;
			
			
			return View();
		}
    }
}