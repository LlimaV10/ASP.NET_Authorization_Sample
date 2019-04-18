using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Dbolilyi.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Dbolilyi
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc();

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			   .AddCookie(options =>
				{
				   options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
			   });
			services.AddDbContext<DbolilyiContext>(options =>
		            options.UseSqlServer(Configuration.GetConnectionString("DbolilyiContext")));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseAuthentication();

			app.UseStaticFiles();

			app.UseStatusCodePagesWithRedirects("/Errors?code={0}");

			app.UseMvc(routes => {
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}"
					);
				//routes.MapRoute(
				//	"Error",
				//	 "{*url}",
				//	 new { controller = "Errors", action = "NotFound" }
				//	);
			});
			//app.Run(async (context) =>
			//{
			//	await context.Response.WriteAsync("Brrrrr");
			//});
		}
	}
}
