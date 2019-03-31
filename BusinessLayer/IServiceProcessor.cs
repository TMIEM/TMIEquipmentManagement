using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;

namespace BusinessLayer
{
    public interface IServiceProcessor
    {
        //props
        ISparePartUsageProcessor SparePartUsageProcessor { get; set; }
        IConsumableBatchUsageProcessor ConsumableBatchUsageProcessor { get; set; }
        ISparePartRemovalProcessor SparePartRemovalProcessor { get; set; }
        IConsumableRemovalProcessor ConsumableRemovalProcessor { get; set; }
        Service Service { get; set; }

        void AddService();
        void UpdateService();
        void LoadServiceDetails(int serviceId);
        void DeleteService();
    }
}