namespace OrfeaBG.Services.Product
{
    //public interface IProductService
    using OrfeaBG.Services.Partner.Models;
    using OrfeaBG.Services.Product.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProductService
    {
        Task<IEnumerable<ProductListingServiceModel>> AllProductsAsync(int page);

        Task<IEnumerable<ProductListingServiceModel>> LatestProductsAsync(int take);

        Task CreateAsync(string title, string content, string imageName);

        Task EditAsync(ProductListingServiceModel product);

        Task<int> TotalAsync();

        Task DeleteAsync(ProductDeleteServiceModel product);

        Task<ProductListingServiceModel> ByIdAsync(int id);
    }
}
