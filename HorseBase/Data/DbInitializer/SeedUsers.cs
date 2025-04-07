using HorseBase.Models;
using HorseBase.Models.ViewModels.User;
using Microsoft.AspNetCore.Identity;

namespace HorseBase.Data.DbInitializer
{
    public static class SeedUsers
    {

        public static async void Seed(IApplicationBuilder applicationBuilder, ApplicationDbContext context)
        {
            using (var scope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetService<UserManager<User>>();
                await CreateUsers(userManager, context);
            }
        }
        public static async Task CreateUsers(UserManager<User> userManager, ApplicationDbContext context)
        {
            if (context.Users.Where(x => x.UserName == "Admin").FirstOrDefault() != null)
            {
                return;
            }
            User admin = new User()
            {
                UserName = "Admin",
                Email = "admin@admin.admin",
                FirstName = "Admin",
                MiddleName = "Adminov",
                LastName = "Adminov",
                IsActive = true
            };

			var result = await userManager.CreateAsync(admin, "123%Ab");
            await userManager.AddToRoleAsync(admin, "Admin");

			var users = new List<User>
				{
					new User { UserName = "michael.brown", Email = "michael.brown@gmail.com", FirstName = "Michael", MiddleName = "James", LastName = "Brown", IsActive = true },
					new User { UserName = "sophia.taylor", Email = "sophia.taylor@yahoo.com", FirstName = "Sophia", MiddleName = "Anne", LastName = "Taylor", IsActive = true },
					new User { UserName = "david.miller", Email = "david.miller@outlook.com", FirstName = "David", MiddleName = "Joseph", LastName = "Miller", IsActive = true },
					new User { UserName = "olivia.wilson", Email = "olivia.wilson@gmail.com", FirstName = "Olivia", MiddleName = "Grace", LastName = "Wilson", IsActive = true },
					new User { UserName = "william.anderson", Email = "william.anderson@hotmail.com", FirstName = "William", MiddleName = "Alexander", LastName = "Anderson", IsActive = true },
					new User { UserName = "ava.martinez", Email = "ava.martinez@icloud.com", FirstName = "Ava", MiddleName = "Marie", LastName = "Martinez", IsActive = true },
					new User { UserName = "james.thomas", Email = "james.thomas@gmail.com", FirstName = "James", MiddleName = "Daniel", LastName = "Thomas", IsActive = true },
					new User { UserName = "mia.harris", Email = "mia.harris@yahoo.com", FirstName = "Mia", MiddleName = "Rose", LastName = "Harris", IsActive = true },
					new User { UserName = "benjamin.clark", Email = "benjamin.clark@outlook.com", FirstName = "Benjamin", MiddleName = "John", LastName = "Clark", IsActive = true },
					new User { UserName = "emma.rodriguez", Email = "emma.rodriguez@gmail.com", FirstName = "Emma", MiddleName = "Sophia", LastName = "Rodriguez", IsActive = true }
				};

			foreach (var user in users)
			{
				var resultUser = await userManager.CreateAsync(user, "Ab!2");
				await userManager.AddToRoleAsync(user, "User");
			}

		}
    }
}