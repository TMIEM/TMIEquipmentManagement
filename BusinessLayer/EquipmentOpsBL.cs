using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using EntityLayer;

namespace BusinessLayer
{
    public class EquipmentOpsBL
    {
        public static void AddNewEquipment(Equipment equipment)
        {
            EquipmentOpsDAL.AddNewEquipment(equipment);
        }

        public static List<Equipment> GetAllEquipments()
        {
            return EquipmentOpsDAL.GetAllEquipments();
        }

        public static Equipment GetEquipmentByModel(string equipmentModel)
        {
            try
            {
                return EquipmentOpsDAL.GetEquipmentByModel(equipmentModel);
            }
            catch (RecordNotFoundException e)
            {
                throw;
            }
        }

        public static void UpdateEquipment(Equipment equipment)
        {
            EquipmentOpsDAL.UpdateEquipment(equipment);
        }
    }
}
