using AllupBackendProject.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AllupBackendProject.ViewComponents
{
    public class SubscribeViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            Subscription sub = new Subscription();
            return View(await Task.FromResult(sub));
        }
    }
}
