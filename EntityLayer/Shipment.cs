using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    [Serializable()]
    public class Shipment
    {
        public string PoNumber { get; set; }
        public string SupplierInvoiceNumber { get; set; }
        public DateTime DateOfArrival { get; set; }
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }
    }
}
