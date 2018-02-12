namespace OrfeaBG.Services.Blog
{
    using OrfeaBG.Services.Blog.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBlogService
    {
        Task<IEnumerable<ArticleListingServiceModel>> AllArtilesAsync(int page);

        Task CreateAsync(string title, string content, string ImageName,string link, string authorId);

        Task EditAsync(ArticleServiceModel article);

        Task<int> TotalAsync();

        Task DeleteAsync(ArticleDeleteServiceModel article);

        Task<ArticleServiceModel> ByIdAsync(int id);
    }
}
