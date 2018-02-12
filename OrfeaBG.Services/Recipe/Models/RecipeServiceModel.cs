namespace OrfeaBG.Services.Recipe.Models
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.ComponentModel.DataAnnotations;
  
    public class RecipeServiceModel 
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string ImageName { get; set; }

        public IFormFile Image { get; set; }

        public DateTime PublishDate { get; set; }

        public string Author { get; set; }
    }
}
