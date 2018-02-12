namespace OrfeaBG.Web.Areas.Recipe.Models
{
    using OrfeaBG.Services;
    using OrfeaBG.Services.Recipe.Models;
    using System;
    using System.Collections.Generic;

    public class RecipeListingViewModel
    {
        public IEnumerable<RecipeListingServiceModel> Recipes { get; set; }

        public int TotalRecipes { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)this.TotalRecipes / ServiceConstants.PageSize);

        public int CurrentPage { get; set; }

        public int PreviousPage => this.CurrentPage <= 1 ? 1 : this.CurrentPage - 1;

        public int NextPage
            => this.CurrentPage == this.TotalPages
                ? this.TotalPages
                : this.CurrentPage + 1;
    }
}
