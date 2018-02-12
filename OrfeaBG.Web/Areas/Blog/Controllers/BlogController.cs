using OrfeaBG.Data;

namespace OrfeaBG.Web.Areas.Blog.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using OrfeaBG.Data.Models;
    using OrfeaBG.Services.Blog;
    using OrfeaBG.Services.Blog.Models;
    using OrfeaBG.Web.Areas.Blog.Models;
    using Services.Html;
    using System.IO;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using static DataConstants;

    [Area(BlogArea)]
    [Authorize(Roles = "Admin, BlogAuthor")]

    public class BlogController:Controller
    {
        private readonly IBlogService blogService;
        private readonly UserManager<User> userManager;
        private IHostingEnvironment hostingEnvironment;
        private readonly IHtmlService html;

        public BlogController(
            IBlogService blogService, 
            UserManager<User> userManager, 
            IHostingEnvironment hostingEnvironment,
            IHtmlService html)
        {
            this.blogService = blogService;
            this.userManager = userManager;
            this.hostingEnvironment = hostingEnvironment;
            this.html = html;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var model = new BlogListingViewModel
            {
                Articles = await this.blogService.AllArtilesAsync(page),
                TotalArticles = await this.blogService.TotalAsync(),
                CurrentPage = page
            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> All(int page = 1)
        {
            var model = new BlogListingViewModel
            {
                Articles = await this.blogService.AllArtilesAsync(page),
                TotalArticles = await this.blogService.TotalAsync(),
                CurrentPage = page
            };
            return View(model);
        }

        [HttpGet]
        [Route("blog/add")]
        public IActionResult Add() => View();

        [HttpPost]
        [Route("blog/add")]
        public async Task<IActionResult> Add(BlogAddViewModel article, IFormFile Image)
        {
            if (!ModelState.IsValid)
            {
                return View(article);
            }

            article.Content = this.html.Sanitize(article.Content);

            var fileName = await SaveImage(Image);

            var userId = this.userManager.GetUserId(User);

            await this.blogService.CreateAsync(article.Title, article.Content, fileName, article.LinkFrom, userId);

            TempData[WebConstants.TempDataSuccessMessageKey] = ($"Article {article.Title} successfuly added.");
            return RedirectToAction(nameof(BlogController.Index),new { page = 1 });
        }

        [HttpGet]
        [Route("blog/edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var article = await this.blogService.ByIdAsync(id);

            if (article == null)
            {
                return BadRequest();
            }

            return View(article);
        }

        [HttpPost]
        [Route("blog/edit")]
        public async Task<IActionResult> Edit(ArticleServiceModel article, IFormFile Image)
        {
            if (!ModelState.IsValid)
            {
                return View(article);
            }

            article.Content = this.html.Sanitize(article.Content);

            if (Image!=null)
            {
                article.ImageName = await SaveImage(Image);
            }
           

            await this.blogService.EditAsync(article);

            TempData[WebConstants.TempDataSuccessMessageKey] = ($"Article {article.Title} successfuly updated.");
            return RedirectToAction(nameof(BlogController.Index), new { page = 1 });
        }

        [HttpGet]
        [Route("blog/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await this.blogService.ByIdAsync(id);

            if (article ==null)
            {
                return BadRequest();
            }

            var articleForDelete = new ArticleDeleteServiceModel
            {
                Id = id,
                Title = article.Title
            };

            return View(articleForDelete);
        }

        [HttpPost]
        [Route("blog/delete/{id}")]
        public IActionResult DeleteConfirmed(ArticleDeleteServiceModel article)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(BlogController.Delete), new { id = article.Id });
            }

            this.blogService.DeleteAsync(article);

            TempData[WebConstants.TempDataSuccessMessageKey] = ($"Article {article.Title} successfuly deleted.");
            return RedirectToAction(nameof(BlogController.Index), new { page = 1 });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("blog/{id}/view")]
        public async Task<IActionResult> Deteil(int id)
        {
            var article = await this.blogService.ByIdAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }
        private async Task<string> SaveImage(IFormFile Image)
        {
            var fileName = string.Empty;
            var articleImageName = string.Empty;

            if (Image != null && Image.Length > 0)
            {

                var file = Image;
                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "blog");

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
