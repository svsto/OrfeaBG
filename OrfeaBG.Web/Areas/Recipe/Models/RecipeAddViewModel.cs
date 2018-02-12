namespace OrfeaBG.Web.Areas.Recipe.Models
{
    using Microsoft.AspNetCore.Http;
    using static Data.DataConstants;
    using System.ComponentModel.DataAnnotations;

    public class RecipeAddViewModel
    {
        [Required]
        [MinLength(TitleMinLength)]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string ImageName { get; set; }

        public IFormFile Image { get; set; }
    }
}
