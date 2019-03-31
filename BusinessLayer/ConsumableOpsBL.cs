using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using EntityLayer;

namespace BusinessLayer
{
    public class ConsumableOpsBL
    {
        public static void AddNewConsumable(Consumable consumable)
        {
            ConsumableOpsDAL.AddNewConsumable(consumable);
        }

        public static List<Consumable> GetAllConsumables()
        {
            return ConsumableOpsDAL.GetAllConsumables();
        }

        public static Consumable GetConsumableByModelNumber(string modelNumber)
        {
            try
            {
                return ConsumableOpsDAL.GetConsumableByModelNumber(modelNumber);
            }
            catch (RecordNotFoundException e)
            {
                throw;
            }
        }

        public static void UpdateConsumable(Consumable consumable)
        {
            ConsumableOpsDAL.UpdateConsumable(consumable);
        }

        public static void DeleteConsumable(Consumable consumable)
        {
            ConsumableOpsDAL.DeleteConsumable(consumable);
        }
    }
}
