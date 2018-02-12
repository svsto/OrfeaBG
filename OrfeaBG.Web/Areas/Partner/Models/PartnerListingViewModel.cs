namespace OrfeaBG.Web.Areas.Partner.Models
{
    using OrfeaBG.Services;
    using OrfeaBG.Services.Partner.Models;
    using System;
    using System.Collections.Generic;

    public class PartnerListingViewModel
    {
        public IEnumerable<PartnerListingServiceModel> Partners { get; set; }

        public int TotalPartners { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)this.TotalPartners / ServiceConstants.PageSize);

        public int CurrentPage { get; set; }

        public int PreviousPage => this.CurrentPage <= 1 ? 1 : this.CurrentPage - 1;

        public int NextPage
            => this.CurrentPage == this.TotalPages
                ? this.TotalPages
                : this.CurrentPage + 1;
    }
}
