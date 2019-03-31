using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    [Serializable()]
    public class SparePartUsageHealth
    {
        public double HealthPercentage { get; set; }
        public SparePartUsage SparePartUsage { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
