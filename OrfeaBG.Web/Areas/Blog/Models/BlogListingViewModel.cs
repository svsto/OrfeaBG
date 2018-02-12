namespace OrfeaBG.Web.Areas.Blog.Models
{
    using OrfeaBG.Services;
    using OrfeaBG.Services.Blog.Models;
    using System;
    using System.Collections.Generic;

    public class BlogListingViewModel
    {
        public IEnumerable<ArticleListingServiceModel> Articles { get; set; }

        public int TotalArticles { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)this.TotalArticles / ServiceConstants.PageSize);

        public int CurrentPage { get; set; }

        public int PreviousPage => this.CurrentPage <= 1 ? 1 : this.CurrentPage - 1;

        public int NextPage
            => this.CurrentPage == this.TotalPages
                ? this.TotalPages
                : this.CurrentPage + 1;
    }
}
