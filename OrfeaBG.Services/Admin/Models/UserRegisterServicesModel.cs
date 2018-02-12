namespace OrfeaBG.Services.Admin.Models
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using OrfeaBG.Common;
    using OrfeaBG.Data.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class UserRegisterServicesModel:IMapFrom<User>
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string[] Role { get; set; }

        public IEnumerable<SelectListItem> Roles { get; set; } = new List<SelectListItem>();
    }
}
