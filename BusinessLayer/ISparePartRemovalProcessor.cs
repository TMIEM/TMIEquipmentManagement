using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;

namespace BusinessLayer
{
    public interface ISparePartRemovalProcessor
    {
        List<SparePartUsage> SparePartRemovals { get; set; }
        void AddSparePartRemoval(SparePartUsage sparePartUsage, bool isEditing);
        void RemoveSparePartRemoval(SparePartUsage sparePartUsage);
        void LoadSparePartRemovals(int serviceId);
        void SaveAddedSparePartRemovals(IServiceProcessor serviceProcessor);
        SparePartUsage FindSparePartRemoval(string sparePartSerialNumber);
    }
}
