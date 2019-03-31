 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer
{
    [Serializable()]
    public class Technician : IComparable<Technician>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        public string  Email { get; set; }
        public List<Service> ServicesPerformed { get; set; }

        public int CompareTo(Technician other)
        {
            return this.Id.CompareTo(other.Id);
        }
    }
}
