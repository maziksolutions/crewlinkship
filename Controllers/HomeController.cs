//using Abp.Web.Mvc.Models;
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

namespace crewlinkship.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly shipCrewlinkContext _context;
        private readonly IHostingEnvironment _appEnvironment;

        public bool IMOFull { get; private set; }

        public HomeController(shipCrewlinkContext context ,IHostingEnvironment appEnvironment)
        {
            // _logger = logger;
            _context = context;
            _appEnvironment = appEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
        public IActionResult Details(int? crewId)
        {
            ViewBag.rankName = _context.TblCrewDetails.Include(x => x.Rank).Include(x => x.Vessel).Where(x => x.IsDeleted == false && x.CrewId == crewId).ToList();

            ViewBag.passport = _context.TblPassports.Where(p => p.CrewId == 355 && p.IsDeleted == false).FirstOrDefault().PassportNumber;
            ViewBag.cdc = _context.TblCdcs.Where(p => p.CrewId == 355 && p.IsDeleted == false).FirstOrDefault().Cdcnumber;

            ViewBag.crewDetails = _context.TblCrewDetails.Include(x => x.Rank).Include(x => x.Vessel).Include(c=>c.Country)
              .Include(c => c.Pool).Where(x => x.CrewId == crewId).ToList();
            return PartialView();
        }


        public IActionResult Address(int? crewId)
        {
            ViewBag.address = _context.TblCrewAddresses.Include(x => x.Country).Include(x => x.State).Include(x => x.City).Include(x => x.Airport).Where(x => x.IsDeleted == false && x.CrewId == crewId).FirstOrDefault();
            ViewBag.corsAddress = _context.TblCrewCorrespondenceAddresses.Include(x => x.Country).Include(x => x.State).Include(x => x.City).Include(x => x.Airport).Where(x => x.IsDeleted == false && x.CrewId == crewId).FirstOrDefault();
            ViewBag.rankName = _context.TblCrewDetails.Include(x => x.Rank).Include(x => x.Vessel).Where(x => x.IsDeleted == false && x.CrewId == crewId).FirstOrDefault();
            return PartialView();
        }


        public ActionResult Bankdetails(int? crewId)
        {
            ViewBag.primaryBank = _context.TblCrewBankDetails.Include(x => x.Country).Include(x => x.State).Include(x => x.City).Where(x => x.IsDeleted == false && x.CrewId == crewId && x.AccountType == "Primary").ToList();
            ViewBag.secondaryBank = _context.TblCrewBankDetails.Include(x => x.Country).Include(x => x.State).Include(x => x.City).Where(x => x.IsDeleted == false && x.CrewId == crewId && x.AccountType == "Secondary").ToList();
            ViewBag.rankName = _context.TblCrewDetails.Include(x => x.Rank).Include(x => x.Vessel).Where(x => x.IsDeleted == false && x.CrewId == crewId).ToList();
            return PartialView();
        }

        public ActionResult License(int? crewId)
        {
            ViewBag.nationalLicence = _context.TblCrewLicenses.Include(x => x.License).Include(x => x.Country).Include(x => x.Authority).Where(x => x.IsDeleted == false && x.CrewId == crewId && x.License.Authority.ToLower().Contains("flag") == false).ToList();
            ViewBag.flagLicence = _context.TblCrewLicenses.Include(x => x.License).Include(x => x.Country).Include(x => x.Authority).Where(x => x.IsDeleted == false && x.CrewId == crewId && x.License.Authority.ToLower().Contains("flag") == true).ToList();
            ViewBag.rankName = _context.TblCrewDetails.Include(x => x.Rank).Include(x => x.Vessel).Where(x => x.IsDeleted == false && x.CrewId == crewId).ToList();
            return PartialView();
        }

        public ActionResult Courses(int? crewId)
        {
            ViewBag.courses = _context.TblCrewCourses.Include(x => x.CourseNavigation).Include(x => x.Institute).Include(x => x.Authority).Where(x => x.IsDeleted == false && x.CrewId == crewId).ToList();
            ViewBag.rankName = _context.TblCrewDetails.Include(x => x.Rank).Include(x => x.Vessel).Where(x => x.IsDeleted == false && x.CrewId == crewId).ToList();
            return PartialView();
        }

        public ActionResult OtherDocuments(int? crewId)
        {
            ViewBag.otherDocuments = _context.TblCrewOtherDocuments.Include(x => x.Document).Include(x=>x.Authority).Where(x => x.IsDeleted == false && x.CrewId == crewId).ToList();
            ViewBag.rankName = _context.TblCrewDetails.Include(x => x.Rank).Include(x => x.Vessel).Where(x => x.IsDeleted == false && x.CrewId == crewId).ToList();
            return PartialView();
        }

        public ActionResult Crewtraveldoc(int? crewId)
        {
            ViewBag.rankName = _context.TblCrewDetails.Include(x => x.Rank).Include(x => x.Vessel).Where(x => x.IsDeleted == false && x.CrewId == crewId).ToList();
            ViewBag.passport= _context.TblPassports.Include(x => x.Country).Where(x => x.CrewId == crewId).ToList();
            ViewBag.cdc = _context.TblCdcs.Include(x => x.Country).Where(x => x.CrewId == crewId).ToList();
            ViewBag.visa = _context.TblVisas.Include(x => x.Country).Where(x => x.CrewId == crewId).ToList();
            ViewBag.yf = _context.TblYellowfevers.Include(x => x.VendorRegister).Where(x => x.CrewId == crewId).ToList();
            return PartialView();
        }

        public ActionResult VesselParticular()
        {
            ViewBag.vesselDetails = _context.TblVessels.Include(x => x.Flag).Include(x => x.Ship).Where(x => x.IsDeleted == false && x.VesselId == 75).FirstOrDefault();

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
            ViewBag.vessels = _context.TblVessels.Where(x => x.IsDeleted == false && x.IsActive == false && x.VesselId == 75).ToList();
            return View(vesselName);
        }

        public IActionResult vwCrewList()
        {
            ViewBag.vesselDetails = _context.TblVessels.Include(x => x.Flag).Include(x => x.Ship).Where(x => x.IsDeleted == false && x.VesselId == 75).FirstOrDefault();
           
            //ViewBag.imo = vesselDetails.Imo;
            //ViewBag.shipType = vesselDetails.Ship.ShipCategory;
            //ViewBag.flag = vesselDetails.Flag.CountryName;

            var crewlist = _context.TblCrewLists.Include(x => x.Crew).Include(x => x.Reliever).Include(x => x.Rank).Include(x => x.Crew.Country).Include(x => x.ReliverRank).Where(x => x.IsDeleted == false && x.VesselId == 75 && x.IsSignOff != true && x.IsDeleted == false).ToList().OrderBy(x => x.Rank.CrewSort).ToList();

            ViewBag.vessels = _context.TblVessels.Where(x => x.IsDeleted == false && x.IsActive == false && x.VesselId == 75).ToList();

            return View(crewlist);


        }

        public IActionResult CBA()
        {
            ViewBag.vesselDetails = _context.TblVessels.Include(x => x.Flag).Include(x => x.Ship).Where(x => x.IsDeleted == false && x.VesselId == 75).FirstOrDefault();

            var vcm = _context.TblVesselCbas.Include(c=>c.OffCBA).Include(x=>x.RatingCBA).Include(x => x.Country).Where(x => x.IsDeleted == false && x.VesselId == 75).ToList();
            ViewBag.vessels = _context.TblVessels.Where(x => x.IsDeleted == false && x.IsActive == false && x.VesselId == 75).ToList();
            return View(vcm);
        }
        public string ConvrtToTitlecase(string value)
        {
            if (value != null)
            {
                return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value?.ToLower());
            }
            else
                return value;
        }

        public IActionResult AtlanticExcelFile(int vesselId = 75)
        {
         IEnumerable<TblCrewList> CrewList = _context.TblCrewLists.Include(c=>c.Crew).Include(r => r.Rank).Include(ct=>ct.Crew.Country).Include(po=>po.Vessel.PortOfTakeovers).Where(x => x.IsDeleted == false && x.VesselId == vesselId && x.IsDeleted == false && x.CrewId!=null).OrderBy(r=>r.Rank.CrewSort).ToList();
            
            string fileName =  "Atlantic Crewlist" + ".xlsx";
            string path_Root = _appEnvironment.WebRootPath;
            //string fullPath = path_Root + "\\Upload\\ExcelFile\\" + fileName;


            //if (employees.Count == 0) return File(Stream.Null, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            //            "employeeReport.xlsx");

            using (var workbook = new XLWorkbook())
            {

                var worksheet = workbook.Worksheets.Add("Atlantic Crewlist");
               
                var imagePath = path_Root + "\\Upload\\Alogo\\Atlanticlogo.png";
                var image = worksheet.AddPicture(imagePath).MoveTo(worksheet.Cell("A1")).WithSize(60, 76);
               
                worksheet.Range("A1:A4").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range("A1:A4").Style.Border.InsideBorderColor = XLColor.White;
               
                worksheet.Column("A").Width = 8.43;
                worksheet.Column("B").Width = 35.29;
                worksheet.Column("C").Width = 22.29;
                worksheet.Column("D").Width = 22.29;
                worksheet.Column("E").Width = 14.29;
                worksheet.Column("F").Width = 17.29;
                worksheet.Column("G").Width = 15.29;
                worksheet.Column("H").Width = 21.29;
                worksheet.Column("I").Width = 16.29;

                worksheet.Range("A5:D5").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                worksheet.Range("A5:D5").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                worksheet.Range("A5:D5").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range("A5:D5").Style.Border.TopBorderColor = XLColor.Black;
                worksheet.Range("A5:D5").Merge();


                var currentRow = 6;
                worksheet.Row(currentRow).Style.Font.SetFontColor(XLColor.White);
                worksheet.Range("A6:I6").Style.Fill.SetBackgroundColor(XLColor.FromHtml("253B5B"));
                worksheet.Row(currentRow).Style.Font.Bold = true;

                worksheet.Cell(currentRow, 1).Value = "Sr. No.";
                worksheet.Cell(currentRow, 2).Value = "Crew name";
                worksheet.Cell(currentRow, 3).Value = "Rank";
                worksheet.Cell(currentRow, 4).Value = "Nationality";
                worksheet.Cell(currentRow, 5).Value = "Passport";
                worksheet.Cell(currentRow, 6).Value = "CDC";
                worksheet.Cell(currentRow, 7).Value = "Date signed on";
                worksheet.Cell(currentRow, 8).Value = "Relief date";
                worksheet.Cell(currentRow, 9).Value = "Port of joining";
                worksheet.Range("A6:I6").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                foreach (var applicant in CrewList)
                {
                    var passport = _context.TblPassports.Where(p => p.CrewId == applicant.CrewId && p.IsDeleted == false).FirstOrDefault();
                    var cdc = _context.TblCdcs.Where(p => p.CrewId == applicant.CrewId && p.IsDeleted == false).FirstOrDefault();


                    worksheet.Cell("A5").Value = "Vessel name:" + applicant.Vessel.VesselName;
                    worksheet.Cell("A5").Style.Font.Bold = true;
                    worksheet.Cell("A5").Style.Font.FontSize = 14;
                    worksheet.Rows("5").Height = 18.75;
                    worksheet.Cell("A5").Style.Font.FontName = "Calibri Light";

                    worksheet.Cell("H2").Value = "Life Boat Capacity:";
                    worksheet.Cell("H3").Value = "Accommodation:";
                    worksheet.Cell("H4").Value = "Report Date:";
                    worksheet.Cell("H2").Style.Font.Underline = XLFontUnderlineValues.Single;
                    worksheet.Cell("H3").Style.Font.Underline = XLFontUnderlineValues.Single;
                    worksheet.Cell("H4").Style.Font.Underline = XLFontUnderlineValues.Single;
                    worksheet.Range("H2:H4").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    worksheet.Cell("I2").Value = applicant.Vessel.Capacity;
                    worksheet.Cell("I3").Value = applicant.Vessel.AccommodationBerth;
                    worksheet.Cell("I4").Value = DateTime.Now.ToString("dd/MMM/yyyy");

                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = currentRow - 6;
                    worksheet.Cell(currentRow, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Column("A").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                    worksheet.Cell(currentRow, 2).Value = $"{ConvrtToTitlecase(applicant.Crew?.FirstName)} {ConvrtToTitlecase(applicant.Crew?.MiddleName)} {ConvrtToTitlecase(applicant.Crew?.LastName)}";
                    worksheet.Cell(currentRow, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 3).Value = ConvrtToTitlecase(applicant.Rank?.Code);
                    worksheet.Cell(currentRow, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 4).Value = applicant.Crew?.Country?.CountryName;
                    worksheet.Cell(currentRow, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 5).Value = passport?.PassportNumber;
                    worksheet.Cell(currentRow, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 6).Value = cdc?.Cdcnumber;
                    worksheet.Cell(currentRow, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                    worksheet.Cell(currentRow, 7).Value = applicant.SignOnDate?.ToString("dd/MMM/yyyy");
                    worksheet.Cell(currentRow, 7).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 8).Value = applicant.Crew?.ReliefDate?.ToString("dd/MMM/yyyy");
                    worksheet.Cell(currentRow, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 9).Value = applicant.Vessel.PortOfTakeovers.SeaportName;
                    worksheet.Cell(currentRow, 9).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    workbook.SaveAs(fileName);
                }
                worksheet.Cells().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Columns().AdjustToContents();
                worksheet.Rows().AdjustToContents();
                var errorMessage = "you can return the errors here!";
             
                return Json(new { fileName = fileName, errorMessage });
    
            }
        }

        public ActionResult DownloadXL(string fileName)
        {
            //string path_Root = _appEnvironment.WebRootPath;
            //string fullPath = path_Root + "\\Upload\\ExcelFile\\" + fileName; 
            byte[] fileByteArray = System.IO.File.ReadAllBytes(fileName);
            System.IO.File.Delete(fileName);
            return File(fileByteArray, "application/vnd.ms-excel", fileName);
        }

        public IActionResult GetOCIMFExcelFile(int vesselId = 75)
        {
            var crew = _context.OCIMFVMs.FromSqlRaw("GetOCIMF @p0", vesselId).ToList();

            string fileName = "OCIMF" + ".xlsx";
            string path_Root = _appEnvironment.WebRootPath;
           

            using (var workbook = new XLWorkbook())
            {

                var worksheet = workbook.Worksheets.Add("OCIMF");

                var imagePath = path_Root + "\\Upload\\Alogo\\ATLANTAS2.png";
                var image = worksheet.AddPicture(imagePath).MoveTo(worksheet.Cell("A1")).WithSize(101, 77);

                worksheet.Range("A1:A4").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range("A1:A4").Style.Border.InsideBorderColor = XLColor.White;

                worksheet.Column("A").Width = 14.29;
                worksheet.Column("B").Width = 14.29;
                worksheet.Column("C").Width = 14.29;
                worksheet.Column("D").Width = 14.29;
                worksheet.Column("E").Width = 14.29;
                worksheet.Column("F").Width = 14.29;
                worksheet.Column("G").Width = 14.29;
                worksheet.Column("H").Width = 14.29;
                worksheet.Column("I").Width = 54.29;
                worksheet.Column("J").Width = 16.29;
                worksheet.Column("K").Width = 14.29;
                worksheet.Column("L").Width = 107.86;
                worksheet.Column("M").Width = 28.29;
                worksheet.Column("N").Width = 20.29;
                worksheet.Column("O").Width = 20.29;
                worksheet.Column("P").Width = 14.29;
                worksheet.Column("Q").Width = 20.29;
                worksheet.Column("R").Width = 16.29;
                worksheet.Column("S").Width = 18.29;
                worksheet.Column("T").Width = 22.29;
                worksheet.Column("U").Width = 17.29;


                worksheet.Range("C2:E3").Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C2:E3").Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Cell("C2").Value = "OCIMF";
                worksheet.Cell("C2").Style.Font.Shadow = true;
                worksheet.Cell("C2").Style.Font.FontSize = 14;
                worksheet.Cell("C2").Style.Font.Bold = true;
                worksheet.Cell("C2").Style.Font.FontName = "Calibri Light";
                worksheet.Cell("C2").Style.Font.Underline = XLFontUnderlineValues.Single;
                worksheet.Cell("C2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell("C2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range("C2:E3").Merge();


                var currentRow = 6;
                worksheet.Row(currentRow).Style.Font.SetFontColor(XLColor.White);
                worksheet.Range("A6:U6").Style.Fill.SetBackgroundColor(XLColor.FromHtml("253B5B"));
                worksheet.Row(currentRow).Style.Font.Bold = true;

                worksheet.Cell(currentRow, 1).Value = "Sr. No.";
                worksheet.Cell(currentRow, 2).Value = "Vessel name";
                worksheet.Cell(currentRow, 3).Value = "Rank name";
                worksheet.Cell(currentRow, 4).Value = "Family name";
                worksheet.Cell(currentRow, 5).Value = "First name";
                worksheet.Cell(currentRow, 6).Value = "Middle initial";
                worksheet.Cell(currentRow, 7).Value = "Identification";
                worksheet.Cell(currentRow, 8).Value = "Nationality";
                worksheet.Cell(currentRow, 9).Value = "Certificate of competency";
                worksheet.Cell(currentRow, 10).Value = "Issuing country";
                worksheet.Cell(currentRow, 11).Value = "Admin accept";
                worksheet.Cell(currentRow, 12).Value = "Tanker certificate";
                worksheet.Cell(currentRow, 13).Value = "Specialized tanker training";
                worksheet.Cell(currentRow, 14).Value = "Radio qualification";
                worksheet.Cell(currentRow, 15).Value = "English proficiency";
                worksheet.Cell(currentRow, 16).Value = "Sign on date";
                worksheet.Cell(currentRow, 17).Value = "Operator experience";
                worksheet.Cell(currentRow, 18).Value = "Rank experience";
                worksheet.Cell(currentRow, 19).Value = "Tanker experience";
                worksheet.Cell(currentRow, 20).Value = "All tanker experience";
                worksheet.Cell(currentRow, 21).Value = "Total experience";


                worksheet.Range("A6:U6").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                foreach (var applicant in crew)
                {

                    worksheet.Cell("G2").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                    worksheet.Cell("G3").Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                    worksheet.Range("G2:G3").Style.Border.RightBorder = XLBorderStyleValues.Medium;
                    worksheet.Range("G2:G3").Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                    worksheet.Range("G2:G3").Style.Border.TopBorderColor = XLColor.Black;
                 

                    worksheet.Cell("G2").Value = "Report Date:";  
                    worksheet.Range("G2:G3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    worksheet.Cell("G3").Value = DateTime.Now.ToString("dd/MMM/yyyy");
                    worksheet.Cell("G2").Style.Font.Bold = true;

                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = currentRow - 6;
                    worksheet.Cell(currentRow, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Column("A").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                    worksheet.Cell(currentRow, 2).Value = applicant.VesselName;
                    worksheet.Cell(currentRow, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                   
                    worksheet.Cell(currentRow, 3).Value = applicant.RankName;
                    worksheet.Cell(currentRow, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 4).Value = applicant.FamilyName;
                    worksheet.Cell(currentRow, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 5).Value = applicant.FirstName;
                    worksheet.Cell(currentRow, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 6).Value = applicant.MiddleInitial;
                    worksheet.Cell(currentRow, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 7).Value = applicant.Identification;
                    worksheet.Cell(currentRow, 7).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 8).Value = applicant.Nationality;
                    worksheet.Cell(currentRow, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 9).Value = applicant.CertofComp;
                    worksheet.Cell(currentRow, 9).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 10).Value = applicant.IssuingCountry;
                    worksheet.Cell(currentRow, 10).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 11).Value = applicant.AdminAccept;
                    worksheet.Cell(currentRow, 11).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 12).Value = applicant.TankerCert;
                    worksheet.Cell(currentRow, 12).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 13).Value = applicant.SpecializedTankerTraining;
                    worksheet.Cell(currentRow, 13).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 14).Value = applicant.RadioQual;
                    worksheet.Cell(currentRow, 14).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 15).Value = applicant.EnglishProf;
                    worksheet.Cell(currentRow, 15).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 16).Value = applicant.SignOnDate;
                    worksheet.Cell(currentRow, 16).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 17).Value = applicant.OperatorExp;
                    worksheet.Cell(currentRow, 17).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 18).Value = applicant.RankExperience;
                    worksheet.Cell(currentRow, 18).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 19).Value = applicant.TankerExp;
                    worksheet.Cell(currentRow, 19).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 20).Value = applicant.AllTankerExp;
                    worksheet.Cell(currentRow, 20).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 21).Value = applicant.TotalExperience;
                    worksheet.Cell(currentRow, 21).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    workbook.SaveAs(fileName);
                }
                worksheet.Cells().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Columns().AdjustToContents();
                worksheet.Rows().AdjustToContents();
                var errorMessage = "you can return the errors here!";

                return Json(new { fileName = fileName, errorMessage });

            }

        }

        public IActionResult OLPExcelFile(int vesselId = 75)
        {
            IEnumerable<TblCrewList> CrewList = _context.TblCrewLists.Include(c => c.Crew).Include(r => r.Rank).Include(ct => ct.Crew.Country).Include(p=>p.Vessel.Pool).Where(x => x.IsDeleted == false && x.VesselId == vesselId && x.IsDeleted == false && x.CrewId != null).OrderBy(r => r.Rank.CrewSort).ToList();

            string fileName = "Seafarers list v1.0" + ".xlsx";
            string path_Root = _appEnvironment.WebRootPath;
            
            using (var workbook = new XLWorkbook())
            {

                var worksheet = workbook.Worksheets.Add("Seafarers list v1.0");


                worksheet.Column("A").Width = 14.29;
                worksheet.Column("B").Width = 30.29;
                worksheet.Column("C").Width = 16.29;
                worksheet.Column("D").Width = 14.29;
                worksheet.Column("E").Width = 23.29;
                worksheet.Column("F").Width = 14.29;
                worksheet.Column("G").Width = 33.29;
                worksheet.Column("H").Width = 14.29;
                worksheet.Column("I").Width = 14.29;
                worksheet.Column("J").Width = 17.29;
                worksheet.Column("K").Width = 25.29;
                worksheet.Column("L").Width = 25.29;

                var currentRow = 1;
               
                worksheet.Cell(currentRow, 1).Value = "vUserID";
                worksheet.Cell(currentRow, 2).Value = "vFirstname";
                worksheet.Cell(currentRow, 3).Value = "vLastname";
                worksheet.Cell(currentRow, 4).Value = "dtBirthdate";
                worksheet.Cell(currentRow, 5).Value = "uRankId";
                worksheet.Cell(currentRow, 6).Value = "vSex";
                worksheet.Cell(currentRow, 7).Value = "vPlaceOfBirth";
                worksheet.Cell(currentRow, 8).Value = "uCountryID";
                worksheet.Cell(currentRow, 9).Value = "vDocumentNo";
                worksheet.Cell(currentRow, 10).Value = "uPersonnelPoolID";
                worksheet.Cell(currentRow, 11).Value = "iManagedByID";
                worksheet.Cell(currentRow, 12).Value = "vEmail";
             

                foreach (var applicant in CrewList)
                {
                    var passport = _context.TblPassports.Where(p => p.CrewId == applicant.CrewId && p.IsDeleted == false).FirstOrDefault();
                    var Email = _context.TblCrewAddresses.Where(p => p.CrewId == applicant.CrewId && p.IsDeleted == false).FirstOrDefault();
                 
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = applicant.Crew?.EmpNumber;
                    worksheet.Cell(currentRow, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                   
                    worksheet.Cell(currentRow, 2).Value = $"{ConvrtToTitlecase(applicant.Crew?.FirstName)} {ConvrtToTitlecase(applicant.Crew?.MiddleName)}";
                    worksheet.Cell(currentRow, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 3).Value = ConvrtToTitlecase(applicant.Crew?.LastName);
                    worksheet.Cell(currentRow, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 4).Value = applicant.Crew?.Dob?.ToString("dd/MMM/yyyy");
                    worksheet.Cell(currentRow, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 5).Value = applicant.Rank.RankName;
                    worksheet.Cell(currentRow, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 6).Value = applicant.Crew?.Gender;
                    worksheet.Cell(currentRow, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;


                    worksheet.Cell(currentRow, 7).Value = applicant.Crew?.PlaceOfBirth;
                    worksheet.Cell(currentRow, 7).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 8).Value = applicant.Crew?.Country?.Iso3;
                    worksheet.Cell(currentRow, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 9).Value = passport?.PassportNumber;
                    worksheet.Cell(currentRow, 9).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 10).Value = applicant.Vessel?.Pool?.PoolName;
                    worksheet.Cell(currentRow, 10).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 11).Value = "Atlantas Crew Management";
                    worksheet.Cell(currentRow, 11).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 12).Value = Email?.Email;
                    worksheet.Cell(currentRow, 12).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    workbook.SaveAs(fileName);
                }
                worksheet.Cells().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Columns().AdjustToContents();
                worksheet.Rows().AdjustToContents();
                var errorMessage = "you can return the errors here!";

                return Json(new { fileName = fileName, errorMessage });

            }
        }

        public IActionResult getIMOdata(int vesselId = 75)
        {
            var CrewList = _context.TblCrewLists.Include(c => c.Crew).Include(r => r.Rank).Include(ct => ct.Crew.Country).Include(p => p.Vessel).Where(x => x.IsDeleted == false && x.VesselId == vesselId && x.IsDeleted == false && x.CrewId != null).OrderBy(r => r.Rank.CrewSort).ToList();

           
            ViewBag.vesselDetails = _context.TblVessels.Include(x => x.Flag).Include(x => x.Ship).Where(x => x.IsDeleted == false && x.VesselId == vesselId).ToList();
           

            var iMOCrewLists = CrewList.AsEnumerable() // Client-side from here on
                       .Select((t, index) => new IMOCrewListVM
                       {
                           RowNumber = index + 1,
                           CrewListId = t.CrewListId,
                           Rank = t.Rank.Code,
                           FirstName = t.Crew?.FirstName,
                           LastName = t.Crew?.LastName,
                           MiddleName = t.Crew?.MiddleName,
                           nationality = t.Crew?.Country?.Nationality,
                           DOB = t.Crew?.Dob ?? DateTime.Parse("01-01-1900"),
                           PassportNo = _context.TblPassports.Where(p => p.CrewId == t.CrewId && p.IsDeleted == false).FirstOrDefault()?.PassportNumber,
                           BirthPlace = t.Crew?.PlaceOfBirth
                       }) ;


            if (CrewList.Count() > 20)
            {
                IMOFull = true;
                ViewBag.count = IMOFull;
                ViewBag.imoData = iMOCrewLists.Take(20).OrderBy(x => x.RowNumber);
            }
            else
            {
                IMOFull = false;
                ViewBag.count = IMOFull;
                ViewBag.imoData = iMOCrewLists;
            }


            return PartialView();

        }


        public IActionResult getIMOdata2(int vesselId =75)
        {

            var CrewList = _context.TblCrewLists.Include(c => c.Crew).Include(r => r.Rank).Include(ct => ct.Crew.Country).Include(p => p.Vessel).Where(x => x.IsDeleted == false && x.VesselId == vesselId && x.IsDeleted == false && x.CrewId != null).OrderBy(r => r.Rank.CrewSort).ToList();

          
            var iMOCrewLists2 = CrewList.AsEnumerable() // Client-side from here on
                       .Select((s, indexs) => new IMOCrewListVM
                       {
                           RowNumber = indexs + 1,
                           CrewListId = s.CrewListId,
                           Rank = s.Rank.Code,
                           FirstName = s.Crew?.FirstName,
                           LastName = s.Crew?.LastName,
                           MiddleName = s.Crew?.MiddleName,
                           nationality = s.Crew?.Country?.Nationality,
                           DOB = s.Crew?.Dob ?? DateTime.Parse("01-01-1900"),
                           PassportNo = _context.TblPassports.Where(p => p.CrewId == s.CrewId && p.IsDeleted == false).FirstOrDefault()?.PassportNumber,
                           BirthPlace = s.Crew?.PlaceOfBirth
                       });


            if (CrewList.Count() > 20)
            {
                ViewBag.imoData2 = iMOCrewLists2.Where(x => x.RowNumber > 20);
            }
             return PartialView();
        }



        #region IMO PDF
        //Generate IMO PDF from crewlist

        public JsonResult GenerateIMOPDF(int vesselId = 75)
        {
            
            var CrewList = _context.TblCrewLists.Include(c => c.Crew).Include(r => r.Rank).Include(ct => ct.Crew.Country).Include(p => p.Vessel).Where(x => x.IsDeleted == false && x.VesselId == vesselId && x.IsDeleted == false && x.CrewId != null).OrderBy(r => r.Rank.CrewSort).ToList();

            //string url =  "api/crewlist/getIMOdata?vesselId=" + vesselId;
            string url = "https://localhost:44336/Home/getIMOdata";
           
            var webRoot = _appEnvironment.WebRootPath;
            string headerUrl = System.IO.Path.Combine(webRoot, "PDFHeaders/PDF_HeaderIMO.htm");
            string pdf_page_size = PdfPageSize.A4.ToString();
            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), pdf_page_size, true);
            string pdf_orientation = PdfPageOrientation.Portrait.ToString();
            PdfPageOrientation pdfOrientation = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                pdf_orientation, true);
            int webPageWidth = 1000;
            int webPageHeight = 0;
            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();
            converter.Header.DisplayOnFirstPage = true;
         
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;
            converter.Options.MarginLeft = 10;
            converter.Options.MarginRight = 10;
            int headerHeight = 80;
            int footerHeight = 15;
            // header settings
            converter.Options.DisplayHeader = true;
            converter.Header.DisplayOnFirstPage = true;
            converter.Header.DisplayOnOddPages = true;
            converter.Header.DisplayOnEvenPages = true;
            converter.Header.Height = headerHeight;
            PdfHtmlSection headerHtml = new PdfHtmlSection(headerUrl);
            headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Header.Add(headerHtml);
            // footer settings
            converter.Options.DisplayFooter = true;
            converter.Footer.DisplayOnFirstPage = true;
            converter.Footer.DisplayOnOddPages = true;
            converter.Footer.DisplayOnEvenPages = true;
            converter.Footer.Height = footerHeight;
            // create a new pdf document converting an url
            PdfDocument doc1 = converter.ConvertUrl(url);


            //page2 

            string url2 = "https://localhost:44336/Home/getIMOdata2";
            //string url2 = localpath+"api/crewlist/getIMOdata2?vesselId=" + vesselId;
            string pdf_page_size2 = PdfPageSize.A4.ToString();
            PdfPageSize pageSize2 = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), pdf_page_size2, true);
            string pdf_orientation2 = PdfPageOrientation.Portrait.ToString();
            PdfPageOrientation pdfOrientation2 = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                pdf_orientation2, true);
            int webPageWidth2 = 1000;
            int webPageHeight2 = 0;
            // instantiate a html to pdf converter object
            HtmlToPdf converter2 = new HtmlToPdf();
            converter2.Options.PdfPageSize = pageSize2;
            converter2.Options.PdfPageOrientation = pdfOrientation2;
            converter2.Options.WebPageWidth = webPageWidth2;
            converter2.Options.WebPageHeight = webPageHeight2;
            converter2.Options.MarginLeft = 10;
            converter2.Options.MarginRight = 10;
            int headerHeight2 = 80;
            int footerHeight2 = 15;
            // header settings
            converter2.Options.DisplayHeader = true;
            converter2.Header.DisplayOnFirstPage = true;
            converter2.Header.DisplayOnOddPages = true;
            converter2.Header.DisplayOnEvenPages = true;
            converter2.Header.Height = headerHeight2;
            PdfHtmlSection headerHtml2 = new PdfHtmlSection(headerUrl);
            headerHtml2.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter2.Header.Add(headerHtml2);
            // footer settings
            converter2.Options.DisplayFooter = true;
            converter2.Footer.DisplayOnFirstPage = true;
            converter2.Footer.DisplayOnOddPages = true;
            converter2.Footer.DisplayOnEvenPages = true;
            converter2.Footer.Height = footerHeight2;


            PdfDocument doc = new PdfDocument();
            doc.Append(doc1);
            if (CrewList.Count() > 20)
            {
                PdfDocument doc2 = converter2.ConvertUrl(url2);
                doc.Append(doc2);
            }
            string fileName = "IMOCrewList" + DateTime.Now.ToString("dd-MMM-yyyy.hhmmss") + ".pdf";
           
            doc.Save(fileName);

            return Json(new { fileName = fileName });
        }

        #endregion


        public IActionResult FPD012(int vesselId = 75)
        {
            var CrewList = _context.TblCrewLists.Include(c => c.Crew).Include(r => r.Rank).Include(ct => ct.Crew.Country).Include(p => p.Vessel).Where(x => x.IsDeleted == false && x.VesselId == vesselId && x.IsDeleted == false && x.CrewId != null).OrderBy(r => r.Rank.CrewSort).ToList();


            ViewBag.vesselDetails = _context.TblVessels.Include(x => x.Flag).Include(x => x.Ship).Where(x => x.IsDeleted == false && x.VesselId == vesselId).ToList();


            var FPDCrewLists = CrewList.AsEnumerable() // Client-side from here on
                       .Select((t, index) => new CrewListDownload
                       {
                           SNo = index + 1,
                           VesselName = t.Vessel?.VesselName,
                           CrewName = (t.Crew.LastName == null ? "" : t.Crew.LastName) + " " + (t.Crew.FirstName == null ? "" : t.Crew.FirstName )+ " " + (t.Crew.MiddleName == null ? "" : t.Crew.MiddleName),
                           Rank = t.Rank?.Code,
                           Nationality = t.Crew?.Country?.Nationality,
                           Passport = _context.TblPassports.Where(p => p.CrewId == t.CrewId && p.IsDeleted == false).FirstOrDefault()?.PassportNumber,
                           CDC = _context.TblCdcs.Where(p => p.CrewId == t.CrewId && p.IsDeleted == false).FirstOrDefault()?.Cdcnumber,
                           DateSignedOn = t.SignOnDate,
                           ReliefDate = t.DueDate,
                           PortOfJoining = _context.TblActivitySignOns.Include(s=>s.Seaport).Where(c=>c.CrewId ==         t.CrewId).FirstOrDefault().Seaport?.SeaportName,
                           DOB = t.Crew?.Dob ?? DateTime.Parse("01-01-1900"),

                       });


            if (CrewList.Count() > 20)
            {
                IMOFull = true;
                ViewBag.count = IMOFull;
                //ViewBag.fpd2 = FPDCrewLists.Take(30).OrderBy(x => x.SNo);
                ViewBag.fpd2 = FPDCrewLists.OrderBy(x => x.SNo);
            }
            else
            {
                IMOFull = false;
                ViewBag.count = IMOFull;
                ViewBag.fpd2 = FPDCrewLists;
            }


            return PartialView();

        }


        public JsonResult generateFPD01(int vesselId =75)
        {
            var CrewList = _context.TblCrewLists.Include(c => c.Crew).Include(r => r.Rank).Include(ct => ct.Crew.Country).Include(p => p.Vessel).Where(x => x.IsDeleted == false && x.VesselId == vesselId && x.IsDeleted == false && x.CrewId != null).OrderBy(r => r.Rank.CrewSort).ToList();

            string url = "https://localhost:44336/Home/FPD012";
            //string url = serverUrl + "api/crewlist/fpd01?vesselId=" + vesselId;
            // string url = localpath+"api/crewlist/getIMOdata?vesselId=" + vesselId;
            var webRoot = _appEnvironment.WebRootPath;
            string headerUrl = System.IO.Path.Combine(webRoot, "PDFHeaders/PDF_HeaderIMO.htm");
            //string footerUrl = System.IO.Path.Combine(webRoot, "PDFHeaders/PDF_Footer.htm");
            string pdf_page_size = PdfPageSize.A4.ToString();
            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), pdf_page_size, true);
            string pdf_orientation = PdfPageOrientation.Portrait.ToString();
            PdfPageOrientation pdfOrientation = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                pdf_orientation, true);
            int webPageWidth = 1000;
            int webPageHeight = 0;
            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;
            converter.Options.MarginLeft = 10;
            converter.Options.MarginRight = 10;
            int footerHeight = 15;
            // footer settings
            converter.Options.DisplayFooter = true;
            converter.Footer.DisplayOnFirstPage = true;
            converter.Footer.DisplayOnOddPages = true;
            converter.Footer.DisplayOnEvenPages = true;
            converter.Footer.Height = footerHeight;
            // create a new pdf document converting an url
            PdfDocument doc1 = converter.ConvertUrl(url);

            //page2 
            //string url2 = serverUrl + "api/crewlist/fpd02?vesselId=" + vesselId;
            ////string url2 = localpath+"api/crewlist/getIMOdata2?vesselId=" + vesselId;
            //string pdf_page_size2 = PdfPageSize.A4.ToString();
            //PdfPageSize pageSize2 = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), pdf_page_size2, true);
            //string pdf_orientation2 = PdfPageOrientation.Portrait.ToString();
            //PdfPageOrientation pdfOrientation2 = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
            //    pdf_orientation2, true);
            //int webPageWidth2 = 1000;
            //int webPageHeight2 = 0;
            //// instantiate a html to pdf converter object
            //HtmlToPdf converter2 = new HtmlToPdf();
            //converter2.Options.PdfPageSize = pageSize2;
            //converter2.Options.PdfPageOrientation = pdfOrientation2;
            //converter2.Options.WebPageWidth = webPageWidth2;
            //converter2.Options.WebPageHeight = webPageHeight2;
            //converter2.Options.MarginLeft = 10;
            //converter2.Options.MarginRight = 10;
            //int footerHeight2 = 15;
            //// footer settings
            //converter2.Options.DisplayFooter = true;
            //converter2.Footer.DisplayOnFirstPage = true;
            //converter2.Footer.DisplayOnOddPages = true;
            //converter2.Footer.DisplayOnEvenPages = true;
            //converter2.Footer.Height = footerHeight2;

            PdfDocument doc = new PdfDocument();
            doc.Append(doc1);
            //if (CrewList.Count() > 30)
            //{
            //    PdfDocument doc2 = converter2.ConvertUrl(url2);
            //    doc.Append(doc2);
            //}

            string fileName = "FPD01Crewlist" + DateTime.Now.ToString("dd-MMM-yyyy.hhmmss") + ".pdf";
           
           
            doc.Save(fileName);


            return Json(new { fileName = fileName });
        }

        public IActionResult UserLogin()
        {
  
            return PartialView();
        }

        public IActionResult Login(string Username ,string Userpwd)
       {
            var User = _context.Userlogins.SingleOrDefault(x => x.UserName == Username && x.Password == Userpwd && x.IsDeleted == false);

            if (User != null)
            {
                //var actions = vwCrewList();     
                return RedirectToAction("vwCrewList");
            }

            return RedirectToAction("UserLogin");
        }

        public IActionResult LogOut()
        {
           
            return PartialView();
        }

        public IActionResult passwordView()
        {
            return PartialView();
        }

      
        public JsonResult Changepassword(string oldpwd, string newpwd, string crfmpwd)
        {
            var User = _context.Userlogins.SingleOrDefault(x => x.Password == oldpwd && x.IsDeleted == false);          
            if(User!= null && newpwd == crfmpwd)
            {
                User.Password = newpwd;
                User.ModifiedDate = DateTime.Now;
                _context.Userlogins.Update(User);
                _context.SaveChanges();
                return Json("success");
            }
            return Json("fail");
        }



    }
}
