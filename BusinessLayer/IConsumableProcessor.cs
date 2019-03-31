using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;

namespace BusinessLayer
{
    public interface IConsumableProcessor
    {
        List<ConsumableBatch> ConsumableBatches { get; set; }
        void AddConsumableBatch(ConsumableBatch consumableBatch, bool isEditing);
        void UpdateConsumableBatch(ConsumableBatch consumableBatch);
        void RemoveConsumableBatch(ConsumableBatch consumableBatch);
        void LoadConsumableBatches(string shipmentPoNumber);
        void SaveAddedConsumableBatchs(IShipmentProcessor shipmentProcessor);
        ConsumableBatch FindConsumableBatch(string consumableModelNumber, string shipmentNo);
    }
}