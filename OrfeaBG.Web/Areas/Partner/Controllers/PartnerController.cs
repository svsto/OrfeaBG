namespace OrfeaBG.Web.Areas.Partner.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using OrfeaBG.Data.Models;
    using OrfeaBG.Services.Html;
    using OrfeaBG.Services.Partner;
    using OrfeaBG.Services.Partner.Models;
    using OrfeaBG.Web.Areas.Partner.Models;
    using System.IO;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using static OrfeaBG.Data.DataConstants;

    //RecipeArea
    [Area("Partner")]
    [Authorize(Roles = AdminRole)]
    public class PartnerController:Controller
    {
        private readonly IPartnerService partnerService;
        private readonly UserManager<User> userManager;
        private IHostingEnvironment hostingEnvironment;
        private readonly IHtmlService html;

        public PartnerController(
            IPartnerService partnerService,
            UserManager<User> userManager,
            IHostingEnvironment hostingEnvironment,
            IHtmlService html)
        {
            this.partnerService = partnerService;
            this.userManager = userManager;
            this.hostingEnvironment = hostingEnvironment;
            this.html = html;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var model = new PartnerListingViewModel
            {
                Partners = await this.partnerService.AllPartnersAsync(page),
                TotalPartners = await this.partnerService.TotalAsync(),
                CurrentPage = page
            };
            return View(model);
        }

        [HttpGet]
        [Route("partner/add")]
        public IActionResult Add() => View();

        [HttpPost]
        [Route("partner/add")]
        public async Task<IActionResult> Add(AddPartnerViewModel partner, IFormFile Image)
        {
            if (!ModelState.IsValid)
            {
                return View(partner);
            }

            var fileName = await SaveImage(Image);

            await this.partnerService.CreateAsync(partner.Name, fileName, partner.Link);

            TempData[WebConstants.TempDataSuccessMessageKey] = ($"Partner {partner.Name} successfuly added.");
            return RedirectToAction(nameof(PartnerController.Index), new { page = 1 });
        }

        [HttpGet]
        [Route("partner/edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var partner = await this.partnerService.ByIdAsync(id);

            if (partner == null)
            {
                return BadRequest();
            }

            return View(partner);
        }

        [HttpPost]
        [Route("partner/edit")]
        public async Task<IActionResult> Edit(PartnerListingServiceModel partner, IFormFile Image)
        {
            if (!ModelState.IsValid)
            {
                return View(partner);
            }

            if (Image != null)
            {
                partner.LogoName = await SaveImage(Image);
            }

            await this.partnerService.EditAsync(partner);

            TempData[WebConstants.TempDataSuccessMessageKey] = ($"Partner {partner.Name} successfuly updated.");
            return RedirectToAction(nameof(PartnerController.Index), new { page = 1 });
        }

        [HttpGet]
        [Route("partner/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var partner = await this.partnerService.ByIdAsync(id);

            if (partner == null)
            {
                return BadRequest();
            }

            var partnerForDelete = new PartnerDeleteServiceModel
            {
                Id = id,
                Name = partner.Name
            };

            return View(partnerForDelete);
        }

        [HttpPost]
        [Route("partner/delete/{id}")]
        public IActionResult DeleteConfirmed(PartnerDeleteServiceModel partner)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(PartnerController.Delete), new { id = partner.Id });
            }

            this.partnerService.DeleteAsync(partner);

            TempData[WebConstants.TempDataSuccessMessageKey] = ($"Partner {partner.Name} successfuly deleted.");
            return RedirectToAction(nameof(PartnerController.Index), new { page = 1 });
        }

        private async Task<string> SaveImage(IFormFile Image)
        {
            var fileName = string.Empty;
            var articleImageName = string.Empty;

            if (Image != null && Image.Length > 0)
            {

                var file = Image;
                var uploads = Path.Combine(hostingEnvironment.WebRootPath, "partners");

                if (file.Length > 0)
                {
                    fileName = ContentDispositionHeaderValue.Parse
                        (file.ContentDisposition).FileName.Trim('"');

                    System.Console.WriteLine(fileName);
                    using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                        articleImageName = file.FileName;
                    }
                }
            }

            return articleImageName;
        }
    }
}
