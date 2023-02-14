using crewlinkship.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace crewlinkship.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult OpenPopup()
        {
            return View();
        }
        public ActionResult Details()
        {

            return PartialView("Details");
        }
        public ActionResult Address()
        {

            return PartialView("Address");
        }
        public ActionResult Bankdetails()
        {

            return PartialView("Bankdetails");
        }
        public ActionResult License()
        {

            return PartialView("License");
        }
        public ActionResult Courses()
        {

            return PartialView("Courses");
        }
        public ActionResult OtherDocuments()
        {

            return PartialView("OtherDocuments");
        }
        public ActionResult Crewtraveldoc()
        {

            return PartialView("Crewtraveldoc");
        }

        public ActionResult VesselParticular()
        {

            return View("VesselParticular");
        }

        public ActionResult Crewlist()
        {

            return View("Crewlist");
        }


    }

}
