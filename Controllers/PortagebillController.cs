using crewlinkship.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using SelectPdf;
using Microsoft.AspNetCore.Mvc.Rendering;
using crewlinkship.ViewModel;

namespace crewlinkship.Controllers
{
    public class PortagebillController : Controller
    {
        private readonly shipCrewlinkContext _context;
        private readonly IHostingEnvironment _appEnvironment;
        public PortagebillController(shipCrewlinkContext context, IHostingEnvironment appEnvironment)
        {
            // _logger = logger;
            _context = context;
            _appEnvironment = appEnvironment;
        }
        public IActionResult Index(int? vesselId, int? month)
        {
            /*int vesselId = 138; int month = 2;*/
            int year = 2023; string ispromoted = "no"; string checkpbtilldate = "";
            var data = _context.PortageBillVMs.FromSqlRaw<PortageBillVM>("getPortageBill @p0, @p1, @p2, @p3, @p4", vesselId, month, year, ispromoted, checkpbtilldate);

            ViewBag.vessel = new SelectList(_context.TblVessels, "VesselId", "VesselName");

            ViewBag.vesselDetails = _context.TblVessels.Include(x => x.Flag).Include(x => x.Ship).Where(x => x.IsDeleted == false && x.VesselId == 75).FirstOrDefault();

            ViewBag.vessels = _context.TblVessels.Where(x => x.IsDeleted == false && x.IsActive == false && x.VesselId == 75).ToList();

            return View(data);
        }

        [HttpGet]
        public IActionResult Index(int? vesselId, int? month, int? year)
        {
            string ispromoted = "no"; string checkpbtilldate = "";
            var data = _context.PortageBillVMs.FromSqlRaw<PortageBillVM>("getPortageBill @p0, @p1, @p2, @p3, @p4", vesselId, month, year, ispromoted, checkpbtilldate);
            ViewBag.vessel = new SelectList(_context.TblVessels, "VesselId", "VesselName");

            ViewBag.vesselDetails = _context.TblVessels.Include(x => x.Flag).Include(x => x.Ship).Where(x => x.IsDeleted == false && x.VesselId == 75).FirstOrDefault();

            ViewBag.vessels = _context.TblVessels.Where(x => x.IsDeleted == false && x.IsActive == false && x.VesselId == 75).ToList();

            return View(data);
        }

    }
}
