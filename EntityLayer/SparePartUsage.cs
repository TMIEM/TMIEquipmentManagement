using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    [Serializable()]
    public class SparePartUsage : IComparable<SparePartUsage>
    {
        public string SparePartItemSerialNumber { get; set; }
        public int WarrantyPeriodMonths { get; set; }
        public int ServiceId { get; set; }
        public int RemovalServiceId { get; set; }
        public SparePartItem SparePartItem { get; set; }
        public Service Service { get; set; }


        public int CompareTo(SparePartUsage other)
        {
            return this.SparePartItemSerialNumber.CompareTo(other.SparePartItemSerialNumber);
        }

    }
}
