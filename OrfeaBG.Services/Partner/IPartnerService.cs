namespace OrfeaBG.Services.Partner
{
    using OrfeaBG.Services.Partner.Models;
    using OrfeaBG.Services.Recipe.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IPartnerService
    {
        Task<IEnumerable<PartnerListingServiceModel>> AllPartnersAsync(int page);

        Task<IEnumerable<PartnerListingServiceModel>> TakeAllProductsAsync();

        Task CreateAsync(string name, string LogoName, string link);

        Task EditAsync(PartnerListingServiceModel partner);

        Task<int> TotalAsync();

        Task DeleteAsync(PartnerDeleteServiceModel partner);

        Task<PartnerListingServiceModel> ByIdAsync(int id);
    }
}
