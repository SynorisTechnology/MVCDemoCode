using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMS.Data.Access.Admin
{
    public interface IPLCService
    {
        int AddPLC(PLCModel model);
        int EditPLC(PLCModel model);
        int DeletePLC(PLCModel model);
        List<PLCModel> GetAllPLC();
        PLCModel GetPLCbyID(int plcID);
        int GetPLCCount();

    }
    public class PLCService : IPLCService
    {
        public int AddPLC(PLCModel model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    Mapper.CreateMap<PLCModel, PLC>();
                    var mdl = Mapper.Map<PLCModel, PLC>(model);
                    context.PLCs.Add(mdl);
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
        public int EditPLC(PLCModel model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    Mapper.CreateMap<PLCModel, PLC>();
                    var mdl = Mapper.Map<PLCModel, PLC>(model);
                    context.PLCs.Add(mdl);
                    context.Entry(mdl).State = EntityState.Modified;
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
        public int DeletePLC(PLCModel model)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                try
                {
                    Mapper.CreateMap<PLCModel, PLC>();
                    var mdl = Mapper.Map<PLCModel, PLC>(model);
                    context.PLCs.Add(mdl);
                    context.Entry(mdl).State = EntityState.Deleted;
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
        public List<PLCModel> GetAllPLC()
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                var model = context.PLCs.ToList();
                List<PLCModel> pmodel = new List<PLCModel>();
                foreach (var md in model)
                {
                    Mapper.CreateMap<PLC, PLCModel>();
                    var plc = Mapper.Map<PLC, PLCModel>(md);
                    pmodel.Add(plc);
                }
                return pmodel;
            }
        }
        public PLCModel GetPLCbyID(int plcID)
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                var model = context.PLCs.Where(pl=>pl.PLCID==plcID).FirstOrDefault();
                Mapper.CreateMap<PLC, PLCModel>();
                var plc = Mapper.Map<PLC, PLCModel>(model);
                return plc;
            }
        }
        public int GetPLCCount()
        {
            using (REMSDBEntities context = new REMSDBEntities())
            {
                var model = context.PLCs.ToList().Count();
                return model;
            }
        }
    }
}
