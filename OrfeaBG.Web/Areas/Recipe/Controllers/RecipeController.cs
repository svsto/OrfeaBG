namespace OrfeaBG.Web.Areas.Recipe.Controllers
{
    using OrfeaBG.Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using OrfeaBG.Data.Models;
    using OrfeaBG.Services.Html;
    using OrfeaBG.Services.Recipe;
    using OrfeaBG.Services.Recipe.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using static OrfeaBG.Data.DataConstants;
    using OrfeaBG.Web.Areas.Recipe.Models;

    [Area(RecipeArea)]
    [Authorize(Roles = "Admin, Chef")]
    public class RecipeController: Controller
    {
        private readonly IRecipeService recipeService;
        private readonly UserManager<User> userManager;
        private IHostingEnvironment hostingEnvironment;
        private readonly IHtmlService html;

        public RecipeController(
            IRecipeService blogService,
            UserManager<User> userManager,
            IHostingEnvironment hostingEnvironment,
            IHtmlService html)
        {
            this.recipeService = blogService;
            this.userManager = userManager;
            this.hostingEnvironment = hostingEnvironment;
            this.html = html;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var model = new RecipeListingViewModel
            {
                Recipes = await this.recipeService.AllRecipesAsync(page),
                TotalRecipes = await this.recipeService.TotalAsync(),
                CurrentPage = page
            };
            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> All(int page = 1)
        {
            var model = new RecipeListingViewModel
            {
                Recipes = await this.recipeService.AllRecipesAsync(page),
                TotalRecipes = await this.recipeService.TotalAsync(),
                CurrentPage = page
            };
            return View(model);
        }

        [HttpGet]
        [Route("recipe/add")]
        public IActionResult Add() => View();

        [HttpPost]
        [Route("recipe/add")]
        public async Task<IActionResult> Add(RecipeServiceModel recipe, IFormFile Image)
        {
            if (!ModelState.IsValid)
            {
                return View(recipe);
            }

            recipe.Content = this.html.Sanitize(recipe.Content);

            var fileName = await SaveImage(Image);

            var userId = this.userManager.GetUserId(User);

            await this.recipeService.CreateAsync(recipe.Title, recipe.Content, fileName, userId);

            TempData[WebConstants.TempDataSuccessMessageKey] = ($"Recipe {recipe.Title} successfuly added.");
            return RedirectToAction(nameof(RecipeController.Index), new { page = 1 });
        }

        [HttpGet]
        [Route("recipe/edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var recipe = await this.recipeService.ByIdAsync(id);

            if (recipe == null)
            {
                return BadRequest();
            }

            return View(recipe);
        }

        [HttpPost]
        [Route("recipe/edit")]
        public async Task<IActionResult> Edit(RecipeEditServiceModel recipe, IFormFile Image)
        {
            if (!ModelState.IsValid)
            {
                return View(recipe);
            }

            recipe.Content = this.html.Sanitize(recipe.Content);

            if (Image != null)
            {
                recipe.ImageName = await SaveImage(Image);
            }


            await this.recipeService.EditAsync(recipe);

            TempData[WebConstants.TempDataSuccessMessageKey] = ($"Article {recipe.Title} successfuly updated.");
            return RedirectToAction(nameof(RecipeController.Index), new { page = 1 });
        }

        [HttpGet]
        [Route("recipe/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var recipe = await this.recipeService.ByIdAsync(id);

            if (recipe == null)
            {
                return BadRequest();
            }

            var recipeForDelete = new RecipeDeleteServiceModel
            {
                Id = id,
                Title = recipe.Title
            };

            return View(recipeForDelete);
        }

        [HttpPost]
        [Route("recipe/delete/{id}")]
        public IActionResult DeleteConfirmed(RecipeDeleteServiceModel article)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(RecipeController.Delete), new { id = article.Id });
            }

            this.recipeService.DeleteAsync(article);

            TempData[WebConstants.TempDataSuccessMessageKey] = ($"Recipe {article.Title} successfuly deleted.");
            return RedirectToAction(nameof(RecipeController.Index), new { page = 1 });
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("recipe/{id}/view")]
        public async Task<IActionResult> Deteil(int id)
        {
            var recipe = await this.recipeService.ByIdAsync(id);

            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        private async Task<string> SaveImage(IFormFile Image)
        {
            var fileName = string.Empty;
            var articleImageName = string.Empty;

            if (Image != null && Image.Length > 0)
            {

                var file = Image;
                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "recipes");

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
