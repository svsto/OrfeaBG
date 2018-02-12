namespace OrfeaBG.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using OrfeaBG.Data.Models;

    public class OrfeaBGDbContext : IdentityDbContext<User>
    {
        public OrfeaBGDbContext(DbContextOptions<OrfeaBGDbContext> options)
            : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }

        public DbSet<BasicPage> BasicPages { get; set; }

        public DbSet<Partner> Partners { get; set; }
  
        public DbSet<Product> Products { get; set; }

        public DbSet<Recipe> Recipes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Article>()
                .HasOne(a => a.Author)
                .WithMany(u => u.Articles)
                .HasForeignKey(a => a.AuthorId);

            builder
                .Entity<Recipe>()
                .HasOne(r => r.Author)
                .WithMany(u => u.Recipes)
                .HasForeignKey(r => r.AuthorId);

            base.OnModelCreating(builder);
        }
    }
}
