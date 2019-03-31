using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using EntityLayer;

namespace BusinessLayer
{
    public class ConsumableBatchOpsBL
    {
        public static void AddNewConsumableBatch(ConsumableBatch consumableBatch)
        {
            ConsumableBatchOpsDAL.AddNewConsumableBatch(consumableBatch);
        }

        public static List<ConsumableBatch> GetAllConsumableBatchs()
        {
            return ConsumableBatchOpsDAL.GetAllConsumableBatchs();
        }

        public static ConsumableBatch GetConsumableBatchById(string modelNumber, string shipmentPoNumber)
        {
            try
            {
                return ConsumableBatchOpsDAL.GetConsumableBatchById(modelNumber, shipmentPoNumber);
            }
            catch (RecordNotFoundException e)
            {
                throw;
            }
        }

        public static void UpdateConsumableBatch(ConsumableBatch consumableBatch)
        {
            ConsumableBatchOpsDAL.UpdateConsumableBatch(consumableBatch);
        }

        public static void DeleteConsumableBatch(ConsumableBatch consumableBatch)
        {
            ConsumableBatchOpsDAL.DeleteConsumableBatch(consumableBatch);
        }

        public static List<ConsumableBatch> GetConsumableBatchesByShipment(string shipmentPoNumber)
        {
            return ConsumableBatchOpsDAL.GetConsumableBatchesByShipment(shipmentPoNumber); 
        }

        public static List<ConsumableBatch> GetAvailableConsumableBatches()
        {
            return ConsumableBatchOpsDAL.GetAvaialbleConsumableBatchs();
        }

    }
}
