using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    [Serializable()]
    public class Equipment
    {
        public string ModelNumber { get; set; }
        public string Make { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string SoftwareVersion { get; set; }
        public int MinimumServicePeriodMonths { get; set; }
    }
}
