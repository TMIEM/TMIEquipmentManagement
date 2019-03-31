using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;

namespace BusinessLayer
{
    public interface IConsumableBatchUsageProcessor
    {
        List<ConsumableBatchServiceUsage> ConsumableBatchServiceUsages { get; set; }
        void AddConsumableBatchServiceUsage(ConsumableBatchServiceUsage consumableBatchServiceUsage, bool isEditing);
        void UpdateConsumableBatchServiceUsage(ConsumableBatchServiceUsage consumableBatchServiceUsage);
        void RemoveConsumableBatchServiceUsage(ConsumableBatchServiceUsage consumableBatchServiceUsage);
        void LoadConsumableBatchServiceUsages(int serviceId);
        void SaveAddedConsumableBatchServiceUsages(IServiceProcessor serviceProcessor);

        ConsumableBatchServiceUsage FindConsumableBatchServiceUsage(string consumableModelNumber, string shipmentPoNo,
            int serviceId);
    }
}