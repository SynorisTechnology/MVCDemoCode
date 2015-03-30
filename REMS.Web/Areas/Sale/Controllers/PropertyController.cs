using REMS.Data.Access.Admin;
using REMS.Data.Access.Master;
using REMS.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace REMS.Web.Areas.Sale.Controllers
{
    public class PropertyController : Controller
    {
        #region Private Fields
        PlanInstallmentService piService;
        PlanTypeMasterService ptService;
        #endregion
        public PropertyController()
        {
            piService = new PlanInstallmentService();
            ptService = new PlanTypeMasterService();
        }
        // GET: Sale/Property
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CalculatePrice(int Id)
        {
            ViewBag.FlatID = Id;
            return View();
        }

        public ActionResult NewSale(int? Id)
        {
            ViewBag.FlatID = Id;
            return View();
        }

        #region ChargeList
        public string GetFlatDetails(int flatid)
        {
            FlatService fservice = new FlatService();
            var model = fservice.GetFlatDetails(flatid);
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
        public string GetPlanTypeMasterByParams(string PlanName, string FType, decimal? Size)
        {
            PlanTypeMasterService ptmservice = new PlanTypeMasterService();
            var model = ptmservice.GetPlanTypeMasterByParams(PlanName, FType, Size);
            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
        }
        #endregion

        #region Installment Plan
        public string GetInstallmentPlan(int PlanTypeMasterID, bool plc, bool acharge, bool aocharge, string plcValue, string achargevalue, string aochargevalue)
        {
            var plantypemaster = ptService.GetPlanTypeMasterByID(PlanTypeMasterID);
            int planid = (int)plantypemaster.PlanID;
            var model = piService.GetPlanInstallmentByPlanID(planid);
            string[] html = new string[3];
            string[] planhtml = planRowHtml(plc, acharge, aocharge);
            string rowhtml = "";
            int index = 0;
            foreach (var md in model)
            {
                string st = planhtml[0];
                st = st.Replace("<% SN %>", md.InstallmentNo.Value.ToString())
                    .Replace("<% InstallmentID %>", "INS" + md.PlanInstallmentID.ToString())
                    .Replace("<% BSPID %>", "BSP" + md.PlanInstallmentID.ToString())
                    .Replace("<% BSPValue %>", md.Amount.Value.ToString().TrimEnd('0').TrimEnd('.'))
                    .Replace("<% Options %>", installmentOptionhtml(model, index))
                    .Replace("<% PLCID %>", "PLC" + md.PlanInstallmentID.ToString())
                    .Replace("<% AdditionalChargeID %>", "ANC" + md.PlanInstallmentID.ToString())
                    .Replace("<% AddOnChargeID %>", "AOC" + md.PlanInstallmentID.ToString());
                if (index == 0)
                {
                    st = st.Replace("<% PLCValue %>", plcValue)
                        .Replace("<% AdditionalChargeValue %>", achargevalue)
                        .Replace("<% AddOnChargeValue %>", aochargevalue);
                }
                else
                {
                    st = st.Replace("<% PLCValue %>", "0")
                       .Replace("<% AdditionalChargeValue %>", "0")
                       .Replace("<% AddOnChargeValue %>", "0");
                }
                rowhtml += st;
                index = index + 1;
            }
            html[0] = rowhtml;
            html[1] = planhtml[1];
            html[2] = planhtml[2];
            return Newtonsoft.Json.JsonConvert.SerializeObject(html);
        }
        public string installmentOptionhtml(List<PlanInstallmentModel> model, int i)
        {
            string html = "";
            int index = 0;
            foreach (var md in model)
            {
                if (i == index)
                    html += "<option value='" + md.PlanInstallmentID.ToString() + "' selected>" + md.Installment + "</option>";
                else
                    html += "<option value='" + md.PlanInstallmentID.ToString() + "'>" + md.Installment + "</option>";
                index = index + 1;
            }
            return html;
        }

        public string[] planRowHtml(bool plc, bool additionalcharge, bool addoncharge)
        {
            Random rd = new Random();
            int rno= rd.Next(1, 7);
            string[] st = new string[3];
            string fhtml = @"<tr><td></td><td><b>Total</b></td><td><b id='TotalBSP'></b></td>";
            string hhtml = @"<tr><th>SN</th><th>Installment</th><th>BSP</th>";
            string html = @"<tr id='"+rno+"'>";
                                      html+=@"<td><% SN %></td>
                                        <td><label class='select'><select id='<% InstallmentID %>' name='<% InstallmentID %>'>
                                        <% Options %>
                                        </select></label></td>";
            html += @"<td><label class='input'><input type='text' id='<% BSPID %>' value='<% BSPValue %>'></label></td>";

            if (plc)
            {
                hhtml += "<th>PLC</th>";
                fhtml += "<td><b id='TotalPLC'></b></td>";
                html += @"<td><label class='input'><input type='text' id='<% PLCID %>' value='<% PLCValue %>'></label></td>";
            }
            if (additionalcharge)
            {
                hhtml += "<th>AdditionalCharge</th>";
                fhtml += "<td><b id='TotalACharge'></b></td>";
                html += @"<td><label class='input'><input type='text' id='<% AdditionalChargeID %>'  value='<% AdditionalChargeValue %>'></label></td>";
            }
            if (addoncharge)
            {
                hhtml += "<th>AddOnCharge</th>";
                fhtml += "<td><b id='TotalAOCharge'></b></td>";
                html += @"<td><label class='input'><input type='text' id='<% AddOnChargeID %>'  value='<% AddOnChargeValue %>'></label></td>";
            }
            html += @" <td><i class='fa fa-trash-o'  onclick='DeletePlanRow(" + rno + ")'></i></td></tr>";
            hhtml += @"<th>Action</th></tr>";
            fhtml += @"<td></td></tr>";
            st[0] = html;
            st[1] = hhtml;
            st[2] = fhtml;
            return st;
        }

        public string AddInstallmentPlanRow(int PlanTypeMasterID, bool plc, bool acharge, bool aocharge)
        {
            var plantypemaster = ptService.GetPlanTypeMasterByID(PlanTypeMasterID);
            int planid = (int)plantypemaster.PlanID;
            var model = piService.GetPlanInstallmentByPlanID(planid);
            string[] planhtml = planRowHtml(plc, acharge, aocharge);
            string rowhtml = "";
            int index = 0;
            foreach (var md in model)
            {
                // add only one row
                if (index == 0)
                {
                    string st = planhtml[0];
                    st = st.Replace("<% SN %>", md.InstallmentNo.Value.ToString())
                        .Replace("<% InstallmentID %>", "INS" + md.PlanInstallmentID.ToString())
                        .Replace("<% BSPID %>", "BSP" + md.PlanInstallmentID.ToString())
                        .Replace("<% BSPValue %>", md.Amount.Value.ToString().TrimEnd('0').TrimEnd('.'))
                        .Replace("<% Options %>", installmentOptionhtml(model, index))
                        .Replace("<% PLCID %>", "PLC" + md.PlanInstallmentID.ToString())
                        .Replace("<% AdditionalChargeID %>", "ANC" + md.PlanInstallmentID.ToString())
                        .Replace("<% AddOnChargeID %>", "AOC" + md.PlanInstallmentID.ToString());

                    st = st.Replace("<% PLCValue %>", "0")
                       .Replace("<% AdditionalChargeValue %>", "0")
                       .Replace("<% AddOnChargeValue %>", "0");
                    rowhtml += st;
                    index = index + 1;
                }
            }
            return Newtonsoft.Json.JsonConvert.SerializeObject(rowhtml);
       
        }
        #endregion
    }
}