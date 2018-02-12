using System;
using System.Collections.Generic;
namespace OrfeaBG.Web.Areas.Pages.Models
{
    using static Data.DataConstants;
    using System.ComponentModel.DataAnnotations;

    public class BasicPagesAddViewModel
    {
        [Required]
        [MinLength(TitleMinLength)]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
