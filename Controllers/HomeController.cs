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
        //private readonly ILogger<HomeController> _logger;
        private readonly shipCrewlinkContext _context;
        public HomeController(shipCrewlinkContext context)
        {
            // _logger = logger;
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
        public IActionResult Address()
        {
            ViewBag.address = _context.TblCrewAddresses.Include(x => x.Country).Include(x => x.State).Include(x => x.City).Include(x => x.Airport).Where(x => x.IsDeleted == false && x.CrewId == 355).ToList();

            ViewBag.corsAddress =_context.TblCrewCorrespondenceAddresses.Include(x=>x.Country).Include(x=>x.State).Include(x=>x.City).Include(x=>x.Airport).Where(x => x.IsDeleted == false && x.CrewId == 355).ToList();
            ViewBag.rankName = _context.TblCrewDetails.Include(x => x.Rank).Include(x => x.Vessel).Where(x => x.IsDeleted == false && x.CrewId == 355).ToList();

            return PartialView();
        }
        public ActionResult Bankdetails()
        {
            ViewBag.primaryBank = _context.TblCrewBankDetails.Include(x => x.Country).Include(x => x.State).Include(x => x.City).Where(x => x.IsDeleted == false && x.CrewId == 1579 && x.AccountType == "Primary").ToList();

            ViewBag.secondaryBank = _context.TblCrewBankDetails.Include(x => x.Country).Include(x => x.State).Include(x => x.City).Where(x => x.IsDeleted == false && x.CrewId == 1579 && x.AccountType == "Secondary").ToList();

            ViewBag.rankName = _context.TblCrewDetails.Include(x => x.Rank).Include(x => x.Vessel).Where(x => x.IsDeleted == false && x.CrewId == 355).ToList();
            return PartialView();
        }

        public ActionResult License()
        {
            ViewBag.nationalLicence = _context.TblCrewLicenses.Include(x => x.License).Include(x => x.Country).Include(x => x.Authority).Where(x => x.IsDeleted == false && x.CrewId == 1579 && x.License.Authority.ToLower().Contains("flag") == false).ToList();

            ViewBag.flagLicence = _context.TblCrewLicenses.Include(x => x.License).Include(x => x.Country).Include(x => x.Authority).Where(x => x.IsDeleted == false && x.CrewId == 1579 && x.License.Authority.ToLower().Contains("flag") == true).ToList();

            ViewBag.rankName = _context.TblCrewDetails.Include(x => x.Rank).Include(x => x.Vessel).Where(x => x.IsDeleted == false && x.CrewId == 355).ToList();
            return PartialView();
        }
        public ActionResult Courses()
        {
            var courses = _context.TblCrewCourses.Include(x => x.CourseNavigation).Include(x => x.Institute).Include(x => x.Authority).Where(x => x.IsDeleted == false && x.CrewId == 1579).ToList();

            ViewBag.rankName = _context.TblCrewDetails.Include(x => x.Rank).Include(x => x.Vessel).Where(x => x.IsDeleted == false && x.CrewId == 355).ToList();

            return PartialView(courses);
        }
        public ActionResult OtherDocuments()
        {
            var otherDocuments = _context.TblCrewOtherDocuments.Include(x => x.Document).Include(x=>x.Authority).Where(x => x.IsDeleted == false && x.CrewId == 1579).ToList();

            ViewBag.rankName = _context.TblCrewDetails.Include(x => x.Rank).Include(x => x.Vessel).Where(x => x.IsDeleted == false && x.CrewId == 355).ToList();

            return PartialView(otherDocuments);
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

        

            var vesselDetails = _context.TblVessels.Include(x => x.Flag).Include(x => x.Ship).Where(x => x.IsDeleted == false && x.VesselId == 19).FirstOrDefault();
            ViewBag.vesselName = vesselDetails.VesselName;
            ViewBag.imo = vesselDetails.Imo;
            ViewBag.shipType = vesselDetails.Ship.ShipCategory;
            ViewBag.flag = vesselDetails.Flag.CountryName;

            ViewBag.HandOverport = "NA";
            var HandOverPortId = _context.TblVessels.Where(x => x.IsDeleted == false && x.VesselId == 103).FirstOrDefault().PortOfHandover;
            if (HandOverPortId != null)
            {
                ViewBag.HandOverport = _context.TblSeaports.Where(x => x.SeaportId == HandOverPortId).FirstOrDefault().SeaportName;

            }

            var vesselName = _context.TblVessels.Include(x => x.Flag).Include(x => x.PortOfRegistryNavigation).Include(x => x.Ship)
                .Include(x => x.Owner).Include(x => x.DisponentOwner).Include(x => x.Manager).Include(x => x.Crewmanager)
                .Include(x => x.Classification).Include(t => t.PortOfTakeovers).Include(p => p.VendorRegisterPi)
                .Include(h => h.VendorRegisterHm).Include(e => e.EngineModel).Include(T => T.EngineType).Include(b => b.Builder)
                .Where(x => x.IsDeleted == false && x.VesselId == 103).ToList();

            return View(vesselName);
        }

        public ActionResult Crewlist()
        {

            return PartialView("Crewlist");
        }

        public async Task<IActionResult> HeadName()
        {
            var vesselName = await _context.TblVessels.Include(x => x.Flag).Include(x => x.Ship).Where(x => x.IsDeleted == false && x.VesselId == 19).ToListAsync();
            return PartialView(vesselName);
        }

        public IActionResult vwCrewList()
        {
            var vesselDetails = _context.TblVessels.Include(x => x.Flag).Include(x => x.Ship).Where(x => x.IsDeleted == false && x.VesselId == 19).FirstOrDefault();
            ViewBag.vesselName = vesselDetails.VesselName;
            ViewBag.imo = vesselDetails.Imo;
            ViewBag.shipType = vesselDetails.Ship.ShipCategory;
            ViewBag.flag = vesselDetails.Flag.CountryName;

            var crewlist = _context.TblCrewLists.Include(x => x.Crew).Include(x => x.Reliever).Include(x => x.Rank).Include(x => x.Crew.Country).Include(x => x.ReliverRank).Where(x => x.IsDeleted == false && x.VesselId == 156 && x.IsSignOff != true && x.IsDeleted == false).ToList().OrderBy(x => x.Rank.CrewSort).ToList();
            return View(crewlist);

        }
        public IActionResult CBA()
        {
            var vesselDetails = _context.TblVessels.Include(x => x.Flag).Include(x => x.Ship).Where(x => x.IsDeleted == false && x.VesselId == 19).FirstOrDefault();
            ViewBag.vesselName = vesselDetails.VesselName;
            ViewBag.imo = vesselDetails.Imo;
            ViewBag.shipType = vesselDetails.Ship.ShipCategory;
            ViewBag.flag = vesselDetails.Flag.CountryName;

            var vcm = _context.TblVesselCbas.Include(x => x.Country).Where(x => x.IsDeleted == false && x.VesselId == 156).ToList();
            return View(vcm);
        }

    }

}
