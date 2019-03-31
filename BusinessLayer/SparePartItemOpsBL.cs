using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using EntityLayer;

namespace BusinessLayer
{
    public class SparePartItemOpsBL
    {
        public static void AddNewSparePartItem(SparePartItem sparePartItem)
        {
            SparePartItemOpsDAL.AddNewSparePartItem(sparePartItem);
        }

        public static List<SparePartItem> GetAllSparePartItems()
        {
            return SparePartItemOpsDAL.GetAllSparePartItems();
        }

        public static SparePartItem GetSparePartItemBySerialNumber(string sparePartItemSerialNumber)
        {
            try
            {
                return SparePartItemOpsDAL.GetSparePartItemBySerialNumber(sparePartItemSerialNumber);
            }
            catch (RecordNotFoundException e)
            {
                throw;
            }
        }

        public static void UpdateSparePartItem(SparePartItem sparePartItem)
        {
            SparePartItemOpsDAL.UpdateSparePartItem(sparePartItem);
        }

        public static void DeleteSparePartItem(SparePartItem sparePartItem)
        {
            SparePartItemOpsDAL.DeleteSparePartItem(sparePartItem);
        }

        public static List<SparePartItem> GetSparePartItemsByShipmentPoNumber(string shipmentPoNumber)
        {
            return SparePartItemOpsDAL.GetSparePartItemsByShipmentPoNumber(shipmentPoNumber);
        }

        public static List<SparePartItem> GetAvailableSparePartItems()
        {
            return SparePartItemOpsDAL.GetAvailableSparePartItems();
        }
    }
}
