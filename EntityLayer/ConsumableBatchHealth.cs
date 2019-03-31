using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class ConsumableBatchHealth
    {
        public double HealthPercentage { get; set; }
        public ConsumableBatchServiceUsage ConsumableBatchUsage { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
