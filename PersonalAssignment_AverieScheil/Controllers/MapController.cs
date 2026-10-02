using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PersonalAssignment_AverieScheil.Controllers
{
    [Authorize]
    public class MapController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SeeMapData()
        {
            return View();
        }
    }
}
