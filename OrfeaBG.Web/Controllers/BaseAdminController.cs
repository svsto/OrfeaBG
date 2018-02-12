namespace OrfeaBG.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using static OrfeaBG.Data.DataConstants;

    [Area(AdminArea)]
    [Authorize(Roles = AdminRole)]
    public class BaseAdminController:Controller
    {
    }
}
