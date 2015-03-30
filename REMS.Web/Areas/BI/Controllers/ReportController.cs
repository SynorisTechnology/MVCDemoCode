using REMS.Data;
using REMS.Data.Access;
using REMS.Data.CustomModel;
using REMS.Data.DataModel;
using REMS.Web.App_Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace REMS.Web.Areas.BI.Controllers
{
    public class ReportController : BaseController
    {
        #region Private Fields
        private Helper hp;
        private REMSDBEntities dbContext;
        #endregion
        public ReportController()
        {
            hp = new Helper();
            dbContext = new REMSDBEntities();
        }
        // GET: BI/Report
        [MyAuthorize]
        public ActionResult Index()
        {
            return View();
        }
        [MyAuthorize]
        public ActionResult RefundProperty()
        {
            DateTime datef = new DateTime();
            DateTime datet = new DateTime();
            // Date.
            datef = DateTime.Now.AddMonths(-1);
            datet = DateTime.Now;
            Session["propertyName"] = "Property Name";
            var md = (from sale in dbContext.SaleFlats join f in dbContext.Flats on sale.FlatID equals f.FlatID join cust in dbContext.Customers on sale.SaleID equals cust.SaleID where sale.SaleDate >= datef && sale.SaleDate <= datet select new { sale = sale, cust=cust, FlatName = f.FlatName });

            List<FlatSaleModel> model = new List<FlatSaleModel>();
            foreach (var v in md)
            {
                model.Add(new FlatSaleModel { FlatName = v.FlatName, FlatID = v.sale.FlatID, SaleRate = v.sale.SaleRate, Remarks = v.sale.Remarks, SaleDate = v.sale.SaleDate, FName = (v.cust.FName + " " + v.cust.LName), PropertyID = v.sale.ProjectID });
            }
            return View(model);
        }


        [MyAuthorize]
        public ActionResult TransferProperty()
        {
            return View();
        }
        [MyAuthorize]
        public ActionResult MonthlySale()
        {
            return View();
        }
        [MyAuthorize]
        public ActionResult CancelCheque()
        {
            return View();
        }
        [MyAuthorize]
        public ActionResult ChequeClearance()
        {
            Session["PropertyName"] = "Property Name";
            DateTime datef = new DateTime();
            DateTime datet = new DateTime();
            // Get this month records
            datef = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            datet = datef.AddMonths(1);
            var model = dbContext.Payments.Where(p => p.PaymentDate >= datef && p.PaymentDate <= datet).OrderByDescending(o => o.PaymentID);
            return View(model);
        }
        [MyAuthorize]
        public ActionResult InstallmentPayment()
        {
            return View();
        }
        [MyAuthorize]
        public ActionResult OtherPayment()
        {
            return View();
        }
        [MyAuthorize]
        public ActionResult CancelChequeOther()
        {
            return View();
        }
        [MyAuthorize]
        public ActionResult ChequeClearanceOther()
        {
            return View();
        }
        [MyAuthorize]
        public ActionResult BrokerList()
        {
            return View();
        }
        [MyAuthorize]
        public ActionResult BrokerFlatApprove()
        {
            return View();
        }
        [MyAuthorize]
        public ActionResult BrokerCommission()
        {
            return View();
        }
        [MyAuthorize]
        public ActionResult AssuredReturn()
        {
            return View();
        }
        [MyAuthorize]
        public ActionResult PendingInstallment()
        {
            return View();
        }



        public ActionResult PropertyRemark()
        {
            return View();
        }



        public ActionResult DemandLettertPrintAction(string id)
        {
            ViewBag.ID = id;
            return View();
        }
        public ActionResult DemandLettert2PrintAction(string id)
        {
            ViewBag.ID = id;
            return View();
        }
        public ActionResult DemandLettert3PrintAction(string id)
        {
            ViewBag.ID = id;
            return View();
        }
        public ActionResult DemandLetter()
        {
            return View();
        }
        public ActionResult DemandLetterView()
        {
            return View();
        }

        public string RefundPropertySearch(string propertyName, string search, string propertyid, string propertySubTypeID, string proSize, string datefrom, string dateto, string searchtext)
        {
            try
            {
                DateTime datef = new DateTime();
                DateTime datet = new DateTime();

                // Date.
                if (datefrom != "" && dateto != "")
                {
                    datef = Convert.ToDateTime(datefrom);
                    datet = Convert.ToDateTime(dateto);
                }
                else
                {
                    datef = DateTime.Now.AddMonths(-1);
                    datet = DateTime.Now;
                }

                if (propertyid == "? undefined:undefined ?" || propertyid == "All" || propertyid == "") propertyid = "0";
                if (propertySubTypeID == "? undefined:undefined ?" || propertySubTypeID == "All" || propertySubTypeID == "") { propertySubTypeID = "0"; Session["propertyName"] = "Property Name"; }
                else { Session["propertyName"] = propertyName; }
                if (proSize == "" || proSize == "? undefined:undefined ?" || proSize == "All") proSize = "0";
                int pid = Convert.ToInt32(propertyid);
                int ptypeid = Convert.ToInt32(propertySubTypeID);
                int psize = Convert.ToInt32(proSize);
                if (propertyid == "0") // All Properties
                {
                    if (search == "All")
                    {
                        var md = (from sale in dbContext.RefundProperties where sale.RefundDate >= datef && sale.RefundDate <= datet select new { sale = sale });
                        List<RefundPropertyModel> model = new List<RefundPropertyModel>();
                        foreach (var v in md)
                        {
                            string rdate = "", cdate = "";
                            if (v.sale.RefundDate != null)
                                rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                            if (v.sale.ChequeDate != null)
                                cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                            model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                        }

                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                        // By default showing last one month sales in all properties
                    }
                    //else if (search == "SubType")
                    //{
                    //    var md = (from sale in dbContext.RefundProperties join f in dbContext.SaleFlats on sale.SaleID equals f.SaleID join ft in dbContext.Flats on f.FlatID equals ft.FlatID where f.PropertyTypeID == ptypeid && f.PropertySizeID == psize select new { sale = sale });

                    //    List<RefundPropertyModel> model = new List<RefundPropertyModel>();
                    //    foreach (var v in md)
                    //    {
                    //        string rdate = "", cdate = "";
                    //        if (v.sale.RefundDate != null)
                    //            rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                    //        if (v.sale.ChequeDate != null)
                    //            cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                    //        model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                    //    }
                    //    return Newtonsoft.Json.JsonConvert.SerializeObject(model);

                    //    //var model = context.tblSSaleFlats.Where(p => p.SaleDate >= datef && p.SaleDate <= datet).OrderBy(o => o.SaleID);
                    //    //return View(model);
                    //}
                    else if (search == "FlatName")
                    {
                        var md = (from sale in dbContext.RefundProperties where sale.FlatName.Contains(searchtext) select new { sale = sale });

                        List<RefundPropertyModel> model = new List<RefundPropertyModel>();
                        foreach (var v in md)
                        {
                            string rdate = "", cdate = "";
                            if (v.sale.RefundDate != null)
                                rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                            if (v.sale.ChequeDate != null)
                                cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                            model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                    else if (search == "RefundDate")
                    {

                        DateTime dtFrom = Convert.ToDateTime(datefrom);
                        DateTime dtTo = Convert.ToDateTime(dateto);

                        var md = (from sale in dbContext.RefundProperties where sale.RefundDate >= dtFrom && sale.RefundDate <= dtTo select new { sale = sale });

                        List<RefundPropertyModel> model = new List<RefundPropertyModel>();
                        foreach (var v in md)
                        {
                            string rdate = "", cdate = "";
                            if (v.sale.RefundDate != null)
                                rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                            if (v.sale.ChequeDate != null)
                                cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                            model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                    else if (search == "SaleDate")
                    {
                        DateTime dtFrom = Convert.ToDateTime(datefrom);
                        DateTime dtTo = Convert.ToDateTime(dateto);

                        var md = (from sale in dbContext.RefundProperties join f in dbContext.SaleFlats on sale.SaleID equals f.SaleID where f.SaleDate >= dtFrom && f.SaleDate <= dtTo select new { sale = sale });

                        List<RefundPropertyModel> model = new List<RefundPropertyModel>();
                        foreach (var v in md)
                        {
                            string rdate = "", cdate = "";
                            if (v.sale.RefundDate != null)
                                rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                            if (v.sale.ChequeDate != null)
                                cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                            model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                    else if (search == "This Month")
                    {
                        DateTime dtFrom = DateTime.Now.AddMonths(-1);
                        DateTime dtTo = DateTime.Now;

                        var md = (from sale in dbContext.RefundProperties where sale.RefundDate >= dtFrom && sale.RefundDate <= dtTo select new { sale = sale });

                        List<RefundPropertyModel> model = new List<RefundPropertyModel>();
                        foreach (var v in md)
                        {
                            string rdate = "", cdate = "";
                            if (v.sale.RefundDate != null)
                                rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                            if (v.sale.ChequeDate != null)
                                cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                            model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                    else if (search == "Last 7 Days")
                    {

                        DateTime dtFrom = DateTime.Now.AddDays(-7);
                        DateTime dtTo = DateTime.Now;

                        var md = (from sale in dbContext.RefundProperties where sale.RefundDate >= dtFrom && sale.RefundDate <= dtTo select new { sale = sale });

                        List<RefundPropertyModel> model = new List<RefundPropertyModel>();
                        foreach (var v in md)
                        {
                            string rdate = "", cdate = "";
                            if (v.sale.RefundDate != null)
                                rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                            if (v.sale.ChequeDate != null)
                                cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                            model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                }
                else // Search by Property id
                {
                    if (search == "All")
                    {
                        var md = (from sale in dbContext.RefundProperties join f in dbContext.SaleFlats on sale.SaleID equals f.SaleID where f.Flat.Floor.Tower.ProjectID == pid select new { sale = sale });

                        List<RefundPropertyModel> model = new List<RefundPropertyModel>();
                        foreach (var v in md)
                        {
                            string rdate = "", cdate = "";
                            if (v.sale.RefundDate != null)
                                rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                            if (v.sale.ChequeDate != null)
                                cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                            model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                    else if (search == "SubType")
                    {
                        if (ptypeid != 0)
                        {
                            if (psize == 0)
                            {
                                var md = (from sale in dbContext.RefundProperties join f in dbContext.SaleFlats on sale.SaleID equals f.SaleID where f.ProjectID == pid select new { sale = sale });
                                List<RefundPropertyModel> model = new List<RefundPropertyModel>();
                                foreach (var v in md)
                                {
                                    string rdate = "", cdate = "";
                                    if (v.sale.RefundDate != null)
                                        rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                                    if (v.sale.ChequeDate != null)
                                        cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                                    model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                                }
                                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                            }
                            else
                            {
                                var md = (from sale in dbContext.RefundProperties join f in dbContext.SaleFlats on sale.SaleID equals f.SaleID where f.ProjectID == pid select new { sale = sale });
                                List<RefundPropertyModel> model = new List<RefundPropertyModel>();
                                foreach (var v in md)
                                {
                                    string rdate = "", cdate = "";
                                    if (v.sale.RefundDate != null)
                                        rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                                    if (v.sale.ChequeDate != null)
                                        cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                                    model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                                }
                                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                            }
                        }
                        else
                        {

                            if (psize == 0)
                            {
                                var md = (from sale in dbContext.RefundProperties join f in dbContext.SaleFlats on sale.SaleID equals f.SaleID where f.ProjectID == pid select new { sale = sale });
                                List<RefundPropertyModel> model = new List<RefundPropertyModel>();
                                foreach (var v in md)
                                {
                                    string rdate = "", cdate = "";
                                    if (v.sale.RefundDate != null)
                                        rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                                    if (v.sale.ChequeDate != null)
                                        cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                                    model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                                }
                                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                            }
                            else
                            {
                                var md = (from sale in dbContext.RefundProperties join f in dbContext.SaleFlats on sale.SaleID equals f.SaleID where f.ProjectID == pid select new { sale = sale });
                                List<RefundPropertyModel> model = new List<RefundPropertyModel>();
                                foreach (var v in md)
                                {
                                    string rdate = "", cdate = "";
                                    if (v.sale.RefundDate != null)
                                        rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                                    if (v.sale.ChequeDate != null)
                                        cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                                    model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                                }
                                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                            }

                        }
                    }
                    else if (search == "FlatName")
                    {
                        var md = (from sale in dbContext.RefundProperties join f in dbContext.SaleFlats on sale.SaleID equals f.SaleID where f.ProjectID == pid && sale.FlatName.Contains(searchtext) select new { sale = sale });

                        List<RefundPropertyModel> model = new List<RefundPropertyModel>();
                        foreach (var v in md)
                        {
                            string rdate = "", cdate = "";
                            if (v.sale.RefundDate != null)
                                rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                            if (v.sale.ChequeDate != null)
                                cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                            model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                    else if (search == "RefundDate")
                    {
                        DateTime dtFrom = Convert.ToDateTime(datefrom);
                        DateTime dtTo = Convert.ToDateTime(dateto);

                        var md = (from sale in dbContext.RefundProperties join f in dbContext.SaleFlats on sale.SaleID equals f.SaleID where f.ProjectID == pid && sale.RefundDate >= dtFrom && sale.RefundDate <= dtTo select new { sale = sale });
                        List<RefundPropertyModel> model = new List<RefundPropertyModel>();
                        foreach (var v in md)
                        {
                            string rdate = "", cdate = "";
                            if (v.sale.RefundDate != null)
                                rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                            if (v.sale.ChequeDate != null)
                                cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                            model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                    else if (search == "SaleDate")
                    {
                        DateTime dtFrom = Convert.ToDateTime(datefrom);
                        DateTime dtTo = Convert.ToDateTime(dateto);

                        var md = (from sale in dbContext.RefundProperties join f in dbContext.SaleFlats on sale.SaleID equals f.SaleID where f.ProjectID == pid && f.SaleDate >= dtFrom && f.SaleDate <= dtTo select new { sale = sale });

                        List<RefundPropertyModel> model = new List<RefundPropertyModel>();
                        foreach (var v in md)
                        {
                            string rdate = "", cdate = "";
                            if (v.sale.RefundDate != null)
                                rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                            if (v.sale.ChequeDate != null)
                                cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                            model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                    else if (search == "This Month")
                    {
                        DateTime dtFrom = DateTime.Now.AddMonths(-1);
                        DateTime dtTo = DateTime.Now;

                        var md = (from sale in dbContext.RefundProperties join f in dbContext.SaleFlats on sale.SaleID equals f.SaleID where f.ProjectID == pid && sale.RefundDate >= dtFrom && sale.RefundDate <= dtTo select new { sale = sale });

                        List<RefundPropertyModel> model = new List<RefundPropertyModel>();
                        foreach (var v in md)
                        {
                            string rdate = "", cdate = "";
                            if (v.sale.RefundDate != null)
                                rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                            if (v.sale.ChequeDate != null)
                                cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                            model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                    else if (search == "Last 7 Days")
                    {
                        DateTime dtFrom = DateTime.Now.AddDays(-7);
                        DateTime dtTo = DateTime.Now;

                        var md = (from sale in dbContext.RefundProperties join f in dbContext.SaleFlats on sale.SaleID equals f.FlatID where f.ProjectID == pid && sale.RefundDate >= dtFrom && sale.RefundDate <= dtTo select new { sale = sale });
                        List<RefundPropertyModel> model = new List<RefundPropertyModel>();
                        foreach (var v in md)
                        {
                            string rdate = "", cdate = "";
                            if (v.sale.RefundDate != null)
                                rdate = Convert.ToDateTime(v.sale.RefundDate).ToString("dd/MM/yyyy");
                            if (v.sale.ChequeDate != null)
                                cdate = Convert.ToDateTime(v.sale.ChequeDate).ToString("dd/MM/yyyy");
                            model.Add(new RefundPropertyModel { RefundPropertyID = v.sale.RefundPropertyID, SaleID = v.sale.SaleID, FlatName = v.sale.FlatName, RefundDateSt = rdate, RefundAmount = v.sale.RefundAmount, PaymentMode = v.sale.PaymentMode, ChequeDateSt = cdate, BankName = v.sale.BankName, BranchName = v.sale.BranchName, Remarks = v.sale.Remarks, ChequeNo = v.sale.ChequeNo });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject("");
            }
            catch (Exception ex)
            {
                Failure = "Invalid search query, please try again.";
                Helper h = new Helper();
                h.LogException(ex);
                return Newtonsoft.Json.JsonConvert.SerializeObject("");
            }
        }

        public string TransferPropertySearch(string propertyName, string search, string propertyid, string propertySubTypeID, string proSize, string datefrom, string dateto, string searchtext)
        {
            try
            {
                DateTime datef = new DateTime();
                DateTime datet = new DateTime();

                // Date.
                if (datefrom != "" && dateto != "")
                {
                    datef = Convert.ToDateTime(datefrom);
                    datet = Convert.ToDateTime(dateto);
                }
                else
                {
                    datef = DateTime.Now.AddMonths(-1);
                    datet = DateTime.Now;
                }

                if (propertyid == "? undefined:undefined ?" || propertyid == "All" || propertyid == "") propertyid = "0";
                if (propertySubTypeID == "? undefined:undefined ?" || propertySubTypeID == "All" || propertySubTypeID == "") { propertySubTypeID = "0"; Session["propertyName"] = "Property Name"; }
                else { Session["propertyName"] = propertyName; }
                if (proSize == "" || proSize == "? undefined:undefined ?" || proSize == "All") proSize = "0";
                int pid = Convert.ToInt32(propertyid);
                int ptypeid = Convert.ToInt32(propertySubTypeID);
                int psize = Convert.ToInt32(proSize);
                if (propertyid == "0") // All Properties
                {
                    if (search == "All")
                    {
                        var md = (from sale in dbContext.PropertyTransfers join f in dbContext.SaleFlats on sale.SaleID equals f.SaleID where sale.TransferDate >= datef && sale.TransferDate <= datet select new { sale = sale, SaleDate = f.SaleDate, PropertyID = f.ProjectID, FlatID = f.FlatID });
                        List<TransferPropertyModel> model = new List<TransferPropertyModel>();
                        foreach (var v in md)
                        {
                            var ncust = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                            var ocudt = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                            var flatname = dbContext.Flats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                            string rdate = "", cdate = "";
                            if (v.sale.TransferDate != null)
                                rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                            if (v.SaleDate != null)
                                cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                            model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                        }

                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                        // By default showing last one month sales in all properties
                    }
                    //else if (search == "SubType")
                    //{
                    //    var md = (from sale in dbContext.PropertyTransfers join f in dbContext.SaleFlats on sale.SaleID equals f.SaleID join ft in dbContext.Flats on f.FlatID equals ft.FlatID where f.PropertyTypeID == ptypeid && f.PropertySizeID == psize select new { sale = sale, SaleDate = f.SaleDate, PropertyID = f.PropertyID, FlatID = f.FlatID });

                    //    List<TransferPropertyModel> model = new List<TransferPropertyModel>();
                    //    foreach (var v in md)
                    //    {
                    //        var ncust = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.).FirstOrDefault();
                    //        var ocudt = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                    //        var flatname = dbContext.SaleFlats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                    //        string rdate = "", cdate = "";
                    //        if (v.sale.TransferDate != null)
                    //            rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                    //        if (v.SaleDate != null)
                    //            cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                    //        model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                    //    }

                    //    return Newtonsoft.Json.JsonConvert.SerializeObject(model);

                    //    //var model = context.tblSSaleFlats.Where(p => p.SaleDate >= datef && p.SaleDate <= datet).OrderBy(o => o.SaleID);
                    //    //return View(model);
                    //}
                    else if (search == "FlatName")
                    {
                        var md = (from sale in dbContext.PropertyTransfers join f in dbContext.SaleFlats on sale.SaleID equals f.SaleID join ft in dbContext.Flats on f.FlatID equals ft.FlatID where ft.FlatName.Contains(searchtext) select new { sale = sale, SaleDate = f.SaleDate, PropertyID = f.ProjectID, FlatID = f.FlatID });

                        List<TransferPropertyModel> model = new List<TransferPropertyModel>();
                        foreach (var v in md)
                        {
                            var ncust = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                            var ocudt = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                            var flatname = dbContext.Flats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                            string rdate = "", cdate = "";
                            if (v.sale.TransferDate != null)
                                rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                            if (v.SaleDate != null)
                                cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                            model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                        }

                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                    else if (search == "TransferDate")
                    {

                        DateTime dtFrom = Convert.ToDateTime(datefrom);
                        DateTime dtTo = Convert.ToDateTime(dateto);

                        var md = (from sale in dbContext.PropertyTransfers join f in dbContext.SaleFlats on sale.SaleID equals f.SaleID where sale.TransferDate >= dtFrom && sale.TransferDate <= dtTo select new { sale = sale, SaleDate = f.SaleDate, PropertyID = f.ProjectID, FlatID = f.FlatID });

                        List<TransferPropertyModel> model = new List<TransferPropertyModel>();
                        foreach (var v in md)
                        {
                            var ncust = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                            var ocudt = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                            var flatname = dbContext.Flats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                            string rdate = "", cdate = "";
                            if (v.sale.TransferDate != null)
                                rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                            if (v.SaleDate != null)
                                cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                            model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                    else if (search == "SaleDate")
                    {
                        DateTime dtFrom = Convert.ToDateTime(datefrom);
                        DateTime dtTo = Convert.ToDateTime(dateto);

                        var md = (from sale in dbContext.PropertyTransfers join f in dbContext.SaleFlats on sale.SaleID equals f.SaleID where f.SaleDate >= dtFrom && f.SaleDate <= dtTo select new { sale = sale, SaleDate = f.SaleDate, PropertyID = f.ProjectID, FlatID = f.FlatID });

                        List<TransferPropertyModel> model = new List<TransferPropertyModel>();
                        foreach (var v in md)
                        {
                            var ncust = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                            var ocudt = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                            var flatname = dbContext.Flats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                            string rdate = "", cdate = "";
                            if (v.sale.TransferDate != null)
                                rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                            if (v.SaleDate != null)
                                cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                            model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                    else if (search == "This Month")
                    {
                        DateTime dtFrom = DateTime.Now.AddMonths(-1);
                        DateTime dtTo = DateTime.Now;

                        var md = (from sale in dbContext.PropertyTransfers join f in dbContext.SaleFlats on sale.SaleID equals f.SaleID where sale.TransferDate >= dtFrom && sale.TransferDate <= dtTo select new { sale = sale, SaleDate = f.SaleDate, PropertyID = f.ProjectID, FlatID = f.FlatID });

                        List<TransferPropertyModel> model = new List<TransferPropertyModel>();
                        foreach (var v in md)
                        {
                            var ncust = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                            var ocudt = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                            var flatname = dbContext.Flats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                            string rdate = "", cdate = "";
                            if (v.sale.TransferDate != null)
                                rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                            if (v.SaleDate != null)
                                cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                            model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                    else if (search == "Last 7 Days")
                    {

                        DateTime dtFrom = DateTime.Now.AddDays(-7);
                        DateTime dtTo = DateTime.Now;

                        var md = (from sale in dbContext.PropertyTransfers join f in dbContext.SaleFlats on sale.SaleID equals f.SaleID where sale.TransferDate >= dtFrom && sale.TransferDate <= dtTo select new { sale = sale, SaleDate = f.SaleDate, PropertyID = f.ProjectID, FlatID = f.FlatID });

                        List<TransferPropertyModel> model = new List<TransferPropertyModel>();
                        foreach (var v in md)
                        {
                            var ncust = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                            var ocudt = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                            var flatname = dbContext.Flats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                            string rdate = "", cdate = "";
                            if (v.sale.TransferDate != null)
                                rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                            if (v.SaleDate != null)
                                cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                            model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                }
                else // Search by Property id
                {
                    if (search == "All")
                    {
                        var md = (from sale in dbContext.PropertyTransfers join f in dbContext.SaleFlats on sale.SaleID equals f.SaleID where f.ProjectID == pid select new { sale = sale, SaleDate = f.SaleDate, PropertyID = f.ProjectID, FlatID = f.FlatID });

                        List<TransferPropertyModel> model = new List<TransferPropertyModel>();
                        foreach (var v in md)
                        {
                            var ncust = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                            var ocudt = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                            var flatname = dbContext.Flats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                            string rdate = "", cdate = "";
                            if (v.sale.TransferDate != null)
                                rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                            if (v.SaleDate != null)
                                cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                            model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                    //else if (search == "SubType")
                    //{
                    //    if (ptypeid != 0)
                    //    {
                    //        if (psize == 0)
                    //        {
                    //            var md = (from sale in dbContext.PropertyTransfers join f in dbContext.SaleFlats on sale.SaleID equals f.SaleID where f.ProjectID == pid && f.PropertyTypeID == ptypeid select new { sale = sale, SaleDate = f.SaleDate, PropertyID = f.PropertyID, FlatID = f.FlatID });
                    //            List<TransferPropertyModel> model = new List<TransferPropertyModel>();
                    //            foreach (var v in md)
                    //            {
                    //                var ncust = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                    //                var ocudt = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                    //                var flatname = dbContext.Flats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                    //                string rdate = "", cdate = "";
                    //                if (v.sale.TransferDate != null)
                    //                    rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                    //                if (v.SaleDate != null)
                    //                    cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                    //                model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                    //            }
                    //            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    //        }
                    //        else
                    //        {
                    //            var md = (from sale in context.PropertyTransfers join f in context.tblSSaleFlats on sale.SaleID equals f.SaleID where f.PropertyID == pid && f.PropertyTypeID == ptypeid && f.PropertySizeID == psize select new { sale = sale, SaleDate = f.SaleDate, PropertyID = f.PropertyID, FlatID = f.FlatID });
                    //            List<TransferPropertyModel> model = new List<TransferPropertyModel>();
                    //            foreach (var v in md)
                    //            {
                    //                var ncust = context.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                    //                var ocudt = context.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                    //                var flatname = context.tblSFlats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                    //                string rdate = "", cdate = "";
                    //                if (v.sale.TransferDate != null)
                    //                    rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                    //                if (v.SaleDate != null)
                    //                    cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                    //                model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                    //            }
                    //            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    //        }
                    //    }
                    //    else
                    //    {

                    //        if (psize == 0)
                    //        {
                    //            var md = (from sale in context.PropertyTransfers join f in context.tblSSaleFlats on sale.SaleID equals f.SaleID where f.PropertyID == pid select new { sale = sale, SaleDate = f.SaleDate, PropertyID = f.PropertyID, FlatID = f.FlatID });
                    //            List<TransferPropertyModel> model = new List<TransferPropertyModel>();
                    //            foreach (var v in md)
                    //            {
                    //                var ncust = context.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                    //                var ocudt = context.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                    //                var flatname = context.tblSFlats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                    //                string rdate = "", cdate = "";
                    //                if (v.sale.TransferDate != null)
                    //                    rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                    //                if (v.SaleDate != null)
                    //                    cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                    //                model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                    //            }
                    //            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    //        }
                    //        else
                    //        {
                    //            var md = (from sale in context.PropertyTransfers join f in context.tblSSaleFlats on sale.SaleID equals f.SaleID where f.PropertyID == pid && f.PropertyTypeID == ptypeid select new { sale = sale, SaleDate = f.SaleDate, PropertyID = f.PropertyID, FlatID = f.FlatID });
                    //            List<TransferPropertyModel> model = new List<TransferPropertyModel>();
                    //            foreach (var v in md)
                    //            {
                    //                var ncust = context.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                    //                var ocudt = context.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                    //                var flatname = context.tblSFlats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                    //                string rdate = "", cdate = "";
                    //                if (v.sale.TransferDate != null)
                    //                    rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                    //                if (v.SaleDate != null)
                    //                    cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                    //                model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                    //            }
                    //            return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    //        }

                    //    }
                    //}
                    else if (search == "FlatName")
                    {
                        var md = (from sale in dbContext.PropertyTransfers join f in dbContext.SaleFlats on sale.SaleID equals f.SaleID join ft in dbContext.Flats on f.FlatID equals ft.FlatID where f.ProjectID == pid && ft.FlatName.Contains(searchtext) select new { sale = sale, SaleDate = f.SaleDate, PropertyID = f.ProjectID, FlatID = f.FlatID });

                        List<TransferPropertyModel> model = new List<TransferPropertyModel>();
                        foreach (var v in md)
                        {
                            var ncust = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                            var ocudt = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                            var flatname = dbContext.Flats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                            string rdate = "", cdate = "";
                            if (v.sale.TransferDate != null)
                                rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                            if (v.SaleDate != null)
                                cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                            model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                    else if (search == "TransferDate")
                    {
                        DateTime dtFrom = Convert.ToDateTime(datefrom);
                        DateTime dtTo = Convert.ToDateTime(dateto);

                        var md = (from sale in dbContext.PropertyTransfers join f in dbContext.SaleFlats on sale.SaleID equals f.SaleID where f.ProjectID == pid && sale.TransferDate >= dtFrom && sale.TransferDate <= dtTo select new { sale = sale, SaleDate = f.SaleDate, PropertyID = f.ProjectID, FlatID = f.FlatID });
                        List<TransferPropertyModel> model = new List<TransferPropertyModel>();
                        foreach (var v in md)
                        {
                            var ncust = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                            var ocudt = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                            var flatname = dbContext.Flats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                            string rdate = "", cdate = "";
                            if (v.sale.TransferDate != null)
                                rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                            if (v.SaleDate != null)
                                cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                            model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                    else if (search == "SaleDate")
                    {
                        DateTime dtFrom = Convert.ToDateTime(datefrom);
                        DateTime dtTo = Convert.ToDateTime(dateto);

                        var md = (from sale in dbContext.PropertyTransfers join f in dbContext.SaleFlats on sale.SaleID equals f.SaleID where f.ProjectID == pid && f.SaleDate >= dtFrom && f.SaleDate <= dtTo select new { sale = sale, SaleDate = f.SaleDate, PropertyID = f.ProjectID, FlatID = f.FlatID });
                        List<TransferPropertyModel> model = new List<TransferPropertyModel>();
                        foreach (var v in md)
                        {
                            var ncust = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                            var ocudt = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                            var flatname = dbContext.Flats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                            string rdate = "", cdate = "";
                            if (v.sale.TransferDate != null)
                                rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                            if (v.SaleDate != null)
                                cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                            model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                    else if (search == "This Month")
                    {
                        DateTime dtFrom = DateTime.Now.AddMonths(-1);
                        DateTime dtTo = DateTime.Now;

                        var md = (from sale in dbContext.PropertyTransfers join f in dbContext.SaleFlats on sale.SaleID equals f.SaleID where f.ProjectID == pid && sale.TransferDate >= dtFrom && sale.TransferDate <= dtTo select new { sale = sale, SaleDate = f.SaleDate, PropertyID = f.ProjectID, FlatID = f.FlatID });
                        List<TransferPropertyModel> model = new List<TransferPropertyModel>();
                        foreach (var v in md)
                        {
                            var ncust = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                            var ocudt = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                            var flatname = dbContext.Flats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                            string rdate = "", cdate = "";
                            if (v.sale.TransferDate != null)
                                rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                            if (v.SaleDate != null)
                                cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                            model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                    else if (search == "Last 7 Days")
                    {
                        DateTime dtFrom = DateTime.Now.AddDays(-7);
                        DateTime dtTo = DateTime.Now;

                        var md = (from sale in dbContext.PropertyTransfers join f in dbContext.SaleFlats on sale.SaleID equals f.FlatID where f.ProjectID == pid && sale.TransferDate >= dtFrom && sale.TransferDate <= dtTo select new { sale = sale, SaleDate = f.SaleDate, PropertyID = f.ProjectID, FlatID = f.FlatID });
                        List<TransferPropertyModel> model = new List<TransferPropertyModel>();
                        foreach (var v in md)
                        {
                            var ncust = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.NewCustomerID).FirstOrDefault();
                            var ocudt = dbContext.Customers.Where(cust => cust.CustomerID == v.sale.OldCustomerID).FirstOrDefault();
                            var flatname = dbContext.Flats.Where(cust => cust.FlatID == v.FlatID).FirstOrDefault().FlatName;
                            string rdate = "", cdate = "";
                            if (v.sale.TransferDate != null)
                                rdate = Convert.ToDateTime(v.sale.TransferDate).ToString("dd/MM/yyyy");
                            if (v.SaleDate != null)
                                cdate = Convert.ToDateTime(v.SaleDate).ToString("dd/MM/yyyy");
                            model.Add(new TransferPropertyModel { PropertyName = flatname, SaleDateSt = cdate, PropertyTransferID = v.sale.PropertyTransferID, SaleID = v.sale.SaleID, OldCustomerID = v.sale.OldCustomerID, NewCustomerID = v.sale.NewCustomerID, TransferDate = v.sale.TransferDate, TransferDateSt = rdate, NewPlanType = v.sale.NewPlanType, OldPlanType = v.sale.OldPlanType, TransferAmount = v.sale.TransferAmount, CustomerFrom = ocudt.AppTitle + " " + ocudt.FName + " " + ocudt.MName + " " + ocudt.LName, CustomerTo = ncust.AppTitle + " " + ncust.FName + " " + ncust.MName + " " + ncust.LName });
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject("");
            }
            catch (Exception ex)
            {
                Failure = "Invalid search query, please try again.";
                Helper h = new Helper();
                h.LogException(ex);
                return Newtonsoft.Json.JsonConvert.SerializeObject("");
            }
        }



        public string SearchPendingInstallment(string search, string propertyid, string datefrom, string dateto, string searchtext)
        {
            try
            {
                int Propertyid = Convert.ToInt32(propertyid);
                DateTime datef = new DateTime();
                DateTime datet = new DateTime();

                // Date.
                if (datefrom != "" && dateto != "")
                {
                    datef = Convert.ToDateTime(datefrom);
                    datet = Convert.ToDateTime(dateto);
                }
                else
                {
                    datef = DateTime.Now.AddMonths(-1);
                    datet = DateTime.Now;
                }

                if (search == "PropertyName")
                {
                    List<FlatSaleModel> model = new List<FlatSaleModel>();
                    var md1 = (from sale in dbContext.SaleFlats
                               join f in dbContext.Flats on sale.FlatID equals f.FlatID
                               join c in dbContext.Customers on sale.SaleID equals c.SaleID
                               where  f.FlatName.Contains(searchtext) && sale.ProjectID == Propertyid
                               select new { saleID = sale.SaleID, Cust=c, FlatName = f.FlatName, MobileNo = c.MobileNo, sale = sale });
                    //&& sale.PropertyTypeID == Propertyid
                    foreach (var v in md1)
                    {
                        decimal paidamount = 0;
                        var mdPaid = (from pay in dbContext.Payments where pay.SaleID == v.saleID select new { paidamount = pay.Amount });

                        foreach (var MdpaidAdmount in mdPaid)
                        {
                            paidamount = paidamount + Convert.ToDecimal(MdpaidAdmount.paidamount);
                        }
                        string bdate = "";
                        if (v.sale.SaleDate != null)
                            bdate = Convert.ToDateTime(v.sale.SaleDate).ToString("dd/MM/yyyy");
                        model.Add(new FlatSaleModel { SaleID = v.sale.SaleID, BookingDateSt = bdate, FlatName = v.FlatName, FlatID = v.sale.FlatID, SaleRate = v.sale.SaleRate, Remarks = v.sale.Remarks, DueDate = v.sale.SaleDate, FName = (v.Cust.FName + " " + v.Cust.LName), PropertyID = v.sale.ProjectID, PaidAmount = paidamount, DueAmount = (v.sale.SaleRate - paidamount) });
                    }
                    return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                }
                else
                {
                    if (search == "BookingDate")
                    {
                        List<FlatSaleModel> model = new List<FlatSaleModel>();
                        var VALUE1 = (from ins in dbContext.FlatInstallmentDetails
                                      where ins.DueDate >= datef
                                      select new { sale = ins.SaleID, amount = ins.TotalAmount }).ToList();
                        List<int?> saleID = VALUE1.Select(e => e.sale).Distinct().ToList();

                        for (int K = 0; K < saleID.Count; K++)
                        {
                            decimal TotalInsAmount = 0;
                            int saleid = Convert.ToInt32(saleID[K].Value);
                            var list = VALUE1.Where(d => d.sale == saleid);

                            foreach (var amount in list)
                            {
                                TotalInsAmount = TotalInsAmount + Convert.ToDecimal(amount.amount);
                            }

                            var md1 = (from sale in dbContext.SaleFlats
                                       join f in dbContext.Flats on sale.FlatID equals f.FlatID
                                       join c in dbContext.Customers on sale.SaleID equals c.SaleID
                                       where sale.SaleID == saleid && sale.ProjectID == Propertyid
                                       select new { saleID = sale.SaleID, Cust=c, FlatName = f.FlatName, MobileNo = c.MobileNo, sale = sale });

                            foreach (var v in md1)
                            {

                                decimal paidamount = 0;
                                var mdPaid = (from pay in dbContext.Payments where pay.SaleID == v.saleID select new { paidamount = pay.Amount });

                                foreach (var MdpaidAdmount in mdPaid)
                                {
                                    paidamount = paidamount + Convert.ToDecimal(MdpaidAdmount.paidamount);
                                }
                                string bdate = "";
                                if (v.sale.SaleDate != null)
                                    bdate = Convert.ToDateTime(v.sale.SaleDate).ToString("dd/MM/yyyy");
                                model.Add(new FlatSaleModel { SaleID = v.sale.SaleID, BookingDateSt = bdate, FlatName = v.FlatName, FlatID = v.sale.FlatID, SaleRate = v.sale.SaleRate, Remarks = v.sale.Remarks, DueDate = v.sale.SaleDate, FName = (v.Cust.FName + " " + v.Cust.LName), PropertyID = v.sale.ProjectID, PaidAmount = paidamount, DueAmount = (TotalInsAmount - paidamount) });
                            }
                        }
                        return Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    }
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject("");
            }
            catch (Exception ex)
            {
                Failure = "Invalid search query, please try again.";
                Helper h = new Helper();
                h.LogException(ex);
                return Newtonsoft.Json.JsonConvert.SerializeObject("");
            }
        }

        public string SearchPropertyRemak(string search, string propertyid, string datefrom, string dateto, string searchtext)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.SerializeObject("");
            }
            catch (Exception ex)
            {
                Failure = "Invalid search query, please try again.";
                Helper h = new Helper();
                h.LogException(ex);
                return Newtonsoft.Json.JsonConvert.SerializeObject("");
            }
        }

        public string SearchDemandLetter(string search, string propertyid, string datefrom)
        {
            try
            {
                int Propertyid = Convert.ToInt32(propertyid);
                DateTime datef = new DateTime();
                datef = Convert.ToDateTime(datefrom);
                int DemandLetterid = 0;
                // Date.
                if (search == "DemandLetter1")
                {
                    DemandLetterid = 1;
                }
                else if (search == "DemandLetter2")
                {
                    DemandLetterid = 2;
                }
                else if (search == "DemandLetter3")
                {
                    DemandLetterid = 3;
                }

                List<FlatSaleModel> model = new List<FlatSaleModel>();

                DataFunctions DF = new DataFunctions();
                DataTable ds = DF.GetDataTable("select I.SaleID, sum(I.TotalAmount) as TotalAmount from FlatInstallmentDetail as I inner join tblsSaleFlat on I.saleid=tblsSaleFlat.SaleID  where I.Duedate<='" + datef + "' and tblsSaleFlat.DemandStatus='" + DemandLetterid + "' group by I.saleid");

                foreach (DataRow row in ds.Rows)
                {
                    decimal TotalInsAmount = 0;
                    int saleid = Convert.ToInt32(row["SaleID"].ToString());
                    TotalInsAmount = Convert.ToDecimal(row["TotalAmount"].ToString());
                    decimal paidamount = 0;
                    var mdPaid = (from pay in dbContext.Payments where pay.SaleID == saleid select new { paidamount = pay.Amount });
                    foreach (var MdpaidAdmount in mdPaid)
                    {
                        paidamount = paidamount + Convert.ToDecimal(MdpaidAdmount.paidamount);
                    }
                    if ((TotalInsAmount - paidamount) > 0)
                    {
                        var md1 = (from sale in dbContext.SaleFlats
                                   join f in dbContext.Flats on sale.FlatID equals f.FlatID
                                   join c in dbContext.Customers on sale.SaleID equals c.SaleID
                                   where sale.SaleID == saleid && sale.ProjectID == Propertyid && sale.DemandStatus == DemandLetterid
                                   select new { saleID = sale.SaleID,cust=c, FlatName = f.FlatName, MobileNo = c.MobileNo, sale = sale });
                        foreach (var v in md1)
                        {
                            string bdate = "";
                            if (v.sale.SaleDate != null)
                                bdate = Convert.ToDateTime(v.sale.SaleDate).ToString("dd/MM/yyyy");
                            model.Add(new FlatSaleModel { SaleID = v.sale.SaleID, BookingDateSt = bdate, FlatName = v.FlatName, FlatID = v.sale.FlatID, SaleRate = v.sale.SaleRate, Remarks = v.sale.Remarks, DueDate = v.sale.SaleDate, FName = (v.cust.FName + " " + v.cust.LName), PropertyID = v.sale.ProjectID, PaidAmount = paidamount, DueAmount = (TotalInsAmount - paidamount) });
                        }
                    }
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
            }
            catch (Exception ex)
            {
                Failure = "Invalid search query, please try again.";
                Helper h = new Helper();
                h.LogException(ex);
                return Newtonsoft.Json.JsonConvert.SerializeObject("");
            }
        }
        public string GanrateDimandLetterDimand(string search, string propertyid, string datefrom, string saleid)
        {
            try
            {
                int Propertyid = Convert.ToInt32(propertyid);
                DateTime datef = new DateTime();
                datef = Convert.ToDateTime(datefrom);

                string[] AllSaleID = saleid.Split(',');
                int DemandLetterid = 0;
                // Date.
                if (search == "DemandLetter1")
                {
                    DemandLetterid = 2;
                }
                else if (search == "DemandLetter2")
                {
                    DemandLetterid = 3;
                }
                else if (search == "DemandLetter3")
                {
                    DemandLetterid = 4;
                }

                for (int K = 0; K < AllSaleID.Length; K++)
                {
                    if (Convert.ToString(AllSaleID[K]) != "")
                    {
                        int SaleID = Convert.ToInt32(AllSaleID[K]);
                        DataFunctions DF = new DataFunctions();
                        DataTable ds = DF.GetDataTable("select SaleID, sum(TotalAmount) as TotalAmount from tblSInstallmentDetail where Duedate<='" + datef + "' and  saleid='" + SaleID + "' group by saleid");

                        foreach (DataRow row in ds.Rows)
                        {
                            decimal TotalInsAmount = 0;
                            TotalInsAmount = Convert.ToDecimal(row["TotalAmount"].ToString());
                            decimal paidamount = 0;
                            var mdPaid = (from pay in dbContext.Payments where pay.SaleID == SaleID select new { paidamount = pay.Amount });
                            foreach (var MdpaidAdmount in mdPaid)
                            {
                                paidamount = paidamount + Convert.ToDecimal(MdpaidAdmount.paidamount);
                            }
                            if ((TotalInsAmount - paidamount) > 0)
                            {
                                var stud = (from s in dbContext.SaleFlats
                                            where s.SaleID == SaleID
                                            select s).FirstOrDefault();
                                stud.DemandStatus = DemandLetterid;
                                dbContext.SaveChanges();
                                ReminderLetter _reminderletter = new ReminderLetter();
                                _reminderletter.CreateDate = DateTime.Now;
                                _reminderletter.LetterType = search;
                                _reminderletter.SaleID = SaleID;
                                _reminderletter.duedate = datef;
                                _reminderletter.DueAmount = TotalInsAmount;
                                dbContext.ReminderLetters.Add(_reminderletter);
                                dbContext.SaveChanges();
                            }
                        }
                    }
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject("");
            }
            catch (Exception ex)
            {
                Failure = "Invalid search query, please try again.";
                Helper h = new Helper();
                h.LogException(ex);
                return Newtonsoft.Json.JsonConvert.SerializeObject("");
            }
        }

        public string DemandLettertPrintReport(string transactionid)
        {
            try
            {
                string duedate = "";
                //DateTime LetterDate = Convert.ToDateTime(duedate);
                string[] saleids = transactionid.Split(',');
                List<ReminderLetterModel> model = new List<ReminderLetterModel>();
                foreach (string saleid in saleids)
                {
                    if (saleid != "")
                    {
                        int sid = Convert.ToInt32(saleid);
                        var md1 = (from sale in dbContext.SaleFlats
                                   join f in dbContext.Flats on sale.FlatID equals f.FlatID
                                   join c in dbContext.Customers on sale.SaleID equals c.SaleID
                                   join d in dbContext.ReminderLetters on sale.SaleID equals d.SaleID
                                   join p in dbContext.Projects on sale.ProjectID equals p.ProjectID
                                   where d.SaleID == sid
                                   select new { saleID = sale.SaleID, FlatName = f.FlatName, CustomrName = c.AppTitle + " " + c.FName + " " + c.MName + " " + c.LName, MobileNo = c.MobileNo, DueDate = d.duedate, LetterDate = d.CreateDate, DueAmount = d.DueAmount, ProjectName = p.PName, CompanyName = p.CompanyName, PropertyAddress = p.Address });
                        foreach (var v in md1)
                        {
                            string duedt = "", Ldate1 = "", LDate2 = "", LDate3 = "";
                            if (v.DueDate != null)
                                duedt = v.DueDate.Value.ToString("dd/MM/yyyy");
                            if (v.LetterDate != null)
                                Ldate1 = v.LetterDate.Value.ToString("dd/MM/yyyy");
                            model.Add(new ReminderLetterModel { SaleID = v.saleID, CompanyName = v.CompanyName, CustomerName = v.CustomrName, InterestRate = "18", LetterDateSt = Ldate1, LetterType = "DemandLetter1", ProjectName = v.ProjectName, PropertyAddress = v.PropertyAddress, FlatName = v.FlatName, DueDateST = duedt, DueAmount = v.DueAmount });
                        }
                    }
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
            }
            catch (Exception ex)
            {
                Failure = "Invalid search query, please try again.";
                Helper h = new Helper();
                h.LogException(ex);
                return Newtonsoft.Json.JsonConvert.SerializeObject("");
            }
        }
        public string DemandLettertPrintReport3(string transactionid)
        {
            try
            {
                string duedate = "";
                int saleid = Convert.ToInt32(transactionid);
                List<FlatSaleModel> model = new List<FlatSaleModel>();
                var VALUE1 = (from ins in dbContext.FlatInstallmentDetails
                              where ins.SaleID == saleid
                              select new { sale = ins.SaleID, amount = ins.TotalAmount, duedate = ins.DueDate }).ToList();
                decimal TotalInsAmount = 0;
                foreach (var amount in VALUE1)
                {
                    TotalInsAmount = TotalInsAmount + Convert.ToDecimal(amount.amount);
                    duedate = Convert.ToString(amount.duedate);
                }
                var md1 = (from sale in dbContext.SaleFlats
                           join f in dbContext.Flats on sale.FlatID equals f.FlatID
                           join c in dbContext.Customers on sale.SaleID equals c.SaleID
                           where  sale.SaleID == saleid
                           select new { saleID = sale.SaleID, FlatName = f.FlatName, MobileNo = c.MobileNo, sale = sale });
                foreach (var v in md1)
                {
                    decimal paidamount = 0;
                    var mdPaid = (from pay in dbContext.Payments where pay.SaleID == v.saleID select new { paidamount = pay.Amount });
                    foreach (var MdpaidAdmount in mdPaid)
                    {
                        paidamount = paidamount + Convert.ToDecimal(MdpaidAdmount.paidamount);
                    }
                    string bdate = "";
                    if (v.sale.SaleDate != null)
                        if ((TotalInsAmount - paidamount) > 0)
                        {
                            string secondDate = "";
                            string fristDate = "";
                        }
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
            }
            catch (Exception ex)
            {
                Failure = "Invalid search query, please try again.";
                Helper h = new Helper();
                h.LogException(ex);
                return Newtonsoft.Json.JsonConvert.SerializeObject("");
            }
        }


        public string DemandLettertPrintReport2(string transactionid)
        {
            try
            {
                string duedate = "";
                int saleid = Convert.ToInt32(transactionid);

                List<FlatSaleModel> model = new List<FlatSaleModel>();

                var VALUE1 = (from ins in dbContext.FlatInstallmentDetails
                              where ins.SaleID == saleid
                              select new { sale = ins.SaleID, amount = ins.TotalAmount, duedate = ins.DueDate }).ToList();
                decimal TotalInsAmount = 0;
                foreach (var amount in VALUE1)
                {
                    TotalInsAmount = TotalInsAmount + Convert.ToDecimal(amount.amount);
                    duedate = Convert.ToString(amount.duedate);
                }

                var md1 = (from sale in dbContext.SaleFlats
                           join f in dbContext.Flats on sale.FlatID equals f.FlatID
                           join c in dbContext.Customers on sale.SaleID equals c.SaleID
                           where  sale.SaleID == saleid
                           select new { saleID = sale.SaleID, FlatName = f.FlatName, MobileNo = c.MobileNo, sale = sale });

                foreach (var v in md1)
                {
                    decimal paidamount = 0;
                    var mdPaid = (from pay in dbContext.Payments where pay.SaleID == v.saleID select new { paidamount = pay.Amount });

                    foreach (var MdpaidAdmount in mdPaid)
                    {
                        paidamount = paidamount + Convert.ToDecimal(MdpaidAdmount.paidamount);
                    }
                    string bdate = "";
                    if (v.sale.SaleDate != null)
                        if ((TotalInsAmount - paidamount) > 0)
                        {
                        }
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
            }
            catch (Exception ex)
            {
                Failure = "Invalid search query, please try again.";
                Helper h = new Helper();
                h.LogException(ex);
                return Newtonsoft.Json.JsonConvert.SerializeObject("");
            }
        }

        #region ViewdemandReport


        public string ViewSearchDemandLetter(string search, string propertyid, string datefrom)
        {
            try
            {
                int Propertyid = Convert.ToInt32(propertyid);
                DateTime datef = new DateTime();
                datef = Convert.ToDateTime(datefrom);

                DateTime searchDatePlusOne = datef.AddDays(-1);
                datef = datef.AddDays(1);


                List<FlatDemandLetter> model = new List<FlatDemandLetter>();
                DataFunctions DF = new DataFunctions();
                DataTable ds = DF.GetDataTable("SELECT * FROM SaleFlat INNER JOIN Flat ON SaleFlat.FlatID = Flat.FlatID INNER JOIN Customer ON SaleFlat.SaleID = Customer.SaleID INNER JOIN ReminderLetter ON SaleFlat.SaleID = ReminderLetter.SaleID where SaleFlat.ProjectID='" + Propertyid + "' and ReminderLetter.LetterType='" + search + "' and ReminderLetter.CreateDate<='" + datef + "'  ");
                foreach (DataRow row in ds.Rows)
                {
                    FlatDemandLetter dm = new FlatDemandLetter();
                    dm.ID = Convert.ToInt32(row["ID"]);
                    dm.SaleID = Convert.ToInt32(row["SaleID"]);
                    dm.DueAmount = Convert.ToDecimal(row["DueAmount"]);
                    dm.FlatName = row["FlatName"].ToString();
                    dm.FName = row["FName"].ToString();
                    dm.MobileNo = row["MobileNo"].ToString();
                    dm.SaleRate = Convert.ToDecimal(row["SaleRate"]);
                    model.Add(dm);
                }
                return Newtonsoft.Json.JsonConvert.SerializeObject(model);
            }
            catch (Exception ex)
            {
                Failure = "Invalid search query, please try again.";
                Helper h = new Helper();
                h.LogException(ex);
                return Newtonsoft.Json.JsonConvert.SerializeObject("");
            }
        }




        #endregion




        #region Search Services
        public ActionResult PrintReport()
        {
            return View();
        }

        public string MailReport(string ReportContent, string emailid)
        {
            string filename = "ReportExport.xls";
            System.IO.File.WriteAllText(Server.MapPath("~/PDF/Temp/" + filename), ReportContent);
            //  string tfile = ExportGrid(transids);
            SendMail sm = new SendMail();
            sm.BackupReceiptMailDataFile("Report from SBP Groups", "", emailid, filename);
            return "/PDF/Temp/" + filename;
        }
        public string ExportReport(string ReportContent)
        {
            string filename = "ReportExport.xls";
            System.IO.File.WriteAllText(Server.MapPath("~/PDF/Temp/" + filename), ReportContent);
            return "/PDF/Temp/" + filename;
        }
        #endregion
    }
}