namespace OrfeaBG.Data.Models
{
    using static DataConstants;
    using System;
    using System.ComponentModel.DataAnnotations;
    
    public class Recipe
    {
        public int Id { get; set; }

        [Required]
        [MinLength(TitleMinLength)]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string ImageName { get; set; }

        public DateTime PublishDate { get; set; }

        public string AuthorId { get; set; }

        public User Author { get; set; }
    }
}
