namespace OrfeaBG.Web.Areas.Pages.Models
{
    using OrfeaBG.Services;
    using OrfeaBG.Services.Pages.Models;
    using System;
    using System.Collections.Generic;

    public class BasicPagesListingViewModel
    {
        public IEnumerable<BasicPagesListingServiceModel> Pages { get; set; }

        public int TotalBasicPages { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)this.TotalBasicPages / ServiceConstants.PageSize);

        public int CurrentPage { get; set; }

        public int PreviousPage => this.CurrentPage <= 1 ? 1 : this.CurrentPage - 1;

        public int NextPage
            => this.CurrentPage == this.TotalPages
                ? this.TotalPages
                : this.CurrentPage + 1;
    }
}
