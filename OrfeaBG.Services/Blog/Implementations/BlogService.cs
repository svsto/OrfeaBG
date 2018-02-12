namespace OrfeaBG.Services.Blog.Implementations
{
    using AutoMapper.QueryableExtensions;
    using OrfeaBG.Data.Models;
    using OrfeaBG.Services.Blog.Models;
    using OrfeaBG.Data;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using System;

    public class BlogService : IBlogService
    {
        private readonly OrfeaBGDbContext db;

        public BlogService(OrfeaBGDbContext db)
        {
            this.db = db;
        }

        public async Task<int> TotalAsync()
            => await this.db.Articles.CountAsync();

        public async Task<IEnumerable<ArticleListingServiceModel>> AllArtilesAsync(int page = 1)
            => await this.db.Articles
                .OrderByDescending(a => a.PublishDate)
                .Skip((page - 1) * ServiceConstants.PageSize)
                .Take(ServiceConstants.PageSize)
                .ProjectTo<ArticleListingServiceModel>()
                .ToListAsync();

        public async Task<ArticleServiceModel> ByIdAsync(int id)
            => await this.db
                .Articles
                .Where(a => a.Id == id)
                .ProjectTo<ArticleServiceModel>()
                .FirstOrDefaultAsync();

        public async Task CreateAsync(string title, string content,string imageName,string link, string authorId)
        {
            var article = new Article
            {
                Title = title,
                Content = content,
                ImageName = imageName,
                LinkFrom = link,
                PublishDate = DateTime.UtcNow,
                AuthorId = authorId
            };

            this.db.Add(article);

            await this.db.SaveChangesAsync();
        }

        public async Task EditAsync(ArticleServiceModel article)
        {
            var articleForEdit = this.db.Articles.Where(a => a.Id == article.Id).FirstOrDefault();

            articleForEdit.Title = article.Title;
            articleForEdit.Content = article.Content;

            if (!string.IsNullOrEmpty(article.LinkFrom))
            {
                articleForEdit.LinkFrom = article.LinkFrom;
            }

            if (!string.IsNullOrEmpty(article.ImageName))
            {
                articleForEdit.ImageName = article.ImageName;
            }

            this.db.Articles.Update(articleForEdit);

            await this.db.SaveChangesAsync();
        }

        public async Task DeleteAsync(ArticleDeleteServiceModel article)
        {
            var articleForDelete = this.db.Articles.Where(a => a.Id == article.Id).First();

            this.db.Articles.Remove(articleForDelete);

            await this.db.SaveChangesAsync();
        }
    }
}
