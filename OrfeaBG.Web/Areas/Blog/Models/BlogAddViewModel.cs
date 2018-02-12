namespace OrfeaBG.Web.Areas.Blog.Models
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants;

    public class BlogAddViewModel
    {
        [Required]
        [MinLength(TitleMinLength)]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

   
        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }

        public string ImageName { get; set; }

        [MinLength(PartnerLinkMinLength)]
        [MaxLength(PartnerLinkMaxLength)]
        public string LinkFrom { get; set; }

        public DateTime PublishDate { get; set; }

        public string Author { get; set; }
    }
}
