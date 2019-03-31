using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;

namespace BusinessLayer
{
    public interface ISparePartUsageProcessor
    {
        List<SparePartUsage> SparePartUsages { get; set; }
        void AddSparePartUsage(SparePartUsage sparePartUsage, bool isEditing);
        void UpdateSparePartUsage(SparePartUsage sparePartUsage);
        void RemoveSparePartUsage(SparePartUsage sparePartUsage);
        void LoadSparePartUsages(int serviceId);
        void SaveAddedSparePartUsages(IServiceProcessor serviceProcessor);
        SparePartUsage FindSparePartUsage(string sparePartSerialNumber);
    }
}
