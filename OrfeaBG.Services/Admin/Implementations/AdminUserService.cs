namespace OrfeaBG.Services.Admin.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using OrfeaBG.Data.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.Rendering;

    public class AdminUserService : IAdminUserService
    {
        private readonly OrfeaBGDbContext db;
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AdminUserService(
            OrfeaBGDbContext db, 
            UserManager<User> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<UserEditServiceModel> GetByIdAsync(string id)
            => await this.db
                    .Users.Where(u => u.Id == id)
                    .ProjectTo<UserEditServiceModel>()            
                    .FirstAsync();


        public void Add(UserRegisterServicesModel user)
        {
            var newUser = new User
            {
                Email = user.Email,
                UserName = user.Name
            };

            Task
                .Run(async () =>
                {
                    await userManager.CreateAsync(newUser, user.Password);
                }).Wait();

            foreach (var role in user.Role)
            {
                Task
                .Run(async () =>
                {
                    await userManager.AddToRoleAsync(newUser, role);
                }).Wait();
            }
        }

        

        public void Delete(User user)
        {
            db.Users.Remove(user);
            db.SaveChanges();
        }

        public async Task<IEnumerable<AdminUserListingServiceModel>> AllAsync()
            => await this.db
                .Users
                .ProjectTo<AdminUserListingServiceModel>()
                .ToListAsync();

        public void Edit(UserEditServiceModel userModel, User user)
        {
            user.Email = userModel.Email;
            user.UserName = userModel.UserName;

            if (userModel.Role != null && userModel.Role.Count()>0)
            {
                var allRoles = OrfeaBGRoles();

                foreach (var role in allRoles)
                {
                    if (userModel.Role.Contains(role.Text))
                    {
                        Task
                            .Run(async () =>
                            {
                                await userManager.AddToRoleAsync(user, role.Text);
                            }).Wait();
                    }
                    else
                    {
                        Task
                            .Run(async () =>
                            {
                                await userManager.RemoveFromRoleAsync(user, role.Text);
                            }).Wait();
                    }
                }
            }
            

            if (userModel.Password != null)
            {
                Task
                .Run(async () =>
                {
                    await this.userManager.ChangePasswordAsync(user, user.PasswordHash, userModel.Password);
                }).Wait();
            }

            Task
                .Run(async () =>
                {
                    await this.userManager.UpdateAsync(user);
                }).Wait();
        }

        public List<SelectListItem> OrfeaBGRoles() => this.roleManager
                .Roles
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                })
                .ToList();
    }
}
