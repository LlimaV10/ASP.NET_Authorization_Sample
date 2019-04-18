﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dbolilyi.Models
{
	public class RoleInitializer
	{
		public static async Task InitializeAsync(UserManager<Dbolilyi.Models.User> userManager, RoleManager<IdentityRole> roleManager)
		{
			string adminLogin = "admin";
			string adminEmail = "admin@gmail.com";
			string password = Dbolilyi.Controllers.AccountController.getMd5Hash("admin");
			if (await roleManager.FindByNameAsync("admin") == null)
			{
				await roleManager.CreateAsync(new IdentityRole("admin"));
			}
			if (await roleManager.FindByNameAsync("user") == null)
			{
				await roleManager.CreateAsync(new IdentityRole("user"));
			}
			if (await userManager.FindByNameAsync(adminEmail) == null)
			{
				Dbolilyi.Models.User admin = new Dbolilyi.Models.User { Login = adminLogin, Email = adminEmail, Passwd = password };
				IdentityResult result = await userManager.CreateAsync(admin, password);
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(admin, "admin");
				}
			}
		}
	}
}
