using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dbolilyi.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dbolilyi.Controllers
{
    public class AccountController : Controller
    {
		private DbolilyiContext db;
		public AccountController(DbolilyiContext context)
		{
			db = context;
		}
		[HttpGet]
		public IActionResult Login()
		{
			if (User.Identity.IsAuthenticated)
				return RedirectToAction("Profile", "Account");
			ViewData["IsAuthenticated"] = User.Identity.IsAuthenticated;
			ViewData["LoggedUser"] = User.Identity.Name;
			return View();
		}
		
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginModel model)
		{
			if (User.Identity.IsAuthenticated)
				return RedirectToAction("Profile", "Account");
			if (ModelState.IsValid)
			{
				User user = await db.User.FirstOrDefaultAsync
					(u => u.Login == model.Login && u.Passwd == getMd5Hash(model.Password));
				if (user != null)
				{
					await Authenticate(model.Login); // аутентификация

					return RedirectToAction("Index", "Home");
				}
				ModelState.AddModelError("", "Некорректные логин и(или) пароль");
			}
			ViewData["IsAuthenticated"] = User.Identity.IsAuthenticated;
			ViewData["LoggedUser"] = User.Identity.Name;
			return View(model);
		}
		[HttpGet]
		public IActionResult Register()
		{
			if (User.Identity.IsAuthenticated)
				return RedirectToAction("Profile", "Account");
			ViewData["IsAuthenticated"] = User.Identity.IsAuthenticated;
			ViewData["LoggedUser"] = User.Identity.Name;
			return View();
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterModel model)
		{
			if (User.Identity.IsAuthenticated)
				return RedirectToAction("Profile", "Account");
			if (ModelState.IsValid)
			{
				User user = await db.User.FirstOrDefaultAsync(u => u.Login == model.Login);
				if (user == null)
				{
					// добавляем пользователя в бд
					string hashPasswd = getMd5Hash(model.Password);
					db.User.Add(new User { Login = model.Login,
						Email = model.Email, Passwd = hashPasswd });
					await db.SaveChangesAsync();

					await Authenticate(model.Login); // аутентификация

					return RedirectToAction("Index", "Home");
				}
				else
					ModelState.AddModelError("", "Пользователь с таким именем уже существует");
			}
			ViewData["IsAuthenticated"] = User.Identity.IsAuthenticated;
			ViewData["LoggedUser"] = User.Identity.Name;
			return View(model);
		}
		private async Task Authenticate(string userName)
		{
			// создаем один claim
			var claims = new List<Claim>
			{
				new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
			};
			// создаем объект ClaimsIdentity
			ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
			// установка аутентификационных куки
			await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
		}
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Home");
		}

		[Authorize]
		public async Task<IActionResult> Profile()
		{
			User user = await db.User.FirstOrDefaultAsync(u => u.Login == User.Identity.Name);
			if (user != null)
			{
				ViewData["IsAuthenticated"] = User.Identity.IsAuthenticated;
				ViewData["LoggedUser"] = User.Identity.Name;
				return View(user);
			}
			return RedirectToAction("Logout", "Account");
		}

		static public string getMd5Hash(string input)
		{ // Create a new instance of the MD5CryptoServiceProvider object.
			MD5 md5Hasher = MD5.Create(); // Convert the input string to a byte array and compute the hash.
			byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
			// Create a new Stringbuilder to collect the bytes // and create a string.
			StringBuilder sBuilder = new StringBuilder(); // Loop through each byte of the hashed data // and format each one as a hexadecimal string.
			for (int i = 0; i < data.Length; i++)
			{
				sBuilder.Append(data[i].ToString("x2"));
			}
			// Return the hexadecimal string.
			return sBuilder.ToString();
		}
	}
}