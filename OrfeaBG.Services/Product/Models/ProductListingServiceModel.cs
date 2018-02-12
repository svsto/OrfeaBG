namespace OrfeaBG.Services.Product.Models
{
    using OrfeaBG.Data.Models;
    using static Data.DataConstants;
    using System.ComponentModel.DataAnnotations;
    using OrfeaBG.Common;
    using Microsoft.AspNetCore.Http;

    public class ProductListingServiceModel:IMapFrom<Product>
    {
        public int Id { get; set; }

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
