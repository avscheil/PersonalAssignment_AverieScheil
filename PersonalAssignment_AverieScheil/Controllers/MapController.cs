using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CsvHelper;

namespace PersonalAssignment_AverieScheil.Controllers
{
    [Authorize]
    public class MapController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SeeMapData(Guid? id)
        {
            ViewBag.DatasetId = id;
            return View();
        }

    }
}
