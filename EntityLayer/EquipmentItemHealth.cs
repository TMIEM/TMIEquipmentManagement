using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    [Serializable()]
    public class EquipmentItemHealth
    {
        public double HealthPercentage { get; set; }
        public EquipmentInstallation EquipmentInstallation { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
