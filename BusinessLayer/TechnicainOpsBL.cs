using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using EntityLayer;

namespace BusinessLayer
{
    public class TechnicianOpsBL
    {
        public static void AddNewTechnician(Technician technician)
        {
            TechnicianOpsDAL.AddNewTechnician(technician);
        }

        public static List<Technician> GetAllTechnicians()
        {
            return TechnicianOpsDAL.GetAllTechnicians();
        }

        public static Technician GetTechnicianById(int technicianId)
        {
            try
            {
                return TechnicianOpsDAL.GetTechnicianById(technicianId);
            }
            catch (RecordNotFoundException e)
            {
                throw;
            }
        }

        public static void UpdateTechnician(Technician technician)
        {
            TechnicianOpsDAL.UpdateTechnician(technician);
        }
    }
}
