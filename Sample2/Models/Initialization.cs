using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample2.Models
{
	public class Initialization
	{
		public static async Task RolesAndAdminInitialization(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
		{
			string adminLogin = "SuperAdmin";
			string adminEmail = "SuperAdmin@no.mail";
			string password = "Admin!!1";
			if (await roleManager.FindByNameAsync("admin") == null)
			{
				await roleManager.CreateAsync(new IdentityRole("admin"));
			}
			if (await roleManager.FindByNameAsync("SuperAdmin") == null)
			{
				await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
			}
			if (await userManager.FindByNameAsync(adminLogin) == null)
			{
				User admin = new User { Email = adminEmail, UserName = adminLogin };
				IdentityResult result = await userManager.CreateAsync(admin, password);
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(admin, "admin");
					await userManager.AddToRoleAsync(admin, "SuperAdmin");
				}
			}
		}
	}
}
