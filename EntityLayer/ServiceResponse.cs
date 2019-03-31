using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    public class ServiceResponse
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string ServiceDescription { get; set; }
        public string CoveredByWarranty { get; set; }
        public double ChargesPaid { get; set; }
        public int ServiceRequestId { get; set; }
        public ServiceRequest ServiceRequest { get; set; }
        public ServiceItem ReplacementItem { get; set; }

    }
}
