namespace OrfeaBG.Services.Recipe.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using OrfeaBG.Services.Recipe.Models;
    using OrfeaBG.Data;
    using OrfeaBG.Data.Models;
    using System.Linq;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    public class RecipeService : IRecipeService
    {
        private readonly OrfeaBGDbContext db;

        public RecipeService(OrfeaBGDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<RecipeListingServiceModel>> AllRecipesAsync(int page = 1)
            => await this.db.Recipes
                .OrderByDescending(a => a.PublishDate)
                .Skip((page - 1) * ServiceConstants.PageSize)
                .Take(ServiceConstants.PageSize)
                .ProjectTo<RecipeListingServiceModel>()
                .ToListAsync();

        public async Task<RecipeServiceModel> ByIdAsync(int id)
            => await this.db
                .Recipes
                .Where(a => a.Id == id)
                .ProjectTo<RecipeServiceModel>()
                .FirstOrDefaultAsync();

        
        public async Task CreateAsync(string title, string content, string imageName, string authorId)
        {
            var recipe = new Recipe
            {
                Title = title,
                Content = content,
                ImageName = imageName,
                PublishDate = DateTime.UtcNow,
                AuthorId = authorId
            };

            this.db.Add(recipe);

            await this.db.SaveChangesAsync();
        }

        public async Task DeleteAsync(RecipeDeleteServiceModel recipe)
        {
            var recipeForDelete = this.db.Recipes.Where(a => a.Id == recipe.Id).First();

            this.db.Recipes.Remove(recipeForDelete);

            await this.db.SaveChangesAsync();
        }

        public async Task EditAsync(RecipeEditServiceModel recipe)
        {
            var articleForEdit = this.db.Recipes.Where(a => a.Id == recipe.Id).FirstOrDefault();

            articleForEdit.Title = recipe.Title;
            articleForEdit.Content = recipe.Content;

            if (!string.IsNullOrEmpty(recipe.ImageName))
            {
                articleForEdit.ImageName = recipe.ImageName;
            }

            this.db.Recipes.Update(articleForEdit);

            await this.db.SaveChangesAsync();
        }

        public async Task<int> TotalAsync() => await this.db.Recipes.CountAsync();
    }
}
