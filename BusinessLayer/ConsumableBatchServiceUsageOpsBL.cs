using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using EntityLayer;

namespace BusinessLayer
{
    public class ConsumableBatchServiceUsageOpsBL
    {
        public static void AddNewConsumableBatchServiceUsage(ConsumableBatchServiceUsage consumableBatchServiceUsage)
        {
            ConsumableBatchServiceUsageOpsDAL.AddNewConsumableBatchServiceUsage(consumableBatchServiceUsage);
        }

        public static List<ConsumableBatchServiceUsage> GetAllConsumableBatchServiceUsages()
        {
            return ConsumableBatchServiceUsageOpsDAL.GetAllConsumableBatchServiceUsages();
        }

        public static ConsumableBatchServiceUsage GetConsumableBatchServiceUsageById(string shipmentPoNumber,
            string modelNumber, int serviceId)
        {
            try
            {
                return ConsumableBatchServiceUsageOpsDAL.GetConsumableBatchServiceUsageById(shipmentPoNumber,
                    modelNumber, serviceId);
            }
            catch (RecordNotFoundException e)
            {
                throw;
            }
        }

        public static void UpdateConsumableBatchServiceUsage(ConsumableBatchServiceUsage consumableBatchServiceUsage)
        {
            ConsumableBatchServiceUsageOpsDAL.UpdateConsumableBatchServiceUsage(consumableBatchServiceUsage);
        }

        public static void DeleteConsumableBatchServiceUsage(ConsumableBatchServiceUsage consumableBatchServiceUsage)
        {
            ConsumableBatchServiceUsageOpsDAL.DeleteConsumableBatchServiceUsage(consumableBatchServiceUsage);
        }

        public static List<ConsumableBatchServiceUsage> GetConsumableBatchServiceUsagesByServiceId(int serviceId)
        {
            return ConsumableBatchServiceUsageOpsDAL.GetConsumableBatchServiceUsagesByServiceId(serviceId);
        }

        public static List<ConsumableBatchServiceUsage> GetConsumableBatchServiceUsagesByBatch(string shipmentPoNumber,
            string modelNumber)
        {
            return ConsumableBatchServiceUsageOpsDAL.GetConsumableBatchServiceUsagesByBatch(shipmentPoNumber,
                modelNumber);
        }

        public static int GetConsumableBatchServiceUsageCountForBatch(string shipmentNo, string modelNo)
        {
            int usage = 0;
            foreach (var consumableBatchServiceUsage in GetConsumableBatchServiceUsagesByBatch(shipmentNo, modelNo))
            {
                usage += consumableBatchServiceUsage.QuantityUsed;
            }

            return usage;
        }

        public static int GetConsumableBatchAvailability(string shipmentNo, string modelNo)
        {
            var usageCount = GetConsumableBatchServiceUsageCountForBatch(shipmentNo, modelNo);
            var consumableBatchById = ConsumableBatchOpsBL.GetConsumableBatchById(modelNo, shipmentNo);

            return consumableBatchById.Quantity - usageCount;
        }

        public static List<ConsumableBatchServiceUsage> GetRemovableConsumableServiceUsagesByEquipment(
            int currentServiceId,
            string equipmentItemSerialNumber)
        {
            return ConsumableBatchServiceUsageOpsDAL.GetRemovableConsumableServiceUsagesByEquipment(currentServiceId,
                equipmentItemSerialNumber);
        }

        public static List<ConsumableBatchServiceUsage> GetConsumablesRemovedByServiceId(int serviceId)
        {
            return ConsumableBatchServiceUsageOpsDAL.GetConsumablesRemovedByServiceId(serviceId);
        }

        public static List<ConsumableBatchServiceUsage> GetConsumableServiceUsagesByEquipment(
            string equipmentItemSerialNumber)
        {
            return ConsumableBatchServiceUsageOpsDAL.GetConsumableServiceUsagesByEquipment(equipmentItemSerialNumber);
        }

        public static List<ConsumableBatchServiceUsage> GetConsumableServiceUsagesByModel(
            string consumableModelNumber)
        {
            return ConsumableBatchServiceUsageOpsDAL.GetConsumableServiceUsagesByModel(consumableModelNumber);
        }
    }
}