namespace OrfeaBG.Web.Areas.Partner.Models
{
    using Microsoft.AspNetCore.Http;
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants;

    public class AddPartnerViewModel
    {
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
