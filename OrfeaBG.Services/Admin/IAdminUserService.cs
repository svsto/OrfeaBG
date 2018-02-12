namespace OrfeaBG.Services.Admin
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Models;
    using OrfeaBG.Data.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAdminUserService
    {
        void Add(UserRegisterServicesModel user);

        Task<UserEditServiceModel> GetByIdAsync(string id);

        void Edit(UserEditServiceModel userModel, User user );

        void Delete(User user);

        Task<IEnumerable<AdminUserListingServiceModel>> AllAsync();

        List<SelectListItem> OrfeaBGRoles();
    }
}
