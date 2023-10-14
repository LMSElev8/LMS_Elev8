using Microsoft.AspNetCore.Identity;

namespace LMS_Project.Models
{
    public class UserRoleInit
    {
        public static async Task InitAsync(IServiceProvider service)
        {
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = service.GetRequiredService<UserManager<AppUser>>();

            string[] roleNames = {"Admin", "User", "Instructor"};

            IdentityResult roleResult;

            foreach(var role in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(role);
                
                if(!roleExist){
                    
                    roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var email = "admin@admin.com";
            var password = "Pa$$w0rd";

            if(userManager.FindByEmailAsync(email).Result == null){

                AppUser admin = new()
                {
                    Email = email,
                    UserName = email
                };

                IdentityResult result = await userManager.CreateAsync(admin, password);
                if(result.Succeeded)
                {
                    userManager.AddToRoleAsync(admin,"Admin").Wait();
                }

            }

        }
    }
}