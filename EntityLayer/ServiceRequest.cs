using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class ServiceRequest
    {
        
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string CurrentProductLocation { get; set; }
        public string Type { get; set; }
        public string UnderWarranty { get; set; }
        public string ProblemDetails { get; set; }
        public DateTime ProblemOccurenceDate { get; set; }
        public string ProblemFrequencyDetails { get; set; }
        public string ProblemReproductionInstructions { get; set; }
        public ServiceItem ServiceItem { get; set; }

        
    }
}
