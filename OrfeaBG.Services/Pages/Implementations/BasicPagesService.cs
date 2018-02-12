namespace OrfeaBG.Services.Pages.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using OrfeaBG.Data;
    using OrfeaBG.Data.Models;
    using OrfeaBG.Services.Pages.Models;
    using OrfeaBG.Services.Partner.Models;
    using OrfeaBG.Services.Product.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class BasicPagesService:IBasicPagesService
    {
        private readonly OrfeaBGDbContext db;

        public BasicPagesService(OrfeaBGDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<BasicPagesListingServiceModel>> AllPagesAsync(int page = 1)
            => await this.db.BasicPages
                .OrderByDescending(p => p.Title)
                .Skip((page - 1) * ServiceConstants.PageSize)
                .Take(ServiceConstants.PageSize)
                .ProjectTo<BasicPagesListingServiceModel>()
                .ToListAsync();

        public async Task<BasicPagesListingServiceModel> ByIdAsync(int id)
            => await this.db
                .BasicPages
                .Where(a => a.Id == id)
                .ProjectTo<BasicPagesListingServiceModel>()
                .FirstOrDefaultAsync();


        public async Task CreateAsync(string title, string content)
        {
            var page = new BasicPage
            {
                Title = title,
                Content = content
            };

            this.db.Add(page);

            await this.db.SaveChangesAsync();
        }

        public async Task DeleteAsync(BasicPagesDeleteServiceModel page)
        {
            var pageForDelete = this.db.BasicPages.Where(a => a.Id == page.Id).First();

            this.db.BasicPages.Remove(pageForDelete);

            await this.db.SaveChangesAsync();
        }

        public async Task EditAsync(BasicPagesListingServiceModel page)
        {
            var pageForEdit = this.db.BasicPages.Where(a => a.Id == page.Id).FirstOrDefault();

            pageForEdit.Title = page.Title;
            pageForEdit.Content = page.Content;

            this.db.BasicPages.Update(pageForEdit);

            await this.db.SaveChangesAsync();
        }

        public async Task<int> TotalAsync() => await this.db.Products.CountAsync();
    }
}

