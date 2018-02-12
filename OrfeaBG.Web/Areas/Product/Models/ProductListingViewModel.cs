namespace OrfeaBG.Web.Areas.Product.Models
{
    using OrfeaBG.Services;
    using OrfeaBG.Services.Product.Models;
    using System;
    using System.Collections.Generic;

    public class ProductListingViewModel
    {
        public IEnumerable<ProductListingServiceModel> Products { get; set; }

        public int TotalProducts { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)this.TotalProducts / ServiceConstants.PageSize);

        public int CurrentPage { get; set; }

        public int PreviousPage => this.CurrentPage <= 1 ? 1 : this.CurrentPage - 1;

        public int NextPage
            => this.CurrentPage == this.TotalPages
                ? this.TotalPages
                : this.CurrentPage + 1;
    }
}
