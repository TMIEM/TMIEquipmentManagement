using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    [Serializable()]
    public class ConsumableBatch : IComparable<ConsumableBatch>
    {
        public string ShipmentPoNumber { get; set; }
        public string ConsumableModelNumber { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public Consumable Consumable { get; set; }
        public Shipment Shipment { get; set; }


        public int CompareTo(ConsumableBatch other)
        {
            return this.ShipmentPoNumber.Equals(other.ShipmentPoNumber) ?
                ConsumableModelNumber.CompareTo(other.ConsumableModelNumber) : 
                ShipmentPoNumber.CompareTo(other.ShipmentPoNumber);
        }
    }
}
