using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;

namespace BusinessLayer
{
    public interface IEquipmentProcessor
    {
        List<EquipmentItem> EquipmentItems { get; set; }
        void AddEquipmentItem(EquipmentItem equipmentItem, bool isEditing);
        void UpdateEquipmentItem(EquipmentItem equipmentItem);
        void RemoveEquipmentItem(EquipmentItem equipmentItem);
        void LoadEquipmentItems(string shipmentPoNumber);
        void SaveAddedEquipmentItems(IShipmentProcessor shipmentProcessor);
        EquipmentItem FindEquipmentItem(string equipmentItemSerial);
    }
}
