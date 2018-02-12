namespace OrfeaBG.Web.Areas.Blog.Models
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants;

    public class BlogEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [MinLength(TitleMinLength)]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string ImageName { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile Image { get; set; }

        [MinLength(PartnerLinkMinLength)]
        [MaxLength(PartnerLinkMaxLength)]
        public string LinkFrom { get; set; }

        public DateTime PublishDate { get; set; }

        public string Author { get; set; }
    }
}
