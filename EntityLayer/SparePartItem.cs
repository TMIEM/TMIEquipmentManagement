using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    [Serializable()]
    public class SparePartItem : IComparable<SparePartItem>, ServiceItem
    {
        public string SerialNumber { get; set; }
        public double Price { get; set; }
        public string ShipmentPoNumber { get; set; }
        public string SparePartModelNumber { get; set; }
        public Shipment Shipment { get; set; }
        public SparePart SparePart { get; set; }

        public int CompareTo(SparePartItem other)
        {
            return this.SerialNumber.CompareTo(other.SerialNumber);
        }


        public ItemType GetItemType()
        {
            return ItemType.SparePart;
        }
    }

}
