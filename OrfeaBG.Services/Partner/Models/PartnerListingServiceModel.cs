﻿namespace OrfeaBG.Services.Partner.Models
{
    using Microsoft.AspNetCore.Http;
    using OrfeaBG.Common;
    using OrfeaBG.Data.Models;
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants;

    public class PartnerListingServiceModel:IMapFrom<Partner>
    {
        public int Id { get; set; }

        [Required]
        [MinLength(TitleMinLength)]
        [MaxLength(TitleMaxLength)]
        public string Name { get; set; }

        public IFormFile Image { get; set; }

        public string LogoName { get; set; }

        [MinLength(PartnerLinkMinLength)]
        [MaxLength(PartnerLinkMaxLength)]
        public string Link { get; set; }
    }
}