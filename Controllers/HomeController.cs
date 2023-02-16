using Abp.Web.Mvc.Models;
using crewlinkship.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        private readonly ShipCrewLinkContext _context;
        public HomeController(ILogger<HomeController> logger , ShipCrewLinkContext context)
        {
            _logger = logger;
            _context = context;
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
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
        public IActionResult OpenPopup()
        {
            return View();
        }
        public ActionResult Details()
        {
            ViewBag.rankName = _context.TblCrewDetails.Include(x => x.Rank).Include(x => x.Vessel).Where(x => x.IsDeleted == false && x.CrewId == 355).ToList();

            var CrewName = _context.TblCrewDetails.Include(x => x.Rank).Include(x => x.Vessel).Include(c=>c.Country)
              .Include(c => c.Pool).Where(x => x.CrewId == 8584).ToList();
            return PartialView(CrewName);
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
            ViewBag.rankName = _context.TblCrewDetails.Include(x => x.Rank).Include(x => x.Vessel).Where(x => x.IsDeleted == false && x.CrewId == 355).ToList();

            ViewBag.passport= _context.TblPassports.Include(x => x.Country).Where(x => x.CrewId == 8584).ToList();
            ViewBag.cdc = _context.TblCdcs.Include(x => x.Country).Where(x => x.CrewId == 8584).ToList();
            ViewBag.visa = _context.TblVisas.Include(x => x.Country).Where(x => x.CrewId == 8584).ToList();
            ViewBag.yf = _context.TblYellowfevers.Include(x => x.VendorRegister).Where(x => x.CrewId == 8584).ToList();

            return PartialView();
        }

        public ActionResult VesselParticular()
        {
            ViewBag.HandOverport = "NA";
            var HandOverPortId = _context.TblVessels.Where(x => x.IsDeleted == false && x.VesselId == 103).FirstOrDefault().PortOfHandover;
            if (HandOverPortId != null)
            {
                ViewBag.HandOverport = _context.TblSeaports.Where(x => x.SeaportId == HandOverPortId).FirstOrDefault().SeaportName;

            }

            var vesselName = _context.TblVessels.Include(x=>x.Flag).Include(x => x.PortOfRegistryNavigation).Include(x => x.Ship)
                .Include(x => x.Owner).Include(x => x.DisponentOwner).Include(x => x.Manager).Include(x => x.Crewmanager)
                .Include(x => x.Classification).Include(t=>t.PortOfTakeovers).Include(p => p.VendorRegisterPi)
                .Include(h => h.VendorRegisterHm).Include(e=>e.EngineModel).Include(T=>T.EngineType).Include(b=>b.Builder)
                .Where(x => x.IsDeleted == false && x.VesselId == 103).ToList();

        


            return View(vesselName);
        }

        public ActionResult Crewlist()
        {

            return View("Crewlist");
        }


    }

}
