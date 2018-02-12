namespace OrfeaBG.Services.Recipe
{
    using OrfeaBG.Services.Recipe.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IRecipeService
    {
        Task<IEnumerable<RecipeListingServiceModel>> AllRecipesAsync(int page);

        Task CreateAsync(string title, string content, string ImageName, string authorId);

        Task EditAsync(RecipeEditServiceModel recipe);

        Task<int> TotalAsync();

        Task DeleteAsync(RecipeDeleteServiceModel recipe);

        Task<RecipeServiceModel> ByIdAsync(int id);
    }
}
