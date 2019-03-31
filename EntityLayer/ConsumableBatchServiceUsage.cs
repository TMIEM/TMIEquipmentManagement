using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    [Serializable()]
    public class ConsumableBatchServiceUsage : IComparable<ConsumableBatchServiceUsage>
    {
        public string ConsumbaleBatchShipmentPONumber { get; set; }
        public string ConsumableBatchModelNumber { get; set; }
        public int ServiceId { get; set; }
        public int QuantityUsed { get; set; }
        public int RemovalServiceId { get; set; }
        public Service Service { get; set; }
        public Shipment Shipment { get; set; }
        public ConsumableBatch ConsumableBatch { get; set; }
        public int CompareTo(ConsumableBatchServiceUsage other)
        {
            var result = this.ConsumbaleBatchShipmentPONumber.CompareTo(other.ConsumbaleBatchShipmentPONumber);
            if(result == 0)
                result = this.ConsumbaleBatchShipmentPONumber.CompareTo(other.ConsumbaleBatchShipmentPONumber);
            if(result == 0)
                result = this.ConsumableBatchModelNumber.CompareTo(other.ConsumableBatchModelNumber);
            if (result == 0)
                result = this.ServiceId.CompareTo(other.ServiceId);
            return result;
        }

       
    }
}
