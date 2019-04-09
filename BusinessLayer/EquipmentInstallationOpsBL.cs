using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using EntityLayer;

namespace BusinessLayer
{
    public class EquipmentInstallationOpsBL
    {
        public static void AddNewEquipmentInstallation(EquipmentInstallation equipmentInstallation)
        {
            EquipmentInstallationOpsDAL.AddNewEquipmentInstallation(equipmentInstallation);
        }

        public static List<EquipmentInstallation> GetAllEquipmentInstallations()
        {
            return EquipmentInstallationOpsDAL.GetAllEquipmentInstallations();
        }

        
        public static EquipmentInstallation GetEquipmentInstallationBySerial(string equipmentInstallationSerial)
        {
            try
            {
                return EquipmentInstallationOpsDAL.GetEquipmentInstallationBySerial(equipmentInstallationSerial);
            }
            catch (RecordNotFoundException e)
            {
                throw;
            }
        }

        public static void UpdateEquipmentInstallation(EquipmentInstallation equipmentInstallation)
        {
            EquipmentInstallationOpsDAL.UpdateEquipmentInstallation(equipmentInstallation);
        }

        public static List<EquipmentInstallation> EagerLoad(List<EquipmentInstallation> equipmentInstallations)
        {
            return EquipmentInstallationOpsDAL.EagerLoad(equipmentInstallations);
        }

        public static EquipmentInstallation EagerLoad(EquipmentInstallation equipmentInstallation)
        {
            return EquipmentInstallationOpsDAL.EagerLoad(equipmentInstallation);
        }

        public static List<EquipmentInstallation> GetAllEquipmentInstallationsForCustomer(int customerId)
        {
            return EquipmentInstallationOpsDAL.GetAllEquipmentInstallationsForCustomer(customerId);
        }

        public static void UpdateEquipmentCommissioningDate(EquipmentInstallation equipmentInstallation)
        {
            EquipmentInstallationOpsDAL.UpdateEquipmentCommissioningDate(equipmentInstallation);
        }
    }
}
