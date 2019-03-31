using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;

namespace BusinessLayer
{
    public interface IShipmentProcessor
    {
        IEquipmentProcessor EquipmentProcessor { get; set; }
        ISparePartProcessor SparePartProcessor { get; set; }
        IConsumableProcessor ConsumableProcessor { get; set; }
        Shipment Shipment { get; set; }
        void AddShipment();
        void UpdateShipment();
        void LoadShipmentDetails(string poNumber);
    }
}
