using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sample2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample2.Data
{
	public class Sample2Context : IdentityDbContext<User>
	{
		public Sample2Context(DbContextOptions<Sample2Context> options)
			: base(options) {}
	}
}
