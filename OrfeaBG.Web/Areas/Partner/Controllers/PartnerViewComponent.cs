namespace OrfeaBG.Web.Areas.Product.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using OrfeaBG.Services.Partner;
    using OrfeaBG.Web.Areas.Partner.Models;
    using System.Threading.Tasks;

    public class PartnerViewComponent : ViewComponent
    {
        private readonly IPartnerService partnerService;

        public PartnerViewComponent(IPartnerService partnerService)
        {
            this.partnerService = partnerService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new PartnerListingViewModel
            {
                Partners = await this.partnerService.TakeAllProductsAsync()
            };

            return View(model);
        }
    }
}
