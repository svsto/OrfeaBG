namespace OrfeaBG.Web.Areas.Product.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using OrfeaBG.Services.Product;
    using OrfeaBG.Services.Product.Models;
    using OrfeaBG.Web.Areas.Product.Models;
    using Services.Html;
    using System.IO;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using static Data.DataConstants;

    [Area("Product")]
    [Authorize(Roles = AdminArea)]

    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private IHostingEnvironment hostingEnvironment;
        private readonly IHtmlService html;

        public ProductController(
            IProductService productService,
            IHostingEnvironment hostingEnvironment,
            IHtmlService html)
        {
            this.productService = productService;
            this.hostingEnvironment = hostingEnvironment;
            this.html = html;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var model = new ProductListingViewModel
            {
                Products = await this.productService.AllProductsAsync(page),
                TotalProducts = await this.productService.TotalAsync(),
                CurrentPage = page
            };

            return View(model);
        }
        
        [AllowAnonymous]
        public async Task<IActionResult> All(int page = 1)
        {
            var model = new ProductListingViewModel
            {
                Products = await this.productService.AllProductsAsync(page),
                TotalProducts = await this.productService.TotalAsync(),
                CurrentPage = page
            };

            return View(model);
        }

        [HttpGet]
        [Route("product/add")]
        public IActionResult Add() => View();

        [HttpPost]
        [Route("product/add")]
        public async Task<IActionResult> Add(ProductAddViewModel product, IFormFile Image)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            product.Content = this.html.Sanitize(product.Content);

            var fileName = await SaveImage(Image);

            await this.productService.CreateAsync(product.Title, product.Content, fileName);

            TempData[WebConstants.TempDataSuccessMessageKey] = ($"Product {product.Title} successfuly added.");
            return RedirectToAction(nameof(ProductController.Index), new { page = 1 });
        }

        [HttpGet]
        [Route("product/edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await this.productService.ByIdAsync(id);

            if (product == null)
            {
                return BadRequest();
            }

            return View(product);
        }

        [HttpPost]
        [Route("product/edit")]
        public async Task<IActionResult> Edit(ProductListingServiceModel product, IFormFile Image)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            product.Content = this.html.Sanitize(product.Content);

            if (Image != null)
            {
                product.ImageName = await SaveImage(Image);
            }


            await this.productService.EditAsync(product);

            TempData[WebConstants.TempDataSuccessMessageKey] = ($"Product {product.Title} successfuly updated.");
            return RedirectToAction(nameof(ProductController.Index), new { page = 1 });
        }

        [HttpGet]
        [Route("product/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await this.productService.ByIdAsync(id);

            if (product == null)
            {
                return BadRequest();
            }

            var productForDelete = new ProductDeleteServiceModel
            {
                Id = id,
                Title = product.Title
            };

            return View(productForDelete);
        }

        [HttpPost]
        [Route("product/delete/{id}")]
        public IActionResult DeleteConfirmed(ProductDeleteServiceModel product)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(ProductController.Delete), new { id = product.Id });
            }

            this.productService.DeleteAsync(product);

            TempData[WebConstants.TempDataSuccessMessageKey] = ($"Product {product.Title} successfuly deleted.");
            return RedirectToAction(nameof(ProductController.Index), new { page = 1 });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("product/{id}/view")]
        public async Task<IActionResult> Deteil(int id)
        {
            var product = await this.productService.ByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        private async Task<string> SaveImage(IFormFile Image)
        {
            var fileName = string.Empty;
            var articleImageName = string.Empty;

            if (Image != null && Image.Length > 0)
            {

                var file = Image;
                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "products");

                if (file.Length > 0)
                {
                    fileName = ContentDispositionHeaderValue.Parse
                        (file.ContentDisposition).FileName.Trim('"');

                    System.Console.WriteLine(fileName);
                    using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                        articleImageName = file.FileName;
                    }
                }
            }

            return articleImageName;
        }
    }
}
