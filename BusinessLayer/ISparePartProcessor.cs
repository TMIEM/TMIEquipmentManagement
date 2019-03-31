using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;

namespace BusinessLayer
{
    public interface ISparePartProcessor
    {
        List<SparePartItem> SparePartItems { get; set; }
        void AddSparePartItem(SparePartItem sparePartItem, bool isEditing);
        void UpdateSparePartItem(SparePartItem sparePartItem);
        void RemoveSparePartItem(SparePartItem sparePartItem);
        void LoadSparePartItems(string shipmentPoNumber);
        void SaveAddedSparePartItems(IShipmentProcessor shipmentProcessor);
        SparePartItem FindSparePartItem(string sparePartItemSerial);
    }
}
