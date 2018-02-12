namespace OrfeaBG.Services.Product.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using OrfeaBG.Data;
    using OrfeaBG.Data.Models;
    using OrfeaBG.Services.Partner.Models;
    using OrfeaBG.Services.Product.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ProductService: IProductService
    {
        private readonly OrfeaBGDbContext db;

        public ProductService(OrfeaBGDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<ProductListingServiceModel>> AllProductsAsync(int page = 1)
            => await this.db.Products
                .OrderByDescending(p => p.Title)
                .Skip((page - 1) * ServiceConstants.PageSize)
                .Take(ServiceConstants.PageSize)
                .ProjectTo<ProductListingServiceModel>()
                .ToListAsync();

        public async Task<IEnumerable<ProductListingServiceModel>> LatestProductsAsync(int take)
            => await this.db.Products
                .OrderByDescending(p => p.Title)
                .Take(take)
                .ProjectTo<ProductListingServiceModel>()
                .ToListAsync();

        public async Task<ProductListingServiceModel> ByIdAsync(int id)
            => await this.db
                .Products
                .Where(a => a.Id == id)
                .ProjectTo<ProductListingServiceModel>()
                .FirstOrDefaultAsync();


        public async Task CreateAsync(string title, string content, string imageName)
        {
            var procuct = new Product
            {
                Title = title,
                Content = content,
                ImageName = imageName,
            };

            this.db.Add(procuct);

            await this.db.SaveChangesAsync();
        }

        public async Task DeleteAsync(ProductDeleteServiceModel procuct)
        {
            var productForDelete = this.db.Products.Where(a => a.Id == procuct.Id).First();

            this.db.Products.Remove(productForDelete);

            await this.db.SaveChangesAsync();
        }

        public async Task EditAsync(ProductListingServiceModel product)
        {
            var productForEdit = this.db.Products.Where(a => a.Id == product.Id).FirstOrDefault();

            productForEdit.Title = product.Title;
            productForEdit.Content = product.Content;

            if (!string.IsNullOrEmpty(product.ImageName))
            {
                productForEdit.ImageName = product.ImageName;
            }

            this.db.Products.Update(productForEdit);

            await this.db.SaveChangesAsync();
        }

        public async Task<int> TotalAsync() => await this.db.Products.CountAsync();
    }
}
