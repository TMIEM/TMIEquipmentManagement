using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    [Serializable()]
    public class SparePart
    {
        public string ModelNumber { get; set; }
        public string Make { get; set; }
        public string Description { get; set; }
        public int LifeSpanMonths { get; set; }
    }
}
