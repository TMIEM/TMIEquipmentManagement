using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using EntityLayer;

namespace BusinessLayer
{
    public class EquipmentItemOpsBL
    {
        public static void AddNewEquipmentItem(EquipmentItem equipmentItem)
        {
            EquipmentItemOpsDAL.AddNewEquipmentItem(equipmentItem);
        }

        public static List<EquipmentItem> GetAllEquipmentItems()
        {
            return EquipmentItemOpsDAL.GetAllEquipmentItems();
        }

        public static EquipmentItem GetEquipmentItemBySerialNumber(string equipmentItemSerialNumber)
        {
            try
            {
                return EquipmentItemOpsDAL.GetEquipmentItemBySerialNumber(equipmentItemSerialNumber);
            }
            catch (RecordNotFoundException e)
            {
                throw;
            }
        }

        public static void UpdateEquipmentItem(EquipmentItem equipmentItem)
        {
            EquipmentItemOpsDAL.UpdateEquipmentItem(equipmentItem);
        }

        public static List<EquipmentItem> GetEquipmentItemByShipmentPoNumber(string shipmentPoNumber)
        {
            try
            {
                return EquipmentItemOpsDAL.GetEquipmentItemsByShipment(shipmentPoNumber);
            }
            catch (RecordNotFoundException e)
            {
                throw;
            }
        }

        public static void DeleteEquipmentItem(EquipmentItem equipmentItem)
        {
            EquipmentItemOpsDAL.DeleteEquipmentItem(equipmentItem);
        }

        public static List<EquipmentItem> GetAvailableEquipmentItems()
        {
            return EquipmentItemOpsDAL.GetAvailableEquipmentItems();
        }
    }
}
