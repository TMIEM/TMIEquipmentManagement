using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using BusinessLayer.Exception;
using EntityLayer;

namespace BusinessLayer
{
    [Serializable()]
    public class ServiceProcessor : IServiceProcessor
    {
        public ISparePartUsageProcessor SparePartUsageProcessor { get; set; }
        public IConsumableBatchUsageProcessor ConsumableBatchUsageProcessor { get; set; }
        public ISparePartRemovalProcessor SparePartRemovalProcessor { get; set; }
        public IConsumableRemovalProcessor ConsumableRemovalProcessor { get; set; }
        public Service Service { get; set; }
        public void AddService()
        {
            if (Service == null) return;
            ExecuteServiceTransaction();
        }

        private void ExecuteServiceTransaction()
        {
            //using a transaction to call the persistance related methods of all items part of the service 
            //including the service itself
            try
            {
                using (var transactionScope = new TransactionScope())
                {
                    SaveServiceDetails();
                    SparePartUsageProcessor.SaveAddedSparePartUsages(this);
                    ConsumableBatchUsageProcessor.SaveAddedConsumableBatchServiceUsages(this);
                    SparePartRemovalProcessor.SaveAddedSparePartRemovals(this);
                    ConsumableRemovalProcessor.SaveAddedConsumableBatchRemovals(this);
                    transactionScope.Complete();
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void SaveServiceDetails()
        {
            var newService = ServiceOpsBL.AddNewService(Service);
            this.Service = newService;
        }

        public void UpdateService()
        {
            ServiceOpsBL.UpdateService(Service);
        }

        public void LoadServiceDetails(int serviceId)
        {
            Service = ServiceOpsBL.GetServiceById(serviceId);
            LoadItemDetails();
        }

        private void LoadItemDetails()
        {
            SparePartUsageProcessor.LoadSparePartUsages(Service.Id);
            ConsumableBatchUsageProcessor.LoadConsumableBatchServiceUsages(Service.Id);
            SparePartRemovalProcessor.LoadSparePartRemovals(Service.Id);
            ConsumableRemovalProcessor.LoadConsumableBatchRemovals(Service.Id);
        }


        public void DeleteService()
        {
            throw new NotImplementedException();
        }
    }
}
