using AutoMapper;
using REMS.Data.CustomModel;
using REMS.Data.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.Access.Admin
{
    public interface IFlatService
    {
        int AddFloor(int towerid, int florno, string username);
        int AddFlats(int TotalFloor, int FlatOrder, List<int> FlatNo, List<bool> PreIncrement, List<PLCModel> pmodel, string username, int towerID, string FltType, string FltTSize);
        string FlatCreateBody();
        int AddNewFlat(Flat flat);
        int EditFlat(Flat flat);
        int DeleteFlat(int flatid);
        FlatModel GetFlatByID(int flatid);
        List<FlatModel> GetFlatsByFloorID(int floorid);
        bool CheckFlatNo(string flatno, int towerID);
        int UpdateFlatTypeAllTowerPerFloor(UpdateFlatTypeModel model);
        int UpdateFlatTypePerTowerAllFloor(UpdateFlatTypeModel model);
        int UpdateFlatTypePerTowerPerFloor(UpdateFlatTypeModel model);

        FlatDetailModel GetFlatDetails(int FlatID);
    }
    public class FlatService : IFlatService
    {
        public int AddFloor(int towerID, int florno, string username)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    for (int i = 1; i <= florno; i++)
                    {
                        //Floor fl = new Floor();
                        //fl.CrBy = username; fl.CrDate = DateTime.Now;
                        //fl.TowerID = towerID;
                        //fl.FloorName = i.ToString();
                        //fl.FloorNo = i;
                        //context.Floors.Add(fl);
                        //context.SaveChanges();
                    }
                    return 1;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }

        }
        public int AddFlats(int TotalFloor, int FlatOrder, List<int> FlatNo, List<bool> PreIncrement, List<PLCModel> pmodel, string username, int towerID, string FltType, string FltTSize)
        {
            PLCService pservice = new PLCService();
            int plcCount = pservice.GetPLCCount();
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {

                    for (int i = 1; i <= TotalFloor; i++)
                    {
                        int kk = 0;
                        Floor flo = new Floor();
                        flo.CrBy = username; flo.CrDate = DateTime.Now;
                        flo.TowerID = towerID;
                        flo.FloorName = i.ToString();
                        flo.FloorNo = i;
                        context.Floors.Add(flo);
                        context.SaveChanges();
                        int jj = 0;
                        foreach (int flat in FlatNo)
                        {
                            int fno = GenFlatNo(i, flat, PreIncrement[jj]);
                            Flat fl = new Flat();
                            fl.FlatOrder = FlatOrder;
                            fl.FlatNo = fno.ToString();
                            fl.FlatName = fno.ToString();
                            fl.FloorID = flo.FloorID;
                            fl.FlatType = FltType;
                            fl.FlatSizeUnit = "SqFt";
                            fl.FlatSize = Convert.ToDecimal(FltTSize);
                            fl.Status = "Available";
                            context.Flats.Add(fl);
                            context.SaveChanges();

                            for (int k = 0; k <= plcCount - 1; k++)
                            {
                                var pl = pmodel[kk];
                                if (pl.PLCID != 0)
                                {
                                    FlatPLC fpcl = new FlatPLC();
                                    fpcl.CrDate = DateTime.Now;
                                    fpcl.FlatID = fl.FlatID;
                                    fpcl.PLCID = pl.PLCID;
                                    context.FlatPLCs.Add(fpcl);
                                    context.SaveChanges();
                                }
                                kk++;
                            }
                            jj++;
                        }

                    }
                    return 1;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }
        private int GenFlatNo(int floorno, int flatno, bool increment)
        {
            int pre = Convert.ToInt32(flatno.ToString().Substring(0, 1));
            int preRem = Convert.ToInt32(flatno.ToString().Substring(1, flatno.ToString().Length - 1));

            int post = Convert.ToInt32(flatno.ToString().Substring(flatno.ToString().Length - 1, 1));
            int postRem = Convert.ToInt32(flatno.ToString().Substring(0, flatno.ToString().Length - 1));

            if (increment)
            {
                return Convert.ToInt32((pre + floorno - 1).ToString() + "" + preRem);
            }
            else
            {
                return Convert.ToInt32(postRem.ToString() + "" + (post + floorno - 1).ToString());
            }
        }
        public string FlatCreateBody()
        {
            String htmlText = @"  
                <div class='col col-md-3'>
                    <label class='input'>
                        <input type='text' name='FlatNo' id='<% FlatNo %>' placeholder='First Floor Flat No'>
                    </label><br />
                    <label class='checkbox'>
                        <input type='checkbox' name='PreIncrement' id='<% PreIncrement %>' checked='checked'>
                        <i></i>Pre-Increment
                    </label>
                    <b>PLC Options</b><br />";
            return htmlText;
        }

        public int AddNewFlat(Flat flat)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.Flats.Add(flat);
                    int i = context.SaveChanges();
                    return i;
                }
                catch (Exception ex)
                {
                    Helper hp = new Helper();
                    hp.LogException(ex);
                    return 0;
                }
            }
        }

        public int EditFlat(Flat flat)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    context.Flats.Add(flat);
                    context.Entry(flat).State = EntityState.Modified;
                    int i = context.SaveChanges();
                    return i;
                }
                catch (Exception ex)
                {
                    Helper hp = new Helper();
                    hp.LogException(ex);
                    return 0;
                }
            }
        }
        public int DeleteFlat(int flatid)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    FlatPLCService fpservice = new FlatPLCService();
                    FlatChargeService fcservice = new FlatChargeService();
                    var model = fpservice.GetFlatPLCListByFlatID(flatid);
                    foreach (var md in model)
                    {
                        FlatPLC plc = new FlatPLC();
                        plc.FlatPLCID = md.FlatPLCID;
                        context.FlatPLCs.Add(plc);
                        context.Entry(plc).State = EntityState.Deleted;
                        context.SaveChanges();
                    }
                    var fmodel = fcservice.GetFlatChargeListByFlatID(flatid);
                    foreach (var md in fmodel)
                    {
                        FlatCharge plc = new FlatCharge();
                        plc.FlatChargeID = md.FlatChargeID;
                        context.FlatCharges.Add(plc);
                        context.Entry(plc).State = EntityState.Deleted;
                        context.SaveChanges();
                    }
                    Flat ft = new Flat();
                    ft.FlatID = flatid;
                    context.Flats.Add(ft);
                    context.Entry(ft).State = EntityState.Deleted;
                    int i = context.SaveChanges();
                    return i;
                }
                catch (Exception ex)
                {
                    Helper hp = new Helper();
                    hp.LogException(ex);
                    return 0;
                }
            }
        }

        public FlatModel GetFlatByID(int flatid)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.Flats.Where(fl => fl.FlatID == flatid).FirstOrDefault();
            Mapper.CreateMap<Flat, FlatModel>();
            var md = Mapper.Map<Flat, FlatModel>(model);
            return md;
        }
        public List<FlatModel> GetFlatsByFloorID(int floorid)
        {
            REMSDBEntities context = new REMSDBEntities();
            var model = context.Flats.Where(fl => fl.FloorID == floorid).ToList();
            Mapper.CreateMap<Flat, FlatModel>();
            var md = Mapper.Map<List<Flat>, List<FlatModel>>(model);
            return md;
        }
        public bool CheckFlatNo(string flatno, int towerID)
        {
            return false;
        }

        public int UpdateFlatTypeAllTowerPerFloor(UpdateFlatTypeModel model)
        {
            TowerService ts = new TowerService();
            FloorService fs = new FloorService();

            var alltower = ts.AllTower();
            foreach (Tower tw in alltower)
            {
                Floor fd = fs.GetFloorByFloorNo(tw.TowerID, model.FloorID);
                var flats = GetFlatsByFloorID(fd.FloorID);
                foreach (var flm in flats)
                {
                    var flat = GetFlatByID(flm.FlatID);
                    flat.FlatSize = model.FlatSize;
                    flat.FlatType = model.FlatType;
                    Mapper.CreateMap<FlatModel, Flat>().ForMember(dest => dest.FlatPLCs, gest => gest.Ignore());
                    var flt = Mapper.Map<FlatModel, Flat>(flat);
                    EditFlat(flt);
                }
            }
            return 1;
        }
        public int UpdateFlatTypePerTowerAllFloor(UpdateFlatTypeModel model)
        {
            TowerService ts = new TowerService();
            FloorService fs = new FloorService();

            var allfloor = fs.AllFloor(model.TowerID);
            foreach (var tw in allfloor)
            {
                var flats = GetFlatsByFloorID(tw.FloorID);
                foreach (var flm in flats)
                {
                    var flat = GetFlatByID(flm.FlatID);
                    flat.FlatSize = model.FlatSize;
                    flat.FlatType = model.FlatType;
                    Mapper.CreateMap<FlatModel, Flat>().ForMember(dest => dest.FlatPLCs, gest => gest.Ignore());
                    var flt = Mapper.Map<FlatModel, Flat>(flat);
                    EditFlat(flt);
                }
            }
            return 1;
        }
        public int UpdateFlatTypePerTowerPerFloor(UpdateFlatTypeModel model)
        {
            var flats = GetFlatsByFloorID(model.FloorID);
            foreach (var flm in flats)
            {
                var flat = GetFlatByID(flm.FlatID);
                flat.FlatSize = model.FlatSize;
                flat.FlatType = model.FlatType;
                Mapper.CreateMap<FlatModel, Flat>().ForMember(dest => dest.FlatPLCs, gest => gest.Ignore());
                var flt = Mapper.Map<FlatModel, Flat>(flat);
                EditFlat(flt);
            }
            return 1;
        }

        //public FlatDetailModel GetFlatDetails(int FlatID)
        //{
        //    REMSDBEntities context = new REMSDBEntities();
        //    var flmodel = context.Flats.Where(fl => fl.FlatID == FlatID).FirstOrDefault();
        //    Mapper.CreateMap<Flat, FlatDetailModel>();
        //    FlatDetailModel model = new FlatDetailModel();
        //    model = Mapper.Map<Flat, FlatDetailModel>(flmodel);
        //    var flor = context.Floors.Where(fl => fl.FloorID == model.FloorID).FirstOrDefault();
        //    model.FloorName = flor.FloorName;
        //    model.FloorNo = flor.FloorNo;
        //    var twr = context.Towers.Where(tw => tw.TowerID == flor.TowerID).FirstOrDefault();
        //    model.TowerID = twr.TowerID;
        //    model.TowerName = twr.TowerName;
        //    model.TowerNo = twr.TowerNo;
        //    model.Block = twr.Block;
        //    var fmodel = context.FlatPLCs.Where(pl => pl.FlatID == FlatID).ToList();
        //    List<FlatPLCModel> FlatPLCList = new List<FlatPLCModel>();
        //    foreach (FlatPLC fp in fmodel)
        //    {
        //        Mapper.CreateMap<FlatPLC, FlatPLCModel>();
        //        var fpm = Mapper.Map<FlatPLC, FlatPLCModel>(fp);
        //        fpm.PLCName = fp.PLC.PLCName;
        //        fpm.AmountSqFt = fp.PLC.AmountSqFt;
        //        fpm.TotalAmount = fp.PLC.AmountSqFt * model.FlatSize;
        //        FlatPLCList.Add(fpm);
        //    }
        //    model.FlatPLCList = FlatPLCList;
        //    var amodel = context.FlatCharges.Where(fc => fc.FlatID == FlatID).ToList();
        //    List<FlatChargeModel> FlatChargeList = new List<FlatChargeModel>();
        //    foreach (var ac in amodel)
        //    {
        //        Mapper.CreateMap<FlatCharge, FlatChargeModel>();
        //        var acmodel = Mapper.Map<FlatCharge, FlatChargeModel>(ac);
        //        acmodel.ChargeName = ac.AdditionalCharge.Name;
        //        acmodel.Amount = ac.AdditionalCharge.Amount;
        //        acmodel.ChargeType = ac.AdditionalCharge.ChargeType;
        //        if (acmodel.ChargeType == "Free")
        //        {
        //            acmodel.TotalAmount = 0;
        //        }
        //        else if (acmodel.ChargeType == "Sq. Ft.")
        //        {
        //            acmodel.TotalAmount = ac.AdditionalCharge.Amount * model.FlatSize;
        //        }
        //        else if (acmodel.ChargeType == "One Time")
        //        {
        //            acmodel.TotalAmount = ac.AdditionalCharge.Amount;
        //        }
        //        FlatChargeList.Add(acmodel);
        //    }
        //    model.FlatChargeList = FlatChargeList;
        //    return model;
        //}

        public FlatDetailModel GetFlatDetails(int FlatID)
        {
            REMSDBEntities context = new REMSDBEntities();
            var flmodel = context.Flats.Where(fl => fl.FlatID == FlatID).FirstOrDefault();
            Mapper.CreateMap<Flat, FlatDetailModel>();
            FlatDetailModel model = new FlatDetailModel();
            model = Mapper.Map<Flat, FlatDetailModel>(flmodel);
            var flor = context.Floors.Where(fl => fl.FloorID == model.FloorID).FirstOrDefault();
            model.FloorName = flor.FloorName;
            model.FloorNo = flor.FloorNo;
            var twr = context.Towers.Where(tw => tw.TowerID == flor.TowerID).FirstOrDefault();
            model.TowerID = twr.TowerID;
            model.TowerName = twr.TowerName;
            model.TowerNo = twr.TowerNo;
            model.Block = twr.Block;
            var fmodel = context.FlatPLCs.Where(pl => pl.FlatID == FlatID).ToList();
            List<FlatPLCModel> FlatPLCList = new List<FlatPLCModel>();
            foreach (FlatPLC fp in fmodel)
            {
                Mapper.CreateMap<FlatPLC, FlatPLCModel>();
                var fpm = Mapper.Map<FlatPLC, FlatPLCModel>(fp);
                fpm.PLCName = fp.PLC.PLCName;
                fpm.AmountSqFt = fp.PLC.AmountSqFt;
                fpm.TotalAmount = fp.PLC.AmountSqFt * model.FlatSize;
                FlatPLCList.Add(fpm);
            }
            model.FlatPLCList = FlatPLCList;
            var amodel = context.FlatCharges.Where(fc => fc.FlatID == FlatID).ToList();
            List<FlatChargeModel> FlatChargeList = new List<FlatChargeModel>();
            foreach (var ac in amodel)
            {
                Mapper.CreateMap<FlatCharge, FlatChargeModel>();
                var acmodel = Mapper.Map<FlatCharge, FlatChargeModel>(ac);
                acmodel.ChargeName = ac.AdditionalCharge.Name;
                acmodel.Amount = ac.AdditionalCharge.Amount;
                acmodel.ChargeType = ac.AdditionalCharge.ChargeType;
                if (acmodel.ChargeType == "Free")
                {
                    acmodel.TotalAmount = 0;
                }
                else if (acmodel.ChargeType == "Sq. Ft.")
                {
                    acmodel.TotalAmount = ac.AdditionalCharge.Amount * model.FlatSize;
                }
                else if (acmodel.ChargeType == "One Time")
                {
                    acmodel.TotalAmount = ac.AdditionalCharge.Amount;
                }
                FlatChargeList.Add(acmodel);
            }
            model.FlatPlanCharge= context.Rem_GetFlatPlanCharge(FlatID).ToList();
            model.FloorWisePlc = context.PLCs.Where(p => p.FloorNo == model.FloorNo).ToList();
            model.FlatChargeList = FlatChargeList;
            List<AdditionalCharge> AdditionalCharge = new List<AdditionalCharge>();
            var q = context.AdditionalCharges.ToList();
            for (int i = 0; i < q.Count - 1; i++)
            {
                AdditionalCharge additionalCharge = new AdditionalCharge();
                additionalCharge.ChargeType = q[i].ChargeType;
                additionalCharge.Amount = q[i].Amount;
                additionalCharge.Name = q[i].Name;
                if (additionalCharge.ChargeType == "Free")
                {
                    additionalCharge.Amount = 0;
                }
                else if (additionalCharge.ChargeType == "Sq. Ft.")
                {
                    additionalCharge.Amount = q[i].Amount * model.FlatSize;
                }
                else if (additionalCharge.ChargeType == "One Time")
                {
                    additionalCharge.Amount = q[i].Amount;
                }
                AdditionalCharge.Add(additionalCharge);

            }
            model.AdditionalCharge = AdditionalCharge;
            return model;
        }
    }

}