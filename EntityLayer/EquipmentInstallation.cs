using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    [Serializable()]
    public class EquipmentInstallation : IComparable<EquipmentInstallation>
    {
        public string EquipmentItemSerialNumber { get; set; }
        public DateTime InstallationDate { get; set; }
        public DateTime CommissioningDate { get; set; }
        public int WarrantyPeriodMonths { get; set; }
        public int ServiceAgreementPeriodMonths { get; set; }
        public string InvoiceId { get; set; }
        public char TrackingStatus { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public EquipmentItem EquipmentItem { get; set; }
        public List<Service> Services { get; set; }

        public int CompareTo(EquipmentInstallation other)
        {
            return this.EquipmentItemSerialNumber.CompareTo(other.EquipmentItemSerialNumber);
        }

        public InstallationLocation InstallationLocation { get; set; }

       
    }
}
