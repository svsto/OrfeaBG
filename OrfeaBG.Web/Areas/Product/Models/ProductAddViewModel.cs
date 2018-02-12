namespace OrfeaBG.Web.Areas.Product.Models
{
    using static Data.DataConstants;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;

    public class ProductAddViewModel
    {
        [Required]
        [MinLength(TitleMinLength)]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public IFormFile Image { get; set; }

        public string ImageName { get; set; }
    }
}