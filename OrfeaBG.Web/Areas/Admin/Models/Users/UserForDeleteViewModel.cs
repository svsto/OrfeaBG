namespace OrfeaBG.Web.Areas.Admin.Models.Users
{
    using OrfeaBG.Common;
    using OrfeaBG.Data.Models;

    public class UserForDeleteViewModel:IMapFrom<User>
    {
        public string Id { get; set; } 

        public string UserName { get; set; }
    }
}
