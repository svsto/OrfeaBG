namespace OrfeaBG.Services.Recipe.Models
{
    using OrfeaBG.Common;
    using System;
    using AutoMapper;
    using OrfeaBG.Data.Models;

    public class RecipeListingServiceModel : IMapFrom<Recipe>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string ImageName { get; set; }

        public DateTime PublishDate { get; set; }

        public string Author { get; set; }

        public void ConfigureMapping(Profile mapper)
            => mapper
                .CreateMap<Recipe, RecipeServiceModel>()
                .ForMember(a => a.Author, cfg => cfg.MapFrom(a => a.Author.UserName));
    }
}
