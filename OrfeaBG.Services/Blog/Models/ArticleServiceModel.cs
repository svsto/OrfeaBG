namespace OrfeaBG.Services.Blog.Models
{
    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using OrfeaBG.Common;
    using OrfeaBG.Data.Models;
    using System;
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants;

    public class ArticleServiceModel:IMapFrom<Article>, IHaveCustomMapping
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

        public void ConfigureMapping(Profile mapper)
            => mapper
                .CreateMap<Article, ArticleServiceModel>()
                .ForMember(a => a.Author, cfg => cfg.MapFrom(a => a.Author.UserName));
    }
}
