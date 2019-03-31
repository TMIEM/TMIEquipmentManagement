using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;

namespace BusinessLayer
{
    public interface IConsumableRemovalProcessor
    {
        List<ConsumableBatchServiceUsage> ConsumableRemovals { get; set; }
        void AddConsumableRemoval(ConsumableBatchServiceUsage consumableBatchServiceUsage, bool isEditing);
        void RemoveConsumableRemoval(ConsumableBatchServiceUsage consumableBatchServiceUsage);
        void LoadConsumableBatchRemovals(int serviceId);
        void SaveAddedConsumableBatchRemovals(IServiceProcessor serviceProcessor);
        ConsumableBatchServiceUsage FindConsumableBatchRemoval(string shipmentNumber, string modelNumber, int serviceId);
    }
}
