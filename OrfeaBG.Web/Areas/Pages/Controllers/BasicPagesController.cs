namespace OrfeaBG.Web.Areas.Pages.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using OrfeaBG.Services.Pages;
    using OrfeaBG.Services.Pages.Models;
    using OrfeaBG.Services.Product;
    using OrfeaBG.Services.Product.Models;
    using OrfeaBG.Web.Areas.Pages.Models;
    using OrfeaBG.Web.Areas.Product.Models;
    using Services.Html;
    using System.IO;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using static Data.DataConstants;

    [Area("Pages")]
    [Authorize(Roles = AdminArea)]
    public class BasicPagesController:Controller
    {
        private readonly IBasicPagesService basicPagesService;
        private IHostingEnvironment hostingEnvironment;
        private readonly IHtmlService html;

        public BasicPagesController(
            IBasicPagesService basicPagesService,
            IHostingEnvironment hostingEnvironment,
            IHtmlService html)
        {
            this.basicPagesService = basicPagesService;
            this.hostingEnvironment = hostingEnvironment;
            this.html = html;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var model = new BasicPagesListingViewModel
            {
                Pages = await this.basicPagesService.AllPagesAsync(page),
                TotalBasicPages = await this.basicPagesService.TotalAsync(),
                CurrentPage = page
            };

            return View(model);
        }

        [HttpGet]
        [Route("page/add")]
        public IActionResult Add() => View();

        [HttpPost]
        [Route("page/add")]
        public async Task<IActionResult> Add(BasicPagesAddViewModel page)
        {
            if (!ModelState.IsValid)
            {
                return View(page);
            }

            page.Content = this.html.Sanitize(page.Content);

            await this.basicPagesService.CreateAsync(page.Title, page.Content);

            TempData[WebConstants.TempDataSuccessMessageKey] = ($"Page {page.Title} successfuly added.");
            return RedirectToAction(nameof(BasicPagesController.Index), new { page = 1 });
        }

        [HttpGet]
        [Route("page/edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var page = await this.basicPagesService.ByIdAsync(id);

            if (page == null)
            {
                return BadRequest();
            }

            return View(page);
        }

        [HttpPost]
        [Route("page/edit")]
        public async Task<IActionResult> Edit(BasicPagesListingServiceModel page)
        {
            if (!ModelState.IsValid)
            {
                return View(page);
            }

            page.Content = this.html.Sanitize(page.Content);

            await this.basicPagesService.EditAsync(page);

            TempData[WebConstants.TempDataSuccessMessageKey] = ($"Page {page.Title} successfuly updated.");
            return RedirectToAction(nameof(BasicPagesController.Index), new { page = 1 });
        }

        [HttpGet]
        [Route("page/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var page = await this.basicPagesService.ByIdAsync(id);

            if (page == null)
            {
                return BadRequest();
            }

            var pageForDelete = new BasicPagesDeleteServiceModel
            {
                Id = id,
                Title = page.Title
            };

            return View(pageForDelete);
        }

        [HttpPost]
        [Route("page/delete/{id}")]
        public IActionResult DeleteConfirmed(BasicPagesDeleteServiceModel page)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(BasicPagesController.Delete), new { id = page.Id });
            }

            this.basicPagesService.DeleteAsync(page);

            TempData[WebConstants.TempDataSuccessMessageKey] = ($"Page {page.Title} successfuly deleted.");
            return RedirectToAction(nameof(BasicPagesController.Index), new { page = 1 });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("page/{id}/view")]
        public async Task<IActionResult> Deteil(int id)
        {
            var page = await this.basicPagesService.ByIdAsync(id);

            if (page == null)
            {
                return NotFound();
            }

            return View(page);
        }
    }
}
