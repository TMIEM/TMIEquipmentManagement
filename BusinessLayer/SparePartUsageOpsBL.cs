using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using EntityLayer;

namespace BusinessLayer
{
    public class SparePartUsageOpsBL
    {
        public static void AddNewSparePartUsage(SparePartUsage sparePartUsage)
        {
            SparePartUsageOpsDAL.AddNewSparePartUsage(sparePartUsage);
        }

        public static List<SparePartUsage> GetAllSparePartUsages()
        {
            return SparePartUsageOpsDAL.GetAllSparePartUsages();
        }

        public static SparePartUsage GetSparePartUsageBySerialNumber(string sparePartUsageSerialNumber)
        {
            try
            {
                return SparePartUsageOpsDAL.GetSparePartUsageBySerialNumber(sparePartUsageSerialNumber);
            }
            catch (RecordNotFoundException e)
            {
                throw;
            }
        }

        public static void UpdateSparePartUsage(SparePartUsage sparePartUsage)
        {
            SparePartUsageOpsDAL.UpdateSparePartUsage(sparePartUsage);
        }

        public static void DeleteSparePartUsage(SparePartUsage sparePartUsage)
        {
            SparePartUsageOpsDAL.DeleteSparePartUsage(sparePartUsage);
        }

        public static List<SparePartUsage> GetSparePartUsagesByServiceId(int serviceId)
        {
            return SparePartUsageOpsDAL.GetSparePartUsagesByServiceId(serviceId);
        }

        public static List<SparePartUsage> GetRemovableSparePartUsages(int currentServiceId, string equipmentItemSerialNumber)
        {
            return SparePartUsageOpsDAL.GetRemovableSparePartsByEquipment(currentServiceId, equipmentItemSerialNumber);
        }

        public static List<SparePartUsage> GetSparePartsRemovedByServiceId(int serviceId)
        {
            return SparePartUsageOpsDAL.GetSparePartsRemovedByServiceId(serviceId);
        }

        public static List<SparePartUsage> GetSparePartUsagesByEquipment(string equipmentItemSerialNumber)
        {
            return SparePartUsageOpsDAL.GetSparePartUsagesByEquipment(equipmentItemSerialNumber);
        }
    }
}
