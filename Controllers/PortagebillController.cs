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
using System.Data;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using Ionic.Zip;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using System.Net.Mail;

namespace crewlinkship.Controllers
{
    public class PortagebillController : Controller
    {
        private readonly shipCrewlinkContext _context;
        private readonly IHostingEnvironment _appEnvironment;
        private readonly IConfiguration _configuration;
        private int vesselidtouse;
        public PortagebillController(shipCrewlinkContext context, IHostingEnvironment appEnvironment, IConfiguration configuration)
        {
            // _logger = logger;
            _context = context;
            _appEnvironment = appEnvironment; _configuration = configuration;
            vesselidtouse= _configuration.GetValue<int>("vesselinfo:checkvessel:datafor");
        }
        // public int vesselidtouse { get => _configuration.GetValue<int>("vesselinfo:checkvessel:datafor"); }
        //public IActionResult Index()
        //{
        //    /*int vesselId = 138; int month = 2;*/
        //    int year = 2023; string ispromoted = "no"; string checkpbtilldate = "";
        //    var data = _context.PortageBillVMs.FromSqlRaw<PortageBillVM>("getPortageBill @p0, @p1, @p2, @p3, @p4", 75, month, year, ispromoted, checkpbtilldate);
        //    ViewBag.vessel = new SelectList(_context.TblVessels.Where(x => x.VesselId == 75), "VesselId", "VesselName");
        //    ViewBag.vesselDetails = _context.TblVessels.Include(x => x.Flag).Include(x => x.Ship).Where(x => x.IsDeleted == false && x.VesselId == 75).FirstOrDefault();
        //    ViewBag.vessels = _context.TblVessels.Where(x => x.IsDeleted == false && x.IsActive == false && x.VesselId == 75).ToList();
        //    return View(data);
        //}

        // [Route("Portagebill/Index/{vesselId:int}/{month:int}/{year:int}")]


        [HttpGet]
        public IActionResult GetAccounts()
        {
            var accounts = _context.TblWageComponents.Include(x => x.SubCode).Where(x => x.Earning == "Reimbursement").ToList();
            return Json(accounts);
        }

        [HttpGet]
        public IActionResult GetAccountsbyid(string id)
        {
            var accounts = _context.TblWageComponents.Include(x => x.SubCode).Where(x => x.SubCodeId.ToString() == id).Select(x=>x.Earning);
            return Json(accounts);
        }


        [HttpGet]
        public IActionResult PortageReimbursementDetailsbyid( int portageBillId,  string type)
        {
            var ReimbursementDetails = _context.PortageEarningDeduction.Where(x => x.PortageBillId== portageBillId &&  x.Type == type).ToList();
            return Json(ReimbursementDetails);
        }



        


        [HttpGet]
        public IActionResult GetAccountotherearnings()
        {
            var accounts = _context.TblWageComponents.Include(x => x.SubCode).Where(x => x.Earning == "Earning").ToList();
            return Json(accounts);
        }

        [HttpGet]
        public IActionResult GetAccountOtherDeductions()
        {
            var accounts = _context.TblWageComponents.Include(x => x.SubCode).Where(x => x.Earning == "Deduction").ToList();
            return Json(accounts);
        }

        [HttpPost]
        public IActionResult ReceiveData([FromBody] List<TblWageComponent> entries)
        {
            if (entries == null || entries.Count == 0)
            {
                return BadRequest("No data received.");
            }

            // Process the entries (e.g., save to database, etc.)
            // For this example, we'll just log them to the console
            foreach (var entry in entries)
            {
                // Replace this with your processing logic
                //System.Console.WriteLine($"ID: {entry.Id}, Name: {entry.Name}, Balance: {entry.Balance}");
            }

            return Ok("Data received successfully.");
        }
    


    //[HttpGet]
    //public IActionResult GetAccountcode(int id )
    //{
    //    var accounts = _context.TblWageComponents.Include(x => x.SubCode).Where(x => x.Earning == "Reimbursement" && x.SubCodeId==id ).Select(x=>x.SubCode.SubCode + " "+ x.SubCode.SubBudget);
    //    return Json(accounts);
    //}

    [HttpGet]
        public IActionResult GetAccountcode(int id)
        {
            var accounts = _context.TblWageComponents
                .Include(x => x.SubCode)
                .Where(x => x.Earning == "Reimbursement" && x.SubCodeId == id)
                .Select(x => x.SubCode.SubCode + " " + x.SubCode.SubBudget)
                .ToList(); // Ensure to call ToList to execute the query

            return Json(accounts);
        }

        [HttpGet]
        public IActionResult GetAccountotherearningcode(int id)
        {
            var accounts = _context.TblWageComponents
                .Include(x => x.SubCode)
                .Where(x => x.Earning == "Earning" && x.SubCodeId == id)
                .Select(x => x.SubCode.SubCode + " " + x.SubCode.SubBudget)
                .ToList(); // Ensure to call ToList to execute the query

            return Json(accounts);
        }

        [HttpGet]
        public IActionResult GetAccountOtherDeductionscode(int id)
        {
            var accounts = _context.TblWageComponents
                .Include(x => x.SubCode)
                .Where(x => x.Earning == "Deduction" && x.SubCodeId == id)
                .Select(x => x.SubCode.SubCode + " " + x.SubCode.SubBudget)
                .ToList(); // Ensure to call ToList to execute the query

            return Json(accounts);
        }

