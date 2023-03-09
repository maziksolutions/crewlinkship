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
            int vesselId = 75; int month = 1; int year = 2023; string ispromoted = "no"; string checkpbtilldate = "";
            var data = _context.PortageBillVM.FromSqlRaw<PortageBillVM>("getPortageBill @p0, @p1, @p2, @p3, @p4", vesselId, month, year, ispromoted, checkpbtilldate);
            ViewBag.vessel = new SelectList(_context.TblVessels, "Vesselid", "vesselName");
            var signoffcrewdata = _context.PortageBillSignoffVM.FromSqlRaw<PortageBillSignoffVM>("getPortageBillOffSigners @p0, @p1, @p2", vesselId, month, year);
            var tables = new PortageViewModel
            {
                onsignersportage = data,
                promotionsportagebill = data,
                offsignersporatge = signoffcrewdata
            };
            return View(tables);
        }

    
        public IActionResult Genneratexcel(string vesselId = "Bakerloo", int month = 12, int year = 2021, string currency = "", string checkpbtilldate = "")
        {
            
            var query = _context.PortageBillPDFVM.FromSqlRaw("spPortagePDFFile @p0, @p1, @p2,@p3,@p4", vesselId, month, year, currency, checkpbtilldate).ToList();

            DataTable dt = new DataTable();
            dt = LINQResultToDataTable(query);

            var signoffdata = _context.PortageBillPDFSignoffVM.FromSqlRaw("spSignOFFPortagePDFFile @p0, @p1, @p2,@p3", vesselId, month, year, currency).ToList();

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
                    dt.Columns[13].ColumnName = "Security Allow";

                    dt.Columns[14].ColumnName = "Leave Pay & Subsitence Allow ";
                    dt.Columns[15].ColumnName = "Uniform Allow. ";
                    dt.Columns[16].ColumnName = "Pension Fund ";
                    dt.Columns[17].ColumnName = "Other Allow. ";
                    dt.Columns[18].ColumnName = "Total Wages per month ";
                    dt.Columns[20].ColumnName = "Overtime Hr";
                    dt.Columns[21].ColumnName = "Basic Wages";
                    dt.Columns[22].ColumnName = "Fixed / Guaranteed OT";
                    dt.Columns[23].ColumnName = "Security Allow ";
                    dt.Columns[24].ColumnName = "Leave Pay & Subsitence Allow.";
                    dt.Columns[25].ColumnName = "Uniform Allow.";
                    dt.Columns[26].ColumnName = "Pension Fund";
                    dt.Columns[27].ColumnName = "Other Allow.";
                    dt.Columns[28].ColumnName = "Extra OT";
                    dt.Columns[29].ColumnName = "Other Earning";
                    dt.Columns[30].ColumnName = "Transit Wages";
                    dt.Columns[31].ColumnName = "Total Earnings (Current Month)";
                    dt.Columns[32].ColumnName = "Bal from Prev Month";
                    dt.Columns[34].ColumnName = "Net Allotment payable";
                    dt.Columns[35].ColumnName = "Leave Pay + Subsitence  C/F (This Month)";
                    dt.Columns[36].ColumnName = "Cash Advance";
                    dt.Columns[37].ColumnName = "Bonded Stores";
                    dt.Columns[38].ColumnName = "Other Deduction";
                    dt.Columns[39].ColumnName = "8 % PF Employee Contribution  (On Basic)";
                    dt.Columns[40].ColumnName = "2.0 % Union Dues (On Gross)";
                    dt.Columns[41].ColumnName = "1.0 % Welfare fund (On Gross)";
                    dt.Columns[42].ColumnName = "WHT@5 % ";
                    dt.Columns[44].ColumnName = "PF Deduction(Indian)";
                    dt.Columns[45].ColumnName = "Total Deductions";
                    dt.Columns[46].ColumnName = "Leave wages B/F";
                    dt.Columns[47].ColumnName = "Leave wages C/F";
                    dt.Columns[48].ColumnName = "Final Balance";
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
                    dtsignoff.Columns[13].ColumnName = "Security Allow ";

                    dtsignoff.Columns[14].ColumnName = "Leave Pay & Subsitence Allow ";
                    dtsignoff.Columns[15].ColumnName = "Uniform Allow. ";
                    dtsignoff.Columns[16].ColumnName = "Pension Fund ";
                    dtsignoff.Columns[17].ColumnName = "Other Allow. ";
                    dtsignoff.Columns[18].ColumnName = "Total Wages per month";
                    dtsignoff.Columns[20].ColumnName = "Overtime Hr";
                    dtsignoff.Columns[21].ColumnName = "Basic Wages";
                    dtsignoff.Columns[22].ColumnName = "Fixed / Guaranteed OT";
                    dtsignoff.Columns[23].ColumnName = "Security Allow";

                    dtsignoff.Columns[24].ColumnName = "Leave Pay & Subsitence Allow.";
                    dtsignoff.Columns[25].ColumnName = "Uniform Allow.";
                    dtsignoff.Columns[26].ColumnName = "Pension Fund";
                    dtsignoff.Columns[27].ColumnName = "Other Allow.";
                    dtsignoff.Columns[28].ColumnName = "Extra OT";
                    dtsignoff.Columns[29].ColumnName = "Other Earning";
                    dtsignoff.Columns[30].ColumnName = "Transit Wages";
                    dtsignoff.Columns[31].ColumnName = "Total Earnings (Current Month)";
                    dtsignoff.Columns[32].ColumnName = "Bal from Prev Month";
                    dtsignoff.Columns[34].ColumnName = "Net Allotment payable";
                    dtsignoff.Columns[35].ColumnName = "Leave Pay + Subsitence  C/F (This Month)";
                    dtsignoff.Columns[36].ColumnName = "Cash Advance";
                    dtsignoff.Columns[37].ColumnName = "Bonded Stores";
                    dtsignoff.Columns[38].ColumnName = "Other Deduction";
                    dtsignoff.Columns[39].ColumnName = "8 % PF Employee Contribution  (On Basic)";
                    dtsignoff.Columns[40].ColumnName = "2.0 % Union Dues (On Gross)";
                    dtsignoff.Columns[41].ColumnName = "1.0 % Welfare fund (On Gross)";
                    dtsignoff.Columns[42].ColumnName = "WHT@5 % ";
                    dtsignoff.Columns[44].ColumnName = "PF Deduction(Indian)";
                    dtsignoff.Columns[45].ColumnName = "Total Deductions";
                    dtsignoff.Columns[46].ColumnName = "Leave wages B/F";
                    dtsignoff.Columns[47].ColumnName = "Leave wages C/F";
                    dtsignoff.Columns[48].ColumnName = "Final Balance";
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
                }
                ws.Range("A3:AZ3").Style.Alignment.TextRotation = 90;
                if (query.Count() > 0)
                {
                    ws.Range("V3:AI3").Style.Fill.BackgroundColor = XLColor.GreenPigment;
                    ws.Range("AJ3:AT3").Style.Fill.BackgroundColor = XLColor.Brown;
                }
                ws.Range("A3:AZ3").Style.Alignment.WrapText = true;
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
    }
}
