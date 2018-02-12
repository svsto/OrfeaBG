namespace OrfeaBG.Data.Models
{
    using static DataConstants;
    using System.ComponentModel.DataAnnotations;

    public class Partner
    {
        public int Id { get; set; }

        [Required]
        [MinLength(TitleMinLength)]
        [MaxLength(TitleMaxLength)]
        public string Name { get; set; }

        public string LogoName { get; set; }

        [MinLength(PartnerLinkMinLength)]
        [MaxLength(PartnerLinkMaxLength)]
        public string Link { get; set; }
    }
}