        [HttpGet]
        public IActionResult Index(int? vesselId, int? month, int? year)
        {
            string checkpbtilldate = "";
            var accessToken = HttpContext.Session.GetString("token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            try
            {

            
            if (accessToken != null)
            {
                ViewBag.name = HttpContext.Session.GetString("name");

            var data = _context.PortageBillVMs.FromSqlRaw<PortageBillVM>("getPortageBill @p0, @p1, @p2, @p3, @p4", vesselId, month, year, "no", checkpbtilldate);

            ViewBag.vessel = new SelectList(_context.TblVessels.Where(x => x.VesselId == vesselidtouse), "VesselId", "VesselName");
                ViewBag.vesselDetails = _context.TblVessels.Include(x => x.Flag).Include(x => x.Ship).Where(x => x.IsDeleted == false && x.VesselId == vesselidtouse).FirstOrDefault();
                ViewBag.vessels = _context.TblVessels.Where(x => x.IsDeleted == false && x.IsActive == false && x.VesselId == vesselId).ToList();
                var promotiondata = _context.PortageBillVMs.FromSqlRaw<PortageBillVM>("spPromotionPortageBill @p0, @p1, @p2, @p3", vesselidtouse, month, year, "yes");
                var signoffcrewdata = _context.PortageBillSignoffVM.FromSqlRaw<PortageBillSignoffVM>("getPortageBillOffSigners @p0, @p1, @p2", vesselidtouse, month, year);

                    ViewBag.portBill = _context.TblPortageBills.Where(x => x.IsDeleted == false && x.From.Value.Month == month && x.From.Value.Year == year && x.Vesselid == vesselId).ToList().FirstOrDefault()?.BillStatus;


                    var tables = new PortageViewModel
                {
                    onsignersportage = data,
                    promotionsportagebill = promotiondata,
                    offsignersporatge = signoffcrewdata
                };

                return View(tables);
            }
            }
            catch (Exception ex) { }
            return RedirectToAction("UserLogin", "Login");
        }

        public JsonResult GetDataByPoratgeId(int portageid,int crewid)
        {
            try
            {
                var data = _context.TblPortageBills.FirstOrDefault(c => c.PortageBillId == portageid && c.CrewId == crewid);
                return Json(data);
            }
            catch(Exception ex) { throw ex; }
            return null;
        }
        public JsonResult getBowRequest(int crewListId)
        {
            var bowreq = _context.TblBowRequests.FirstOrDefault(t => t.CrewListId == crewListId && t.IsDeleted == false);
            return Json(bowreq);
        }
        public JsonResult getBOWData(int crewId, int crewListId, DateTime signOffDate)
        {
            try
            {
                var data = _context.portageBillBows.FromSqlRaw("getBOWData @p0, @p1, @p2", crewId, crewListId, signOffDate).ToList();
                return Json(data);
            }
            catch (DbUpdateException dbEx)
            {
                var validationContext = new ValidationContext(dbEx);
                Validator.ValidateObject(dbEx, validationContext, validateAllProperties: true);
            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
            }
            //catch(Exception ex) { }
            return null;
        }

     int portageBillId = 0;
        [HttpPost]
        public int AddPortageBill(TblPortageBill item)
        {
            try
            {
                if (item.PortageBillId == 0)
                {
                    //_context.TblPortageBills.Add(new TblPortageBill
                    var data = new TblPortageBill
                    {
                        CrewId = item.CrewId,
                        CrewListId = item.CrewListId,
                        ContractId = item.ContractId,
                        From = item.From,
                        To = item.To,
                        Days = item.Days,
                        Othours = item.Othours,
                        ExtraOt = item.ExtraOt,
                        OtherEarnings = item.OtherEarnings,
                        TransitDays = item.TransitDays,
                        TransitWages = item.TransitWages,
                        TotalEarnings = item.TotalEarnings,
                        PrevMonthBal = item.PrevMonthBal,
                        // PrevMonthBal = item.FinalBalance,
                        Reimbursement = item.Reimbursement,
                        TotalPayable = item.TotalPayable,
                        LeaveWagesCf = item.LeaveWagesCf,
                        CashAdvance = item.CashAdvance,
                        BondedStores = item.BondedStores,
                        OtherDeductions = item.OtherDeductions,
                        Allotments = item.Allotments,
                        TotalDeductions = item.TotalDeductions,
                        SignOffDate = item.SignOffDate,
                        Remarks = item.Remarks,
                        LeaveWagesBf = item.LeaveWagesBf,
                        FinalBalance = item.FinalBalance,
                        AppliedCba = item.AppliedCba,
                      //  CreatedBy = crewid,
                        BankId = item.BankId,
                        Vesselid = item.Vesselid,
                        Udamount = item.Udamount,
                        Tax = item.Tax,
                        Wfamount = item.Wfamount,
                        IsPromoted = item.IsPromoted,
                        IsLeaveWagesCf = item.IsLeaveWagesCf,
                        Attachment = item.Attachment,
                        Gratuity = item.Gratuity,
                        Avc = item.Avc,
                        IndPfamount = item.IndPfamount,
                        IsAddPrevBal = item.IsAddPrevBal,
                    };
                    _context.TblPortageBills.Add(data);
                    _context.SaveChanges();
                    portageBillId = data.PortageBillId;
                    return data.PortageBillId;
                }
                else
                {
                    var data = _context.TblPortageBills.FirstOrDefault(c => c.PortageBillId == item.PortageBillId);
                    if (data != null)
                    {
                        data.CrewId = item.CrewId;
                        data.CrewListId = item.CrewListId;
                        data.ContractId = item.ContractId;
                        data.From = item.From;
                        data.To = item.To;
                        data.Days = item.Days;
                        data.Othours = item.Othours;
                        data.ExtraOt = item.ExtraOt;
                        data.OtherEarnings = item.OtherEarnings;
                        data.TransitDays = item.TransitDays;
                        data.TransitWages = item.TransitWages;
                        data.TotalEarnings = item.TotalEarnings;
                        data.PrevMonthBal = item.PrevMonthBal;
                        data.Reimbursement = item.Reimbursement;
                        data.TotalPayable = item.TotalPayable;
                        data.LeaveWagesCf = item.LeaveWagesCf;
                        data.CashAdvance = item.CashAdvance;
                        data.BondedStores = item.BondedStores;
                        data.OtherDeductions = item.OtherDeductions;
                        data.Allotments = item.Allotments;
                        data.TotalDeductions = item.TotalDeductions;
                        data.LeaveWagesBf = item.LeaveWagesBf;
                        data.FinalBalance = item.FinalBalance;
                        // data.SignOffDate = item.SignOffDate;
                        data.Remarks = item.Remarks;
                      //  data.ModifiedBy = crewid;
                        data.ModifiedDate = DateTime.Now;
                        data.AppliedCba = item.AppliedCba;
                        data.BankId = item.BankId;
                        data.Vesselid = item.Vesselid;
                        data.Udamount = item.Udamount;
                        data.Tax = item.Tax;
                        data.Wfamount = item.Wfamount;
                        data.IsLeaveWagesCf = item.IsLeaveWagesCf;
                        data.Attachment = item.Attachment;
                        data.Gratuity = item.Gratuity;
                        data.Avc = item.Avc;
                        data.IndPfamount = item.IndPfamount;
                        data.IsAddPrevBal = item.IsAddPrevBal;
                        _context.TblPortageBills.Update(data);
                        _context.SaveChanges();
                        return data.PortageBillId;

                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
                throw ex;
            }
            return 0;
        }
   
        public JsonResult AddBankAllotment(TblPbbankAllotment item, int itemlength)
        {
            DateTime today = DateTime.Today;
            DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            DateTime endOfMonth = new DateTime(today.Year, today.Month, DateTime.DaysInMonth(today.Year, today.Month));
            if (item.From == DateTime.MinValue)
            {
                item.From = firstDayOfMonth;
                item.To = endOfMonth;
            }
            try
            {
                if (item.BankAllotmentId == 0)
                {
                    _context.TblPbbankAllotments.Add(new TblPbbankAllotment
                    {
                        Crew = item.Crew,
                        VesselId = item.VesselId,
                        BankId = item.BankId,
                        From = item.From,
                        To = item.To,
                        Allotments = item.Allotments,
                        IsPromoted = item.IsPromoted,
                        IsMidMonthAllotment = item.IsMidMonthAllotment,
                    }); _context.SaveChanges();
                    return Json("success");
                }
                else
                {
                    if (item.IsMidMonthAllotment == false)
                    {
                        var checkdata = _context.TblPbbankAllotments.Where(x => x.From.Month == item.From.Month && x.From.Year == item.From.Year && x.IsMidMonthAllotment == false && x.Crew == item.Crew && x.VesselId == item.VesselId).ToList();
                        var checkpbdata = _context.TblPortageBills.Where(x => x.From.Value.Month == item.From.Month && x.From.Value.Year == item.From.Year && x.CrewId == item.Crew).FirstOrDefault();
                        var data = _context.TblPbbankAllotments.FirstOrDefault(c => c.BankAllotmentId == item.BankAllotmentId);
                        if (checkdata.Count() == itemlength)
                        {
                            if (data != null)
                            {
                                data.Crew = item.Crew;
                                data.VesselId = item.VesselId;
                                data.BankId = item.BankId;
                                data.From = item.From;
                                data.To = item.To;
                                data.Allotments = item.Allotments;
                                _context.TblPbbankAllotments.Update(data);
                                _context.SaveChanges();
                                return Json("success");
                            }
                        }
                        else
                        {
                            if (data != null)
                            {
                                foreach (var b in checkdata)
                                {
                                    if (data.BankAllotmentId == b.BankAllotmentId)
                                    {
                                        data.Crew = item.Crew;
                                        data.VesselId = item.VesselId;
                                        data.BankId = item.BankId;
                                        data.From = item.From;
                                        data.To = item.To;
                                        data.Allotments = item.Allotments;
                                        _context.TblPbbankAllotments.Update(data);
                                        _context.SaveChanges();
                                    }
                                    else
                                    {
                                        var datanew = _context.TblPbbankAllotments.FirstOrDefault(c => c.BankAllotmentId == b.BankAllotmentId);
                                        datanew.IsDeleted = true;
                                        _context.TblPbbankAllotments.Update(datanew);
                                        _context.SaveChanges();
                                    }
                                }
                                return Json("success");
                            }
                        }
                    }
                    else
                    {
                        var checkdata = _context.TblPbbankAllotments.Where(x => x.From.Month == item.From.Month && x.From.Year == item.From.Year && x.IsMidMonthAllotment == true && x.Crew == item.Crew && x.VesselId == item.VesselId).ToList();
                        var checkpbdata = _context.TblPortageBills.Where(x => x.From.Value.Month == item.From.Month && x.From.Value.Year == item.From.Year && x.CrewId == item.Crew).FirstOrDefault();
                        var data = _context.TblPbbankAllotments.FirstOrDefault(c => c.BankAllotmentId == item.BankAllotmentId);
                        if (checkdata.Count() == itemlength)
                        {
                            if (data != null)
                            {
                                data.Crew = item.Crew;
                                data.VesselId = item.VesselId;
                                data.BankId = item.BankId;
                                data.From = item.From;
                                data.To = item.To;
                                data.Allotments = item.Allotments;
                                _context.TblPbbankAllotments.Update(data);
                                _context.SaveChanges();
                                return Json("success");
                            }
                        }
                        else
                        {
                            if (data != null)
                            {
                                foreach (var b in checkdata)
                                {
                                    if (data.BankAllotmentId == b.BankAllotmentId)
                                    {
                                        data.Crew = item.Crew;
                                        data.VesselId = item.VesselId;
                                        data.BankId = item.BankId;
                                        data.From = item.From;
                                        data.To = item.To;
                                        data.Allotments = item.Allotments;
                                        _context.TblPbbankAllotments.Update(data);
                                        _context.SaveChanges();
                                        return Json("success");
                                    }
                                    else
                                    {
                                        var datanew = _context.TblPbbankAllotments.FirstOrDefault(c => c.BankAllotmentId == b.BankAllotmentId);
                                        datanew.IsDeleted = true;
                                        _context.TblPbbankAllotments.Update(datanew);
                                        _context.SaveChanges();
                                    }
                                }
                            }
                            return Json("success");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }


        public JsonResult AddPortageEarningDeduction(PortageEarningDeduction item)
        {
            _context.PortageEarningDeduction.Add(new PortageEarningDeduction
            {
                CrewId = item.CrewId,
                Vesselid = item.Vesselid,
                PortageBillId = item.PortageBillId,
                Currency=item.Currency,
                From=item.From,
                To=item.To,
                RecDate=DateTime.UtcNow,
                CreatedBy=1,
                SubCodeId=item.SubCodeId,
               Type=item.Type,
               Amount=item.Amount,
               IsDeleted=false
            });          
            _context.SaveChanges();
            return Json("success");
        }



        public JsonResult GetBankAllotment(int vesselId, int month, int year, int crewid, string ispromoted)
        {
            bool promoted = false;
            if (ispromoted == "no")
                promoted = false;
            else if (ispromoted == "trf")
                promoted = false;
            else
                promoted = true;
            var data = _context.TblPbbankAllotments.Where(x => x.VesselId == vesselId && x.From.Month == month && x.From.Year == year && x.Crew == crewid && x.IsPromoted == promoted && x.IsMidMonthAllotment == false && x.IsDeleted == false).ToList();
            return Json(data);
        }
        public JsonResult deletebow(int id)
        {
            try
            {
                var bill = _context.TblPortageBills.FirstOrDefault(p => p.PortageBillId == id);
                if (bill != null)
                {
                    bill.IsDeleted = true;
                    _context.TblPortageBills.Update(bill);
                    _context.SaveChanges();
                    return Json("success");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json("");
        }
        public JsonResult GetCrewBankDetails(int? crewId)
        {
            var data = _context.TblCrewBankDetails.Where(t => t.CrewId == crewId && t.IsDeleted == false).OrderBy(x => x.AccountType).ToList();
          return Json(data);
        }
      
        public IActionResult Genneratexcel(string vesselId, int month, int year, string currency, string checkpbtilldate)
        {
            string vid = vesselId.Trim();
            // int? vesselId, int? month, int? year
             var query = _context.PortageBillPDFVM.FromSqlRaw("spPortagePDFFile @p0, @p1, @p2,@p3,@p4", vid, month, year, currency, checkpbtilldate).ToList();

            DataTable dt = new DataTable();
            dt = LINQResultToDataTable(query);

            var signoffdata = _context.PortageBillPDFSignoffVM.FromSqlRaw("spSignOFFPortagePDFFile @p0, @p1, @p2,@p3", vid, month, year, currency).ToList();

            DataTable dtsignoff = new DataTable();
            dtsignoff = LINQResultToDataTable(signoffdata);
            string vesselname = "";
            string fileName = "";

            if (dt.Rows.Count > 0)
                vesselname = dt.Rows[0]["vessel"].ToString();
            else
                vesselname = dtsignoff.Rows[0]["vessel"].ToString();

            
            fileName = "PortageBill" + "_" + vesselname + DateTime.Now.ToString("ddmmyyyyhhmmss") + ".xlsx";
            string path_Root = _appEnvironment.WebRootPath;

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Portage Bill");
                //  var wsoffsigners = wb.Worksheets.Add("Offsigners");
                ws.Cell(1, 1).Value = "Vessel Name : " + vesselname;
                if (dt.Rows.Count > 0)
                    ws.Cell(2, 1).Value = "Portage bill for the Month : " + DateTime.Parse(dt.Rows[0]["from"].ToString()).ToString("MMM-yyyy");
                else
                    ws.Cell(2, 1).Value = "Portage bill for the Month : " + DateTime.Parse(dtsignoff.Rows[0]["from"].ToString()).ToString("MMM-yyyy");
                ws.Cell(1, 1).Style.Font.Bold = true;
                ws.Cell(2, 1).Style.Font.Bold = true;
                var rownumbs = 0;
                if (query.Count() > 0)
                {
                    DataColumn dtSignature = new DataColumn("Signature", typeof(string));
                    dtSignature.DefaultValue = " ";
                    dt.Columns.Add(dtSignature);
                    dt.Columns.Remove("numberofday");
                    dt.Columns.Remove("PortageBillId");
                    dt.Columns.Remove("CrewId");
                    dt.Columns.Remove("ContractId");
                    dt.Columns.Remove("Rankid");
                    dt.Columns.Remove("AppliedCBA");
                    dt.Columns.Remove("SignOffDate");
                    dt.Columns.Remove("CrewListId");
                    dt.Columns.Remove("TransitDays");
                    dt.Columns.Remove("duration");
                    //dt.Columns.Remove("Tax");
                    dt.Columns.Remove("Vessel");
                    dt.Columns.Remove("Deduction");
                    dt.Columns.Remove("PFAmount10");
                    dt.Columns[0].ColumnName = "Sr No";
                    dt.Columns[1].ColumnName = "Crew Name";
                    dt.Columns[2].ColumnName = "CDC";
                    dt.Columns[3].ColumnName = "Rank";
                    //dt.Columns[4].ColumnName = "Contrcatual months";
                    dt.Columns[5].ColumnName = "Pay Commence Date";
                    dt.Columns[6].ColumnName = "DOJ";
                    dt.Columns[9].ColumnName = "Total Days";
                    dt.Columns[11].ColumnName = "Basic Wages ";
                    dt.Columns[12].ColumnName = "Fixed / Guaranteed OT ";
                    dt.Columns[13].ColumnName = "Leave Pay & Subsitence Allow ";
                    dt.Columns[14].ColumnName = "Uniform Allow. ";
                    dt.Columns[15].ColumnName = "Pension Fund ";
                    dt.Columns[16].ColumnName = "Special Allowance ";
                    dt.Columns[17].ColumnName = "Company Allowance";

                    dt.Columns[18].ColumnName = "Incentive Allowance";
                    dt.Columns[19].ColumnName = "Seniority  ";
                    dt.Columns[20].ColumnName = "Tanker Allowance";
                    dt.Columns[21].ColumnName = "Housing  ";
                    dt.Columns[22].ColumnName = "Transport  ";
                    dt.Columns[23].ColumnName = "Utility  ";
                    dt.Columns[24].ColumnName = "Bonus  ";
                    dt.Columns[25].ColumnName = "Seafarers PF";
                    dt.Columns[26].ColumnName = "Leave Pay Add.";

                    dt.Columns[27].ColumnName = "Security Allow";
                    dt.Columns[28].ColumnName = "Temp. Fuel Allow.";
                    dt.Columns[29].ColumnName = "Other Allow. ";
                    dt.Columns[30].ColumnName = "Total Wages per month ";
                    dt.Columns[31].ColumnName = "OT Rate";
                    dt.Columns[32].ColumnName = "Overtime Hr";
                    dt.Columns[33].ColumnName = "Basic Wages";
                    dt.Columns[34].ColumnName = "Fixed / Guaranteed OT";
                    dt.Columns[35].ColumnName = "Leave Pay & Subsitence Allow.";
                    dt.Columns[36].ColumnName = "Uniform Allow.";
                    dt.Columns[37].ColumnName = "Pension Fund";
                    dt.Columns[38].ColumnName = "Special Allowance";
                    dt.Columns[39].ColumnName = "Company Allowance ";                    
                    dt.Columns[40].ColumnName = "Incentive Allowance ";
                    dt.Columns[41].ColumnName = "Seniority ";
                    dt.Columns[42].ColumnName = "Tanker Allowance ";
                    dt.Columns[43].ColumnName = "Housing ";
                    dt.Columns[44].ColumnName = "Transport ";
                    dt.Columns[45].ColumnName = "Utility ";
                    dt.Columns[46].ColumnName = "Bonus ";
                    dt.Columns[47].ColumnName = "Seafarers PF ";
                    dt.Columns[48].ColumnName = "Leave Pay Add. ";
                    dt.Columns[49].ColumnName = "Security Allow ";
                    dt.Columns[50].ColumnName = "Temp. Fuel Allow. ";
                    dt.Columns[51].ColumnName = "Other Allow.";
                    dt.Columns[52].ColumnName = "Extra OT";
                    dt.Columns[53].ColumnName = "Other Earning";
                    dt.Columns[54].ColumnName = "Transit Wages";
                    dt.Columns[55].ColumnName = "Total Earnings (Current Month)";
                    dt.Columns[56].ColumnName = "Bal from Prev Month";
                    dt.Columns[58].ColumnName = "Net Allotment payable";
                    dt.Columns[59].ColumnName = "Leave Pay + Subsitence  C/F (This Month)";
                    dt.Columns[60].ColumnName = "Cash Advance";
                    dt.Columns[61].ColumnName = "Bonded Stores";
                    dt.Columns[62].ColumnName = "Other Deduction";
                    dt.Columns[63].ColumnName = "8 % PF Employee Contribution  (On Basic)";
                    dt.Columns[64].ColumnName = "2.0 % Union Dues (On Gross)";
                    DateTime value = new DateTime(2023, 12, 1);
                    if (dt.Rows.Count > 0)
                    {
                        if (DateTime.Parse(dt.Rows[0]["from"].ToString()) >= value)
                            dt.Columns[65].ColumnName = "Welfare fund 1% Officers & 2% Ratings (On Gross)";
                        else
                            dt.Columns[65].ColumnName = "1.0 % Welfare fund (On Gross)";
                    }
                    else
                    {
                        if (DateTime.Parse(dtsignoff.Rows[0]["from"].ToString()) >= value)
                            dt.Columns[65].ColumnName = "Welfare fund 1% Officers & 2% Ratings (On Gross)";
                        else
                            dt.Columns[65].ColumnName = "1.0 % Welfare fund (On Gross)";
                    }
                    dt.Columns[66].ColumnName = "WHT@5 % ";
                    dt.Columns[68].ColumnName = "PF Deduction(Indian)";
                    dt.Columns[69].ColumnName = "Total Deductions";
                    dt.Columns[70].ColumnName = "Leave wages B/F";
                    dt.Columns[71].ColumnName = "Leave wages C/F";
                    dt.Columns[72].ColumnName = "Final Balance";
                    dt.Columns.Remove("EPF");
                    wb.Worksheet(1).Cell(3, 1).InsertTable(dt);
                    rownumbs = ws.RowsUsed().Count();
                    var totalRows = 0;
                    totalRows = ws.RowsUsed().Count();
                    int firstrow2table = rownumbs + 1;
                    //Onsigners Total
                    ws.Cell(firstrow2table, 1).Value = "Onsigners Total";
                    ws.Row(firstrow2table).Style.Font.Bold = true;
                    // string colrowlasti = "I" + (totalRows + 1).ToString();
                    // string colrowgi = "I4:I" + rownumbs;
                    //ws.Cell(colrowlasti).FormulaA1 = "=sum(" + colrowgi + ")";
                    //string colrowlastJ = "J" + (totalRows + 1).ToString();
                    //string colrowgJ = "J4:J" + rownumbs;
                    //ws.Cell(colrowlastJ).FormulaA1 = "=sum(" + colrowgJ + ")";
                    //string colrowlastK = "K" + (totalRows + 1).ToString();
                    //string colrowgK = "K4:K" + rownumbs;
                    //ws.Cell(colrowlastK).FormulaA1 = "=sum(" + colrowgK + ")";
                    string colrowlastL = "L" + (totalRows + 1).ToString();
                    string colrowgL = "L4:L" + rownumbs;
                    ws.Cell(colrowlastL).FormulaA1 = "=sum(" + colrowgL + ")";
                    ws.Cell(colrowlastL).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgL).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastM = "M" + (totalRows + 1).ToString();
                    string colrowgM = "M4:M" + rownumbs;
                    ws.Cell(colrowlastM).FormulaA1 = "=sum(" + colrowgM + ")";
                    ws.Cell(colrowlastM).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgM).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastN = "N" + (totalRows + 1).ToString();
                    string colrowgN = "N4:N" + rownumbs;
                    ws.Cell(colrowlastN).FormulaA1 = "=sum(" + colrowgN + ")";
                    ws.Cell(colrowlastN).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgN).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastO = "O" + (totalRows + 1).ToString();
                    string colrowgO = "O4:O" + rownumbs;
                    ws.Cell(colrowlastO).FormulaA1 = "=sum(" + colrowgO + ")";
                    ws.Cell(colrowlastO).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgO).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastP = "P" + (totalRows + 1).ToString();
                    string colrowgP = "P4:P" + rownumbs;
                    ws.Cell(colrowlastP).FormulaA1 = "=sum(" + colrowgP + ")";
                    ws.Cell(colrowlastP).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgP).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastQ = "Q" + (totalRows + 1).ToString();
                    string colrowgQ = "Q4:Q" + rownumbs;
                    ws.Cell(colrowlastQ).FormulaA1 = "=sum(" + colrowgQ + ")";
                    ws.Cell(colrowlastQ).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgQ).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastR = "R" + (totalRows + 1).ToString();
                    string colrowgR = "R4:R" + rownumbs;
                    ws.Cell(colrowlastR).FormulaA1 = "=sum(" + colrowgR + ")";
                    ws.Cell(colrowlastR).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgR).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastS = "S" + (totalRows + 1).ToString();
                    string colrowgS = "S4:S" + rownumbs;
                    ws.Cell(colrowlastS).FormulaA1 = "=sum(" + colrowgS + ")";
                    ws.Cell(colrowlastS).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgR).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastT = "T" + (totalRows + 1).ToString();
                    string colrowgT = "T4:T" + rownumbs;
                    ws.Cell(colrowlastT).FormulaA1 = "=sum(" + colrowgT + ")";
                    ws.Cell(colrowlastT).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastU = "U" + (totalRows + 1).ToString();
                    string colrowgU = "U4:U" + rownumbs;
                    ws.Cell(colrowlastU).FormulaA1 = "=sum(" + colrowgU + ")";
                    ws.Cell(colrowlastU).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgU).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastV = "V" + (totalRows + 1).ToString();
                    string colrowgV = "V4:V" + rownumbs;
                    ws.Cell(colrowlastV).FormulaA1 = "=sum(" + colrowgV + ")";
                    ws.Cell(colrowlastV).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgV).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastW = "W" + (totalRows + 1).ToString();
                    string colrowgW = "W4:W" + rownumbs;
                    ws.Cell(colrowlastW).FormulaA1 = "=sum(" + colrowgW + ")";
                    ws.Cell(colrowlastW).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgW).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastX = "X" + (totalRows + 1).ToString();
                    string colrowgX = "X4:X" + rownumbs;
                    ws.Cell(colrowlastX).FormulaA1 = "=sum(" + colrowgX + ")";
                    ws.Cell(colrowlastX).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgX).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastY = "Y" + (totalRows + 1).ToString();
                    string colrowgY = "Y4:Y" + rownumbs;
                    ws.Cell(colrowlastY).FormulaA1 = "=sum(" + colrowgY + ")";
                    ws.Cell(colrowlastY).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgY).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastZ = "Z" + (totalRows + 1).ToString();
                    string colrowgZ = "Z4:Z" + rownumbs;
                    ws.Cell(colrowlastZ).FormulaA1 = "=sum(" + colrowgZ + ")";
                    ws.Cell(colrowlastZ).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgZ).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAA = "AA" + (totalRows + 1).ToString();
                    string colrowgAA = "AA4:AA" + rownumbs;
                    ws.Cell(colrowlastAA).FormulaA1 = "=sum(" + colrowgAA + ")";
                    ws.Cell(colrowlastAA).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgAA).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAB = "AB" + (totalRows + 1).ToString();
                    string colrowgAB = "AB4:AB" + rownumbs;
                    ws.Cell(colrowlastAB).FormulaA1 = "=sum(" + colrowgAB + ")";
                    ws.Cell(colrowlastAB).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgAB).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAC = "AC" + (totalRows + 1).ToString();
                    string colrowgAC = "AC4:AC" + rownumbs;
                    ws.Cell(colrowlastAC).FormulaA1 = "=sum(" + colrowgAC + ")";
                    ws.Cell(colrowlastAC).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgAC).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAD = "AD" + (totalRows + 1).ToString();
                    string colrowgAD = "AD4:AD" + rownumbs;
                    ws.Cell(colrowlastAD).FormulaA1 = "=sum(" + colrowgAD + ")";
                    ws.Cell(colrowlastAD).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgAD).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAE = "AE" + (totalRows + 1).ToString();
                    string colrowgAE = "AE4:AE" + rownumbs;
                    ws.Cell(colrowlastAE).FormulaA1 = "=sum(" + colrowgAE + ")";
                    ws.Cell(colrowlastAE).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgAE).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAF = "AF" + (totalRows + 1).ToString();
                    string colrowgaf = "AF4:AF" + rownumbs;
                    ws.Cell(colrowlastAF).FormulaA1 = "=sum(" + colrowgaf + ")";
                    ws.Cell(colrowlastAF).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgaf).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAG = "AG" + (totalRows + 1).ToString();
                    string colrowgAG = "AG4:AG" + rownumbs;
                    ws.Cell(colrowlastAG).FormulaA1 = "=sum(" + colrowgAG + ")";
                    ws.Cell(colrowlastAG).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgAG).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAH = "AH" + (totalRows + 1).ToString();
                    string colrowgAH = "AH4:AH" + rownumbs;
                    ws.Cell(colrowlastAH).FormulaA1 = "=sum(" + colrowgAH + ")";
                    ws.Cell(colrowlastAH).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgAH).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAI = "AI" + (totalRows + 1).ToString();
                    string colrowgAI = "AI4:AI" + rownumbs;
                    ws.Cell(colrowlastAI).FormulaA1 = "=sum(" + colrowgAI + ")";
                    ws.Cell(colrowlastAI).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgAI).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAJ = "AJ" + (totalRows + 1).ToString();
                    string colrowgAJ = "AJ4:AJ" + rownumbs;
                    ws.Cell(colrowlastAJ).FormulaA1 = "=sum(" + colrowgAJ + ")";
                    ws.Cell(colrowlastAJ).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgAJ).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAK = "AK" + (totalRows + 1).ToString();
                    string colrowgAK = "AK4:AK" + rownumbs;
                    ws.Cell(colrowlastAK).FormulaA1 = "=sum(" + colrowgAK + ")";
                    ws.Cell(colrowlastAK).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgAK).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAL = "AL" + (totalRows + 1).ToString();
                    string colrowgAL = "AL4:AL" + rownumbs;
                    ws.Cell(colrowlastAL).FormulaA1 = "=sum(" + colrowgAL + ")";
                    ws.Cell(colrowlastAL).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgAL).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAM = "AM" + (totalRows + 1).ToString();
                    string colrowgAM = "AM4:AM" + rownumbs;
                    ws.Cell(colrowlastAM).FormulaA1 = "=sum(" + colrowgAM + ")";
                    ws.Cell(colrowlastAM).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgAM).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAN = "AN" + (totalRows + 1).ToString();
                    string colrowgAN = "AN4:AN" + rownumbs;
                    ws.Cell(colrowlastAN).FormulaA1 = "=sum(" + colrowgAN + ")";
                    ws.Cell(colrowlastAN).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgAN).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAO = "AO" + (totalRows + 1).ToString();
                    string colrowgAO = "AO4:AO" + rownumbs;
                    ws.Cell(colrowlastAO).FormulaA1 = "=sum(" + colrowgAO + ")";
                    ws.Cell(colrowlastAO).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgAO).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAP = "AP" + (totalRows + 1).ToString();
                    string colrowgAP = "AP4:AP" + rownumbs;
                    ws.Cell(colrowlastAP).FormulaA1 = "=sum(" + colrowgAP + ")";
                    ws.Cell(colrowlastAP).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgAP).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAQ = "AQ" + (totalRows + 1).ToString();
                    string colrowgAQ = "AQ4:AQ" + rownumbs;
                    ws.Cell(colrowlastAQ).FormulaA1 = "=sum(" + colrowgAQ + ")";
                    ws.Cell(colrowlastAQ).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgAQ).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAR = "AR" + (totalRows + 1).ToString();
                    string colrowgAR = "AR4:AR" + rownumbs;
                    ws.Cell(colrowlastAR).FormulaA1 = "=sum(" + colrowgAR + ")";
                    ws.Cell(colrowlastAR).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgAR).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAS = "AS" + (totalRows + 1).ToString();
                    string colrowgAS = "AS4:AS" + rownumbs;
                    ws.Cell(colrowlastAS).FormulaA1 = "=sum(" + colrowgAS + ")";
                    ws.Cell(colrowlastAS).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgAS).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAT = "AT" + (totalRows + 1).ToString();
                    string colrowgAT = "AT4:AT" + rownumbs;
                    ws.Cell(colrowlastAT).FormulaA1 = "=sum(" + colrowgAT + ")";
                    ws.Cell(colrowlastAT).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgAT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAU = "AU" + (totalRows + 1).ToString();
                    string colrowgAU = "AU4:AU" + rownumbs;
                    ws.Cell(colrowlastAU).FormulaA1 = "=sum(" + colrowgAU + ")";
                    ws.Cell(colrowlastAU).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgAU).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAV = "AV" + (totalRows + 1).ToString();
                    string colrowgAV = "AV4:AV" + rownumbs;
                    ws.Cell(colrowlastAV).FormulaA1 = "=sum(" + colrowgAV + ")";
                    ws.Cell(colrowlastAV).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowlastAV).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAW = "AW" + (totalRows + 1).ToString();
                    string colrowgAW = "AW4:AW" + rownumbs;
                    ws.Cell(colrowlastAW).FormulaA1 = "=sum(" + colrowgAW + ")";
                    ws.Cell(colrowlastAW).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgAW).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAX = "AX" + (totalRows + 1).ToString();
                    string colrowgAX = "AX4:AX" + rownumbs;
                    ws.Cell(colrowlastAX).FormulaA1 = "=sum(" + colrowgAX + ")";
                    ws.Cell(colrowlastAX).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgAX).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAY = "AY" + (totalRows + 1).ToString();
                    string colrowgAY = "AY4:AY" + rownumbs;
                    ws.Cell(colrowlastAY).FormulaA1 = "=sum(" + colrowgAY + ")";
                    ws.Cell(colrowlastAY).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgAY).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAZ = "AZ" + (totalRows + 1).ToString();
                    string colrowgAZ = "AZ4:AZ" + rownumbs;
                    ws.Cell(colrowlastAZ).FormulaA1 = "=sum(" + colrowgAZ + ")";
                    ws.Cell(colrowlastAZ).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgAZ).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBA = "BA" + (totalRows + 1).ToString();
                    string colrowgBA = "BA4:BA" + rownumbs;
                    ws.Cell(colrowlastBA).FormulaA1 = "=sum(" + colrowgBA + ")";
                    ws.Cell(colrowlastBA).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgBA).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBB = "BB" + (totalRows + 1).ToString();
                    string colrowgBB = "BB4:BB" + rownumbs;
                    ws.Cell(colrowlastBB).FormulaA1 = "=sum(" + colrowgBB + ")";
                    ws.Cell(colrowlastBB).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgBB).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBC = "BC" + (totalRows + 1).ToString();
                    string colrowgBC = "BC4:BC" + rownumbs;
                    ws.Cell(colrowlastBC).FormulaA1 = "=sum(" + colrowgBC + ")";
                    ws.Cell(colrowlastBC).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgBC).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBD = "BD" + (totalRows + 1).ToString();
                    string colrowgBD = "BD4:BD" + rownumbs;
                    ws.Cell(colrowlastBD).FormulaA1 = "=sum(" + colrowgBD + ")";
                    ws.Cell(colrowlastBD).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgBD).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBE = "BE" + (totalRows + 1).ToString();
                    string colrowgBE = "BE4:BE" + rownumbs;
                    ws.Cell(colrowlastBE).FormulaA1 = "=sum(" + colrowgBE + ")";
                    ws.Cell(colrowlastBE).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgBE).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBF = "BF" + (totalRows + 1).ToString();
                    string colrowgBF = "BF4:BF" + rownumbs;
                    ws.Cell(colrowlastBF).FormulaA1 = "=sum(" + colrowgBF + ")";
                    ws.Cell(colrowlastBF).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgBF).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBG = "BG" + (totalRows + 1).ToString();
                    string colrowgBG = "BG4:BG" + rownumbs;
                    ws.Cell(colrowlastBG).FormulaA1 = "=sum(" + colrowgBG + ")";
                    ws.Cell(colrowlastBG).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgBG).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBH = "BH" + (totalRows + 1).ToString();
                    string colrowgBH = "BH4:BH" + rownumbs;
                    ws.Cell(colrowlastBH).FormulaA1 = "=sum(" + colrowgBH + ")";
                    ws.Cell(colrowlastBH).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgBH).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBI = "BI" + (totalRows + 1).ToString();
                    string colrowgBI = "BI4:BI" + rownumbs;
                    ws.Cell(colrowlastBI).FormulaA1 = "=sum(" + colrowgBI + ")";
                    ws.Cell(colrowlastBI).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgBI).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBJ = "BJ" + (totalRows + 1).ToString();
                    string colrowgBJ = "BJ4:BJ" + rownumbs;
                    ws.Cell(colrowlastBJ).FormulaA1 = "=sum(" + colrowgBJ + ")";
                    ws.Cell(colrowlastBJ).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgBJ).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBK = "BK" + (totalRows + 1).ToString();
                    string colrowgBK = "BK4:BK" + rownumbs;
                    ws.Cell(colrowlastBK).FormulaA1 = "=sum(" + colrowgBK + ")";
                    ws.Cell(colrowlastBK).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgBK).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBL = "BL" + (totalRows + 1).ToString();
                    string colrowgBL = "BL4:BL" + rownumbs;
                    ws.Cell(colrowlastBL).FormulaA1 = "=sum(" + colrowgBL + ")";
                    ws.Cell(colrowlastBL).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgBL).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBM = "BM" + (totalRows + 1).ToString();
                    string colrowgBM = "BM4:BM" + rownumbs;
                    ws.Cell(colrowlastBM).FormulaA1 = "=sum(" + colrowgBM + ")";
                    ws.Cell(colrowlastBM).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgBM).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBN = "BN" + (totalRows + 1).ToString();
                    string colrowgBN = "BN4:BN" + rownumbs;
                    ws.Cell(colrowlastBN).FormulaA1 = "=sum(" + colrowgBN + ")";
                    ws.Cell(colrowlastBN).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgBN).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBO = "BO" + (totalRows + 1).ToString();
                    string colrowgBO = "BO4:BO" + rownumbs;
                    ws.Cell(colrowlastBO).FormulaA1 = "=sum(" + colrowgBO + ")";
                    ws.Cell(colrowlastBO).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgBO).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBP = "BP" + (totalRows + 1).ToString();
                    string colrowgBP = "BP4:BP" + rownumbs;
                    ws.Cell(colrowlastBP).FormulaA1 = "=sum(" + colrowgBP + ")";
                    ws.Cell(colrowlastBP).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgBP).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBQ = "BQ" + (totalRows + 1).ToString();
                    string colrowgBQ = "BQ4:BQ" + rownumbs;
                    ws.Cell(colrowlastBQ).FormulaA1 = "=sum(" + colrowgBQ + ")";
                    ws.Cell(colrowlastBQ).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgBQ).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBR = "BR" + (totalRows + 1).ToString();
                    string colrowgBR = "BR4:BR" + rownumbs;
                    ws.Cell(colrowlastBR).FormulaA1 = "=sum(" + colrowgBR + ")";
                    ws.Cell(colrowlastBR).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgBR).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBS = "BS" + (totalRows + 1).ToString();
                    string colrowgBS = "BS4:BS" + rownumbs;
                    ws.Cell(colrowlastBS).FormulaA1 = "=sum(" + colrowgBS + ")";
                    ws.Cell(colrowlastBS).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgBS).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBT = "BT" + (totalRows + 1).ToString();
                    string colrowgBT = "BT4:BT" + rownumbs;
                    ws.Cell(colrowlastBT).FormulaA1 = "=sum(" + colrowgBT + ")";
                    ws.Cell(colrowlastBT).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgBT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBU = "BU" + (totalRows + 1).ToString();
                    string colrowgBU = "BU4:BU" + rownumbs;
                    ws.Cell(colrowlastBU).FormulaA1 = "=sum(" + colrowgBU + ")";
                    ws.Cell(colrowlastBU).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range(colrowgBU).Style.NumberFormat.NumberFormatId = 1;
                }
                int ROWNO = ws.RowsUsed().Count() + 2;
                //Delete unwanted column
                //ws.Column(2).Delete();
                // ws.Columns(5, 7).Delete();

                if (signoffdata.Count() > 0)
                {
                    DataColumn dtcname = new DataColumn("Signature", typeof(string));
                    dtcname.DefaultValue = "";
                    dtsignoff.Columns.Add(dtcname); dtsignoff.Columns.Remove("numberofday");
                    dtsignoff.Columns.Remove("PortageBillId");
                    dtsignoff.Columns.Remove("CrewId");
                    dtsignoff.Columns.Remove("ContractId");
                    dtsignoff.Columns.Remove("Rankid");
                    dtsignoff.Columns.Remove("AppliedCBA");
                    dtsignoff.Columns.Remove("SignOffDate");
                    dtsignoff.Columns.Remove("CrewListId");
                    dtsignoff.Columns.Remove("TransitDays");
                    dtsignoff.Columns.Remove("duration");
                    dtsignoff.Columns.Remove("EPF");
                    dtsignoff.Columns.Remove("IsLeaveWagesCF");
                    dtsignoff.Columns.Remove("Vessel");
                    dtsignoff.Columns.Remove("Deduction");
                    dtsignoff.Columns.Remove("PFAmount10");
                    dtsignoff.Columns[0].ColumnName = "Sr No";
                    dtsignoff.Columns[1].ColumnName = "Crew Name";
                    dtsignoff.Columns[2].ColumnName = "CDC";
                    dtsignoff.Columns[3].ColumnName = "Rank";
                    //dt.Columns[4].ColumnName = "Contrcatual months";
                    dtsignoff.Columns[5].ColumnName = "Pay Commence Date";
                    dtsignoff.Columns[6].ColumnName = "DOJ";
                    dtsignoff.Columns[9].ColumnName = "Total Days";
                    dtsignoff.Columns[11].ColumnName = "Basic Wages ";
                    dtsignoff.Columns[12].ColumnName = "Fixed / Guaranteed OT ";
                    dtsignoff.Columns[13].ColumnName = "Leave Pay & Subsitence Allow ";
                    dtsignoff.Columns[14].ColumnName = "Uniform Allow. ";
                    dtsignoff.Columns[15].ColumnName = "Pension Fund ";
                    dtsignoff.Columns[16].ColumnName = "Special Allowance ";
                    dtsignoff.Columns[17].ColumnName = "Company Allowance";

                    dtsignoff.Columns[18].ColumnName = "Incentive Allowance";
                    dtsignoff.Columns[19].ColumnName = "Seniority   ";
                    dtsignoff.Columns[20].ColumnName = "Tanker Allowance";
                    dtsignoff.Columns[21].ColumnName = "Housing  ";
                    dtsignoff.Columns[22].ColumnName = "Transport  ";
                    dtsignoff.Columns[23].ColumnName = "Utility  ";
                    dtsignoff.Columns[24].ColumnName = "Bonus  ";
                    dtsignoff.Columns[25].ColumnName = "Seafarers PF";
                    dtsignoff.Columns[26].ColumnName = "Leave Pay Add.";

                    dtsignoff.Columns[27].ColumnName = "Security Allow";
                    dtsignoff.Columns[28].ColumnName = "Temp. Fuel Allow.";
                    dtsignoff.Columns[29].ColumnName = "Other Allow. ";
                    dtsignoff.Columns[30].ColumnName = "Total Wages per month ";
                    dtsignoff.Columns[31].ColumnName = "OT Rate";
                    dtsignoff.Columns[32].ColumnName = "Overtime Hr";
                    dtsignoff.Columns[33].ColumnName = "Basic Wages";
                    dtsignoff.Columns[34].ColumnName = "Fixed / Guaranteed OT";
                    dtsignoff.Columns[35].ColumnName = "Leave Pay & Subsitence Allow.";
                    dtsignoff.Columns[36].ColumnName = "Uniform Allow.";
                    dtsignoff.Columns[37].ColumnName = "Pension Fund";
                    dtsignoff.Columns[38].ColumnName = "Special Allowance";
                    dtsignoff.Columns[39].ColumnName = "Company Allowance ";

                    dtsignoff.Columns[40].ColumnName = "Incentive Allowance ";
                    dtsignoff.Columns[41].ColumnName = "Seniority ";
                    dtsignoff.Columns[42].ColumnName = "Tanker Allowance ";
                    dtsignoff.Columns[43].ColumnName = "Housing ";
                    dtsignoff.Columns[44].ColumnName = "Transport ";
                    dtsignoff.Columns[45].ColumnName = "Utility ";
                    dtsignoff.Columns[46].ColumnName = "Bonus ";
                    dtsignoff.Columns[47].ColumnName = "Seafarers PF ";
                    dtsignoff.Columns[48].ColumnName = "Leave Pay Add. ";
                    dtsignoff.Columns[49].ColumnName = "Security Allow ";//
                    dtsignoff.Columns[50].ColumnName = "Temp. Fuel Allow. ";
                    dtsignoff.Columns[51].ColumnName = "Other Allow.";
                    dtsignoff.Columns[52].ColumnName = "Extra OT";
                    dtsignoff.Columns[53].ColumnName = "Other Earning";
                    dtsignoff.Columns[54].ColumnName = "Transit Wages";
                    dtsignoff.Columns[55].ColumnName = "Total Earnings (Current Month)";
                    dtsignoff.Columns[56].ColumnName = "Bal from Prev Month";//

                    dtsignoff.Columns[58].ColumnName = "Net Allotment payable";
                    dtsignoff.Columns[59].ColumnName = "Leave Pay + Subsitence  C/F (This Month)";
                    dtsignoff.Columns[60].ColumnName = "Cash Advance";
                    dtsignoff.Columns[61].ColumnName = "Bonded Stores";
                    dtsignoff.Columns[62].ColumnName = "Other Deduction";
                    dtsignoff.Columns[63].ColumnName = "8 % PF Employee Contribution  (On Basic)";
                    dtsignoff.Columns[64].ColumnName = "2.0 % Union Dues (On Gross)";
                    DateTime value = new DateTime(2023, 12, 1);
                    if (DateTime.Parse(dtsignoff.Rows[0]["from"].ToString()) >= value)
                        dtsignoff.Columns[65].ColumnName = "Welfare fund 1% Officers & 2% Ratings (On Gross)";
                    else
                        dtsignoff.Columns[65].ColumnName = "1.0 % Welfare fund (On Gross)";
                    dtsignoff.Columns[66].ColumnName = "WHT@5 % ";
                    dtsignoff.Columns[68].ColumnName = "PF Deduction(Indian)";
                    dtsignoff.Columns[69].ColumnName = "Total Deductions";
                    dtsignoff.Columns[70].ColumnName = "Leave wages B/F";
                    dtsignoff.Columns[71].ColumnName = "Leave wages C/F";
                    dtsignoff.Columns[72].ColumnName = "Final Balance";
                    wb.Worksheet(1).Cell(ROWNO, 1).InsertTable(dtsignoff);
                    var rowsignoff = ws.RowsUsed().Count();
                    var signRows = 0;
                    signRows = ws.RowsUsed().Count() + 1;
                    int firstrowtable = rowsignoff + 1;
                    //signoff calculation
                    //string colrowlastJs = "j" + (signRows + 1).ToString();
                    //string column = "j" + ROWNO;
                    //string colrowgJs = "j"+ ROWNO +":j" + rowsignoff + ":" + "j" + firstrowtable;
                    //ws.Cell(colrowlastJs).FormulaA1 = "=sum(" + colrowgJs + ")";
                    string columns = "A" + ROWNO + ":AZ" + ROWNO;
                    ws.Range(columns).Style.Alignment.TextRotation = 90;
                    ws.Range(columns).Style.Alignment.WrapText = true;
                    ws.Range("U" + ROWNO + ":AI" + ROWNO).Style.Fill.BackgroundColor = XLColor.GreenPigment;
                    ws.Range("AJ" + ROWNO + ":AT" + ROWNO).Style.Fill.BackgroundColor = XLColor.Brown;
                    ws.Row(ROWNO).Height = 52;
                    //Off - signers Total
                    ws.Cell(signRows + 1, 1).Value = "Off - signers Total";
                    ws.Row(signRows + 1).Style.Font.Bold = true;
                    //string colrowlastis = "i" + (signRows + 1).ToString();
                    //string colrowgis = "i" + ROWNO + ":i" + rowsignoff + ":" + "i" + firstrowtable;
                    //ws.Cell(colrowlastis).FormulaA1 = "=sum(" + colrowgis + ")";
                    //string colrowlastJs = "j" + (signRows + 1).ToString();
                    //string colrowgJs = "j" + ROWNO + ":j" + rowsignoff + ":" + "j" + firstrowtable;
                    //ws.Cell(colrowlastJs).FormulaA1 = "=sum(" + colrowgJs + ")";
                    //string colrowlastKs = "K" + (signRows + 1).ToString();
                    //string colrowgKs = "K" + ROWNO + ":K" + rowsignoff + ":" + "K" + firstrowtable;
                    //ws.Cell(colrowlastKs).FormulaA1 = "=sum(" + colrowgKs + ")";
                    string colrowlastLs = "L" + (signRows + 1).ToString();
                    string colrowgLs = "l" + ROWNO + ":l" + rowsignoff + ":" + "l" + firstrowtable;
                    ws.Cell(colrowlastLs).FormulaA1 = "=sum(" + colrowgLs + ")";
                    ws.Cell(colrowlastLs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("L" + ROWNO + ":" + "L" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastMs = "M" + (signRows + 1).ToString();
                    string colrowgMs = "M" + ROWNO + ":M" + rowsignoff + ":" + "m" + firstrowtable;
                    ws.Cell(colrowlastMs).FormulaA1 = "=sum(" + colrowgMs + ")";
                    ws.Cell(colrowlastMs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("M" + ROWNO + ":" + "M" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastNs = "N" + (signRows + 1).ToString();
                    string colrowgNs = "N" + ROWNO + ":N" + rowsignoff + ":" + "N" + firstrowtable;
                    ws.Cell(colrowlastNs).FormulaA1 = "=sum(" + colrowgNs + ")";
                    ws.Cell(colrowlastNs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("N" + ROWNO + ":" + "N" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastOs = "O" + (signRows + 1).ToString();
                    string colrowgOs = "O" + ROWNO + ":O" + rowsignoff + ":" + "O" + firstrowtable;
                    ws.Cell(colrowlastOs).FormulaA1 = "=sum(" + colrowgOs + ")";
                    ws.Cell(colrowlastOs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("O" + ROWNO + ":" + "O" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastPs = "P" + (signRows + 1).ToString();
                    string colrowgPs = "P" + ROWNO + ":P" + rowsignoff + ":" + "P" + firstrowtable;
                    ws.Cell(colrowlastPs).FormulaA1 = "=sum(" + colrowgPs + ")";
                    ws.Cell(colrowlastPs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("P" + ROWNO + ":" + "P" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastQs = "Q" + (signRows + 1).ToString();
                    string colrowgQs = "Q" + ROWNO + ":Q" + rowsignoff + ":" + "Q" + firstrowtable;
                    ws.Cell(colrowlastQs).FormulaA1 = "=sum(" + colrowgQs + ")";
                    ws.Cell(colrowlastQs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("Q" + ROWNO + ":" + "Q" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastRs = "R" + (signRows + 1).ToString();
                    string colrowgRs = "R" + ROWNO + ":R" + rowsignoff + ":" + "R" + firstrowtable;
                    ws.Cell(colrowlastRs).FormulaA1 = "=sum(" + colrowgRs + ")";
                    ws.Cell(colrowlastRs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("R" + ROWNO + ":" + "R" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastSs = "S" + (signRows + 1).ToString();
                    string colrowgSs = "S" + ROWNO + ":S" + rowsignoff + ":" + "S" + firstrowtable;
                    ws.Cell(colrowlastSs).FormulaA1 = "=sum(" + colrowgSs + ")";
                    ws.Cell(colrowlastSs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("S" + ROWNO + ":" + "S" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastTs = "T" + (signRows + 1).ToString();
                    string colrowgTs = "T" + ROWNO + ":T" + rowsignoff + ":" + "T" + firstrowtable;
                    ws.Cell(colrowlastTs).FormulaA1 = "=sum(" + colrowgTs + ")";
                    ws.Cell(colrowlastTs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("T" + ROWNO + ":" + "T" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastUs = "U" + (signRows + 1).ToString();
                    string colrowgUs = "U" + ROWNO + ":U" + rowsignoff + ":" + "U" + firstrowtable;
                    ws.Cell(colrowlastUs).FormulaA1 = "=sum(" + colrowgUs + ")";
                    ws.Cell(colrowlastUs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("U" + ROWNO + ":" + "U" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastVs = "V" + (signRows + 1).ToString();
                    string colrowgVs = "V" + ROWNO + ":V" + rowsignoff + ":" + "V" + firstrowtable;
                    ws.Cell(colrowlastVs).FormulaA1 = "=sum(" + colrowgVs + ")";
                    ws.Cell(colrowlastVs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("V" + ROWNO + ":" + "V" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastWs = "W" + (signRows + 1).ToString();
                    string colrowgWs = "W" + ROWNO + ":W" + rowsignoff + ":" + "W" + firstrowtable;
                    ws.Cell(colrowlastWs).FormulaA1 = "=sum(" + colrowgWs + ")";
                    ws.Cell(colrowlastWs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("W" + ROWNO + ":" + "W" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastXs = "X" + (signRows + 1).ToString();
                    string colrowgXs = "X" + ROWNO + ":X" + rowsignoff + ":" + "X" + firstrowtable;
                    ws.Cell(colrowlastXs).FormulaA1 = "=sum(" + colrowgXs + ")";
                    ws.Cell(colrowlastXs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("X" + ROWNO + ":" + "X" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastYs = "Y" + (signRows + 1).ToString();
                    string colrowgYs = "Y" + ROWNO + ":Y" + rowsignoff + ":" + "Y" + firstrowtable;
                    ws.Cell(colrowlastYs).FormulaA1 = "=sum(" + colrowgYs + ")";
                    ws.Cell(colrowlastYs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("Y" + ROWNO + ":" + "Y" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastZs = "Z" + (signRows + 1).ToString();
                    string colrowgZs = "Z" + ROWNO + ":Z" + rowsignoff + ":" + "Z" + firstrowtable;
                    ws.Cell(colrowlastZs).FormulaA1 = "=sum(" + colrowgZs + ")";
                    ws.Cell(colrowlastZs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("Z" + ROWNO + ":" + "Z" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAAs = "AA" + (signRows + 1).ToString();
                    string colrowgAAs = "AA" + ROWNO + ":AA" + rowsignoff + ":" + "AA" + firstrowtable;
                    ws.Cell(colrowlastAAs).FormulaA1 = "=sum(" + colrowgAAs + ")";
                    ws.Cell(colrowlastAAs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("AA" + ROWNO + ":" + "AA" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastABs = "AB" + (signRows + 1).ToString();
                    string colrowgABs = "AB" + ROWNO + ":AB" + rowsignoff + ":" + "AB" + firstrowtable;
                    ws.Cell(colrowlastABs).FormulaA1 = "=sum(" + colrowgABs + ")";
                    ws.Cell(colrowlastABs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("AB" + ROWNO + ":" + "AB" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastACs = "AC" + (signRows + 1).ToString();
                    string colrowgACs = "AC" + ROWNO + ":AC" + rowsignoff + ":" + "AC" + firstrowtable;
                    ws.Cell(colrowlastACs).FormulaA1 = "=sum(" + colrowgACs + ")";
                    ws.Cell(colrowlastACs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("AC" + ROWNO + ":" + "AC" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastADs = "AD" + (signRows + 1).ToString();
                    string colrowgADs = "AD" + ROWNO + ":AD" + rowsignoff + ":" + "AD" + firstrowtable;
                    ws.Cell(colrowlastADs).FormulaA1 = "=sum(" + colrowgADs + ")";
                    ws.Cell(colrowlastADs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("AD" + ROWNO + ":" + "AD" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAEs = "AE" + (signRows + 1).ToString();
                    string colrowgAEs = "AE" + ROWNO + ":AE" + rowsignoff + ":" + "AE" + firstrowtable;
                    ws.Cell(colrowlastAEs).FormulaA1 = "=sum(" + colrowgAEs + ")";
                    ws.Cell(colrowlastAEs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("AE" + ROWNO + ":" + "AE" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAFs = "AF" + (signRows + 1).ToString();
                    string colrowgafs = "AF" + ROWNO + ":AF" + rowsignoff + ":" + "AF" + firstrowtable;
                    ws.Cell(colrowlastAFs).FormulaA1 = "=sum(" + colrowgafs + ")";
                    ws.Cell(colrowlastAFs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("AF" + ROWNO + ":" + "AF" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAGs = "AG" + (signRows + 1).ToString();
                    string colrowgAGs = "AG" + ROWNO + ":AG" + rowsignoff + ":" + "AG" + firstrowtable;
                    ws.Cell(colrowlastAGs).FormulaA1 = "=sum(" + colrowgAGs + ")";
                    ws.Cell(colrowlastAGs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("AG" + ROWNO + ":" + "AG" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAHs = "AH" + (signRows + 1).ToString();
                    string colrowgAHs = "AH" + ROWNO + ":AH" + rowsignoff + ":" + "AH" + firstrowtable;
                    ws.Cell(colrowlastAHs).FormulaA1 = "=sum(" + colrowgAHs + ")";
                    ws.Cell(colrowlastAHs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("AH" + ROWNO + ":" + "AH" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAIs = "AI" + (signRows + 1).ToString();
                    string colrowgAIs = "AI" + ROWNO + ":AI" + rowsignoff + ":" + "AI" + firstrowtable;
                    ws.Cell(colrowlastAIs).FormulaA1 = "=sum(" + colrowgAIs + ")";
                    ws.Cell(colrowlastAIs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("AI" + ROWNO + ":" + "AI" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAJs = "AJ" + (signRows + 1).ToString();
                    string colrowgAJs = "AJ" + ROWNO + ":AJ" + rowsignoff + ":" + "AJ" + firstrowtable;
                    ws.Cell(colrowlastAJs).FormulaA1 = "=sum(" + colrowgAJs + ")";
                    ws.Cell(colrowlastAJs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("AJ" + ROWNO + ":" + "AJ" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAKs = "AK" + (signRows + 1).ToString();
                    string colrowgAKs = "AK" + ROWNO + ":AK" + rowsignoff + ":" + "AK" + firstrowtable;
                    ws.Cell(colrowlastAKs).FormulaA1 = "=sum(" + colrowgAKs + ")";
                    ws.Cell(colrowlastAKs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("AK" + ROWNO + ":" + "AK" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastALs = "AL" + (signRows + 1).ToString();
                    string colrowgALs = "AL" + ROWNO + ":AL" + rowsignoff + ":" + "AL" + firstrowtable;
                    ws.Cell(colrowlastALs).FormulaA1 = "=sum(" + colrowgALs + ")";
                    ws.Cell(colrowlastALs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("AL" + ROWNO + ":" + "AL" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAMs = "AM" + (signRows + 1).ToString();
                    string colrowgAMs = "AM" + ROWNO + ":AM" + rowsignoff + ":" + "AM" + firstrowtable;
                    ws.Cell(colrowlastAMs).FormulaA1 = "=sum(" + colrowgAMs + ")";
                    ws.Cell(colrowlastAMs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("AM" + ROWNO + ":" + "AM" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastANs = "AN" + (signRows + 1).ToString();
                    string colrowgANs = "AN" + ROWNO + ":AN" + rowsignoff + ":" + "AN" + firstrowtable;
                    ws.Cell(colrowlastANs).FormulaA1 = "=sum(" + colrowgANs + ")";
                    ws.Cell(colrowlastANs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("AN" + ROWNO + ":" + "AN" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAOs = "AO" + (signRows + 1).ToString();
                    string colrowgAOs = "AO" + ROWNO + ":AO" + rowsignoff + ":" + "AO" + firstrowtable;
                    ws.Cell(colrowlastAOs).FormulaA1 = "=sum(" + colrowgAOs + ")";
                    ws.Cell(colrowlastAOs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("AO" + ROWNO + ":" + "AO" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAPs = "AP" + (signRows + 1).ToString();
                    string colrowgAPs = "AP" + ROWNO + ":AP" + rowsignoff + ":" + "AP" + firstrowtable;
                    ws.Cell(colrowlastAPs).FormulaA1 = "=sum(" + colrowgAPs + ")";
                    ws.Cell(colrowlastAPs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("AP" + ROWNO + ":" + "AP" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAQs = "AQ" + (signRows + 1).ToString();
                    string colrowgAQs = "AQ" + ROWNO + ":AQ" + rowsignoff + ":" + "AQ" + firstrowtable;
                    ws.Cell(colrowlastAQs).FormulaA1 = "=sum(" + colrowgAQs + ")";
                    ws.Cell(colrowlastAQs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("AQ" + ROWNO + ":" + "AQ" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastARs = "AR" + (signRows + 1).ToString();
                    string colrowgARs = "AR" + ROWNO + ":AR" + rowsignoff + ":" + "AR" + firstrowtable;
                    ws.Cell(colrowlastARs).FormulaA1 = "=sum(" + colrowgARs + ")";
                    ws.Cell(colrowlastARs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("AR" + ROWNO + ":" + "AR" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastASs = "AS" + (signRows + 1).ToString();
                    string colrowgASs = "AS" + ROWNO + ":AS" + rowsignoff + ":" + "AS" + firstrowtable;
                    ws.Cell(colrowlastASs).FormulaA1 = "=sum(" + colrowgASs + ")";
                    ws.Cell(colrowlastASs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("AS" + ROWNO + ":" + "AS" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastATs = "AT" + (signRows + 1).ToString();
                    string colrowgATs = "AT" + ROWNO + ":AT" + rowsignoff + ":" + "AT" + firstrowtable;
                    ws.Cell(colrowlastATs).FormulaA1 = "=sum(" + colrowgATs + ")";
                    ws.Cell(colrowlastATs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("AT" + ROWNO + ":" + "AT" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAUs = "AU" + (signRows + 1).ToString();
                    string colrowgAUs = "AU" + ROWNO + ":AU" + rowsignoff + ":" + "AU" + firstrowtable;
                    ws.Cell(colrowlastAUs).FormulaA1 = "=sum(" + colrowgAUs + ")";
                    ws.Cell(colrowlastAUs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("AU" + ROWNO + ":" + "AU" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAVs = "AV" + (signRows + 1).ToString();
                    string colrowgAVs = "AV" + ROWNO + ":AV" + rowsignoff + ":" + "AV" + firstrowtable;
                    ws.Cell(colrowlastAVs).FormulaA1 = "=sum(" + colrowgAVs + ")";
                    ws.Cell(colrowlastAVs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("AV" + ROWNO + ":" + "AV" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAWs = "AW" + (signRows + 1).ToString();
                    string colrowgAWs = "AW" + ROWNO + ":AW" + rowsignoff + ":" + "AW" + firstrowtable;
                    ws.Cell(colrowlastAWs).FormulaA1 = "=sum(" + colrowgAWs + ")";
                    ws.Cell(colrowlastAWs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("AW" + ROWNO + ":" + "AW" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAXs = "AX" + (signRows + 1).ToString();
                    string colrowgAXs = "AX" + ROWNO + ":AX" + rowsignoff + ":" + "AX" + firstrowtable;
                    ws.Cell(colrowlastAXs).FormulaA1 = "=sum(" + colrowgAXs + ")";
                    ws.Cell(colrowlastAXs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("AX" + ROWNO + ":" + "AX" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAYs = "AY" + (signRows + 1).ToString();
                    string colrowgAYs = "AY" + ROWNO + ":AY" + rowsignoff + ":" + "AY" + firstrowtable;
                    ws.Cell(colrowlastAYs).FormulaA1 = "=sum(" + colrowgAYs + ")";
                    ws.Cell(colrowlastAYs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("AY" + ROWNO + ":" + "AY" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAZs = "AZ" + (signRows + 1).ToString();
                    string colrowgAZs = "AZ" + ROWNO + ":AZ" + rowsignoff + ":" + "AZ" + firstrowtable;
                    ws.Cell(colrowlastAZs).FormulaA1 = "=sum(" + colrowgAZs + ")";
                    ws.Cell(colrowlastAZs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("AZ" + ROWNO + ":" + "AZ" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBAs = "BA" + (signRows + 1).ToString();
                    string colrowgBAs = "BA" + ROWNO + ":BA" + rowsignoff + ":" + "BA" + firstrowtable;
                    ws.Cell(colrowlastBAs).FormulaA1 = "=sum(" + colrowgBAs + ")";
                    ws.Cell(colrowlastBAs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("BA" + ROWNO + ":" + "BA" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBBs = "BB" + (signRows + 1).ToString();
                    string colrowgBBs = "BB" + ROWNO + ":BB" + rowsignoff + ":" + "BB" + firstrowtable;
                    ws.Cell(colrowlastBBs).FormulaA1 = "=sum(" + colrowgBBs + ")";
                    ws.Cell(colrowlastBBs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("BB" + ROWNO + ":" + "BB" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBCs = "BC" + (signRows + 1).ToString();
                    string colrowgBCs = "BC" + ROWNO + ":BC" + rowsignoff + ":" + "BC" + firstrowtable;
                    ws.Cell(colrowlastBCs).FormulaA1 = "=sum(" + colrowgBCs + ")";
                    ws.Cell(colrowlastBCs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("BC" + ROWNO + ":" + "BC" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBDs = "BD" + (signRows + 1).ToString();
                    string colrowgBDs = "BD" + ROWNO + ":BD" + rowsignoff + ":" + "BD" + firstrowtable;
                    ws.Cell(colrowlastBDs).FormulaA1 = "=sum(" + colrowgBDs + ")";
                    ws.Cell(colrowlastBDs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("BD" + ROWNO + ":" + "BD" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBEs = "BE" + (signRows + 1).ToString();
                    string colrowgBEs = "BE" + ROWNO + ":BE" + rowsignoff + ":" + "BE" + firstrowtable;
                    ws.Cell(colrowlastBEs).FormulaA1 = "=sum(" + colrowgBEs + ")";
                    ws.Cell(colrowlastBEs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("BE" + ROWNO + ":" + "BE" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBFs = "BF" + (signRows + 1).ToString();
                    string colrowgBFs = "BF" + ROWNO + ":BF" + rowsignoff + ":" + "BF" + firstrowtable;
                    ws.Cell(colrowlastBFs).FormulaA1 = "=sum(" + colrowgBFs + ")";
                    ws.Cell(colrowlastBFs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("BF" + ROWNO + ":" + "BF" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBGs = "BG" + (signRows + 1).ToString();
                    string colrowgBGs = "BG" + ROWNO + ":BG" + rowsignoff + ":" + "BG" + firstrowtable;
                    ws.Cell(colrowlastBGs).FormulaA1 = "=sum(" + colrowgBGs + ")";
                    ws.Cell(colrowlastBGs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("BG" + ROWNO + ":" + "BG" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBHs = "BH" + (signRows + 1).ToString();
                    string colrowgBHs = "BH" + ROWNO + ":BH" + rowsignoff + ":" + "BH" + firstrowtable;
                    ws.Cell(colrowlastBHs).FormulaA1 = "=sum(" + colrowgBHs + ")";
                    ws.Cell(colrowlastBHs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("BH" + ROWNO + ":" + "BH" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBIs = "BI" + (signRows + 1).ToString();
                    string colrowgBIs = "BI" + ROWNO + ":BI" + rowsignoff + ":" + "BI" + firstrowtable;
                    ws.Cell(colrowlastBIs).FormulaA1 = "=sum(" + colrowgBIs + ")";
                    ws.Cell(colrowlastBIs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("BI" + ROWNO + ":" + "BI" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBJs = "BJ" + (signRows + 1).ToString();
                    string colrowgBJs = "BJ" + ROWNO + ":BJ" + rowsignoff + ":" + "BJ" + firstrowtable;
                    ws.Cell(colrowlastBJs).FormulaA1 = "=sum(" + colrowgBJs + ")";
                    ws.Cell(colrowlastBJs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("BJ" + ROWNO + ":" + "BJ" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBKs = "BK" + (signRows + 1).ToString();
                    string colrowgBKs = "BK" + ROWNO + ":BK" + rowsignoff + ":" + "BK" + firstrowtable;
                    ws.Cell(colrowlastBKs).FormulaA1 = "=sum(" + colrowgBKs + ")";
                    ws.Cell(colrowlastBKs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("BK" + ROWNO + ":" + "BK" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBLs = "BL" + (signRows + 1).ToString();
                    string colrowgBLs = "BL" + ROWNO + ":BL" + rowsignoff + ":" + "BL" + firstrowtable;
                    ws.Cell(colrowlastBLs).FormulaA1 = "=sum(" + colrowgBLs + ")";
                    ws.Cell(colrowlastBLs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("BL" + ROWNO + ":" + "BL" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBMs = "BM" + (signRows + 1).ToString();
                    string colrowgBMs = "BM" + ROWNO + ":BM" + rowsignoff + ":" + "BM" + firstrowtable;
                    ws.Cell(colrowlastBMs).FormulaA1 = "=sum(" + colrowgBMs + ")";
                    ws.Cell(colrowlastBMs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("BM" + ROWNO + ":" + "BM" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBNs = "BN" + (signRows + 1).ToString();
                    string colrowgBNs = "BN" + ROWNO + ":BN" + rowsignoff + ":" + "BN" + firstrowtable;
                    ws.Cell(colrowlastBNs).FormulaA1 = "=sum(" + colrowgBNs + ")";
                    ws.Cell(colrowlastBNs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("BN" + ROWNO + ":" + "BN" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBOs = "BO" + (signRows + 1).ToString();
                    string colrowgBOs = "BO" + ROWNO + ":BO" + rowsignoff + ":" + "BO" + firstrowtable;
                    ws.Cell(colrowlastBOs).FormulaA1 = "=sum(" + colrowgBOs + ")";
                    ws.Cell(colrowlastBOs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("BO" + ROWNO + ":" + "BO" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBPs = "BP" + (signRows + 1).ToString();
                    string colrowgBPs = "BP" + ROWNO + ":BP" + rowsignoff + ":" + "BP" + firstrowtable;
                    ws.Cell(colrowlastBPs).FormulaA1 = "=sum(" + colrowgBPs + ")";
                    ws.Cell(colrowlastBPs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("BP" + ROWNO + ":" + "BP" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBQs = "BQ" + (signRows + 1).ToString();
                    string colrowgBQs = "BQ" + ROWNO + ":BQ" + rowsignoff + ":" + "BQ" + firstrowtable;
                    ws.Cell(colrowlastBQs).FormulaA1 = "=sum(" + colrowgBQs + ")";
                    ws.Cell(colrowlastBQs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("BQ" + ROWNO + ":" + "BQ" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBRs = "BR" + (signRows + 1).ToString();
                    string colrowgBRs = "BR" + ROWNO + ":BR" + rowsignoff + ":" + "BR" + firstrowtable;
                    ws.Cell(colrowlastBRs).FormulaA1 = "=sum(" + colrowgBRs + ")";
                    ws.Cell(colrowlastBRs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("BR" + ROWNO + ":" + "BR" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBSs = "BS" + (signRows + 1).ToString();
                    string colrowgBSs = "BS" + ROWNO + ":BS" + rowsignoff + ":" + "BS" + firstrowtable;
                    ws.Cell(colrowlastBSs).FormulaA1 = "=sum(" + colrowgBSs + ")";
                    ws.Cell(colrowlastBSs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("BS" + ROWNO + ":" + "BS" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBTs = "BT" + (signRows + 1).ToString();
                    string colrowgBTs = "BT" + ROWNO + ":BT" + rowsignoff + ":" + "BT" + firstrowtable;
                    ws.Cell(colrowlastBTs).FormulaA1 = "=sum(" + colrowgBTs + ")";
                    ws.Cell(colrowlastBTs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("BT" + ROWNO + ":" + "BT" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBUs = "BU" + (signRows + 1).ToString();
                    string colrowgBUs = "BU" + ROWNO + ":BU" + rowsignoff + ":" + "BU" + firstrowtable;
                    ws.Cell(colrowlastBUs).FormulaA1 = "=sum(" + colrowgBUs + ")";
                    ws.Cell(colrowlastBUs).Style.NumberFormat.NumberFormatId = 1;
                    ws.Range("BU" + ROWNO + ":" + "BU" + firstrowtable).Style.NumberFormat.NumberFormatId = 1;
                    //TOtal calculation
                    ws.Cell(signRows + 2, 1).Value = "Grand Total";
                    ws.Row(signRows + 2).Style.Font.Bold = true;
                    //string colrowlastIT = "i" + (signRows + 2).ToString();
                    //string colrowgIT = "i" + (rownumbs + 1) + "+i" + (signRows + 1);
                    //ws.Cell(colrowlastIT).FormulaA1 = "=sum(" + colrowgIT + ")";
                    //string colrowlastJT = "j" + (signRows + 2).ToString();
                    //string colrowgJT = "j" + (rownumbs + 1) + "+j" + (signRows + 1);
                    //ws.Cell(colrowlastJT).FormulaA1 = "=sum(" + colrowgJT + ")";
                    //string colrowlastKT = "K" + (signRows + 2).ToString();
                    //string colrowgKT = "K" + (rownumbs + 1) + "+K" + (signRows + 1);
                    //ws.Cell(colrowlastKT).FormulaA1 = "=sum(" + colrowgKT + ")";
                    string colrowlastLT = "L" + (signRows + 2).ToString();
                    string colrowgLT = "L" + (rownumbs + 1) + "+L" + (signRows + 1);
                    ws.Cell(colrowlastLT).FormulaA1 = "=sum(" + colrowgLT + ")";
                    ws.Cell(colrowlastLT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastMT = "M" + (signRows + 2).ToString();
                    string colrowgMT = "M" + (rownumbs + 1) + "+M" + (signRows + 1);
                    ws.Cell(colrowlastMT).FormulaA1 = "=sum(" + colrowgMT + ")";
                    ws.Cell(colrowlastMT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastnT = "n" + (signRows + 2).ToString();
                    string colrowgnT = "n" + (rownumbs + 1) + "+n" + (signRows + 1);
                    ws.Cell(colrowlastnT).FormulaA1 = "=sum(" + colrowgnT + ")";
                    ws.Cell(colrowlastnT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastOT = "O" + (signRows + 2).ToString();
                    string colrowgOT = "O" + (rownumbs + 1) + "+O" + (signRows + 1);
                    ws.Cell(colrowlastOT).FormulaA1 = "=sum(" + colrowgOT + ")";
                    ws.Cell(colrowlastOT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastPT = "P" + (signRows + 2).ToString();
                    string colrowgPT = "P" + (rownumbs + 1) + "+P" + (signRows + 1);
                    ws.Cell(colrowlastPT).FormulaA1 = "=sum(" + colrowgPT + ")";
                    ws.Cell(colrowlastPT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastQT = "Q" + (signRows + 2).ToString();
                    string colrowgQT = "Q" + (rownumbs + 1) + "+Q" + (signRows + 1);
                    ws.Cell(colrowlastQT).FormulaA1 = "=sum(" + colrowgQT + ")";
                    ws.Cell(colrowlastQT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastRT = "R" + (signRows + 2).ToString();
                    string colrowgRT = "R" + (rownumbs + 1) + "+R" + (signRows + 1);
                    ws.Cell(colrowlastRT).FormulaA1 = "=sum(" + colrowgRT + ")";
                    ws.Cell(colrowlastRT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastST = "S" + (signRows + 2).ToString();
                    string colrowgST = "S" + (rownumbs + 1) + "+S" + (signRows + 1);
                    ws.Cell(colrowlastST).FormulaA1 = "=sum(" + colrowgST + ")";
                    ws.Cell(colrowlastST).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastTT = "T" + (signRows + 2).ToString();
                    string colrowgTT = "T" + (rownumbs + 1) + "+T" + (signRows + 1);
                    ws.Cell(colrowlastTT).FormulaA1 = "=sum(" + colrowgTT + ")";
                    ws.Cell(colrowlastTT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastUT = "U" + (signRows + 2).ToString();
                    string colrowgUT = "U" + (rownumbs + 1) + "+U" + (signRows + 1);
                    ws.Cell(colrowlastUT).FormulaA1 = "=sum(" + colrowgUT + ")";
                    ws.Cell(colrowlastUT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastVT = "V" + (signRows + 2).ToString();
                    string colrowgVT = "V" + (rownumbs + 1) + "+V" + (signRows + 1);
                    ws.Cell(colrowlastVT).FormulaA1 = "=sum(" + colrowgVT + ")";
                    ws.Cell(colrowlastVT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastWT = "W" + (signRows + 2).ToString();
                    string colrowgWT = "W" + (rownumbs + 1) + "+W" + (signRows + 1);
                    ws.Cell(colrowlastWT).FormulaA1 = "=sum(" + colrowgWT + ")";
                    ws.Cell(colrowlastWT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastXT = "X" + (signRows + 2).ToString();
                    string colrowgXT = "X" + (rownumbs + 1) + "+X" + (signRows + 1);
                    ws.Cell(colrowlastXT).FormulaA1 = "=sum(" + colrowgXT + ")";
                    ws.Cell(colrowlastXT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastYT = "Y" + (signRows + 2).ToString();
                    string colrowgYT = "Y" + (rownumbs + 1) + "+Y" + (signRows + 1);
                    ws.Cell(colrowlastYT).FormulaA1 = "=sum(" + colrowgYT + ")";
                    ws.Cell(colrowlastYT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastZT = "Z" + (signRows + 2).ToString();
                    string colrowgZT = "Z" + (rownumbs + 1) + "+Z" + (signRows + 1);
                    ws.Cell(colrowlastZT).FormulaA1 = "=sum(" + colrowgZT + ")";
                    ws.Cell(colrowlastZT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAAT = "AA" + (signRows + 2).ToString();
                    string colrowgAAT = "AA" + (rownumbs + 1) + "+AA" + (signRows + 1);
                    ws.Cell(colrowlastAAT).FormulaA1 = "=sum(" + colrowgAAT + ")";
                    ws.Cell(colrowlastAAT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastABT = "AB" + (signRows + 2).ToString();
                    string colrowgABT = "AB" + (rownumbs + 1) + "+AB" + (signRows + 1);
                    ws.Cell(colrowlastABT).FormulaA1 = "=sum(" + colrowgABT + ")";
                    ws.Cell(colrowlastABT).Style.NumberFormat.NumberFormatId = 1;
                    string colrowlastACT = "AC" + (signRows + 2).ToString();
                    string colrowgACT = "AC" + (rownumbs + 1) + "+AC" + (signRows + 1);
                    ws.Cell(colrowlastACT).FormulaA1 = "=sum(" + colrowgACT + ")";
                    ws.Cell(colrowlastACT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastADT = "AD" + (signRows + 2).ToString();
                    string colrowgADT = "AD" + (rownumbs + 1) + "+AD" + (signRows + 1);
                    ws.Cell(colrowlastADT).FormulaA1 = "=sum(" + colrowgADT + ")";
                    ws.Cell(colrowlastADT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAET = "AE" + (signRows + 2).ToString();
                    string colrowgAET = "AE" + (rownumbs + 1) + "+AE" + (signRows + 1);
                    ws.Cell(colrowlastAET).FormulaA1 = "=sum(" + colrowgAET + ")";
                    ws.Cell(colrowlastAET).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAFT = "AF" + (signRows + 2).ToString();
                    string colrowgAFT = "AF" + (rownumbs + 1) + "+AF" + (signRows + 1);
                    ws.Cell(colrowlastAFT).FormulaA1 = "=sum(" + colrowgAFT + ")";
                    ws.Cell(colrowlastAFT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAGT = "AG" + (signRows + 2).ToString();
                    string colrowgAGT = "AG" + (rownumbs + 1) + "+AG" + (signRows + 1);
                    ws.Cell(colrowlastAGT).FormulaA1 = "=sum(" + colrowgAGT + ")";
                    ws.Cell(colrowlastAGT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAHT = "AH" + (signRows + 2).ToString();
                    string colrowgAHT = "AH" + (rownumbs + 1) + "+AH" + (signRows + 1);
                    ws.Cell(colrowlastAHT).FormulaA1 = "=sum(" + colrowgAHT + ")";
                    ws.Cell(colrowlastAHT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAIT = "AI" + (signRows + 2).ToString();
                    string colrowgAIT = "AI" + (rownumbs + 1) + "+AI" + (signRows + 1);
                    ws.Cell(colrowlastAIT).FormulaA1 = "=sum(" + colrowgAIT + ")";
                    ws.Cell(colrowlastAIT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAJT = "AJ" + (signRows + 2).ToString();
                    string colrowgAJT = "AJ" + (rownumbs + 1) + "+AJ" + (signRows + 1);
                    ws.Cell(colrowlastAJT).FormulaA1 = "=sum(" + colrowgAJT + ")";
                    ws.Cell(colrowlastAJT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAKT = "AK" + (signRows + 2).ToString();
                    string colrowgAKT = "AK" + (rownumbs + 1) + "+AK" + (signRows + 1);
                    ws.Cell(colrowlastAKT).FormulaA1 = "=sum(" + colrowgAKT + ")";
                    ws.Cell(colrowlastAKT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastALT = "AL" + (signRows + 2).ToString();
                    string colrowgALT = "AL" + (rownumbs + 1) + "+AL" + (signRows + 1);
                    ws.Cell(colrowlastALT).FormulaA1 = "=sum(" + colrowgALT + ")";
                    ws.Cell(colrowlastALT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAMT = "AM" + (signRows + 2).ToString();
                    string colrowgAMT = "AM" + (rownumbs + 1) + "+AM" + (signRows + 1);
                    ws.Cell(colrowlastAMT).FormulaA1 = "=sum(" + colrowgAMT + ")";
                    ws.Cell(colrowlastAMT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastANT = "AN" + (signRows + 2).ToString();
                    string colrowgANT = "AN" + (rownumbs + 1) + "+AN" + (signRows + 1);
                    ws.Cell(colrowlastANT).FormulaA1 = "=sum(" + colrowgANT + ")";
                    ws.Cell(colrowlastANT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAOT = "AO" + (signRows + 2).ToString();
                    string colrowgAOT = "AO" + (rownumbs + 1) + "+AO" + (signRows + 1);
                    ws.Cell(colrowlastAOT).FormulaA1 = "=sum(" + colrowgAOT + ")";
                    ws.Cell(colrowlastAOT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAPT = "AP" + (signRows + 2).ToString();
                    string colrowgAPT = "AP" + (rownumbs + 1) + "+AP" + (signRows + 1);
                    ws.Cell(colrowlastAPT).FormulaA1 = "=sum(" + colrowgAPT + ")";
                    ws.Cell(colrowlastAPT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAQT = "AQ" + (signRows + 2).ToString();
                    string colrowgAQT = "AQ" + (rownumbs + 1) + "+AQ" + (signRows + 1);
                    ws.Cell(colrowlastAQT).FormulaA1 = "=sum(" + colrowgAQT + ")";
                    ws.Cell(colrowlastAQT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastART = "AR" + (signRows + 2).ToString();
                    string colrowgART = "AR" + (rownumbs + 1) + "+AR" + (signRows + 1);
                    ws.Cell(colrowlastART).FormulaA1 = "=sum(" + colrowgART + ")";
                    ws.Cell(colrowlastART).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAST = "AS" + (signRows + 2).ToString();
                    string colrowgAST = "AS" + (rownumbs + 1) + "+AS" + (signRows + 1);
                    ws.Cell(colrowlastAST).FormulaA1 = "=sum(" + colrowgAST + ")";
                    ws.Cell(colrowlastAST).Style.NumberFormat.NumberFormatId = 1;
                    string colrowlastATT = "AT" + (signRows + 2).ToString();
                    string colrowgATT = "AT" + (rownumbs + 1) + "+AT" + (signRows + 1);
                    ws.Cell(colrowlastATT).FormulaA1 = "=sum(" + colrowgATT + ")";
                    ws.Cell(colrowlastATT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAUT = "AU" + (signRows + 2).ToString();
                    string colrowgAUT = "AU" + (rownumbs + 1) + "+AU" + (signRows + 1);
                    ws.Cell(colrowlastAUT).FormulaA1 = "=sum(" + colrowgAUT + ")";
                    ws.Cell(colrowlastAUT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAVT = "AV" + (signRows + 2).ToString();
                    string colrowgAVT = "AV" + (rownumbs + 1) + "+AV" + (signRows + 1);
                    ws.Cell(colrowlastAVT).FormulaA1 = "=sum(" + colrowgAVT + ")";
                    ws.Cell(colrowlastAVT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAWT = "AW" + (signRows + 2).ToString();
                    string colrowgAWT = "AW" + (rownumbs + 1) + "+AW" + (signRows + 1);
                    ws.Cell(colrowlastAWT).FormulaA1 = "=sum(" + colrowgAWT + ")";
                    ws.Cell(colrowlastAWT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAXT = "AX" + (signRows + 2).ToString();
                    string colrowgAXT = "AX" + (rownumbs + 1) + "+AX" + (signRows + 1);
                    ws.Cell(colrowlastAXT).FormulaA1 = "=sum(" + colrowgAXT + ")";
                    ws.Cell(colrowlastAXT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAYT = "AY" + (signRows + 2).ToString();
                    string colrowgAYT = "AY" + (rownumbs + 1) + "+AY" + (signRows + 1);
                    ws.Cell(colrowlastAYT).FormulaA1 = "=sum(" + colrowgAYT + ")";
                    ws.Cell(colrowlastAYT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastAZT = "AZ" + (signRows + 2).ToString();
                    string colrowgAZT = "AZ" + (rownumbs + 1) + "+AZ" + (signRows + 1);
                    ws.Cell(colrowlastAZT).FormulaA1 = "=sum(" + colrowgAZT + ")";
                    ws.Cell(colrowlastAZT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBAT = "BA" + (signRows + 2).ToString();
                    string colrowgBAT = "BA" + (rownumbs + 1) + "+BA" + (signRows + 1);
                    ws.Cell(colrowlastBAT).FormulaA1 = "=sum(" + colrowgBAT + ")";
                    ws.Cell(colrowlastBAT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBBT = "BB" + (signRows + 2).ToString();
                    string colrowgBBT = "BB" + (rownumbs + 1) + "+BB" + (signRows + 1);
                    ws.Cell(colrowlastBBT).FormulaA1 = "=sum(" + colrowgBBT + ")";
                    ws.Cell(colrowlastBBT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBCT = "BC" + (signRows + 2).ToString();
                    string colrowgBCT = "BC" + (rownumbs + 1) + "+BC" + (signRows + 1);
                    ws.Cell(colrowlastBCT).FormulaA1 = "=sum(" + colrowgBCT + ")";
                    ws.Cell(colrowlastBCT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBDT = "BD" + (signRows + 2).ToString();
                    string colrowgBDT = "BD" + (rownumbs + 1) + "+BD" + (signRows + 1);
                    ws.Cell(colrowlastBDT).FormulaA1 = "=sum(" + colrowgBDT + ")";
                    ws.Cell(colrowlastBDT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBET = "BE" + (signRows + 2).ToString();
                    string colrowgBET = "BE" + (rownumbs + 1) + "+BE" + (signRows + 1);
                    ws.Cell(colrowlastBET).FormulaA1 = "=sum(" + colrowgBET + ")";
                    ws.Cell(colrowlastBET).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBFT = "BF" + (signRows + 2).ToString();
                    string colrowgBFT = "BF" + (rownumbs + 1) + "+BF" + (signRows + 1);
                    ws.Cell(colrowlastBFT).FormulaA1 = "=sum(" + colrowgBFT + ")";
                    ws.Cell(colrowlastBFT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBGT = "BG" + (signRows + 2).ToString();
                    string colrowgBGT = "BG" + (rownumbs + 1) + "+BG" + (signRows + 1);
                    ws.Cell(colrowlastBGT).FormulaA1 = "=sum(" + colrowgBGT + ")";
                    ws.Cell(colrowlastBGT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBHT = "BH" + (signRows + 2).ToString();
                    string colrowgBHT = "BH" + (rownumbs + 1) + "+BH" + (signRows + 1);
                    ws.Cell(colrowlastBHT).FormulaA1 = "=sum(" + colrowgBHT + ")";
                    ws.Cell(colrowlastBHT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBIT = "BI" + (signRows + 2).ToString();
                    string colrowgBIT = "BI" + (rownumbs + 1) + "+BI" + (signRows + 1);
                    ws.Cell(colrowlastBIT).FormulaA1 = "=sum(" + colrowgBIT + ")";
                    ws.Cell(colrowlastBIT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBJT = "BJ" + (signRows + 2).ToString();
                    string colrowgBJT = "BJ" + (rownumbs + 1) + "+BJ" + (signRows + 1);
                    ws.Cell(colrowlastBJT).FormulaA1 = "=sum(" + colrowgBJT + ")";
                    ws.Cell(colrowlastBJT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBKT = "BK" + (signRows + 2).ToString();
                    string colrowgBKT = "BK" + (rownumbs + 1) + "+BK" + (signRows + 1);
                    ws.Cell(colrowlastBKT).FormulaA1 = "=sum(" + colrowgBKT + ")";
                    ws.Cell(colrowlastBKT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBLT = "BL" + (signRows + 2).ToString();
                    string colrowgBLT = "BL" + (rownumbs + 1) + "+BL" + (signRows + 1);
                    ws.Cell(colrowlastBLT).FormulaA1 = "=sum(" + colrowgBLT + ")";
                    ws.Cell(colrowlastBLT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBMT = "BM" + (signRows + 2).ToString();
                    string colrowgBMT = "BM" + (rownumbs + 1) + "+BM" + (signRows + 1);
                    ws.Cell(colrowlastBMT).FormulaA1 = "=sum(" + colrowgBMT + ")";
                    ws.Cell(colrowlastBMT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBNT = "BN" + (signRows + 2).ToString();
                    string colrowgBNT = "BN" + (rownumbs + 1) + "+BN" + (signRows + 1);
                    ws.Cell(colrowlastBNT).FormulaA1 = "=sum(" + colrowgBNT + ")";
                    ws.Cell(colrowlastBNT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBOT = "BO" + (signRows + 2).ToString();
                    string colrowgBOT = "BO" + (rownumbs + 1) + "+BO" + (signRows + 1);
                    ws.Cell(colrowlastBOT).FormulaA1 = "=sum(" + colrowgBOT + ")";
                    ws.Cell(colrowlastBOT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBPT = "BP" + (signRows + 2).ToString();
                    string colrowgBPT = "BP" + (rownumbs + 1) + "+BP" + (signRows + 1);
                    ws.Cell(colrowlastBPT).FormulaA1 = "=sum(" + colrowgBPT + ")";
                    ws.Cell(colrowlastBPT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBQT = "BQ" + (signRows + 2).ToString();
                    string colrowgBQT = "BQ" + (rownumbs + 1) + "+BQ" + (signRows + 1);
                    ws.Cell(colrowlastBQT).FormulaA1 = "=sum(" + colrowgBQT + ")";
                    ws.Cell(colrowlastBQT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBRT = "BR" + (signRows + 2).ToString();
                    string colrowgBRT = "BR" + (rownumbs + 1) + "+BR" + (signRows + 1);
                    ws.Cell(colrowlastBRT).FormulaA1 = "=sum(" + colrowgBRT + ")";
                    ws.Cell(colrowlastBRT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBST = "BS" + (signRows + 2).ToString();
                    string colrowgBST = "BS" + (rownumbs + 1) + "+BS" + (signRows + 1);
                    ws.Cell(colrowlastBST).FormulaA1 = "=sum(" + colrowgBST + ")";
                    ws.Cell(colrowlastBST).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBTT = "BT" + (signRows + 2).ToString();
                    string colrowgBTT = "BT" + (rownumbs + 1) + "+BT" + (signRows + 1);
                    ws.Cell(colrowlastBTT).FormulaA1 = "=sum(" + colrowgBTT + ")";
                    ws.Cell(colrowlastBTT).Style.NumberFormat.NumberFormatId = 1;

                    string colrowlastBUT = "BU" + (signRows + 2).ToString();
                    string colrowgBUT = "BU" + (rownumbs + 1) + "+BU" + (signRows + 1);
                    ws.Cell(colrowlastBUT).FormulaA1 = "=sum(" + colrowgBUT + ")";
                    ws.Cell(colrowlastBUT).Style.NumberFormat.NumberFormatId = 1;
                }
                ws.Range("A3:BU3").Style.Alignment.TextRotation = 90;
                if (query.Count() > 0)
                {
                    ws.Range("AH3:BG3").Style.Fill.BackgroundColor = XLColor.GreenPigment;
                    ws.Range("BH3:BR3").Style.Fill.BackgroundColor = XLColor.Brown;
                }
                ws.Range("A3:BU3").Style.Alignment.WrapText = true;
                ws.Row(3).Height = 55;
                //  ws.Style.Font.FontSize = 7;
                ws.Column(1).Width = 2; ws.Column(2).Width = 25;
                ws.Column(5).Width = 10; ws.Column(6).Width = 10; ws.Column(7).Width = 10;
                ws.Column(8).Width = 10;
                ws.Column(9).Width = 10;
                ws.PageSetup.ShowGridlines = true;
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
               
                wb.SaveAs(fileName);

                var errorMessage = "you can return the errors here!";

                return Json(new { fileName = fileName, errorMessage });
            }
        }


        public JsonResult VesselPayslip(string vesselId, int month, int year)
        {
            var getpblist = _context.TblPortageBills.Where(x => x.IsDeleted == false && x.From.Value.Month == month && x.From.Value.Year == year && x.Vesselid == int.Parse(vesselId)).ToList().Select(x => x.PortageBillId);
            string pid = string.Join(",", getpblist);
            string[] portageid = pid.Split(',');          

            using (ZipFile zip = new ZipFile())
            {               
                zip.AlternateEncodingUsage = ZipOption.AsNecessary;
                // zip.AddDirectoryByName("Files");
                foreach (var pitem in portageid)
                {
                    //var datas = _context.PortageBillVMs.FromSqlRaw<PortageBillVM>("spVesselPayslip @p0", pitem);
                    //var request = HttpContext.Current.Request;                  
                    string requesturl = HttpContext.Request.GetEncodedUrl();
                    string newurl = requesturl.Substring(0, requesturl.LastIndexOf("/"));
                    string finalurl = newurl.Substring(0, newurl.LastIndexOf("/"));
                    string localpath = finalurl +"/";
                    string url = "";
                    //var portageids = datas;
                    //foreach (var item in portageids)
                    //{
                        ViewBag.vesselname = "";
                        string crewpayslipname = "";
                        string cu = "";
                     //   int portagebillid = item.PortageBillId.Value;
                     //   var data = datas.FirstOrDefault(x => x.pid == portagebillid);
                        string attachment = "";
                        string functionreturn = RetunFileName(int.Parse(pitem));
                        cu = functionreturn.Substring(0, functionreturn.IndexOf("-"));
                        crewpayslipname = functionreturn.Substring(functionreturn.IndexOf("-") + 1);
                        string filename = crewpayslipname + "_" + "Salaryslip" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";
                        if (attachment == null || attachment == "")
                        {
                            if (cu != null)
                            {
                                if (cu == "NGN")
                                    url = localpath + "Portagebill/Nigpayslips?pid=" + pitem;
                                else
                                    url = localpath + "Portagebill/Indpayslip?pid=" + pitem;
                            }
                            var webRoot = _appEnvironment.WebRootPath;
                            string footerUrl = System.IO.Path.Combine(webRoot, "PDFHeaders/PDFFooter.htm");
                            string pdf_page_size = PdfPageSize.A4.ToString();
                            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), pdf_page_size, true);
                            string pdf_orientation = PdfPageOrientation.Portrait.ToString();
                            PdfPageOrientation pdfOrientation = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation), pdf_orientation, true);
                            int webPageWidth = 1000;
                            int webPageHeight = 0;
                            // instantiate a html to pdf converter object
                            HtmlToPdf converter = new HtmlToPdf();
                            converter.Header.DisplayOnFirstPage = false;
                            converter.Header.DisplayOnOddPages = true;
                            converter.Header.DisplayOnEvenPages = true;
                            // set converter options
                            converter.Options.PdfPageSize = pageSize;
                            converter.Options.PdfPageOrientation = pdfOrientation;
                            converter.Options.WebPageWidth = webPageWidth;
                            converter.Options.WebPageHeight = webPageHeight;
                            int headerHeight = 60;
                            //int footerHeight = 50;
                            // header settings
                            converter.Options.DisplayHeader = true;
                            converter.Header.DisplayOnFirstPage = false;
                            converter.Header.DisplayOnOddPages = true;
                            converter.Header.DisplayOnEvenPages = true;
                            converter.Header.Height = headerHeight;
                            PdfDocument doc = converter.ConvertUrl(url);
                            // byte[] pdf = doc.Save();
                            MemoryStream ms = new System.IO.MemoryStream();
                            doc.Save(ms);
                            //byte[] bytes = ms.ToArray();
                            ms.Position = 0;
                            string folderName = "Salaryslip";
                            //string localpath = _config.GetValue<string>("serverUrl");
                            string webRootPath = _appEnvironment.WebRootPath;
                            string PathToSave = Path.Combine(webRootPath, folderName + "/" + filename);
                            string PathToShow = Path.Combine(localpath, folderName + "/" + filename);
                            FileStream file = new FileStream(PathToSave, FileMode.Create, FileAccess.Write);
                            ms.WriteTo(file);
                            file.Close();
                            string filePath = filename;
                            //zip.AddFile(PathToSave, "Files");
                            zip.AddFile(PathToSave).FileName = filename;
                        }
                        else
                        {
                            string filepath = attachment.Substring(attachment.LastIndexOf("/") + 1);
                            string folderName = "Upload/Payslip/";
                            string webRootPath = _appEnvironment.WebRootPath;
                            string PathToSave = Path.Combine(webRootPath, folderName + "/" + filepath);
                            zip.AddFile(PathToSave).FileName = filename;
                            //zip.AddFile(PathToSave, "Files");
                        }
                    //}
                }
                string vesselnames = ViewBag.vesselname;
                string zipName = vesselnames + "_" + DateTime.Now.ToString("yyyy-MMM-dd") + ".zip";
                zip.Save(zipName);
                return Json(new { fileName = zipName });
                
            }
            return null;
        }

        public string RetunFileName(int portagebillid)
        {
            _context.ChangeTracker.Clear();
            var PortageBill = _context.PortageBillVMs.FromSqlRaw<PortageBillVM>("spVesselPayslip @p0", portagebillid).AsTracking().AsEnumerable().FirstOrDefault();
            return PortageBill.Currency + "-" + PortageBill.RankName + " " + PortageBill.CrewName + "_" + Convert.ToDateTime(PortageBill.From).ToString("MMM-yyyy");
        }
        public async Task<IActionResult> Indpayslip(string pid)
        {
            var data = _context.PortageBillVMs.FromSqlRaw<PortageBillVM>("spVesselPayslip @p0", pid);
            var vesselId = data.AsEnumerable().FirstOrDefault().Vesselid;
            var mangerId = _context.TblVessels.Where(x => x.VesselId == vesselId).FirstOrDefault().TechManager1;
            var managernames = _context.TblManagers.Where(x => x.ManagerId == mangerId).FirstOrDefault().Managers;
            ViewBag.managername = managernames;
            ViewBag.crewdeatil = _context.PortageBillVMs.FromSqlRaw<PortageBillVM>("spVesselPayslip @p0", pid);
            await Task.Delay(500);
            return PartialView();
        }

        public async Task<IActionResult> Nigpayslips(string pid)
        {
            await Task.Delay(500);
            ViewBag.crewdeatil = _context.PortageBillVMs.FromSqlRaw<PortageBillVM>("spVesselPayslip @p0", pid);
            return PartialView();
        }       
        private void DeletePreviousFiles(string foldername)
        {
            string webRootPath = _appEnvironment.WebRootPath;
            string filePath = "";
            if (foldername == "Salaryslip")
                filePath = Path.Combine(webRootPath, "Salaryslip");
            else
                filePath = Path.Combine(webRootPath, "BOW");
            var files = new DirectoryInfo(@filePath).GetFiles();
            foreach (var file in files)
            {
                if (DateTime.UtcNow - file.CreationTimeUtc > TimeSpan.FromDays(1))
                {
                    System.IO.File.Delete(file.FullName);
                }
            }
        }
        public async Task<IActionResult> NigBowpayslips(int crewId, int crewListId, DateTime signOffDate)
        {
            var data = _context.portageBillBows.FromSqlRaw("getBOWData @p0, @p1, @p2", crewId, crewListId, signOffDate).ToList();
            var vesselId = data.AsEnumerable().FirstOrDefault().Vesselid;
            var mangerId = _context.TblVessels.Where(x => x.VesselId == vesselId).FirstOrDefault().TechManager1;
            var managernames = _context.TblManagers.Where(x => x.ManagerId == mangerId).FirstOrDefault().Managers;
            ViewBag.managername = managernames;
            ViewBag.crewdeatil = _context.PortageBillVMs.FromSqlRaw<PortageBillVM>("spVesselPayslip @p0", data.Select(x => x.PortageBillId).FirstOrDefault());
            await Task.Delay(500);
            return PartialView();
        }
        public JsonResult generateBowPDF(int crewId, int crewListId, string signOffDate, string currency)
        {
            try
            {
                string requesturl = HttpContext.Request.GetEncodedUrl();
                string newurl = requesturl.Substring(0, requesturl.LastIndexOf("/"));
                string finalurl = newurl.Substring(0, newurl.LastIndexOf("/"));
                string localpath = finalurl + "/";
                DeletePreviousFiles("BOW");
                string url = "";
                if (currency == "NGN")
                    url = localpath + "Portagebill/NigBowpayslips?crewid=" + crewId + "&crewListId=" + crewListId + "&Signoffdate=" + signOffDate;
                else
                    url = localpath + "Portagebill/IndBowpayslip?crewid=" + crewId + "&crewListId=" + crewListId + "&Signoffdate=" + signOffDate;
                var webRoot = _appEnvironment.WebRootPath;
                string headerUrl = System.IO.Path.Combine(webRoot, "PDFHeaders/PDFEmptyHeader.htm");
                string footerUrl = System.IO.Path.Combine(webRoot, "PDFHeaders/PDFFooter.htm");
                string pdf_page_size = PdfPageSize.A4.ToString();
                PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), pdf_page_size, true);

                string pdf_orientation = PdfPageOrientation.Portrait.ToString();
                PdfPageOrientation pdfOrientation = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation), pdf_orientation, true);
                int webPageWidth = 1000;
                int webPageHeight = 0;
                // instantiate a html to pdf converter object
                HtmlToPdf converter = new HtmlToPdf();
                converter.Header.DisplayOnFirstPage = false;
                converter.Header.DisplayOnOddPages = true;
                converter.Header.DisplayOnEvenPages = true;
                // set converter options
                converter.Options.PdfPageSize = pageSize;
                converter.Options.PdfPageOrientation = pdfOrientation;
                converter.Options.WebPageWidth = webPageWidth;
                converter.Options.WebPageHeight = webPageHeight;
                int headerHeight = 60;
                //int footerHeight = 50;
                // header settings
                converter.Options.DisplayHeader = true;
                converter.Header.DisplayOnFirstPage = false;
                converter.Header.DisplayOnOddPages = true;
                converter.Header.DisplayOnEvenPages = true;
                converter.Header.Height = headerHeight;               
                PdfDocument doc = converter.ConvertUrl(url);
                string filename = "BOW" + crewId + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";
                MemoryStream ms = new System.IO.MemoryStream();
                doc.Save(ms);
                ms.Position = 0;
                string folderName = "BOW";
                string webRootPath = _appEnvironment.WebRootPath;
                string PathToSave = Path.Combine(webRootPath, folderName + "/" + filename);
                string PathToShow = Path.Combine(localpath, folderName + "/" + filename);
                FileStream file = new FileStream(PathToSave, FileMode.Create, FileAccess.Write);
                ms.WriteTo(file);
                file.Close();
                return Json(file);
            }
            catch (Exception ex) { throw ex; }
        }

        public async Task<IActionResult> IndBowpayslip(int crewId, int crewListId, DateTime signOffDate)
        {
        //    var data = _context.PortageBillVMs.FromSqlRaw<PortageBillVM>("getBOWData @p0", pid);
            var data = _context.portageBillBows.FromSqlRaw("getBOWData @p0, @p1, @p2", crewId, crewListId, signOffDate).ToList();
            var vesselId = data.AsEnumerable().FirstOrDefault().Vesselid;
            var mangerId = _context.TblVessels.Where(x => x.VesselId == vesselId).FirstOrDefault().TechManager1;
            var managernames = _context.TblManagers.Where(x => x.ManagerId == mangerId).FirstOrDefault().Managers;
            ViewBag.managername = managernames;
            ViewBag.crewdeatil = _context.PortageBillVMs.FromSqlRaw<PortageBillVM>("spVesselPayslip @p0", data.Select(x=>x.PortageBillId).FirstOrDefault());
            await Task.Delay(500);
            return PartialView();
        }
        //Convert var data to Datatable
        public DataTable LINQResultToDataTable<T>(IEnumerable<T> Linqlist)
        {
            DataTable dt = new DataTable();
            PropertyInfo[] columns = null;

            if (Linqlist == null) return dt;

            foreach (T Record in Linqlist)
            {

                if (columns == null)
                {
                    columns = ((Type)Record.GetType()).GetProperties();
                    foreach (PropertyInfo GetProperty in columns)
                    {
                        Type colType = GetProperty.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dt.Columns.Add(new DataColumn(GetProperty.Name, colType));
                    }
                }

                DataRow dr = dt.NewRow();

                foreach (PropertyInfo pinfo in columns)
                {
                    dr[pinfo.Name] = pinfo.GetValue(Record, null) == null ? DBNull.Value : pinfo.GetValue
                    (Record, null);
                }

                dt.Rows.Add(dr);
            }
            return dt;
        }

        public JsonResult LockPortageBill(string vesselId, int month, int year)
        {
            var Portdata = _context.TblPortageBills.Where(x => x.IsDeleted == false && x.From.Value.Month == month && x.From.Value.Year == year && x.Vesselid == int.Parse(vesselId)).ToList();

            string checkpbtilldate = "";

            var PortdataVM = _context.PortageBillVMs.FromSqlRaw<PortageBillVM>("getPortageBill @p0, @p1, @p2, @p3, @p4", vesselId, month, year, "no", checkpbtilldate).ToList();     

            return Json(new { portageBillData = Portdata , portageBillVM = PortdataVM });
        }

        public JsonResult UpdateBillStatus(string vesselId, int month, int year)
        {
            Sendbackup();
              var portUpdate = _context.TblPortageBills.Where(x => x.IsDeleted == false && x.From.Value.Month == month && x.From.Value.Year == year && x.Vesselid == int.Parse(vesselId)).ToList();

            foreach(var item in portUpdate)
            {
                 item.BillStatus = 1;
                _context.TblPortageBills.Update(item);
                _context.SaveChanges();
            }
            return Json(new { updatePortageBill = portUpdate });
        }

        public void Sendbackup()
        {
            var currentDate = DateTime.Now;
            var sixMonth = currentDate.AddDays(-6);           

            var ActivitySignOns = _context.TblActivitySignOns.Where(x => x.IsDeleted == false && (x.RecDate >= sixMonth || x.ModifiedDate >= sixMonth)).Select(x => new TblActivitySignOnVM
            {

                ActivitySignOnId = x.ActivitySignOnId,
                CrewId = x.CrewId.Value,
                ContractId = x.ContractId.Value,
                VesselId = x.VesselId.Value,
                CountryId = x.CountryId.Value,
                SeaportId = x.SeaportId.Value,
                RankId = x.RankId.Value,
                SignOnReasonId = x.SignOnReasonId.Value,
                ReliveesCrewListId = x.ReliveesCrewListId.Value,
                Contract = x.Contract,
                ExpectedSignOnDate = x.ExpectedSignOnDate.ToString(),
                Duration = x.Duration,
                ReliefDate = x.ReliefDate.Value.ToString(),
                ExpectedTravelDate = x.ExpectedTravelDate.Value.ToString(),
                ExtraCrewOnBoard = x.ExtraCrewOnBoard,
                ExtraCrewReasonId = x.ExtraCrewReasonId.Value,
                ExtraApprovedBy = x.ExtraApprovedBy,
                DocsValidityCheckPeriod = x.DocsValidityCheckPeriod,
                AllowBeginTravel = x.AllowBeginTravel.HasValue ? x.AllowBeginTravel.Value : default,
                PreJoiningMedicals = x.PreJoiningMedicals,
                Appraisal = x.Appraisal.Value,
                OwnerWage = x.OwnerWage.Value,
                Remarks = x.Remarks,
                ModifiedBy = x.ModifiedBy,
                ModifiedDate = x.ModifiedDate.Value.ToString(),
                IsDeleted = x.IsDeleted.Value,
                IsSignon = x.IsSignon.Value,
                RecDate = x.RecDate.Value.ToString(),
                CreatedBy = x.CreatedBy
            }).ToList();
          
            var AssignmentsWithOthers = _context.TblAssignmentsWithOthers.Where(x => x.IsDeleted == false && (x.RecDate >= sixMonth || x.ModifiedDate >= sixMonth));
         
            var CrewDetails = _context.TblCrewDetails.Where(x => x.IsDeleted == false && (x.RecDate >= sixMonth || x.ModifiedDate >= sixMonth)).Select(x => new TblCrewDetailVM
            {

                CrewId = x.CrewId,
                CountryId = x.CountryId.HasValue ? x.CountryId.Value : default,
                RankId = x.RankId.HasValue ? x.RankId.Value : default,
                PoolId = x.PoolId.HasValue ? x.PoolId.Value : default,
                ZonalId = x.ZonalId.HasValue ? x.ZonalId.Value : default,
                MtunionId = x.MtunionId.HasValue ? x.MtunionId.Value : default,
                NtbrReasonId = x.NtbrReasonId.HasValue ? x.NtbrReasonId.Value : default,
                InActiveReasonId = x.InActiveReasonId.HasValue ? x.InActiveReasonId.Value : default,
                EmpNumber = x.EmpNumber,
                Status = x.Status,
                PreviousStatus = x.PreviousStatus,
                FirstName = x.FirstName,
                MiddleName = x.MiddleName,
                LastName = x.LastName,
                Dob = x.Dob.HasValue ? x.Dob.Value.ToString() : default,
                PlaceOfBirth = x.PlaceOfBirth,
                CivilStatus = x.CivilStatus,
                Doa = x.Doa.HasValue ? x.Doa.Value.ToString() : default,
                Gender = x.Gender,
                EnglishFluency = x.EnglishFluency,
                UserImage = x.UserImage,
                ShipCategory = x.ShipCategory,
                AppliedOn = x.AppliedOn.HasValue ? x.AppliedOn.Value.ToString() : default,
                FirstJoinDate = x.FirstJoinDate.HasValue ? x.FirstJoinDate.Value.ToString() : default,
                OtherTravelDocNo = x.OtherTravelDocNo,
                ManningOffice = x.ManningOffice,
                MembershipNumber = x.MembershipNumber,
                DateOfJoining = x.DateOfJoining.HasValue ? x.DateOfJoining.Value.ToString() : default,
                Attachment = x.Attachment,
                Benefits = x.Benefits,
                Height = x.Height,
                Weight = x.Weight,
                ShoesSize = x.ShoesSize,
                BoilerSuitSize = x.BoilerSuitSize,
                ShirtSize = x.ShirtSize,
                TrouserSize = x.TrouserSize,
                HairColor = x.HairColor,
                EyeColor = x.EyeColor,
                DistinguishMark = x.DistinguishMark,
                Resume = x.Resume,
                Remark = x.Remark,
                ApplicantStatus = x.ApplicantStatus,
                LastVessel = x.LastVessel.HasValue ? x.LastVessel.Value : default,
                VesselId = x.VesselId.HasValue ? x.VesselId.Value : default,
                ReliefDate = x.ReliefDate.HasValue ? x.ReliefDate.Value.ToString() : default,
                IsNtbr = x.IsNtbr.HasValue ? x.IsNtbr.Value : default,
                Ntbron = x.Ntbron.HasValue ? x.Ntbron.Value.ToString() : default,
                Ntbrby = x.Ntbrby,
                InActive = x.InActive.HasValue ? x.InActive.Value : default,
                InActiveOn = x.InActiveOn.HasValue ? x.InActiveOn.Value.ToString() : default,
                InActiveBy = x.InActiveBy,
                IsDeleted = x.IsDeleted.HasValue ? x.IsDeleted.Value : default,
                RecDate = x.RecDate.HasValue ? x.RecDate.Value.ToString() : default,
                ModifiedBy = x.ModifiedBy,
                ModifiedDate = x.ModifiedDate.HasValue ? x.ModifiedDate.Value.ToString() : default,
                Signature = x.Signature,
                PlanRankId = x.PlanRankId.HasValue ? x.PlanRankId.Value : default,
                PlanStatus = x.PlanStatus,
                PlanVesselId = x.PlanVesselId.HasValue ? x.PlanVesselId.Value : default,
                CreatedBy = x.CreatedBy.HasValue ? x.CreatedBy.Value : default,
                ImpRemark = x.ImpRemark,
                ApprovedBy = x.ApprovedBy.HasValue ? x.ApprovedBy.Value : default,
                MaskRemarks = x.MaskRemarks,
                MaskAttachment = x.MaskAttachment,
                MaskedBy = x.MaskedBy,
            }).ToList();

            var CrewLists = _context.TblCrewLists.Where(x => x.IsDeleted == false && (x.RecDate >= sixMonth || x.ModifiedDate >= sixMonth)).Select(x => new TblCrewListVM
            {
                CrewListId = x.CrewListId,
                RankId = x.RankId.HasValue ? x.RankId.Value : default,
                VesselId = x.VesselId.HasValue ? x.VesselId.Value : default,
                CrewId = x.CrewId.HasValue ? x.CrewId.Value : default,
                SignOnDate = x.SignOnDate.HasValue ? x.SignOnDate.ToString() : default,
                DueDate = x.DueDate.HasValue ? x.DueDate.ToString() : default,
                Reliever1 = x.Reliever1.HasValue ? x.Reliever1.Value : default,
                Reliever2 = x.Reliever2.HasValue ? x.Reliever2.Value : default,
                ReptriationPort = x.ReptriationPort,
                EngagementPort = x.EngagementPort,
                Er = x.Er,
                Ermonth = x.Ermonth,
                OldDueDate = x.OldDueDate.HasValue ? x.OldDueDate.ToString() : default,
                Status = x.Status,
                IsDeleted = x.IsDeleted.HasValue ? x.IsDeleted.Value : default,
                RecDate = x.RecDate.HasValue ? x.RecDate.ToString() : default,
                ModifiedBy = x.ModifiedBy,
                ModifiedDate = x.ModifiedDate.HasValue ? x.ModifiedDate.ToString() : default,
                IsSignOff = x.IsSignOff.HasValue ? x.IsSignOff.Value : default,
                IsPromoted = x.IsPromoted.HasValue ? x.IsPromoted.Value : default,
                ActivityCode = x.ActivityCode.HasValue ? x.ActivityCode.Value : default,
                PlanActivityCode = x.PlanActivityCode.HasValue ? x.PlanActivityCode.Value : default,
                ReplacedWith = x.ReplacedWith,
                ReliverRankId = x.ReliverRankId.HasValue ? x.ReliverRankId.Value : default,
            }).ToList();
           
            var PBBankAllotment = _context.TblPbbankAllotments.Where(x => x.IsDeleted == false && x.Recdate >= sixMonth).Select(x => new tblPBBankAllotmentVM
            {
                VesselPortId = x.BankAllotmentId,
                Crew = x.Crew,
                VesselId = x.VesselId,
                BankId = x.BankId,
                From = x.From,
                To = x.To,
                Allotments = x.Allotments,
                IsMidMonthAllotment = x.IsMidMonthAllotment,
                IsDeleted = x.IsDeleted,
                Recdate = x.Recdate,
                IsPromoted = x.IsPromoted
            }).ToList();
            var PortageBills = _context.TblPortageBills.Where(x => x.IsDeleted == false && (x.RecDate >= sixMonth || x.ModifiedDate >= sixMonth)).Select(x => new TblPortageBillVM
            {
                VesselPortId = x.PortageBillId,
                CrewId = x.CrewId,
                CrewListId = x.CrewListId,
                ContractId = x.ContractId,
                From = x.From.Value.ToString(),
                To = x.To.Value.ToString(),
                Days = x.Days,
                Othours = x.Othours,
                ExtraOt = x.ExtraOt,
                OtherEarnings = x.OtherEarnings,
                TransitDays = x.TransitDays,
                TransitWages = x.TransitWages,
                TotalEarnings = x.TotalEarnings,
                PrevMonthBal = x.PrevMonthBal,
                Reimbursement = x.Reimbursement,
                TotalPayable = x.TotalPayable,
                LeaveWagesCf = x.LeaveWagesCf,
                CashAdvance = x.CashAdvance,
                BondedStores = x.BondedStores,
                OtherDeductions = x.OtherDeductions,
                Allotments = x.Allotments,
                TotalDeductions = x.TotalDeductions,
                LeaveWagesBf = x.LeaveWagesBf,
                FinalBalance = x.FinalBalance,
                SignOffDate = x.SignOffDate.Value.ToString(),
                Remarks = x.Remarks,
                IsDeleted = x.IsDeleted,
                RecDate = x.RecDate.Value.ToString(),
                CreatedBy = x.CreatedBy,
                ModifiedBy = x.ModifiedBy,
                ModifiedDate = x.ModifiedDate.Value.ToString(),
                AppliedCba = x.AppliedCba,
                BillStatus = x.BillStatus,
                BankId = x.BankId,
                Vesselid = x.Vesselid,
                Udamount = x.Udamount,
                Wfamount = x.Wfamount,
                Tax = x.Tax,
                IsTransitApply = x.IsTransitApply,
                IsPromoted = x.IsPromoted,
                IsLeaveWagesCf = x.IsLeaveWagesCf,
                Attachment = x.Attachment,
                IndPfamount = x.IndPfamount,
                Gratuity = x.Gratuity,
                Avc = x.Avc,
                IsAddPrevBal = x.IsAddPrevBal,
                IsHoldWageAllotment = x.IsHoldWageAllotment
            }).ToList();
            var crewlistdats = _context.TblCrewLists.ToList();
            DataTable dtcrewlistdats = new DataTable();
            dtcrewlistdats = LINQResultToDataTable(crewlistdats);
            DataTable dtCloned = dtcrewlistdats.Clone();
            dtCloned.Columns["SignOnDate"].DataType = typeof(string);
            dtCloned.Columns["DueDate"].DataType = typeof(string);
            foreach (DataRow row in dtcrewlistdats.Rows)
            {
                dtCloned.ImportRow(row);
            }
            try
            {
                var TblPortageBills = _context.TblPortageBills.ToList();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    int x = 1;
                    if (ActivitySignOns.Count > 0)
                    {
                        var wsActivitySignOns = wb.Worksheets.Add("tblImportActivitySignOn");
                        wb.Worksheet(x).Cell(1, 1).InsertTable(ActivitySignOns);
                        x++;
                    }
                    if (CrewDetails.Count > 0)
                    {
                        var wsCrewDetails = wb.Worksheets.Add("tblImportCrewDetail");
                        wb.Worksheet(x).Cell(1, 1).InsertTable(CrewDetails);
                        x++;
                    }
                    if (CrewLists.Count > 0)
                    {
                        var wsCrewList = wb.Worksheets.Add("tblImportCrewList");
                        wb.Worksheet(x).Cell(1, 1).InsertTable(CrewLists);
                        x++;
                    }
                    if (PBBankAllotment.Count > 0)
                    {
                        var wsPBBankAllotment = wb.Worksheets.Add("tblImportPBBankAllotment");
                        wb.Worksheet(x).Cell(1, 1).InsertTable(PBBankAllotment);
                        x++;
                    }
                    if (PortageBills.Count > 0)
                    {
                        var wsPortageBill = wb.Worksheets.Add("tblImportPortageBill");
                        wb.Worksheet(x).Cell(1, 1).InsertTable(PortageBills);
                        x++;
                    }
                    int wbcount = wb.Worksheets.Count();
                    if (wbcount > 0)
                    {
                        var getemail = _context.TblEmails.FirstOrDefault();
                        string filename = "ShipModuleBackup_" + DateTime.Now.ToString("ddmmyyyyhhmmss") + ".xlsx";
                        MemoryStream mstream = new MemoryStream();
                        wb.SaveAs(mstream);
                        mstream.Position = 0;
                        string folderName = "Salaryslip";
                        string webRootPath = _appEnvironment.WebRootPath;
                        string PathToSave = Path.Combine(webRootPath, folderName + "/" + filename);
                        FileStream file = new FileStream(PathToSave, FileMode.Create, FileAccess.Write);
                        mstream.WriteTo(file);
                        file.Close();
                        // return File(mstream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                        MailMessage mail = new MailMessage();
                        mail.From = new MailAddress(getemail.EmailId, "Crewlink Backup");
                        mail.Subject = "Crewlink Backup";
                        mail.IsBodyHtml = true;
                        mail.Attachments.Add(new Attachment(PathToSave));
                        mail.To.Add(new MailAddress("info@maziksolutions.com"));
                        SmtpClient smtp = new SmtpClient();
                        smtp.UseDefaultCredentials = true;
                        smtp.Host = getemail.Smtp;
                        if (getemail.Port != null && getemail.Port != 0)
                            smtp.Port = getemail.Port.Value;
                        smtp.Credentials = new System.Net.NetworkCredential(getemail.EmailId, getemail.Password);
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        // smtp.EnableSsl = true;
                        smtp.Send(mail);
                        Backuplogs("Backup email has been sent succesfully.");                       
                    }
                }
            }
            catch (Exception ex)
            {
                Backuplogs("Error : Backup email is not sent. (" + ex.InnerException + ")");
                ViewBag.status = "backup email not sent";
                throw ex;
            }
        }
        public void Backuplogs(string message)
        {
            _context.tblBackupLog.Add(new tblBackupLog
            {
                LogDescription = message
            });
            _context.SaveChanges();
        }

    }
}
