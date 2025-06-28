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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Helpers;
using System.Data;
using System.Reflection;
using System.Data.OleDb;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

using Microsoft.EntityFrameworkCore;
//using OpenPop.Pop3;
using crewlinkship.ViewModel;
//using OpenPop.Mime;
using Limilabs.Client.IMAP;
using Limilabs.Mail;
using Limilabs.Mail.MIME;
using System.Net.Security;
using System.Net.Sockets;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.VariantTypes;
using DocumentFormat.OpenXml.Wordprocessing;
using Intersoft.Crosslight;

namespace crewlinkship.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly shipCrewlinkContext _context;
        private readonly IHostingEnvironment _appEnvironment;
        private readonly AppSettings _appSettings;
        private readonly IConfiguration _configuration;
        public bool IMOFull { get; private set; }
        private int vesselidtouse;
        public HomeController(shipCrewlinkContext context, IHostingEnvironment appEnvironment, IConfiguration configuration, IOptions<AppSettings> appSettings)
        {
            // _logger = logger;
            _configuration = configuration;
            _context = context;
            _appEnvironment = appEnvironment;
            _appSettings = appSettings.Value; vesselidtouse = _configuration.GetValue<int>("vesselinfo:checkvessel:datafor");
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult BackUplog()
        {            
            var accessToken = HttpContext.Session.GetString("token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            if (accessToken != null)
            {
                ViewBag.name = HttpContext.Session.GetString("name");
                ViewBag.vesselDetails = _context.TblVessels.Include(x => x.Flag).Include(x => x.Ship).Where(x => x.IsDeleted == false && x.VesselId == vesselidtouse).FirstOrDefault();
                var crewlist = _context.TblCrewLists.Include(x => x.Crew).Include(x => x.Reliever).Include(x => x.Rank).Include(x => x.Crew.Country).Include(x => x.ReliverRank).Where(x => x.IsDeleted == false && x.VesselId == vesselidtouse && x.IsSignOff != true && x.IsDeleted == false).ToList().OrderBy(x => x.Rank.CrewSort).ToList();
                ViewBag.vessels = _context.TblVessels.Where(x => x.IsDeleted == false && x.IsActive == false && x.VesselId == vesselidtouse).ToList();               
            }
            return View(this.getbackuplog(1));
        }
        [HttpPost]
        public IActionResult BackUplog(int currentPageIndex)
        {
            var accessToken = HttpContext.Session.GetString("token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            if (accessToken != null)
            {
                ViewBag.name = HttpContext.Session.GetString("name");
                ViewBag.vesselDetails = _context.TblVessels.Include(x => x.Flag).Include(x => x.Ship).Where(x => x.IsDeleted == false && x.VesselId == vesselidtouse).FirstOrDefault();
                var crewlist = _context.TblCrewLists.Include(x => x.Crew).Include(x => x.Reliever).Include(x => x.Rank).Include(x => x.Crew.Country).Include(x => x.ReliverRank).Where(x => x.IsDeleted == false && x.VesselId == vesselidtouse && x.IsSignOff != true && x.IsDeleted == false).ToList().OrderBy(x => x.Rank.CrewSort).ToList();
                ViewBag.vessels = _context.TblVessels.Where(x => x.IsDeleted == false && x.IsActive == false && x.VesselId == vesselidtouse).ToList();
            }
            return View(this.getbackuplog(currentPageIndex));
        }
        private tblBackupLogModel getbackuplog(int currentPage)
        {
            int maxRows = 20;
            var backuplogs = _context.tblBackupLog.ToList();
            tblBackupLogModel logModel = new tblBackupLogModel();
            logModel.tblBackupLog = backuplogs.OrderBy(tblBackupLog => tblBackupLog.BackupId)
                        .Skip((currentPage - 1) * maxRows)
                        .Take(maxRows).ToList();
            double pageCount = (double)((decimal)backuplogs.Count() / Convert.ToDecimal(maxRows));
            logModel.PageCount = (int)Math.Ceiling(pageCount);
            logModel.CurrentPageIndex = currentPage;
            return logModel;
        }
        [AllowAnonymous]
        public IActionResult ReadEmail()
        {
            try
            {
                var getemail = _context.TblEmails.FirstOrDefault();               
                using (Imap imap = new Imap())
                {
                    imap.Connect(getemail.Pop);   // or ConnectSSL for SSL
                    imap.UseBestLogin(getemail.EmailId, getemail.Password);
                    imap.SelectInbox();
                    List<long> uids = imap.Search(Flag.Unseen);
                    int count = uids.Count();
                    foreach (long uid in uids)
                    {
                        var eml = imap.GetMessageByUID(uid);
                        IMail email = new MailBuilder()
                            .CreateFromEml(eml);
                        string subject = email.Subject;
                        string test = email.TextDataString;
                        var attachments = email.Attachments;
                        if (attachments.Count != 0)
                        {
                            foreach (MimeData attach in email.Attachments)
                            {
                                string filename = attach.FileName;
                                string extension = Path.GetExtension(filename);
                                if ((extension == ".xlsx" || extension == ".xls"))//&& subject == "Crewlink Backup"
                                {
                                    string folderName = "Upload/backup/";
                                    string webRootPath = _appEnvironment.WebRootPath;
                                    string newPath = Path.Combine(webRootPath, folderName);
                                    string fullPath = Path.Combine(newPath, filename);
                                    using (var stream = new FileStream(fullPath, FileMode.Create))
                                    {
                                        attach.Save(stream);
                                        var serverUrl = _configuration["serverUrl"];
                                        string conString = string.Empty;
                                        switch (extension)
                                        {
                                            case ".xls": //Excel 97-03.
                                                conString = _configuration.GetConnectionString("excelconnection03");
                                                break;
                                            case ".xlsx": //Excel 07 and above.
                                                conString = _configuration.GetConnectionString("excelconnection07");
                                                break;
                                        }
                                        DataSet dsTables = new DataSet();
                                        using (XLWorkbook workBook = new XLWorkbook(stream))
                                        {
                                            int x = 1;
                                            foreach (IXLWorksheet wk in workBook.Worksheets)
                                            {
                                                IXLWorksheet workSheet = workBook.Worksheet(x);
                                                string sheetname = workSheet.Name;
                                                //Create a new DataTable.
                                                DataTable dt = new DataTable();
                                                dt.TableName = sheetname;
                                                //Loop through the Worksheet rows.
                                                bool firstRow = true;
                                                foreach (IXLRow row in workSheet.Rows())
                                                {
                                                    //Use the first row to add columns to DataTable.
                                                    if (firstRow)
                                                    {
                                                        foreach (IXLCell cell in row.Cells())
                                                        {
                                                            dt.Columns.Add(cell.Value.ToString());
                                                        }
                                                        firstRow = false;
                                                    }
                                                    else
                                                    {
                                                        //Add rows to DataTable.
                                                        dt.Rows.Add();
                                                        int i = 0;
                                                        foreach (IXLCell cell in row.Cells(1, dt.Columns.Count))
                                                        {
                                                            if (cell.Value.ToString() == "")
                                                                dt.Rows[dt.Rows.Count - 1][i] = null;
                                                            else
                                                                dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                                                            i++;
                                                        }
                                                    }
                                                }
                                                x++;
                                                dsTables.Tables.Add(dt);
                                            }
                                        }
                                        conString = _configuration.GetConnectionString("sqlcon");
                                        for (int i = 0; i < dsTables.Tables.Count; i++)
                                        {
                                            ViewBag.Message = String.Format("success");
                                            if (dsTables.Tables[i].Rows.Count > 0)
                                            {
                                                using (SqlConnection con = new SqlConnection(conString))
                                                {
                                                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                                                    {
                                                        sqlBulkCopy.DestinationTableName = dsTables.Tables[i].TableName;
                                                        con.Open();
                                                        if (dsTables.Tables[i].Rows.Count > 0)
                                                        {
                                                            sqlBulkCopy.WriteToServer(dsTables.Tables[i]);
                                                            con.Close();
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        _context.Database.ExecuteSqlRaw("EXEC ImportCrewlinkdata");
                                    }
                                }
                            }
                        }
                    }
                    imap.Close();
                }
                Backuplogs("Backup has been synchronized successfully"); ViewBag.JavaScriptFunction = "ShowSuccessMsg();";
                ViewBag.status = "backup has been synchronized successfully";
            }
            catch (Exception ex) {
                Backuplogs("Error : Backup is not synchronized. (" + ex.InnerException + ")"); ViewBag.JavaScriptFunction = "ShowSuccessMsg();";
                ViewBag.status = "backup not synchronized"; throw ex;                
            };
            return View();
        }
      
        [AllowAnonymous]
        public IActionResult SendAutoBackup()
        {
            var vesseldata = _context.TblVessels.Where(x => x.VesselId == vesselidtouse).FirstOrDefault();
            var currentDate = DateTime.UtcNow;
            var sixMonth = currentDate.AddDays(-1);
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

            //var PBBankAllotment = _context.TblPbbankAllotments.Where(x => x.IsDeleted == false && x.Recdate >= sixMonth).Select(x => new tblPBBankAllotmentVM
            //{
            //    VesselPortId = x.BankAllotmentId,
            //    Crew = x.Crew,
            //    VesselId = x.VesselId,
            //    BankId = x.BankId,
            //    From = x.From,
            //    To = x.To,
            //    Allotments = x.Allotments,
            //    IsMidMonthAllotment = x.IsMidMonthAllotment,
            //    IsDeleted = x.IsDeleted,
            //    Recdate = x.Recdate,
            //    IsPromoted = x.IsPromoted
            //}).ToList();
            var PBBankAllotment = _context.TblPbbankAllotments.Where(x =>  x.Recdate >= sixMonth).ToList();
            var PortageEarningDeduction = (from pbearn in _context.PortageEarningDeduction
                                           where  (pbearn.RecDate >= sixMonth || pbearn.ModifiedDate >= sixMonth)
                                           select new
                                           {
                                               PortageEarningDeductionId = pbearn.PortageEarningDeductionId,
                                               CrewId = pbearn.CrewId,
                                               PortageBillId = pbearn.PortageBillId,
                                               Vesselid = pbearn.Vesselid,
                                               SubCodeId = pbearn.SubCodeId,
                                               Amount = pbearn.Amount,
                                               From = pbearn.From,
                                               To = pbearn.To,
                                               Type = pbearn.Type,
                                               Currency = pbearn.Currency,
                                               IsDeleted = pbearn.IsDeleted,
                                               RecDate = pbearn.RecDate,
                                               CreatedBy = pbearn.CreatedBy,
                                               ModifiedBy = pbearn.ModifiedBy,
                                               ModifiedDate = pbearn.ModifiedDate,
                                               VesselPortId = pbearn.VesselPortId,
                                               OfficePBId = pbearn.OfficePBId,
                                               IsPortagelocked = pbearn.IsPortagelocked
                                           }).ToList();
            var PortageBills = _context.TblPortageBills.Where(x => (x.RecDate >= sixMonth || x.ModifiedDate >= sixMonth)).Select(x => new
            {
                PortageBillId = x.PortageBillId,
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
                IsHoldWageAllotment = x.IsHoldWageAllotment,
                VesselPortId = x.VesselPortId,
                OfficePBId = x.OfficePBId,
                IsPortagelocked = x.IsPortagelocked,
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
               // var TblPortageBills = _context.TblPortageBills.ToList();
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
                    if (PortageEarningDeduction.Count > 0)
                    {
                        var wsPortageEarningDeduction = wb.Worksheets.Add("tblimportPortageEarningDedu");
                        wb.Worksheet(x).Cell(1, 1).InsertTable(PortageEarningDeduction);
                        x++;
                    }
                    int wbcount = wb.Worksheets.Count();
                    if (wbcount > 0)
                    {                  
                     var getemail = _context.TblEmails.FirstOrDefault();
                    string filename = vesseldata.Imo+ "_" + vesselidtouse + "_ShipModuleBackup_" + DateTime.UtcNow.ToString("ddmmyyyyhhmmss") + ".xlsx";
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
                            mail.To.Add(new MailAddress(getemail.EmailSentTo));
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
                        ViewBag.status = "backup email has been sent";
                        ViewBag.JavaScriptFunction = "ShowSuccessMsg();";
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                Backuplogs("Error : Backup email is not sent. (" + ex.InnerException + ")");
                ViewBag.status = "backup email not sent";
                throw ex;
            }
            return View();
        }
        public class JavaScriptResult : ContentResult
        {
            public JavaScriptResult(string script)
            {
                this.Content = script;
                this.ContentType = "application/javascript";
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

        [HttpPost]
        // [Route("importdata")]
        public IActionResult ImportData()
        {
            try
            {
                if (Request.Form.Files.Count > 0)
                {
                    var file = Request.Form.Files[0];
                    string folderName = "Import/";
                    string webRootPath = _appEnvironment.WebRootPath;
                    string newPath = Path.Combine(webRootPath, folderName);
                    if (!Directory.Exists(newPath))
                    {
                        Directory.CreateDirectory(newPath);
                    }
                    if (file.Length > 0)
                    {
                        string fileExtension = Path.GetExtension(ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('\"'));
                        string fileName = "Import_" + Guid.NewGuid() + fileExtension;
                        string fullPath = Path.Combine(newPath, fileName);
                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                            var serverUrl = _configuration["serverUrl"];
                            // item.Attachment = serverUrl + folderName + fileName;
                        }
                        string extension = Path.GetExtension(fileName);
                        string conString = string.Empty;
                        switch (extension)
                        {
                            case ".xls": //Excel 97-03.
                                conString = _configuration.GetConnectionString("excelconnection03");
                                break;
                            case ".xlsx": //Excel 07 and above.
                                conString = _configuration.GetConnectionString("excelconnection07");
                                break;
                        }
                        DataSet dsTables = new DataSet();
                        using (XLWorkbook workBook = new XLWorkbook(fullPath))
                        {
                            int x = 1;
                            foreach (IXLWorksheet wk in workBook.Worksheets)
                            {
                                IXLWorksheet workSheet = workBook.Worksheet(x);
                                string sheetname = workSheet.Name;
                                //Create a new DataTable.
                                DataTable dt = new DataTable();
                                dt.TableName = sheetname;
                            //Loop through the Worksheet rows.
                            bool firstRow = true;
                                foreach (IXLRow row in workSheet.Rows())
                            {
                                //Use the first row to add columns to DataTable.
                                if (firstRow)
                                {
                                    foreach (IXLCell cell in row.Cells())
                                    {
                                        dt.Columns.Add(cell.Value.ToString());
                                    }
                                    firstRow = false;
                                }
                                else
                                {
                                    //Add rows to DataTable.
                                    dt.Rows.Add();
                                    int i = 0;
                                        foreach (IXLCell cell in row.Cells(1, dt.Columns.Count))
                                        {
                                            if (cell.Value.ToString() == "")
                                                dt.Rows[dt.Rows.Count - 1][i] = null;
                                            else
                                                dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                                        i++;
                                    }
                                }                               
                            }
                                x++;
                                dsTables.Tables.Add(dt);
                            }                           
                        }
                        conString = _configuration.GetConnectionString("sqlcon");
                        for (int i = 0; i < dsTables.Tables.Count; i++)
                        {
                            ViewBag.Message = String.Format("success");
                            if (dsTables.Tables[i].Rows.Count > 0)
                            {
                                using (SqlConnection con = new SqlConnection(conString))
                                {
                                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                                    {
                                        sqlBulkCopy.DestinationTableName = dsTables.Tables[i].TableName;
                                        con.Open();
                                        if (dsTables.Tables[i].Rows.Count > 0)
                                        {
                                            sqlBulkCopy.WriteToServer(dsTables.Tables[i]);
                                            con.Close();
                                        }
                                    }
                                }
                            }
                        }

                        // var constr = _configuration.GetConnectionString("excelconnection");
                        //conString = string.Format(conString, fullPath);                       
                        //using (OleDbConnection connExcel = new OleDbConnection(conString))
                        //{
                        //    using (OleDbCommand cmdExcel = new OleDbCommand())
                        //    {
                        //        ViewBag.Message = String.Format("success");
                        //        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        //        {
                        //            ViewBag.Message = String.Format("success");
                        //            cmdExcel.Connection = connExcel;
                        //            connExcel.Open();
                        //            DataTable dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        //            connExcel.Close();
                        //            for (int i = 0; i < dtExcelSchema.Rows.Count; i++)
                        //            {
                        //                ViewBag.Message = String.Format("success");
                        //                string sheetName = dtExcelSchema.Rows[i]["TABLE_NAME"].ToString();
                        //                //Read Data from current Sheet.
                        //                connExcel.Open();
                        //                cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                        //                odaExcel.SelectCommand = cmdExcel;
                        //                DataTable dt = new DataTable(sheetName.Replace("$", string.Empty));
                        //                odaExcel.Fill(dt);
                        //                if (dt.Rows.Count > 0 && dt.Rows[0][0].ToString() != "")
                        //                {
                        //                    dsTables.Tables.Add(dt);
                        //                }
                        //                connExcel.Close();
                        //            }
                        //        }
                        //    }
                        //}
                        //conString = _configuration.GetConnectionString("sqlcon");
                        //for (int i = 0; i < dsTables.Tables.Count; i++)
                        //{
                        //    ViewBag.Message = String.Format("success");
                        //    if (dsTables.Tables[i].Rows.Count > 0)
                        //    {
                        //        using (SqlConnection con = new SqlConnection(conString))
                        //        {
                        //            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                        //            {
                        //                sqlBulkCopy.DestinationTableName = "dbo." + dsTables.Tables[i].TableName;
                        //                con.Open();
                        //                if (dsTables.Tables[i].Rows.Count > 0)
                        //                {
                        //                    sqlBulkCopy.WriteToServer(dsTables.Tables[i]);
                        //                    con.Close();
                        //                }
                        //            }
                        //        }
                        //    }
                        //}
                        _context.Database.ExecuteSqlRaw("EXEC ImportCrewlinkdata");
                    }
                }
            }
            catch (Exception ex) { throw ex; };
            return RedirectToAction("ImportExportPage", "home");
        }

        public IActionResult Takebackup()
        {
            var currentDate = DateTime.UtcNow;
            var sixMonth = currentDate.AddDays(-6);  
            var ActivitySignOns = _context.TblActivitySignOns.Where(x => x.IsDeleted == false && (x.RecDate >= sixMonth || x.ModifiedDate>= sixMonth)).Select(x => new TblActivitySignOnVM
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
            //var PortageEarningDeduction = (from pbearn in _context.PortageEarningDeduction
            //                               where pbearn.IsDeleted == false && (pbearn.RecDate >= sixMonth || pbearn.ModifiedDate >= sixMonth)
            //                               select pbearn).ToList();
            var PBBankAllotment = _context.TblPbbankAllotments.Where(x => x.Recdate >= sixMonth && (x.IsPortagelocked == null || x.IsPortagelocked == false)).ToList();
            // var PortageBills = _context.TblPortageBills.AsNoTracking().Where(x => x.IsDeleted == false && (x.RecDate >= sixMonth || x.ModifiedDate >= sixMonth)).ToList();
            var PortageEarningDeduction = (from pbearn in _context.PortageEarningDeduction
                                           where  (pbearn.RecDate >= sixMonth || pbearn.ModifiedDate >= sixMonth) && (pbearn.IsPortagelocked == null || pbearn.IsPortagelocked == false)
                                           select new
                                           {
                                               PortageEarningDeductionId = pbearn.PortageEarningDeductionId,
                                               CrewId = pbearn.CrewId,
                                               PortageBillId = pbearn.PortageBillId,
                                               Vesselid = pbearn.Vesselid,
                                               SubCodeId = pbearn.SubCodeId,
                                               Amount = pbearn.Amount,
                                               From = pbearn.From,
                                               To = pbearn.To,
                                               Type = pbearn.Type,
                                               Currency = pbearn.Currency,
                                               IsDeleted = pbearn.IsDeleted,
                                               RecDate = pbearn.RecDate,
                                               CreatedBy = pbearn.CreatedBy,
                                               ModifiedBy = pbearn.ModifiedBy,
                                               ModifiedDate = pbearn.ModifiedDate,
                                               VesselPortId = pbearn.VesselPortId,
                                               OfficePBId = pbearn.OfficePBId,
                                               IsPortagelocked = pbearn.IsPortagelocked
                                           }).ToList();
            //var PBBankAllotment = _context.TblPbbankAllotments.Where(x => x.IsDeleted == false && x.Recdate >= sixMonth).Select(x => new tblPBBankAllotmentVM
            //{
            //    VesselPortId = x.BankAllotmentId,
            //    Crew = x.Crew,
            //    VesselId = x.VesselId,
            //    BankId = x.BankId,
            //    From = x.From,
            //    To = x.To,
            //    Allotments = x.Allotments,
            //    IsMidMonthAllotment = x.IsMidMonthAllotment,
            //    IsDeleted = x.IsDeleted,
            //    Recdate = x.Recdate,
            //    IsPromoted = x.IsPromoted
            //}).ToList();
            var PortageBills = _context.TblPortageBills.Where(x => (x.RecDate >= sixMonth || x.ModifiedDate >= sixMonth) && (x.IsPortagelocked== null || x.IsPortagelocked == false)).Select(x => new
            {
                PortageBillId = x.PortageBillId,
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
                IsHoldWageAllotment = x.IsHoldWageAllotment,
                VesselPortId = x.VesselPortId,
                OfficePBId = x.OfficePBId,
                IsPortagelocked = x.IsPortagelocked,
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
               // var TblPortageBills = _context.TblPortageBills.ToList();
                using (XLWorkbook wb = new XLWorkbook())
                {

                    var wsActivitySignOns = wb.Worksheets.Add("tblImportActivitySignOn");
                    wb.Worksheet(1).Cell(1, 1).InsertTable(ActivitySignOns);

                    var wsCrewDetails = wb.Worksheets.Add("tblImportCrewDetail");
                    wb.Worksheet(2).Cell(1, 1).InsertTable(CrewDetails);

                    var wsCrewList = wb.Worksheets.Add("tblImportCrewList");
                    wb.Worksheet(3).Cell(1, 1).InsertTable(CrewLists);

                    var wsPBBankAllotment = wb.Worksheets.Add("tblImportPBBankAllotment");
                    wb.Worksheet(4).Cell(1, 1).InsertTable(PBBankAllotment);

                    var wsPortageBill = wb.Worksheets.Add("tblImportPortageBill");
                    wb.Worksheet(5).Cell(1, 1).InsertTable(PortageBills);

                    var wsPortageEarningDeduction = wb.Worksheets.Add("tblimportPortageEarningDedu");
                    wb.Worksheet(6).Cell(1, 1).InsertTable(PortageEarningDeduction);
                 
                    string filename = "ShipModuleBackup_"+ DateTime.UtcNow.ToString("dd-MMM-yyyy hh:mm:ss") + ".xlsx";
                    using (MemoryStream mstream = new MemoryStream())
                    {
                        wb.SaveAs(mstream);
                        return File(mstream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //var data = _context.Database.ExecuteSqlRaw("DBBACKUP");
            return null;
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
            var accessToken = HttpContext.Session.GetString("token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            if (accessToken != null)
            {
                ViewBag.rankName = _context.TblCrewDetails.Include(x => x.Rank).Include(x => x.Vessel).Where(x => x.IsDeleted == false && x.CrewId == crewId).ToList();

                ViewBag.passport = _context.TblPassports.Where(p => p.CrewId == crewId && p.IsDeleted == false).FirstOrDefault()?.PassportNumber;
                ViewBag.cdc = _context.TblCdcs.Where(p => p.CrewId == crewId && p.IsDeleted == false).FirstOrDefault()?.Cdcnumber;

                ViewBag.crewDetails = _context.TblCrewDetails.Include(x => x.Rank).Include(x => x.Vessel).Include(c => c.Country)
                  .Include(c => c.Pool).Where(x => x.CrewId == crewId).ToList();
                return PartialView();
            }
            return RedirectToAction("UserLogin", "Login");
        }


        public IActionResult Address(int? crewId)
        {
            var accessToken = HttpContext.Session.GetString("token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            if (accessToken != null)
            {
                ViewBag.address = _context.TblCrewAddresses.Include(x => x.Country).Include(x => x.State).Include(x => x.City).Include(x => x.Airport).Where(x => x.IsDeleted == false && x.CrewId == crewId).FirstOrDefault();
                ViewBag.corsAddress = _context.TblCrewCorrespondenceAddresses.Include(x => x.Country).Include(x => x.State).Include(x => x.City).Include(x => x.Airport).Where(x => x.IsDeleted == false && x.CrewId == crewId).FirstOrDefault();
                ViewBag.rankName = _context.TblCrewDetails.Include(x => x.Rank).Include(x => x.Vessel).Where(x => x.IsDeleted == false && x.CrewId == crewId).FirstOrDefault();
                ViewBag.crewidforBank = crewId;
                return PartialView();
            }
            return RedirectToAction("UserLogin", "Login");
        }


        public ActionResult Bankdetails(int? crewId)
        {
            var accessToken = HttpContext.Session.GetString("token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (accessToken != null)
            {
                ViewBag.primaryBank = _context.TblCrewBankDetails.Include(x => x.Country).Include(x => x.State).Include(x => x.City).Where(x => x.IsDeleted == false && x.CrewId == crewId && x.AccountType == "Primary").ToList();
                ViewBag.secondaryBank = _context.TblCrewBankDetails.Include(x => x.Country).Include(x => x.State).Include(x => x.City).Where(x => x.IsDeleted == false && x.CrewId == crewId && x.AccountType == "Secondary").ToList();
                ViewBag.rankName = _context.TblCrewDetails.Include(x => x.Rank).Include(x => x.Vessel).Where(x => x.IsDeleted == false && x.CrewId == crewId).ToList();
                ViewBag.crewidforBank = crewId;
                return PartialView();
            }
            return RedirectToAction("UserLogin", "Login");
        }
        public ActionResult License(int? crewId)
        {
            var accessToken = HttpContext.Session.GetString("token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (accessToken != null)
            {
                ViewBag.nationalLicence = _context.TblCrewLicenses.Include(x => x.License).Include(x => x.Country).Include(x => x.Authority).Where(x => x.IsDeleted == false && x.CrewId == crewId && x.License.Authority.ToLower().Contains("flag") == false).ToList();
                ViewBag.flagLicence = _context.TblCrewLicenses.Include(x => x.License).Include(x => x.Country).Include(x => x.Authority).Where(x => x.IsDeleted == false && x.CrewId == crewId && x.License.Authority.ToLower().Contains("flag") == true).ToList();
                ViewBag.rankName = _context.TblCrewDetails.Include(x => x.Rank).Include(x => x.Vessel).Where(x => x.IsDeleted == false && x.CrewId == crewId).ToList();
                ViewBag.crewidforBank = crewId;
                return PartialView();
            }
            return RedirectToAction("UserLogin", "Login");
        }

        public ActionResult Courses(int? crewId)
        {
            var accessToken = HttpContext.Session.GetString("token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (accessToken != null)
            {
                ViewBag.courses = _context.TblCrewCourses.Include(x => x.CourseNavigation).Include(x => x.Institute).Include(x => x.Authority).Where(x => x.IsDeleted == false && x.CrewId == crewId).ToList();
                ViewBag.rankName = _context.TblCrewDetails.Include(x => x.Rank).Include(x => x.Vessel).Where(x => x.IsDeleted == false && x.CrewId == crewId).ToList();
                ViewBag.crewidforBank = crewId;
                return PartialView();
            }
            return RedirectToAction("UserLogin", "Login");
        }

        public ActionResult OtherDocuments(int? crewId)
        {
            var accessToken = HttpContext.Session.GetString("token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var excludedDoc = ",1,4,9,11,14,15,16,45,47,48,";
            if (accessToken != null)
            {
                ViewBag.otherDocuments = _context.TblCrewOtherDocuments.Include(x => x.Document).Include(x => x.Authority).Where(x => x.IsDeleted == false && x.CrewId == crewId && excludedDoc.Contains("," + x.DocumentId + ",") == false).ToList();
                ViewBag.rankName = _context.TblCrewDetails.Include(x => x.Rank).Include(x => x.Vessel).Where(x => x.IsDeleted == false && x.CrewId == crewId).ToList();
                ViewBag.crewidforBank = crewId;
                return PartialView();
            }
            return RedirectToAction("UserLogin", "Login");
        }

        public ActionResult Crewtraveldoc(int? crewId)
        {
            var accessToken = HttpContext.Session.GetString("token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (accessToken != null)
            {
                ViewBag.rankName = _context.TblCrewDetails.Include(x => x.Rank).Include(x => x.Vessel).Where(x => x.IsDeleted == false && x.CrewId == crewId).ToList();
                ViewBag.passport = _context.TblPassports.Include(x => x.Country).Where(x => x.CrewId == crewId).ToList();
                ViewBag.cdc = _context.TblCdcs.Include(x => x.Country).Where(x => x.CrewId == crewId).ToList();
                ViewBag.visa = _context.TblVisas.Include(x => x.Country).Where(x => x.CrewId == crewId).ToList();
                ViewBag.yf = _context.TblYellowfevers.Include(x => x.VendorRegister).Where(x => x.CrewId == crewId).ToList();
                ViewBag.crewidforBank = crewId;
                return PartialView();
            }
            return RedirectToAction("UserLogin", "Login");
        }

        public ActionResult VesselParticular()
        {
            var accessToken = HttpContext.Session.GetString("token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (accessToken != null)
            {
                ViewBag.name = HttpContext.Session.GetString("name");
                ViewBag.vesselDetails = _context.TblVessels.Include(x => x.Flag).Include(x => x.Ship).Where(x => x.IsDeleted == false && x.VesselId == vesselidtouse).FirstOrDefault();

                ViewBag.HandOverport = "NA";
                var HandOverPortId = _context.TblVessels.Where(x => x.IsDeleted == false && x.VesselId == vesselidtouse).FirstOrDefault().PortOfHandover;
                if (HandOverPortId != null)
                {
                    ViewBag.HandOverport = _context.TblSeaports.Where(x => x.SeaportId == HandOverPortId).FirstOrDefault().SeaportName;
                }
                var vesselName = _context.TblVessels.Include(x => x.Flag).Include(x => x.PortOfRegistryNavigation).Include(x => x.Ship)
                    .Include(x => x.Owner).Include(x => x.DisponentOwner).Include(x => x.Manager).Include(x => x.Crewmanager)
                    .Include(x => x.Classification).Include(t => t.PortOfTakeovers).Include(p => p.VendorRegisterPi)
                    .Include(h => h.VendorRegisterHm).Include(e => e.EngineModel).Include(T => T.EngineType).Include(b => b.Builder)
                    .Where(x => x.IsDeleted == false && x.VesselId == vesselidtouse).ToList();
                ViewBag.vessels = _context.TblVessels.Where(x => x.IsDeleted == false && x.IsActive == true && x.VesselId == vesselidtouse).ToList();
                return View(vesselName);
            }
            return RedirectToAction("UserLogin", "Login");
        }

        public IActionResult vwCrewList(int? crewId)
        {
            var accessToken = HttpContext.Session.GetString("token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            crewId = vesselidtouse;
            if (accessToken != null)
            {

                //ViewBag.name = TempData["name"] as string;
                ViewBag.name = HttpContext.Session.GetString("name");

                ViewBag.vesselDetails = _context.TblVessels.Include(x => x.Flag).Include(x => x.Ship).Where(x => x.IsDeleted == false && x.VesselId == vesselidtouse).FirstOrDefault();

                var crewlist = _context.TblCrewLists.Include(x => x.Crew).Include(x => x.Reliever).Include(x => x.Rank).Include(x => x.Crew.Country).Include(x => x.ReliverRank).Where(x => x.IsDeleted == false && x.VesselId == vesselidtouse && x.IsSignOff != true && x.IsDeleted == false).ToList().OrderBy(x => x.Rank.CrewSort).ToList();

                ViewBag.crewDetails = _context.TblActivitySignOns.Include(x => x.Rank).Include(x => x.Seaport).Include(x => x.SignOnReason).Include(x => x.Crew).Include(c => c.Country).Where(x => x.IsDeleted == false && x.CrewId == crewId).ToList();
                ViewBag.bowRequestCount = _context.TblBowRequests.Where(x => x.VesselId == vesselidtouse && x.Status == "Requested").Count();
                //ViewBag.imo = vesselDetails.Imo;
                //ViewBag.shipType = vesselDetails.Ship.ShipCategory;
                //ViewBag.flag = vesselDetails.Flag.CountryName;
                ViewBag.vessels = _context.TblVessels.Where(x => x.IsDeleted == false && x.IsActive == false && x.VesselId == vesselidtouse).ToList();
                return View(crewlist);
            }
            return RedirectToAction("UserLogin", "Login");
        }

        public IActionResult CBA()
        {
            var accessToken = HttpContext.Session.GetString("token");
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            if (accessToken != null)
            {
                ViewBag.name = HttpContext.Session.GetString("name");
                ViewBag.vesselDetails = _context.TblVessels.Include(x => x.Flag).Include(x => x.Ship).Where(x => x.IsDeleted == false && x.VesselId == vesselidtouse).FirstOrDefault();
                //var vesselDetails = _context.TblVessels.Include(x => x.Flag).Include(x => x.Ship).Where(x => x.IsDeleted == false && x.VesselId == 19).FirstOrDefault();
                //ViewBag.vesselName = vesselDetails.VesselName;
                //ViewBag.imo = vesselDetails.Imo;
                //ViewBag.shipType = vesselDetails.Ship.ShipCategory;
                //ViewBag.flag = vesselDetails.Flag.CountryName;
                ViewBag.vessels = _context.TblVessels.Where(x => x.IsDeleted == false && x.IsActive == false && x.VesselId == vesselidtouse).ToList();
                var vcm = _context.TblVesselCbas.Include(x => x.Country).Include(h => h.OffCBA).Include(x => x.RatingCBA).Where(x => x.IsDeleted == false && x.VesselId == vesselidtouse).ToList();
                return PartialView(vcm);
            }
            return RedirectToAction("UserLogin", "Login");
        }


        public IActionResult ImportExportPage()
        {
            ViewBag.name = HttpContext.Session.GetString("name");
            ViewBag.vesselDetails = _context.TblVessels.Include(x => x.Flag).Include(x => x.Ship).Where(x => x.IsDeleted == false && x.VesselId == vesselidtouse).FirstOrDefault();
            ViewBag.vessels = _context.TblVessels.Where(x => x.IsDeleted == false && x.IsActive == false && x.VesselId == vesselidtouse).ToList();
            return PartialView();
        }



        public IActionResult TravelToVessel(int? crewId)
        {
            var vesselDetails = _context.TblVessels.Include(x => x.Flag).Include(x => x.Ship).Where(x => x.IsDeleted == false && x.VesselId == vesselidtouse).FirstOrDefault();
            ViewBag.vesselName = vesselDetails.VesselName;
            ViewBag.imo = vesselDetails.Imo;
            ViewBag.shipType = vesselDetails.Ship.ShipCategory;
            ViewBag.flag = vesselDetails.Flag.CountryName;
            var vcm = _context.TblVesselCbas.Include(x => x.Country).Where(x => x.IsDeleted == false && x.VesselId == 156).ToList();
            return PartialView(vcm);
        }


        public JsonResult TravelToVessels(int? crewId)
        {
            var travelToVesse = _context.TblActivitySignOns.Include(x => x.Rank).Include(x => x.Seaport).Include(x => x.SignOnReason).Include(x => x.Crew).Include(c => c.Country).Where(x => x.IsDeleted == false && x.CrewId == crewId).FirstOrDefault();
            ViewBag.countryList = new SelectList(_context.TblCountries, "CountryId", "CountryName");
            var result = new { Result = travelToVesse, countryList = ViewBag.countryList };
            return Json(result);
        }
        public JsonResult GetSeaPort(int? CountryId)
        {
            ViewBag.seaPort = _context.TblSeaports.Where(x => x.CountryId == CountryId).ToList();
            return Json(new SelectList(ViewBag.seaPort, "SeaportId", "SeaportName"));
        }

        public JsonResult GetSeaPortByCountry(int? CountryId)
        {
            var seaport = _context.TblSeaports.Where(x => x.CountryId == CountryId).ToList();

            return Json(seaport);
        }
        [HttpPost]
        public JsonResult TravelToVesselReverse(int? crewId)
        {
            var updateCrewDetails = _context.TblCrewDetails.FirstOrDefault(c => c.CrewId == crewId);
            if (updateCrewDetails != null)
            {
                updateCrewDetails.PreviousStatus = "Approved"; //Approver
                updateCrewDetails.PlanStatus = "Travel To Vessel"; // Travel to vessel
                updateCrewDetails.Status = "Travel To Vessel"; // Travel to vessel
                updateCrewDetails.ModifiedBy = "Master";
                updateCrewDetails.ModifiedDate = DateTime.UtcNow;
                _context.TblCrewDetails.Update(updateCrewDetails);
                _context.SaveChanges();
            }
            return Json(updateCrewDetails);
        }
        [HttpPost]
        public int TravelToVesselUpdate(TblActivitySignOn tblActivitySignOn,string ExpectedSignOnDate, string ReliefDate)
        {
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                var crew = _context.TblActivitySignOns.OrderByDescending(x => x.ActivitySignOnId).FirstOrDefault(c => c.ActivitySignOnId == tblActivitySignOn.ActivitySignOnId);
                //DateTime dt = DateTime.ParseExact(ExpectedSignOnDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                //DateTime dt2 = DateTime.ParseExact(ReliefDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                if (crew != null && crew.IsSignon != true)
                {
                    crew.CountryId = tblActivitySignOn.CountryId;
                    crew.SeaportId = tblActivitySignOn.SeaportId;
                    crew.ExpectedSignOnDate = DateTime.Parse(ExpectedSignOnDate);
                    crew.ReliefDate = DateTime.Parse(ReliefDate);
                    crew.Remarks = tblActivitySignOn.Remarks;
                    crew.ModifiedDate = DateTime.UtcNow;
                    _context.TblActivitySignOns.Update(crew);
                    _context.SaveChanges();
                    //update status in crewdetails 
                    var vesselPooId = _context.TblVessels.FirstOrDefault(c => c.VesselId == crew.VesselId);
                    var updateCrewDetails = _context.TblCrewDetails.FirstOrDefault(c => c.CrewId == crew.CrewId);
                    if (updateCrewDetails != null)
                    {
                        updateCrewDetails.PreviousStatus = "Travel To Vessel"; //Approver
                        updateCrewDetails.PlanStatus = "Sign In Transit"; // Travel to vessel
                        updateCrewDetails.Status = "Sign In Transit"; // Travel to vessel
                        updateCrewDetails.PoolId = vesselPooId.PoolId;
                        updateCrewDetails.ModifiedBy = "Master";
                        updateCrewDetails.ModifiedDate = DateTime.UtcNow;
                        _context.TblCrewDetails.Update(updateCrewDetails);
                        _context.SaveChanges();
                    }
                    //Need to check & Refine

                    //var contract = _context.Contract.OrderByDescending(a => a.ContractId).FirstOrDefault(c => c.CrewId == item.CrewId && c.VesselId == item.VesselId);
                    //if (contract != null)
                    //{
                    //    contract.SignonDate = item.ExpectedSignOnDate;
                    //    _context.Contract.Update(contract);
                    //    _context.SaveChanges();
                    //}
                    //    _context.Update(tblActivitySignOn);
                    //_context.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                return -1;
            }

            return 1;

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

        public IActionResult AtlanticExcelFile()
        {
            IEnumerable<TblCrewList> CrewList = _context.TblCrewLists.Include(c => c.Crew).Include(r => r.Rank).Include(ct => ct.Crew.Country).Include(po => po.Vessel.PortOfTakeovers).Where(x => x.IsDeleted == false && x.VesselId == vesselidtouse && x.IsDeleted == false && x.CrewId != null).OrderBy(r => r.Rank.CrewSort).ToList();

            string fileName = "Atlantic Crewlist" + ".xlsx";
            string path_Root = _appEnvironment.WebRootPath;
            //string fullPath = path_Root + "\\Upload\\ExcelFile\\" + fileName;


            //if (employees.Count == 0) return File(Stream.Null, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            //            "employeeReport.xlsx");

            using (var workbook = new XLWorkbook())
            {

                var worksheet = workbook.Worksheets.Add("Atlantic Crewlist");

                var imagePath = path_Root + "\\Upload\\Alogo\\Atlanticlogo.png";
                var image = worksheet.AddPicture(imagePath).MoveTo(worksheet.Cell("A1")).WithSize(87, 76);

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
                    worksheet.Rows("5").Height = 18.110;
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
                    worksheet.Cell("I4").Value = DateTime.UtcNow.ToString("dd-MMM-yyyy");
                    worksheet.Cell("I3").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;


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


                    worksheet.Cell(currentRow, 7).Value = applicant.SignOnDate?.ToString("dd-MMM-yyyy");
                    worksheet.Cell(currentRow, 7).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 8).Value = applicant.Crew?.ReliefDate?.ToString("dd-MMM-yyyy");
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
           // string fullPath = path_Root + "\\Upload\\ExcelFile\\" + fileName;
            byte[] fileByteArray = System.IO.File.ReadAllBytes(fileName);
            System.IO.File.Delete(fileName);
            return File(fileByteArray, "application/vnd.ms-excel", fileName);
        }

        public IActionResult GetOCIMFExcelFile()
        {
            var crew = _context.OCIMFVMs.FromSqlRaw("GetOCIMF @p0", vesselidtouse).ToList();

            string fileName = "OCIMF" + ".xlsx";
            string path_Root = _appEnvironment.WebRootPath;

            using (var workbook = new XLWorkbook())
            {

                var worksheet = workbook.Worksheets.Add("OCIMF");

                var imagePath = path_Root + "\\Upload\\Alogo\\ATLANTAS2.png";
                var image = worksheet.AddPicture(imagePath).MoveTo(worksheet.Cell("A1")).WithSize(115, 77);

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
                    worksheet.Cell("G3").Value = DateTime.UtcNow.ToString("dd-MMM-yyyy");
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

        public IActionResult OLPExcelFile()
        {
            IEnumerable<TblCrewList> CrewList = _context.TblCrewLists.Include(c => c.Crew).Include(r => r.Rank).Include(ct => ct.Crew.Country).Include(p => p.Vessel.Pool).Where(x => x.IsDeleted == false && x.VesselId == vesselidtouse && x.IsDeleted == false && x.CrewId != null).OrderBy(r => r.Rank.CrewSort).ToList();

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

                    worksheet.Cell(currentRow, 4).Value = applicant.Crew?.Dob?.ToString("dd-MMM-yyyy");
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

        //public IActionResult getIMOdata(int vesselId = vesselidtouse)
        //{
        //    var CrewList = _context.TblCrewLists.Include(c => c.Crew).Include(r => r.Rank).Include(ct => ct.Crew.Country).Include(p => p.Vessel).Where(x => x.IsDeleted == false && x.VesselId == vesselId && x.IsDeleted == false && x.CrewId != null).OrderBy(r => r.Rank.CrewSort).ToList();


        //    ViewBag.vesselDetails = _context.TblVessels.Include(x => x.Flag).Include(x => x.Ship).Where(x => x.IsDeleted == false && x.VesselId == vesselId).ToList();


        //    var iMOCrewLists = CrewList.AsEnumerable() // Client-side from here on
        //               .Select((t, index) => new IMOCrewListVM
        //               {
        //                   RowNumber = index + 1,
        //                   CrewListId = t.CrewListId,
        //                   Rank = t.Rank.Code,
        //                   FirstName = t.Crew?.FirstName,
        //                   LastName = t.Crew?.LastName,
        //                   MiddleName = t.Crew?.MiddleName,
        //                   nationality = t.Crew?.Country?.Nationality,
        //                   DOB = t.Crew?.Dob ?? DateTime.Parse("01-01-1900"),
        //                   PassportNo = _context.TblPassports.Where(p => p.CrewId == t.CrewId && p.IsDeleted == false).FirstOrDefault()?.PassportNumber,
        //                   BirthPlace = t.Crew?.PlaceOfBirth
        //               });


        //    if (CrewList.Count() > 20)
        //    {
        //        IMOFull = true;
        //        ViewBag.count = IMOFull;
        //        ViewBag.imoData = iMOCrewLists.Take(20).OrderBy(x => x.RowNumber);
        //    }
        //    else
        //    {
        //        IMOFull = false;
        //        ViewBag.count = IMOFull;
        //        ViewBag.imoData = iMOCrewLists;
        //    }


        //    return PartialView();

        //}


        //public IActionResult getIMOdata2(int vesselId = vesselidtouse)
        //{

        //    var CrewList = _context.TblCrewLists.Include(c => c.Crew).Include(r => r.Rank).Include(ct => ct.Crew.Country).Include(p => p.Vessel).Where(x => x.IsDeleted == false && x.VesselId == vesselId && x.IsDeleted == false && x.CrewId != null).OrderBy(r => r.Rank.CrewSort).ToList();


        //    var iMOCrewLists2 = CrewList.AsEnumerable() // Client-side from here on
        //               .Select((s, indexs) => new IMOCrewListVM
        //               {
        //                   RowNumber = indexs + 1,
        //                   CrewListId = s.CrewListId,
        //                   Rank = s.Rank.Code,
        //                   FirstName = s.Crew?.FirstName,
        //                   LastName = s.Crew?.LastName,
        //                   MiddleName = s.Crew?.MiddleName,
        //                   nationality = s.Crew?.Country?.Nationality,
        //                   DOB = s.Crew?.Dob ?? DateTime.Parse("01-01-1900"),
        //                   PassportNo = _context.TblPassports.Where(p => p.CrewId == s.CrewId && p.IsDeleted == false).FirstOrDefault()?.PassportNumber,
        //                   BirthPlace = s.Crew?.PlaceOfBirth
        //               });


        //    if (CrewList.Count() > 20)
        //    {
        //        ViewBag.imoData2 = iMOCrewLists2.Where(x => x.RowNumber > 20);
        //    }
        //    return PartialView();
        //}



        public IActionResult IMOExcelFile()
        {
            var CrewList = _context.TblCrewLists.Include(c => c.Crew).Include(r => r.Rank).Include(ct => ct.Crew.Country).Include(p => p.Vessel).Where(x => x.IsDeleted == false && x.VesselId == vesselidtouse && x.CrewId != null).OrderBy(r => r.Rank.CrewSort).ToList();


            var vesselDetails = _context.TblVessels.Include(x => x.Flag).Include(x => x.Ship).Where(x => x.IsDeleted == false && x.VesselId == vesselidtouse).ToList();

            string fileName = "IMO" + ".xlsx";
            string path_Root = _appEnvironment.WebRootPath;

            using (var workbook = new XLWorkbook())
            {

                var worksheet = workbook.Worksheets.Add("IMO");

                //var imagePath = path_Root + "\\Upload\\Alogo\\ATLANTAS2.png";
                //var image = worksheet.AddPicture(imagePath).MoveTo(worksheet.Cell("A1")).WithSize(115, 77);

                var imagePath = path_Root + "\\Upload\\Alogo\\Atlanticlogo.png";
                var image = worksheet.AddPicture(imagePath).MoveTo(worksheet.Cell("A1")).WithSize(90, 76);



                worksheet.Range("A1:A4").Style.Border.InsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range("A1:A4").Style.Border.InsideBorderColor = XLColor.White;

                worksheet.Column("A").Width = 7.00;
                worksheet.Column("B").Width = 27.00;
                worksheet.Column("C").Width = 19.57;
                worksheet.Column("D").Width = 14.29;
                worksheet.Column("E").Width = 41.00;
                worksheet.Column("F").Width = 28.71;
              

       
                worksheet.Range("C2:E3").Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C2:E3").Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Cell("C2").Value = "Crew List";
                worksheet.Cell("C2").Style.Font.Shadow = true;
                worksheet.Cell("C2").Style.Font.FontSize = 14;
                worksheet.Cell("C2").Style.Font.Bold = true;
                worksheet.Cell("C2").Style.Font.FontName = "Calibri Light";
                worksheet.Cell("C2").Style.Font.Underline = XLFontUnderlineValues.Single;
                worksheet.Cell("C2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Cell("C2").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Range("C2:E3").Merge();




                var currentRow = 11;
                //worksheet.Row(currentRow).Style.Font.SetFontColor(XLColor.White);
                //worksheet.Range("A11:F11").Style.Fill.SetBackgroundColor(XLColor.FromHtml("253B5B"));
                worksheet.Row(currentRow).Style.Font.Bold = true;
                //worksheet.Cell("F10").Style.Fill.SetBackgroundColor(XLColor.FromHtml("253B5B"));

                worksheet.Range("A6:C6").Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                worksheet.Range("A6:C6").Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Range("D6:F6").Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                worksheet.Range("D6:F6").Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Range("A10:C10").Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                worksheet.Range("A10:C10").Style.Border.OutsideBorderColor = XLColor.Black;

                worksheet.Range("D10:E10").Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                worksheet.Range("D10:E10").Style.Border.OutsideBorderColor = XLColor.Black;

                worksheet.Range("A7:A10").Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                worksheet.Range("A7:A10").Style.Border.LeftBorderColor = XLColor.Black;

                worksheet.Range("F7:F10").Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                worksheet.Range("F7:F10").Style.Border.OutsideBorderColor = XLColor.Black;

                worksheet.Cell("F10").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                worksheet.Cell("F10").Style.Border.TopBorderColor= XLColor.Black;

                worksheet.Range("D6:E6").Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                worksheet.Range("D6:E6").Style.Border.OutsideBorderColor = XLColor.Black;

                worksheet.Range("A11:F11").Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                worksheet.Range("A11:F11").Style.Border.OutsideBorderColor = XLColor.Black;

                worksheet.Range("A7:C9").Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                worksheet.Range("A7:C9").Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Cell("B7").Value = "Name and type of ship";
                worksheet.Cell("B8").Value = "IMO number";
                worksheet.Cell("B9").Value = "Call sign";
                worksheet.Cell("B10").Value = "Flag State of ship";

                worksheet.Cell("E10").Value = "5.Last port of call";
                
                worksheet.Cell("A7").Value = "1.1";
                worksheet.Cell("A8").Value = "1.2";
                worksheet.Cell("A9").Value = "1.3";
                worksheet.Cell("A10").Value = "4";


                worksheet.Cell("E6").Value ="Arrival";
                worksheet.Cell("F6").Value ="Departure";
                
                worksheet.Cell("E7").Value = "2 Port of arrival / departure";
                worksheet.Cell("F7").Value = "3 Date of arrival/departure";

                //worksheet.Range("B6:B9").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                foreach (var applicant in vesselDetails)
                {
                    worksheet.Cell("C7").Value = applicant.VesselName +"|" +applicant.Ship.ShipCategory;
                    worksheet.Cell("C7").Style.Font.Bold = true;

                    worksheet.Cell("C8").Value = applicant.Imo;
                    worksheet.Cell("C8").Style.Font.Bold = true;

                    worksheet.Cell("C9").Value = applicant.CallSign;
                    worksheet.Cell("C9").Style.Font.Bold = true;

                    worksheet.Cell("C10").Value = applicant.Flag.CountryName;
                    worksheet.Cell("C10").Style.Font.Bold = true;
                }



                worksheet.Cell(currentRow, 1).Value = "7 Sr. No.";
                worksheet.Cell(currentRow, 2).Value = "8  Family name, given names ";
                worksheet.Cell(currentRow, 3).Value = "9 Rank or rating";
                worksheet.Cell(currentRow, 4).Value = "10 Nationality";
                worksheet.Cell(currentRow, 5).Value = "11 Date and place of birth";
                worksheet.Cell("F10").Value = "6  Nature and No. of identity document(seaman’s passport)";
               
                //worksheet.Cell("F10").Style.Font.SetFontColor(XLColor.White);
                worksheet.Cell("F10").Style.Font.Bold = true;
                worksheet.Cell("F10").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Range("F10:F11").Merge();
                worksheet.Cell("F10").Style.Alignment.WrapText = true;



                worksheet.Range("A11:F11").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                foreach (var applicant in CrewList)
                {
                    var passport = _context.TblPassports.Where(p => p.CrewId == applicant.CrewId && p.IsDeleted == false).FirstOrDefault();

                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = currentRow - 11;
                    worksheet.Cell(currentRow, 1).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Column("A").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    worksheet.Cell("A11").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;


                    worksheet.Cell(currentRow, 2).Value = $"{ConvrtToTitlecase(applicant.Crew?.LastName)} {ConvrtToTitlecase(applicant.Crew?.FirstName)} {ConvrtToTitlecase(applicant.Crew?.MiddleName)}"; 
                    worksheet.Cell(currentRow, 2).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 3).Value = applicant.Rank.Code;
                    worksheet.Cell(currentRow, 3).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 4).Value = applicant.Crew?.Country?.Nationality;
                    worksheet.Cell(currentRow, 4).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                    worksheet.Cell(currentRow, 5).Value = (applicant.Crew?.Dob ?? DateTime.Parse("01-01-1900")).ToString("dd-MMM-yyyy") +","
                        +"  "+applicant.Crew?.PlaceOfBirth;
                    worksheet.Cell(currentRow, 5).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Column("E").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                    worksheet.Cell("E11").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    worksheet.Cell(currentRow, 6).Value = passport?.PassportNumber;
                    worksheet.Cell(currentRow, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;

                   
                    workbook.SaveAs(fileName);
                }
                worksheet.Cells().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Columns().AdjustToContents();
                worksheet.Rows().AdjustToContents();
                var errorMessage = "you can return the errors here!";

                return Json(new { fileName = fileName, errorMessage });

            }

        }





        #region IMO PDF
        //Generate IMO PDF from crewlist

        //public JsonResult GenerateIMOPDF(int vesselId = vesselidtouse)
        //{

        //    var CrewList = _context.TblCrewLists.Include(c => c.Crew).Include(r => r.Rank).Include(ct => ct.Crew.Country).Include(p => p.Vessel).Where(x => x.IsDeleted == false && x.VesselId == vesselId && x.IsDeleted == false && x.CrewId != null).OrderBy(r => r.Rank.CrewSort).ToList();

        //    //string url =  "api/crewlist/getIMOdata?vesselId=" + vesselId;
        //    string url = " http://ship.crewlinkasm.com/Home/getIMOdata";

        //    var webRoot = _appEnvironment.WebRootPath;
        //    string headerUrl = System.IO.Path.Combine(webRoot, "PDFHeaders/PDF_HeaderIMO.htm");
        //    string pdf_page_size = PdfPageSize.A4.ToString();
        //    PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), pdf_page_size, true);
        //    string pdf_orientation = PdfPageOrientation.Portrait.ToString();
        //    PdfPageOrientation pdfOrientation = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
        //        pdf_orientation, true);
        //    int webPageWidth = 1000;
        //    int webPageHeight = 0;
        //    // instantiate a html to pdf converter object
        //    HtmlToPdf converter = new HtmlToPdf();
        //    converter.Header.DisplayOnFirstPage = true;

        //    converter.Options.PdfPageSize = pageSize;
        //    converter.Options.PdfPageOrientation = pdfOrientation;
        //    converter.Options.WebPageWidth = webPageWidth;
        //    converter.Options.WebPageHeight = webPageHeight;
        //    converter.Options.MarginLeft = 10;
        //    converter.Options.MarginRight = 10;
        //    int headerHeight = 80;
        //    int footerHeight = 15;
        //    // header settings
        //    converter.Options.DisplayHeader = true;
        //    converter.Header.DisplayOnFirstPage = true;
        //    converter.Header.DisplayOnOddPages = true;
        //    converter.Header.DisplayOnEvenPages = true;
        //    converter.Header.Height = headerHeight;
        //    PdfHtmlSection headerHtml = new PdfHtmlSection(headerUrl);
        //    headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
        //    converter.Header.Add(headerHtml);
        //    // footer settings
        //    converter.Options.DisplayFooter = true;
        //    converter.Footer.DisplayOnFirstPage = true;
        //    converter.Footer.DisplayOnOddPages = true;
        //    converter.Footer.DisplayOnEvenPages = true;
        //    converter.Footer.Height = footerHeight;
        //    // create a new pdf document converting an url
        //    PdfDocument doc1 = converter.ConvertUrl(url);


        //    //page2 

        //    string url2 = " http://ship.crewlinkasm.com/Home/getIMOdata2";
        //    //string url2 = localpath+"api/crewlist/getIMOdata2?vesselId=" + vesselId;
        //    string pdf_page_size2 = PdfPageSize.A4.ToString();
        //    PdfPageSize pageSize2 = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), pdf_page_size2, true);
        //    string pdf_orientation2 = PdfPageOrientation.Portrait.ToString();
        //    PdfPageOrientation pdfOrientation2 = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
        //        pdf_orientation2, true);
        //    int webPageWidth2 = 1000;
        //    int webPageHeight2 = 0;
        //    // instantiate a html to pdf converter object
        //    HtmlToPdf converter2 = new HtmlToPdf();
        //    converter2.Options.PdfPageSize = pageSize2;
        //    converter2.Options.PdfPageOrientation = pdfOrientation2;
        //    converter2.Options.WebPageWidth = webPageWidth2;
        //    converter2.Options.WebPageHeight = webPageHeight2;
        //    converter2.Options.MarginLeft = 10;
        //    converter2.Options.MarginRight = 10;
        //    int headerHeight2 = 80;
        //    int footerHeight2 = 15;
        //    // header settings
        //    converter2.Options.DisplayHeader = true;
        //    converter2.Header.DisplayOnFirstPage = true;
        //    converter2.Header.DisplayOnOddPages = true;
        //    converter2.Header.DisplayOnEvenPages = true;
        //    converter2.Header.Height = headerHeight2;
        //    PdfHtmlSection headerHtml2 = new PdfHtmlSection(headerUrl);
        //    headerHtml2.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
        //    converter2.Header.Add(headerHtml2);
        //    // footer settings
        //    converter2.Options.DisplayFooter = true;
        //    converter2.Footer.DisplayOnFirstPage = true;
        //    converter2.Footer.DisplayOnOddPages = true;
        //    converter2.Footer.DisplayOnEvenPages = true;
        //    converter2.Footer.Height = footerHeight2;


        //    PdfDocument doc = new PdfDocument();
        //    doc.Append(doc1);
        //    if (CrewList.Count() > 20)
        //    {
        //        PdfDocument doc2 = converter2.ConvertUrl(url2);
        //        doc.Append(doc2);
        //    }
        //    string fileName = "IMOCrewList" + DateTime.UtcNow.ToString("dd-MMM-yyyy.hhmmss") + ".pdf";

        //    doc.Save(fileName);

        //    return Json(new { fileName = fileName });
        //}

        #endregion


        public IActionResult FPD012()
        {
            var CrewList = _context.TblCrewLists.Include(c => c.Crew).Include(r => r.Rank).Include(ct => ct.Crew.Country).Include(p => p.Vessel).Where(x => x.IsDeleted == false && x.VesselId == vesselidtouse && x.IsDeleted == false && x.CrewId != null).OrderBy(r => r.Rank.CrewSort).ToList();


            ViewBag.vesselDetails = _context.TblVessels.Include(x => x.Flag).Include(x => x.Ship).Where(x => x.IsDeleted == false && x.VesselId == vesselidtouse).ToList();


            var FPDCrewLists = CrewList.AsEnumerable() // Client-side from here on
                       .Select((t, index) => new CrewListDownload
                       {
                           SNo = index + 1,
                           VesselName = t.Vessel?.VesselName,
                           CrewName = (t.Crew.LastName == null ? "" : t.Crew.LastName) + " " + (t.Crew.FirstName == null ? "" : t.Crew.FirstName) + " " + (t.Crew.MiddleName == null ? "" : t.Crew.MiddleName),
                           Rank = t.Rank?.Code,
                           Nationality = t.Crew?.Country?.Nationality,
                           Passport = _context.TblPassports.Where(p => p.CrewId == t.CrewId && p.IsDeleted == false).FirstOrDefault()?.PassportNumber,
                           CDC = _context.TblCdcs.Where(p => p.CrewId == t.CrewId && p.IsDeleted == false).FirstOrDefault()?.Cdcnumber,
                           DateSignedOn = t.SignOnDate,
                           ReliefDate = t.DueDate,
                           PortOfJoining = _context.TblActivitySignOns.Include(s => s.Seaport).Where(c => c.CrewId == t.CrewId).FirstOrDefault().Seaport?.SeaportName,
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


        public JsonResult generateFPD01()
        {
            var CrewList = _context.TblCrewLists.Include(c => c.Crew).Include(r => r.Rank).Include(ct => ct.Crew.Country).Include(p => p.Vessel).Where(x => x.IsDeleted == false && x.VesselId == vesselidtouse && x.IsDeleted == false && x.CrewId != null).OrderBy(r => r.Rank.CrewSort).ToList();
            var location = new Uri($"{Request.Scheme}://{Request.Host}");//new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}");
            var urls = location.AbsoluteUri;
            string url = urls +"Home/FPD012";
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

            string fileName = "FPD01Crewlist" + DateTime.UtcNow.ToString("dd-MMM-yyyy.hhmmss") + ".pdf";


            doc.Save(fileName);


            return Json(new { fileName = fileName });
        }


        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("UserLogin", "Login");
        }

        public IActionResult passwordView()
        {
            return PartialView();
        }


        public JsonResult Changepassword(string oldpwd, string newpwd, string crfmpwd)
        {
            var name = HttpContext.Session.GetString("name");

            var User = _context.Userlogins.SingleOrDefault(x => x.Password == oldpwd && x.UserName == name && x.IsDeleted == false);
            if (User != null && newpwd == crfmpwd)
            {
                User.Password = newpwd;
                User.ModifiedDate = DateTime.UtcNow;
                _context.Userlogins.Update(User);
                _context.SaveChanges();
                return Json("success");
            }
            return Json("fail");
        }


        public IActionResult ExportVesselParticulars()
        {

            var vesselName = _context.TblVessels.Include(x => x.Flag).Include(x => x.PortOfRegistryNavigation).Include(x => x.Ship)
                .Include(x => x.Owner).Include(x => x.DisponentOwner).Include(x => x.Manager).Include(x => x.Crewmanager)
                .Include(x => x.Classification).Include(t => t.PortOfTakeovers).Include(p => p.VendorRegisterPi)
                .Include(h => h.VendorRegisterHm).Include(e => e.EngineModel).Include(T => T.EngineType).Include(b => b.Builder)
                .Where(x => x.IsDeleted == false && x.VesselId == vesselidtouse).ToList();

            string fileName = "VesselParticulars" + ".xlsx";
            string path_Root = _appEnvironment.WebRootPath;


            using (var workbook = new XLWorkbook())
            {

                var worksheet = workbook.Worksheets.Add("Vessel Particulars");

                //Common settings                             
                worksheet.Style.Font.FontSize = 11;
                //worksheet.Row(3).Height = 30;
                //worksheet.RowHeight = 20;

                worksheet.Style.Font.FontName = "Calibri Light";
                worksheet.Column("A").Width = 2;
                worksheet.Column("B").Width = 2;
                worksheet.Column("G").Width = 2;

                worksheet.Column("C").Width = 35;
                worksheet.Column("D").Width = 22;
                worksheet.Column("E").Width = 25;
                worksheet.Column("F").Width = 30;
                worksheet.Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
                worksheet.Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                worksheet.Style.Alignment.Indent = 1;
                //worksheet.Range("A1:B3").Merge();

                //Outer Border Of Excel
                worksheet.Range("B2:G2").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                worksheet.Range("B2:G2").Style.Border.TopBorderColor = XLColor.Black;
                worksheet.Range("B2:B98").Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                worksheet.Range("B2:B98").Style.Border.LeftBorderColor = XLColor.Black;
                worksheet.Range("G2:G98").Style.Border.RightBorder = XLBorderStyleValues.Medium;
                worksheet.Range("G2:G98").Style.Border.RightBorderColor = XLColor.Black;
                worksheet.Range("B98:G98").Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                worksheet.Range("B98:G98").Style.Border.BottomBorderColor = XLColor.Black;

                //InnerBorder of Excel general Information
                worksheet.Range("C5:F5").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C5:F5").Style.Border.TopBorderColor = XLColor.Black;
                worksheet.Range("C6:F6").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C6:F6").Style.Border.TopBorderColor = XLColor.Black;

                worksheet.Range("C5:C16").Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C5:C16").Style.Border.LeftBorderColor = XLColor.Black;
                worksheet.Range("F5:F16").Style.Border.RightBorder = XLBorderStyleValues.Medium;
                worksheet.Range("F5:F16").Style.Border.RightBorderColor = XLColor.Black;
                worksheet.Range("C16:F16").Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C16:F16").Style.Border.BottomBorderColor = XLColor.Black;

                worksheet.Range("C6:C16").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range("C6:C16").Style.Border.RightBorderColor = XLColor.Black;

                worksheet.Range("D6:D16").Style.Border.RightBorder = XLBorderStyleValues.Medium;
                worksheet.Range("D6:D16").Style.Border.RightBorderColor = XLColor.Black;

                worksheet.Range("E6:E16").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range("E6:E16").Style.Border.RightBorderColor = XLColor.Black;



                //InnerBorder of Excel Principal Dimensions
                worksheet.Range("C18:F18").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C18:F18").Style.Border.TopBorderColor = XLColor.Black;
                worksheet.Range("C19:F19").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C19:F19").Style.Border.TopBorderColor = XLColor.Black;

                worksheet.Range("C18:C25").Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C18:C25").Style.Border.LeftBorderColor = XLColor.Black;
                worksheet.Range("F18:F25").Style.Border.RightBorder = XLBorderStyleValues.Medium;
                worksheet.Range("F18:F25").Style.Border.RightBorderColor = XLColor.Black;
                worksheet.Range("C25:F25").Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C25:F25").Style.Border.BottomBorderColor = XLColor.Black;

                worksheet.Range("C19:C25").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range("C19:C25").Style.Border.RightBorderColor = XLColor.Black;



                //InnerBorder of Excel  Coomunication
                worksheet.Range("C27:F27").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C27:F27").Style.Border.TopBorderColor = XLColor.Black;
                worksheet.Range("C28:F28").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C28:F28").Style.Border.TopBorderColor = XLColor.Black;

                worksheet.Range("C27:C32").Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C27:C32").Style.Border.LeftBorderColor = XLColor.Black;
                worksheet.Range("F27:F32").Style.Border.RightBorder = XLBorderStyleValues.Medium;
                worksheet.Range("F27:F32").Style.Border.RightBorderColor = XLColor.Black;
                worksheet.Range("C32:F32").Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C32:F32").Style.Border.BottomBorderColor = XLColor.Black;

                worksheet.Range("C28:C32").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range("C28:C32").Style.Border.RightBorderColor = XLColor.Black;


                //InnerBorder of Excel  Owners
                worksheet.Range("C34:F34").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C34:F34").Style.Border.TopBorderColor = XLColor.Black;
                worksheet.Range("C35:F35").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C35:F35").Style.Border.TopBorderColor = XLColor.Black;

                worksheet.Range("C34:C36").Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C34:C36").Style.Border.LeftBorderColor = XLColor.Black;
                worksheet.Range("F34:F36").Style.Border.RightBorder = XLBorderStyleValues.Medium;
                worksheet.Range("F34:F36").Style.Border.RightBorderColor = XLColor.Black;
                worksheet.Range("C36:F36").Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C36:F36").Style.Border.BottomBorderColor = XLColor.Black;

                worksheet.Range("C35:C36").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range("C35:C36").Style.Border.RightBorderColor = XLColor.Black;



                //InnerBorder of Excel  Manager
                worksheet.Range("C38:F38").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C38:F38").Style.Border.TopBorderColor = XLColor.Black;
                worksheet.Range("C39:F39").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C39:F39").Style.Border.TopBorderColor = XLColor.Black;

                worksheet.Range("C38:C40").Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C38:C40").Style.Border.LeftBorderColor = XLColor.Black;
                worksheet.Range("F38:F40").Style.Border.RightBorder = XLBorderStyleValues.Medium;
                worksheet.Range("F38:F40").Style.Border.RightBorderColor = XLColor.Black;
                worksheet.Range("C40:F40").Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C40:F40").Style.Border.BottomBorderColor = XLColor.Black;

                worksheet.Range("C39:C40").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range("C39:C40").Style.Border.RightBorderColor = XLColor.Black;


                //InnerBorder of Excel  Classification
                worksheet.Range("C42:F42").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C42:F42").Style.Border.TopBorderColor = XLColor.Black;
                worksheet.Range("C43:F43").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C43:F43").Style.Border.TopBorderColor = XLColor.Black;

                worksheet.Range("C42:C46").Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C42:C46").Style.Border.LeftBorderColor = XLColor.Black;
                worksheet.Range("F42:F46").Style.Border.RightBorder = XLBorderStyleValues.Medium;
                worksheet.Range("F42:F46").Style.Border.RightBorderColor = XLColor.Black;
                worksheet.Range("C46:F46").Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C46:F46").Style.Border.BottomBorderColor = XLColor.Black;

                worksheet.Range("C43:C46").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range("C43:C46").Style.Border.RightBorderColor = XLColor.Black;



                //InnerBorder of Excel  Insurance
                worksheet.Range("C48:F48").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C48:F48").Style.Border.TopBorderColor = XLColor.Black;
                worksheet.Range("C49:F49").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C49:F49").Style.Border.TopBorderColor = XLColor.Black;

                worksheet.Range("C48:C51").Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C48:C51").Style.Border.LeftBorderColor = XLColor.Black;
                worksheet.Range("F48:F51").Style.Border.RightBorder = XLBorderStyleValues.Medium;
                worksheet.Range("F48:F51").Style.Border.RightBorderColor = XLColor.Black;
                worksheet.Range("C51:F51").Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C51:F51").Style.Border.BottomBorderColor = XLColor.Black;

                worksheet.Range("C49:C51").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range("C49:C51").Style.Border.RightBorderColor = XLColor.Black;



                //InnerBorder of Excel  Tonnage
                worksheet.Range("C53:F53").Style.Border.OutsideBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C53:F53").Style.Border.OutsideBorderColor = XLColor.Black;

                worksheet.Range("C53:C56").Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C53:C56").Style.Border.LeftBorderColor = XLColor.Black;
                worksheet.Range("F53:F56").Style.Border.RightBorder = XLBorderStyleValues.Medium;
                worksheet.Range("F53:F56").Style.Border.RightBorderColor = XLColor.Black;
                worksheet.Range("C56:F56").Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C56:F56").Style.Border.BottomBorderColor = XLColor.Black;

                worksheet.Range("C54:C56").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range("C54:C56").Style.Border.RightBorderColor = XLColor.Black;


                //InnerBorder of Excel  Machinery			

                worksheet.Range("C58:F58").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C58:F58").Style.Border.TopBorderColor = XLColor.Black;
                worksheet.Range("C59:F59").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C59:F59").Style.Border.TopBorderColor = XLColor.Black;

                worksheet.Range("C59:F59").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                worksheet.Range("C66:F66").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                worksheet.Range("C66:F66").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                worksheet.Range("C70:F70").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                worksheet.Range("C70:F70").Style.Border.BottomBorder = XLBorderStyleValues.Thin;

                worksheet.Range("C58:C74").Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C58:C74").Style.Border.LeftBorderColor = XLColor.Black;
                worksheet.Range("F58:F74").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range("F58:F74").Style.Border.RightBorderColor = XLColor.Black;
                worksheet.Range("C74:F74").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                worksheet.Range("C74:F74").Style.Border.BottomBorderColor = XLColor.Black;

                worksheet.Range("C60:C65").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range("C60:C65").Style.Border.RightBorderColor = XLColor.Black;
                worksheet.Range("C67:C69").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range("C67:C69").Style.Border.RightBorderColor = XLColor.Black;
                worksheet.Range("D67:D69").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range("D67:D69").Style.Border.RightBorderColor = XLColor.Black;
                worksheet.Range("E67:E69").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range("E67:E69").Style.Border.RightBorderColor = XLColor.Black;
                worksheet.Range("F67:F69").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range("F67:F69").Style.Border.RightBorderColor = XLColor.Black;

                worksheet.Range("C71:C73").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range("C71:C73").Style.Border.RightBorderColor = XLColor.Black;


                //InnerBorder of Excel  Cargo Other Equipments

                worksheet.Range("C76:F76").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C76:F76").Style.Border.TopBorderColor = XLColor.Black;
                worksheet.Range("C77:F77").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C77:F77").Style.Border.TopBorderColor = XLColor.Black;

                worksheet.Range("C76:C91").Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C76:C91").Style.Border.LeftBorderColor = XLColor.Black;
                worksheet.Range("F76:F91").Style.Border.RightBorder = XLBorderStyleValues.Medium;
                worksheet.Range("F76:F91").Style.Border.RightBorderColor = XLColor.Black;
                worksheet.Range("C91:F91").Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C91:F91").Style.Border.BottomBorderColor = XLColor.Black;

                worksheet.Range("C77:C91").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range("C77:C91").Style.Border.RightBorderColor = XLColor.Black;

                worksheet.Cell("C81").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                worksheet.Cell("C81").Style.Border.RightBorderColor = XLColor.Black;

                //InnerBorder of Excel  ECDIS

                worksheet.Range("C93:F93").Style.Border.TopBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C93:F93").Style.Border.TopBorderColor = XLColor.Black;
                worksheet.Range("C94:F94").Style.Border.BottomBorder = XLBorderStyleValues.Thin;
                worksheet.Range("C94:F94").Style.Border.BottomBorderColor = XLColor.Black;

                worksheet.Range("C93:C97").Style.Border.LeftBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C93:C97").Style.Border.LeftBorderColor = XLColor.Black;
                worksheet.Range("F93:F97").Style.Border.RightBorder = XLBorderStyleValues.Medium;
                worksheet.Range("F93:F97").Style.Border.RightBorderColor = XLColor.Black;
                worksheet.Range("C97:F97").Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                worksheet.Range("C97:F97").Style.Border.BottomBorderColor = XLColor.Black;

                worksheet.Range("C94:C97").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range("C94:C97").Style.Border.RightBorderColor = XLColor.Black;
                worksheet.Range("D94:D97").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range("D94:D97").Style.Border.RightBorderColor = XLColor.Black;
                worksheet.Range("E94:E97").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                worksheet.Range("E94:E97").Style.Border.RightBorderColor = XLColor.Black;



                //Headers & Titles Merge & text


                //Main Title
                worksheet.Range("C3:F3").Merge();
                worksheet.Cell("C3").Value = "VESSEL PARTICULARS";
                worksheet.Cell("C3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell("C3").Style.Fill.SetBackgroundColor(XLColor.FromHtml("253B5B"));
                worksheet.Cell("C3").Style.Font.Bold = true;
                worksheet.Cell("C3").Style.Font.FontSize = 13;
                worksheet.Cell("C3").Style.Font.FontColor = XLColor.White;



                //General Information	

                worksheet.Range("C5:F5").Merge();
                worksheet.Cell("C5").Value = "General Information";
                worksheet.Cell("C5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell("C5").Style.Fill.SetBackgroundColor(XLColor.FromHtml("253B5B"));
                worksheet.Cell("C5").Style.Font.Bold = false;
                worksheet.Cell("C5").Style.Font.FontSize = 12;
                worksheet.Cell("C5").Style.Font.FontColor = XLColor.White;

                //Principal Dimensions
                worksheet.Range("C18:F18").Merge();
                worksheet.Cell("C18").Value = "Principal Dimensions";
                worksheet.Cell("C18").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell("C18").Style.Fill.SetBackgroundColor(XLColor.FromHtml("253B5B"));
                worksheet.Cell("C18").Style.Font.Bold = false;
                worksheet.Cell("C18").Style.Font.FontSize = 12;
                worksheet.Cell("C18").Style.Font.FontColor = XLColor.White;

                //Communicatin
                worksheet.Range("C27:F27").Merge();
                worksheet.Cell("C27").Value = "Communication";
                worksheet.Cell("C27").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell("C27").Style.Fill.SetBackgroundColor(XLColor.FromHtml("253B5B"));
                worksheet.Cell("C27").Style.Font.Bold = false;
                worksheet.Cell("C27").Style.Font.FontSize = 12;
                worksheet.Cell("C27").Style.Font.FontColor = XLColor.White;

                //Owners
                worksheet.Range("C34:F34").Merge();
                worksheet.Cell("C34").Value = "Owners";
                worksheet.Cell("C34").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell("C34").Style.Fill.SetBackgroundColor(XLColor.FromHtml("253B5B"));
                worksheet.Cell("C34").Style.Font.Bold = false;
                worksheet.Cell("C34").Style.Font.FontSize = 12;
                worksheet.Cell("C34").Style.Font.FontColor = XLColor.White;

                //Managers
                worksheet.Range("C38:F38").Merge();
                worksheet.Cell("C38").Value = "Managers";
                worksheet.Cell("C38").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell("C38").Style.Fill.SetBackgroundColor(XLColor.FromHtml("253B5B"));
                worksheet.Cell("C38").Style.Font.Bold = false;
                worksheet.Cell("C38").Style.Font.FontSize = 12;
                worksheet.Cell("C38").Style.Font.FontColor = XLColor.White;

                //Classification
                worksheet.Range("C42:F42").Merge();
                worksheet.Cell("C42").Value = "Classification";
                worksheet.Cell("C42").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell("C42").Style.Fill.SetBackgroundColor(XLColor.FromHtml("253B5B"));
                worksheet.Cell("C42").Style.Font.Bold = false;
                worksheet.Cell("C42").Style.Font.FontSize = 12;
                worksheet.Cell("C42").Style.Font.FontColor = XLColor.White;


                //Insurance
                worksheet.Range("C48:F48").Merge();
                worksheet.Cell("C48").Value = "Insurance";
                worksheet.Cell("C48").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell("C48").Style.Fill.SetBackgroundColor(XLColor.FromHtml("253B5B"));
                worksheet.Cell("C48").Style.Font.Bold = false;
                worksheet.Cell("C48").Style.Font.FontSize = 12;
                worksheet.Cell("C48").Style.Font.FontColor = XLColor.White;


                //Tonnage
                worksheet.Range("C53:F53").Merge();
                worksheet.Cell("C53").Value = "Tonnage";
                worksheet.Cell("C53").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell("C53").Style.Fill.SetBackgroundColor(XLColor.FromHtml("253B5B"));
                worksheet.Cell("C53").Style.Font.Bold = false;
                worksheet.Cell("C53").Style.Font.FontSize = 12;
                worksheet.Cell("C53").Style.Font.FontColor = XLColor.White;

                //Machinery
                worksheet.Range("C58:F58").Merge();
                worksheet.Cell("C58").Value = "Machinery";
                worksheet.Cell("C58").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell("C58").Style.Fill.SetBackgroundColor(XLColor.FromHtml("253B5B"));
                worksheet.Cell("C58").Style.Font.Bold = false;
                worksheet.Cell("C58").Style.Font.FontSize = 12;
                worksheet.Cell("C58").Style.Font.FontColor = XLColor.White;

                worksheet.Range("C59:F59").Merge();
                worksheet.Cell("C59").Value = "Main Engine";
                worksheet.Cell("C59").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell("C59").Style.Fill.SetBackgroundColor(XLColor.FromHtml("D2D2D2"));
                worksheet.Cell("C59").Style.Font.Bold = true;
                worksheet.Cell("C59").Style.Font.FontSize = 12;

                worksheet.Range("C66:F66").Merge();
                worksheet.Cell("C66").Value = "Auxiliary Engine";
                worksheet.Cell("C66").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell("C66").Style.Fill.SetBackgroundColor(XLColor.FromHtml("D2D2D2"));
                worksheet.Cell("C66").Style.Font.Bold = true;
                worksheet.Cell("C66").Style.Font.FontSize = 12;
                worksheet.Range("C67:F67").Style.Fill.SetBackgroundColor(XLColor.FromHtml("D2D2D2"));

                worksheet.Range("C70:F70").Merge();
                worksheet.Cell("C70").Value = "Boilers";
                worksheet.Cell("C70").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell("C70").Style.Fill.SetBackgroundColor(XLColor.FromHtml("D2D2D2"));
                worksheet.Cell("C70").Style.Font.Bold = true;
                worksheet.Cell("C70").Style.Font.FontSize = 12;


                //Other Equipments
                worksheet.Range("C76:F76").Merge();
                worksheet.Cell("C76").Value = "Other Equipments";
                worksheet.Cell("C76").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell("C76").Style.Fill.SetBackgroundColor(XLColor.FromHtml("253B5B"));
                worksheet.Cell("C76").Style.Font.Bold = false;
                worksheet.Cell("C76").Style.Font.FontSize = 12;
                worksheet.Cell("C76").Style.Font.FontColor = XLColor.White;

                worksheet.Range("C76:F76").Merge();
                worksheet.Cell("C76").Value = "Other Equipments";
                worksheet.Cell("C76").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell("C76").Style.Fill.SetBackgroundColor(XLColor.FromHtml("253B5B"));
                worksheet.Cell("C76").Style.Font.Bold = false;
                worksheet.Cell("C76").Style.Font.FontSize = 12;



                //ECDIS
                worksheet.Range("C93:F93").Merge();
                worksheet.Cell("C93").Value = "ECDIS";
                worksheet.Cell("C93").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Cell("C93").Style.Fill.SetBackgroundColor(XLColor.FromHtml("253B5B"));
                worksheet.Cell("C93").Style.Font.Bold = false;
                worksheet.Cell("C93").Style.Font.FontSize = 12;
                worksheet.Cell("C93").Style.Font.FontColor = XLColor.White;



                //Put Data now.= in each Cell
                foreach (var applicant in vesselName)
                {

                    ViewBag.HandOverport = "NA";
                    var HandOverPortId = _context.TblVessels.Where(x => x.IsDeleted == false && x.VesselId == applicant.VesselId).FirstOrDefault().PortOfHandover;
                    if (HandOverPortId != null)
                    {
                        ViewBag.HandOverport = _context.TblSeaports.Where(x => x.SeaportId == HandOverPortId).FirstOrDefault().SeaportName;
                    }


                    worksheet.Cell("C6").Value = "Vessel Name";
                    worksheet.Cell("D6").Value = applicant.VesselName;
                    worksheet.Cell("C7").Value = "Ex - Names";
                    worksheet.Cell("D7").Value = applicant.PreviousName;
                    worksheet.Cell("C8").Value = "IMO No.";
                    worksheet.Cell("D8").Value = applicant.Imo;
                    worksheet.Cell("C9").Value = "Flag";
                    worksheet.Cell("D9").Value = applicant.Flag != null ? applicant.Flag.CountryName : "";
                    worksheet.Cell("C10").Value = "Port of Registery";
                    worksheet.Cell("D10").Value = applicant.PortOfRegistryNavigation != null ? applicant.PortOfRegistryNavigation.SeaportName : "";
                    worksheet.Cell("C11").Value = "Call Sign";
                    worksheet.Cell("D11").Value = applicant.CallSign;
                    worksheet.Cell("C12").Value = "Official No.";
                    worksheet.Cell("D12").Value = applicant.Official;
                    worksheet.Cell("C13").Value = "MMSI No.";
                    worksheet.Cell("D13").Value = applicant.Mmsi;
                    worksheet.Cell("C14").Value = "Vessel Short Code";
                    worksheet.Cell("D14").Value = applicant.VesselCode;
                    worksheet.Cell("C15").Value = "Ship Type";
                    worksheet.Cell("D15").Value = applicant.Ship != null ? applicant.Ship.Type : "";

                    worksheet.Cell("E6").Value = "Hull No.";
                    worksheet.Cell("F6").Value = applicant.HullNo;
                    worksheet.Cell("E7").Value = "Keel Laid Date";
                    worksheet.Cell("F7").Value = string.IsNullOrEmpty(applicant.KeelLaid) ? "" : DateTime.Parse(applicant.KeelLaid).ToShortDateString();
                    worksheet.Cell("E8").Value = "Launch Date";
                    worksheet.Cell("F8").Value = string.IsNullOrEmpty(applicant.Launched) ? "" : DateTime.Parse(applicant.Launched).ToShortDateString();
                    worksheet.Cell("E9").Value = "Delivery Date";
                    worksheet.Cell("F9").Value = string.IsNullOrEmpty(applicant.Delivery) ? "" : DateTime.Parse(applicant.Delivery).ToShortDateString();
                    worksheet.Cell("E10").Value = "Builder Name";
                    worksheet.Cell("F10").Value = applicant.Builder.Builder != null ? applicant.Builder.Builder : "";
                    worksheet.Cell("F10").Style.Alignment.SetWrapText(true);


                    if (applicant.Builder != null)
                    {
                        if (applicant.Builder.Builder.Length > 20)
                        {
                            worksheet.Row(10).Height = 38;
                        }
                    }


                    worksheet.Cell("E11").Value = "Takeover Date";
                    worksheet.Cell("F11").Value = applicant.TakeoverDate == null ? "" : DateTime.Parse(applicant.TakeoverDate.ToString()).ToShortDateString();
                    worksheet.Cell("E12").Value = "Takeover Port";
                    worksheet.Cell("F12").Value = applicant.PortOfTakeovers?.SeaportName;
                    worksheet.Cell("E13").Value = "Handover Date";
                    worksheet.Cell("F13").Value = applicant.HandoverDate == null ? "" : DateTime.Parse(applicant.HandoverDate.ToString()).ToShortDateString();
                    worksheet.Cell("E14").Value = "Handover Port";
                    worksheet.Cell("F14").Value = ViewBag.HandOverport;
                    worksheet.Cell("E15").Value = "Operating Area";
                    worksheet.Cell("F15").Value = applicant.OperatingArea;


                    worksheet.Cell("C19").Value = "LOA";
                    worksheet.Cell("D19").Value = applicant.Loa;
                    worksheet.Cell("C20").Value = "LBP";
                    worksheet.Cell("D20").Value = applicant.Lbp;
                    worksheet.Cell("C21").Value = "Breadth(Extreme)";
                    worksheet.Cell("D21").Value = applicant.Breadth;
                    worksheet.Cell("C22").Value = "Depth(Molded)";
                    worksheet.Cell("D22").Value = applicant.Depth;
                    worksheet.Cell("C23").Value = "Height(Maximum)";
                    worksheet.Cell("D23").Value = applicant.Height;
                    worksheet.Cell("C24").Value = "Bridge Front -BOW";
                    worksheet.Cell("D24").Value = applicant.Bow;
                    worksheet.Cell("C25").Value = "Bridge Front -Stern";
                    worksheet.Cell("D25").Value = applicant.Stern;

                    worksheet.Cell("C28").Value = "Vessel E-mail";
                    worksheet.Cell("D28").Value = applicant.Email;
                    worksheet.Cell("C29").Value = "Phone No. 1";
                    worksheet.Cell("D29").Value = applicant.SatBphone;
                    worksheet.Cell("C30").Value = "Phone No. 2";
                    worksheet.Cell("D30").Value = applicant.Fleet77Phone;
                    worksheet.Cell("C31").Value = "Phone No. 3";
                    worksheet.Cell("D31").Value = applicant.Vsatphone;
                    worksheet.Cell("C32").Value = "Mobile No.";
                    worksheet.Cell("D32").Value = applicant.MobileNo;

                    //Style Cell Value Datatype
                    worksheet.Cell("D29").Style.NumberFormat.Format = "0";
                    worksheet.Cell("D30").Style.NumberFormat.Format = "0";
                    worksheet.Cell("D31").Style.NumberFormat.Format = "0";

                    //worksheet.Cell("D29").DataType = XLDataType.Number;
                    //worksheet.Cell("D30").DataType = XLDataType.Number;
                    //worksheet.Cell("D31").DataType = XLDataType.Number;



                    worksheet.Cell("C35").Value = "Registered Owner";
                    worksheet.Cell("D35").Value = applicant.Owner != null ? applicant.Owner.OwnerName : "";
                    worksheet.Cell("C36").Value = "Disponent Owner";
                    worksheet.Cell("D36").Value = applicant.DisponentOwner != null ? applicant.DisponentOwner.DisponentOwners : "";

                    worksheet.Cell("C39").Value = "Technical Manager";
                    worksheet.Cell("D39").Value = applicant.Manager.Managers;
                    worksheet.Cell("C40").Value = "Crewing Manager";
                    worksheet.Cell("D40").Value = applicant.Crewmanager.Managers;

                    worksheet.Cell("C43").Value = "Classification Society";
                    worksheet.Cell("D43").Value = applicant.Classification != null ? applicant.Classification.Classifications : "";

                    worksheet.Cell("C44").Value = "Class No.";
                    worksheet.Cell("D44").Value = applicant.ClassNo;
                    worksheet.Cell("C45").Value = "Class Notation";
                    worksheet.Cell("D45").Value = applicant.ClassNotation;
                    worksheet.Cell("C46").Value = "Ice Class";
                    worksheet.Cell("D46").Value = applicant.IceClass;


                    worksheet.Cell("C49").Value = "P & I Club";
                    worksheet.Cell("D49").Value = applicant.VendorRegisterPi.VendorName;
                    worksheet.Cell("C50").Value = "H & M";
                    worksheet.Cell("D50").Value = applicant.VendorRegisterHm?.VendorName;
                    worksheet.Cell("C51").Value = "Qualified Individual";
                    worksheet.Cell("D51").Value = applicant.QualifiedIndividual;

                    worksheet.Cell("C54").Value = "NRT";
                    worksheet.Cell("D54").Value = applicant.Net;
                    worksheet.Cell("C55").Value = "GRT";
                    worksheet.Cell("D55").Value = applicant.Gross;
                    worksheet.Cell("C56").Value = "DWT";
                    worksheet.Cell("D56").Value = applicant.SummerDeadWeight;

                    worksheet.Cell("C60").Value = "Make";
                    worksheet.Cell("D60").Value = applicant.EngineModel != null ? applicant.EngineModel.Maker : "";
                    worksheet.Cell("C61").Value = "Model";
                    worksheet.Cell("D61").Value = applicant.EngineModel != null ? applicant.EngineModel.Model : "";
                    worksheet.Cell("C62").Value = "Type";
                    worksheet.Cell("D62").Value = applicant.EngineType != null ? applicant.EngineType.Type : "";
                    worksheet.Cell("C63").Value = "M.C.R.KW";
                    worksheet.Cell("D63").Value = applicant.Kw;
                    worksheet.Cell("C64").Value = "No.of Main Engines";
                    worksheet.Cell("D64").Value = applicant.MainEngineCount;
                    worksheet.Cell("C65").Value = "CPP";
                    worksheet.Cell("D65").Value = applicant.Cpp;

                    worksheet.Cell("C68").Value = "Make";
                    worksheet.Cell("C69").Value = "Model";
                    worksheet.Cell("D67").Value = "No. 1";
                    worksheet.Cell("D68").Value = applicant.AuxMake1;
                    worksheet.Cell("D69").Value = applicant.AuxModel1;
                    worksheet.Cell("E67").Value = "No. 2";
                    worksheet.Cell("E68").Value = applicant.AuxMake2;
                    worksheet.Cell("E69").Value = applicant.AuxModel2;
                    worksheet.Cell("F67").Value = "No. 3";
                    worksheet.Cell("F68").Value = applicant.AuxMake3;
                    worksheet.Cell("F69").Value = applicant.AuxModel3;



                    worksheet.Cell("C71").Value = "Make";
                    worksheet.Cell("D71").Value = applicant.AuxMake4;
                    worksheet.Cell("C72").Value = "Model";
                    worksheet.Cell("D72").Value = applicant.AuxModel4;
                    worksheet.Cell("C73").Value = "No of Boilers";
                    worksheet.Cell("D73").Value = applicant.AuxBoiler;

                    if (applicant.Ship != null)
                    {
                        if (applicant.Ship.Type.Contains("Bulk") == false)
                        {

                            worksheet.Cell("C78").Value = "Cargo Tanks";
                            worksheet.Cell("C78").Style.Font.Bold = true;
                            worksheet.Cell("C79").Value = "No.of Tanks";
                            worksheet.Cell("D79").Value = applicant.CargoTanks;
                            worksheet.Cell("C80").Value = "Coating";
                            worksheet.Cell("D80").Value = applicant.CargoCoating;

                            worksheet.Cell("C82").Value = "Cargo Pumps";
                            worksheet.Cell("C82").Style.Font.Bold = true;
                            worksheet.Cell("C83").Value = "No of Pumps";
                            worksheet.Cell("D83").Value = applicant.CargoPump;
                            worksheet.Cell("C84").Value = "Type of Pumps";
                            worksheet.Cell("D84").Value = applicant.CargoPumpType;
                            worksheet.Cell("C85").Value = "Capacity of Pumps";
                            worksheet.Cell("D85").Value = applicant.PumpCapacity;


                            worksheet.Cell("C88").Value = "Ballast Pumps";
                            worksheet.Cell("C88").Style.Font.Bold = true;
                            worksheet.Cell("C89").Value = "No.of Pumps";
                            worksheet.Cell("D89").Value = applicant.BallastPump;
                            worksheet.Cell("C90").Value = "Type of Pumps";
                            worksheet.Cell("D90").Value = applicant.BallastPumpType;
                            worksheet.Cell("C91").Value = "Ballast Eductor";
                            worksheet.Cell("D91").Value = applicant.BallastEductor;
                        }
                        else
                        {
                            worksheet.Cell("C78").Value = "Cargo Holds";
                            worksheet.Cell("C78").Style.Font.Bold = true;
                            worksheet.Cell("C79").Value = "No.of Holds";
                            worksheet.Cell("D79").Value = applicant.CargoHolds;


                            worksheet.Cell("C82").Value = "Cargo Cranes";
                            worksheet.Cell("C82").Style.Font.Bold = true;
                            worksheet.Cell("C83").Value = "Make";
                            worksheet.Cell("D83").Value = applicant.CargoCranesMaker;
                            worksheet.Cell("C84").Value = "Model";
                            worksheet.Cell("D84").Value = applicant.CranesModel;
                            worksheet.Cell("C85").Value = "Capacity";
                            worksheet.Cell("D85").Value = applicant.CranesCapacity;
                            worksheet.Cell("C86").Value = "No.of Cranes";
                            worksheet.Cell("D86").Value = applicant.CranesNumber;

                            worksheet.Cell("C88").Value = "Grabs";
                            worksheet.Cell("C88").Style.Font.Bold = true;
                            worksheet.Cell("C89").Value = "Make";
                            worksheet.Cell("D89").Value = applicant.GrabMaker;
                            worksheet.Cell("C90").Value = "Capacity";
                            worksheet.Cell("D90").Value = applicant.GrabCapacity;
                            worksheet.Cell("C91").Value = "No.of Grabs";
                            worksheet.Cell("D91").Value = applicant.GrabsNumber;
                        }
                    }

                    worksheet.Cell("D94").Value = "ECDIS 1";
                    worksheet.Cell("E94").Value = "ECDIS 2";
                    worksheet.Cell("F94").Value = "ECDIS 3";
                    worksheet.Cell("C95").Value = "Maker";
                    worksheet.Cell("C96").Value = "Model";

                    worksheet.Cell("D95").Value = applicant.Ecdisid1Navigation != null ? applicant.Ecdisid1Navigation.Maker : "";
                    worksheet.Cell("D96").Value = applicant.Ecdisid1Navigation != null ? applicant.Ecdisid1Navigation.Model : "";

                    worksheet.Cell("E95").Value = applicant.Ecdisid2Navigation != null ? applicant.Ecdisid2Navigation.Maker : "";
                    worksheet.Cell("E96").Value = applicant.Ecdisid2Navigation != null ? applicant.Ecdisid2Navigation.Model : "";

                    worksheet.Cell("F95").Value = applicant.Ecdisid3Navigation != null ? applicant.Ecdisid3Navigation.Maker : "";
                    worksheet.Cell("F96").Value = applicant.Ecdisid3Navigation != null ? applicant.Ecdisid3Navigation.Model : "";


                    worksheet.Cell("C97").Value = "Type";

                    worksheet.Cell("D97").Value = applicant.Ecdistype1;
                    worksheet.Cell("E97").Value = applicant.Ecdistype2;
                    worksheet.Cell("F97").Value = applicant.Ecdistype3;

                    workbook.SaveAs(fileName);
                }

                var errorMessage = "you can return the errors here!";

                return Json(new { fileName = fileName, errorMessage });
            }
        }


        public IActionResult PdfVesselPaticular()
        {
            ViewBag.vesseldata = _context.TblVessels.Include(x => x.Flag).Include(x => x.PortOfRegistryNavigation).Include(x => x.Ship)
               .Include(x => x.Owner).Include(x => x.DisponentOwner).Include(x => x.Manager).Include(x => x.Crewmanager)
               .Include(x => x.Classification).Include(t => t.PortOfTakeovers).Include(p => p.VendorRegisterPi)
               .Include(h => h.VendorRegisterHm).Include(e => e.EngineModel).Include(T => T.EngineType).Include(b => b.Builder)
               .Where(x => x.IsDeleted == false && x.VesselId == vesselidtouse).ToList();

            ViewBag.HandOverport = "NA";
            var HandOverPortId = _context.TblVessels.Where(x => x.IsDeleted == false && x.VesselId == vesselidtouse).FirstOrDefault().PortOfHandover;
            if (HandOverPortId != null)
            {
                ViewBag.HandOverport = _context.TblSeaports.Where(x => x.SeaportId == HandOverPortId).FirstOrDefault().SeaportName;
            }

            return PartialView();

        }

        public JsonResult GeneratevesselPDF()
        {

            //string url = serverUrl + "api/vessel/vesselData?vesselId=" + vesselId;
            var location = new Uri($"{Request.Scheme}://{Request.Host}");//new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}");
            var urls = location.AbsoluteUri;
            string url = urls + "Home/PdfVesselPaticular";
           // string url = "  http://ship.crewlinkasm.com/Home/PdfVesselPaticular";


            string pdf_page_size = PdfPageSize.A4.ToString();
            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), pdf_page_size, true);
            string pdf_orientation = PdfPageOrientation.Portrait.ToString();
            PdfPageOrientation pdfOrientation = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation),
                pdf_orientation, true);
            int webPageWidth = 1000;
            int webPageHeight = 0;
            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();
            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;
            converter.Options.MarginLeft = 10;
            converter.Options.MarginRight = 10;
            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertUrl(url);

            string fileName = "VesselParticularPDF" + ".pdf";

            doc.Save(fileName);

            return Json(new { fileName = fileName });
        }

        public IActionResult Emailconfigure()
        {
            ViewBag.name = HttpContext.Session.GetString("name");
            ViewBag.vesselDetails = _context.TblVessels.Include(x => x.Flag).Include(x => x.Ship).Where(x => x.IsDeleted == false && x.VesselId == vesselidtouse).FirstOrDefault();
            ViewBag.vessels = _context.TblVessels.Where(x => x.IsDeleted == false && x.IsActive == false && x.VesselId == vesselidtouse).ToList();
             ViewBag.Email = _context.TblEmails.FirstOrDefault();
            return PartialView();
        }


        [HttpPost]
        public IActionResult EmailSave(TblEmail tblEmail)
        {
           var Email = _context.TblEmails.FirstOrDefault();
            if (Email != null)
            {
                Email.ID = Email.ID;
                Email.EmailId = tblEmail.EmailId;
                Email.Password = tblEmail.Password;
                Email.Smtp = tblEmail.Smtp;
                Email.Port = tblEmail.Port;
                Email.Pop = tblEmail.Pop;
                Email.PopPort = tblEmail.PopPort;
                Email.EmailSentTo = tblEmail.EmailSentTo;
                Email.NotificationEmailSentTo = tblEmail.NotificationEmailSentTo;
                _context.TblEmails.Update(Email);
            }
            if (Email == null) { 
            _context.TblEmails.Add(tblEmail);                        
            }
            _context.SaveChanges();
            return RedirectToAction("Emailconfigure");
        }


        public JsonResult CheckSMTP(string smtp, int portNo)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    var server = smtp;
                    var port = portNo;

                    IPHostEntry ipHostInfo = Dns.GetHostEntry(smtp);
                    client.Connect(server, port);

                    // As GMail requires SSL we should use SslStream
                    // If your SMTP server doesn't support SSL you can
                    // work directly with the underlying stream
                    using (var stream = client.GetStream())
                    //using (var sslStream = new SslStream(stream))
                    {
                        //sslStream.AuthenticateAsClient(server);
                        using (var writer = new StreamWriter(stream))
                        using (var reader = new StreamReader(stream))
                        {
                            writer.WriteLine("EHLO " + server);
                            writer.Flush();

                            //Console.WriteLine(reader.ReadLine());
                            string result = reader.ReadLine();
                            var messageSuccess = true;
                            return Json(new { message = messageSuccess = true });
                            //client.Close();
                            // GMail responds with: 220 mx.google.com ESMTP
                        }
                    }
                  
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                var messageError = false;
                return Json(new { message = messageError = false });
            }
            return null;

        }

        public JsonResult POPCheck(string pop, int portpop,string email,string password)
        {
            try
            {
                using (TcpClient client = new TcpClient(pop, portpop))
                using (NetworkStream stream = client.GetStream())
                using (StreamReader reader = new StreamReader(stream))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    string readserver =reader.ReadLine(); // Read server greeting

                    // Send user command
                    writer.WriteLine(email);
                    writer.Flush();
                    string username = reader.ReadLine();

                    // Send password command
                    writer.WriteLine(password);
                    writer.Flush();
                    string pwd = reader.ReadLine();

                    // Send quit command
                    writer.WriteLine("QUIT");
                    writer.Flush();
                    string result = reader.ReadLine();
                    var messageSuccess = true;
                    return Json(new { popmessage = messageSuccess = true });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                var errormessage = false;
                return Json(new { popmessage = errormessage = false });
            }
            return null;

        }


        }

}

