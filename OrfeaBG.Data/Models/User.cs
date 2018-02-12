namespace OrfeaBG.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System.Collections.Generic;

    public class User : IdentityUser
    {
        public List<Article> Articles { get; set; } = new List<Article>();

        public List<Recipe> Recipes { get; set; } = new List<Recipe>();
    }
}
