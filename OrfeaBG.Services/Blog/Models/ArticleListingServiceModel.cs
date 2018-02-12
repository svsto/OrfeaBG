namespace OrfeaBG.Services.Blog.Models
{
    using AutoMapper;
    using OrfeaBG.Common;
    using OrfeaBG.Data.Models;
    using System;
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants;

    public class ArticleListingServiceModel:IMapFrom<Article>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageName { get; set; }

        public string LinkFrom { get; set; }

        public DateTime PublishDate { get; set; }

        public string Author { get; set; }

        public void ConfigureMapping(Profile mapper)
            => mapper
                .CreateMap<Article, ArticleListingServiceModel>()
                .ForMember(a => a.Author, cfg => cfg.MapFrom(a => a.Author.UserName));
    }
}
