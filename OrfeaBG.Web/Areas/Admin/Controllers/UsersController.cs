namespace OrfeaBG.Web.Areas.Admin.Controllers
{
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using OrfeaBG.Data;
    using OrfeaBG.Services.Admin.Models;
    using OrfeaBG.Web.Areas.Admin.Models.Users;
    using OrfeaBG.Web.Controllers;
    using Services.Admin;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [Route("admin")]
    public class UsersController : BaseAdminController
    {
        private readonly OrfeaBGDbContext db;
        private readonly IAdminUserService users;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;

        public UsersController(
            OrfeaBGDbContext db,
            IAdminUserService users,
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            this.users = users;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        [Route("users/add")]
        public async Task<IActionResult> Add()
        {
            var roles = await OrfeaBGRoles();

            return View(new UserRegisterServicesModel { Roles = roles });
        }

        [HttpPost]
        [Route("users/add")]
        public IActionResult Add(UserRegisterServicesModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            user.Name = user.Email;
            this.users.Add(user);

            TempData[WebConstants.TempDataSuccessMessageKey] = ($"User {user.Name} successfully added.");
            return RedirectToAction(nameof(Users));
        }

        [HttpGet]
        [Route("users/edit/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            var userForEdit = await this.users.GetByIdAsync(id);

            if (userForEdit == null)
            {
                return BadRequest();
            }

            var roles = this.users.OrfeaBGRoles();
            userForEdit.Roles = roles;

            return View(userForEdit);
        }

        [HttpPost]
        [Route("users/edit/{id}")]
        public async Task<IActionResult> Edit(UserEditServiceModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var userForEdit = await this.userManager.FindByIdAsync(user.Id);

            if (userForEdit == null)
            {
                return BadRequest();
            }

            this.users.Edit(user, userForEdit);

            TempData[WebConstants.TempDataSuccessMessageKey] = ($"User {userForEdit.UserName} successfully updated.");

            return RedirectToAction(nameof(Users));
        }

        [HttpGet]
        [Route("users/delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);
            
            if (user == null)
            {
                return BadRequest();
            }

            var UserForDelete = new UserForDeleteViewModel
            {
                Id = user.Id,
                UserName = user.UserName
            };

            return View(UserForDelete);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(string Id)
        {
            if (!ModelState.IsValid)
            {
                return View("Delete", Id);
            }

            var UserForDeleteViewModel = await this.userManager.FindByIdAsync(Id);

            if (UserForDeleteViewModel==null)
            {
                return BadRequest();
            }

            TempData[WebConstants.TempDataSuccessMessageKey] = ($"User {UserForDeleteViewModel.UserName} successfully deleted.");
            this.users.Delete(UserForDeleteViewModel);

            return RedirectToAction(nameof(Users));
        }

        [Route("users")]
        public async Task<IActionResult> Users()
        {
            var users = await this.users.AllAsync();

            foreach (var user in users)
            {
                var currentUser = await this.userManager.FindByEmailAsync(user.Email);
                user.Roles = await this.userManager.GetRolesAsync(currentUser);
            }

            return View(users);
        }

        private async Task<List<SelectListItem>> OrfeaBGRoles()=> await this.roleManager
                .Roles
                .Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                })
                .ToListAsync();
    }
}
