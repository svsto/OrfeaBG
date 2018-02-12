namespace OrfeaBG.Services.Pages.Models
{
    using OrfeaBG.Data.Models;
    using static Data.DataConstants;
    using System.ComponentModel.DataAnnotations;
    using OrfeaBG.Common;

    public class BasicPagesListingServiceModel : IMapFrom<BasicPage>
    {
        public int Id { get; set; }

        [Required]
        [MinLength(TitleMinLength)]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
