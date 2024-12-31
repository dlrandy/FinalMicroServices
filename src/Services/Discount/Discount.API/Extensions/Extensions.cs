using System;
using Discount.API.Data;
using Npgsql;
using Microsoft.EntityFrameworkCore;

namespace Discount.API.Extensions
{
	public static class Extensions
	{
		public static IApplicationBuilder UseMigration(this IApplicationBuilder app)
		{
			using var scope = app.ApplicationServices.CreateScope();
			using var dbContext = scope.ServiceProvider.GetRequiredService<DiscountContext>();
			dbContext.Database.MigrateAsync();
			return app;
		}
	}
}

