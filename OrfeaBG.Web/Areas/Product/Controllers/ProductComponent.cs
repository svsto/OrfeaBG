namespace OrfeaBG.Web.Areas.Product.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using OrfeaBG.Services.Product;
    using OrfeaBG.Web.Areas.Product.Models;
    using System.Threading.Tasks;

    public class ProductViewComponent : ViewComponent
    {
        private readonly IProductService productService;

        public ProductViewComponent(IProductService productService)
        {
            this.productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int take)
        {
            var model = new ProductListingViewModel
            {
                Products = await this.productService.LatestProductsAsync(take)
            };

            return View(model);
        }
    }
}
