namespace OrfeaBG.Services.Partner.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using OrfeaBG.Data;
    using OrfeaBG.Data.Models;
    using OrfeaBG.Services.Partner.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PartnerService:IPartnerService
    {
        private readonly OrfeaBGDbContext db;

        public PartnerService(OrfeaBGDbContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<PartnerListingServiceModel>> AllPartnersAsync(int page = 1)
            => await this.db.Partners
                .OrderByDescending(p => p.Name)
                .Skip((page - 1) * ServiceConstants.PageSize)
                .Take(ServiceConstants.PageSize)
                .ProjectTo<PartnerListingServiceModel>()
                .ToListAsync();

        public async Task<IEnumerable<PartnerListingServiceModel>> TakeAllProductsAsync()
           => await this.db.Partners
                .OrderByDescending(p => p.Name)
                .ProjectTo<PartnerListingServiceModel>()
                .ToListAsync();

        public async Task<PartnerListingServiceModel> ByIdAsync(int id)
            => await this.db
                .Partners
                .Where(a => a.Id == id)
                .ProjectTo<PartnerListingServiceModel>()
                .FirstOrDefaultAsync();


        public async Task CreateAsync(string name, string logo, string link)
        {
            var partner = new Partner
            {
                Name = name,
                LogoName =logo,
                Link = link
            };

            this.db.Add(partner);

            await this.db.SaveChangesAsync();
        }

        public async Task DeleteAsync(PartnerDeleteServiceModel partner)
        {
            var partnerForDelete = this.db.Partners.Where(a => a.Id == partner.Id).First();

            this.db.Partners.Remove(partnerForDelete);

            await this.db.SaveChangesAsync();
        }

        public async Task EditAsync(PartnerListingServiceModel partner)
        {
            var partnerForEdit = this.db.Partners.Where(a => a.Id == partner.Id).FirstOrDefault();

            partnerForEdit.Name = partner.Name;

            if (!string.IsNullOrEmpty(partner.Link))
            {
                partnerForEdit.Link = partner.Link;
            }

            if (!string.IsNullOrEmpty(partner.LogoName))
            {
                partnerForEdit.LogoName = partner.LogoName;
            }

            this.db.Partners.Update(partnerForEdit);

            await this.db.SaveChangesAsync();
        }

        public async Task<int> TotalAsync() => await this.db.Partners.CountAsync();
    }
}
