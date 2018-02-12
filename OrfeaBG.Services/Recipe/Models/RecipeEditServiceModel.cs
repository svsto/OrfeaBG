namespace OrfeaBG.Services.Recipe.Models
{
    using Microsoft.AspNetCore.Http;

    public class RecipeEditServiceModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageName { get; set; }

        public IFormFile Image { get; set; }
    }
}
