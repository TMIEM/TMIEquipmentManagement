using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    [Serializable()]
    public class Service : IComparable<Service>
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string ProblemDescription { get; set; }
        public string ServiceDescription { get; set; }
        public string SpecialNote { get; set; }
        public string InstalledEquipmentSerialNumber { get; set; }
        public int TechnicianId { get; set; }
        public Technician Technician { get; set; }
        public EquipmentInstallation EquipmentInstallation { get; set; }
        public List<SparePartUsage> SparePartUsages { get; set; }

        public int CompareTo(Service other)
        {
            return this.Id.CompareTo(other.Id);
        }
    }
}
