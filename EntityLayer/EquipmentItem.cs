using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    [Serializable()]
    public class EquipmentItem : IComparable<EquipmentItem>, ServiceItem
    {
        public string SerialNumber { get; set; }
        public double Price { get; set; }
        public string ShipmentPoNumber { get; set; }
        public string EquipmentModelNumber { get; set; }
        public Equipment Equipment { get; set; }
        public Shipment Shipment { get; set; }
        public int CompareTo(EquipmentItem other)
        {
            return this.SerialNumber.CompareTo(other.SerialNumber);
        }

        public ItemType GetItemType()
        {
            return ItemType.Equipment;
        }

    }
}
