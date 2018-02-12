namespace OrfeaBG.Data.Models
{
    using static DataConstants;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Article
    {
        public int Id { get; set; }

        [Required]
        [MinLength(TitleMinLength)]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string ImageName { get; set; }

        [MinLength(PartnerLinkMinLength)]
        [MaxLength(PartnerLinkMaxLength)]
        public string LinkFrom { get; set; }

        public DateTime PublishDate { get; set; }

        public string AuthorId { get; set; }

        public User Author { get; set; }
    }
}
