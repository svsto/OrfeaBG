namespace OrfeaBG.Services.Admin.Models
{
    using Data.Models;
    using OrfeaBG.Common;
    using System.Collections.Generic;

    public class AdminUserListingServiceModel : IMapFrom<User>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public IEnumerable<string> Roles { get; set; } = new List<string>();
    }
}
