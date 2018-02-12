namespace OrfeaBG.Services.Pages
{
    using OrfeaBG.Services.Pages.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBasicPagesService
    {

        Task<IEnumerable<BasicPagesListingServiceModel>> AllPagesAsync(int page);

        Task CreateAsync(string title, string content);

        Task EditAsync(BasicPagesListingServiceModel page);

        Task<int> TotalAsync();

        Task DeleteAsync(BasicPagesDeleteServiceModel page);

        Task<BasicPagesListingServiceModel> ByIdAsync(int id);
    }
}
