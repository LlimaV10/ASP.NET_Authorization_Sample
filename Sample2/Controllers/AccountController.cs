using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sample2.Models;

namespace Sample2.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;

		public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}
		[HttpGet]
		public IActionResult Register()
		{
			if (User.Identity.IsAuthenticated)
				return RedirectToAction("Profile", "Account");
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterModel model)
		{
			if (User.Identity.IsAuthenticated)
				return RedirectToAction("Profile", "Account");
			if (ModelState.IsValid)
			{
				User user = new User { Email = model.Email, UserName = model.Login };
				// добавляем пользователя
				var result = await _userManager.CreateAsync(user, model.Password);
				if (result.Succeeded)
				{
					// установка куки
					await _signInManager.SignInAsync(user, false);
					return RedirectToAction("Index", "Home");
				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
			}
			return View(model);
		}

		[HttpGet]
		public IActionResult Login(string returnUrl = null)
		{
			if (User.Identity.IsAuthenticated)
				return RedirectToAction("Profile", "Account");
			return View(new LoginModel { ReturnUrl = returnUrl });
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginModel model)
		{
			if (User.Identity.IsAuthenticated)
				return RedirectToAction("Profile", "Account");
			if (ModelState.IsValid)
			{
				var result =
					await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
				if (result.Succeeded)
				{
					// проверяем, принадлежит ли URL приложению
					if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
					{
						return Redirect(model.ReturnUrl);
					}
					else
					{
						return RedirectToAction("Index", "Home");
					}
				}
				else
				{
					ModelState.AddModelError("", "Неправильный логин и (или) пароль");
				}
			}
			return View(model);
		}
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		[Authorize(Roles = "admin")]
		public IActionResult EditUsers()
		{
			return View(_userManager.Users.ToList());
		}

		[Authorize(Roles = "admin")]
		public async Task<IActionResult> Delete(string id)
		{
			var user = await _userManager.FindByIdAsync(id);

			if (!await _userManager.IsInRoleAsync(user, "admin") &&
					!await _userManager.IsInRoleAsync(user, "SuperAdmin"))
				await _userManager.DeleteAsync(user);
			return RedirectToAction("EditUsers", "Account");
		}

		public IActionResult AccessDenied()
		{
			return View();
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> Profile()
		{
			var user = await _userManager.FindByNameAsync(User.Identity.Name);
			ProfileEditModel model = new ProfileEditModel { UserName = user.UserName, Email = user.Email };
			if (user != null)
				return View(model);
			return RedirectToAction("Logout", "Account");
		}
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> Profile(ProfileEditModel model)
		{
			var user = await _userManager.FindByNameAsync(User.Identity.Name);
			if (user == null)
				return RedirectToAction("Logout", "Account");
			if (ModelState.IsValid)
			{
				user.UserName = model.UserName;
				user.Email = model.Email;
				var result = await _userManager.UpdateAsync(user);
				if (result.Succeeded)
				{
					Claim claim = ((ClaimsIdentity)User.Identity).FindFirst("IsPersistent");
					bool IsPersistent = claim != null ? Convert.ToBoolean(claim.Value) : false;
					await _signInManager.SignOutAsync();
					await _signInManager.SignInAsync(user, IsPersistent);
					return RedirectToAction("Profile", "Account");
				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
			}
			return View(model);
		}
		[Authorize]
		[HttpGet]
		public IActionResult EditPassword()
		{
			return View();
		}
		[Authorize]
		[HttpPost]
		public async Task<IActionResult> EditPassword(PasswordEditModel model)
		{
			if (!ModelState.IsValid)
				return View(model);
			var user = await _userManager.FindByNameAsync(User.Identity.Name);
			if (user == null)
				return RedirectToAction("Logout", "Account");
			var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);
			if (result.Succeeded)
				return RedirectToAction("Index", "Home");
			else
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}
			return View(model);
		}
	}
}