namespace OrfeaBG.Data.Models
{
    using static DataConstants;
    using System.ComponentModel.DataAnnotations;

    public class BasicPage
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
