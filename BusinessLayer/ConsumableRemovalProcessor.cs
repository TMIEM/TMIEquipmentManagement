using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Exception;
using EntityLayer;

namespace BusinessLayer
{
    [Serializable()]
    public class ConsumableRemovalProcessor : IConsumableRemovalProcessor
    {
        public List<ConsumableBatchServiceUsage> ConsumableRemovals { get; set; }

        public ConsumableRemovalProcessor()
        {
            ConsumableRemovals = new List<ConsumableBatchServiceUsage>();
        }
        public void AddConsumableRemoval(ConsumableBatchServiceUsage consumableBatchServiceUsage, bool isEditing)
        {
            ConsumableRemovals = ConsumableRemovals ?? new List<ConsumableBatchServiceUsage>();

            //checking for duplicate serial number
            if (!IsConsumableRemovedAlready(consumableBatchServiceUsage))
            {
                ConsumableRemovals.Add(consumableBatchServiceUsage);
                ConsumableRemovals.Sort();
                //adding the item directly to the DB if an item is added to the service when editing
                if (isEditing)
                {
                    ConsumableBatchServiceUsageOpsBL.UpdateConsumableBatchServiceUsage(consumableBatchServiceUsage);
                }
            }
            else
            {
                throw new DuplicateRecordIdentifierException(
                    "PO " + consumableBatchServiceUsage.ConsumbaleBatchShipmentPONumber + " model "
                    +consumableBatchServiceUsage.ConsumableBatchModelNumber+" is already in use.");
            }
        }

        private bool IsConsumableRemovedAlready(ConsumableBatchServiceUsage consumableUsage)
        {
            //checking in the consumableBatchServiceUsages list
            var binarySearchResult = ConsumableRemovals.BinarySearch(consumableUsage);
            if (binarySearchResult >= 0)
            {
                return true;
            }

            //checking in the database
            try
            {
                var consumableBatchServiceUsageBySerialNumber =
                    ConsumableBatchServiceUsageOpsBL
                        .GetConsumableBatchServiceUsageById(consumableUsage.ConsumbaleBatchShipmentPONumber,
                            consumableUsage.ConsumableBatchModelNumber, consumableUsage.ServiceId);

                return consumableBatchServiceUsageBySerialNumber.RemovalServiceId > 0;
            }
            catch (RecordNotFoundException e)
            {
                return false;
            }
        }

        public void RemoveConsumableRemoval(ConsumableBatchServiceUsage consumableBatchServiceUsage)
        {
            var itemIndex = ConsumableRemovals.BinarySearch(consumableBatchServiceUsage);
            //if found
            if (itemIndex >= 0)
            {
                //replace existing item with updated item
                ConsumableRemovals.RemoveAt(itemIndex);
            }

            try
            {
                //checking if item is already in database
                var consumableBatchServiceUsageBySerialNumber =
                    ConsumableBatchServiceUsageOpsBL.GetConsumableBatchServiceUsageById(consumableBatchServiceUsage.ConsumbaleBatchShipmentPONumber, 
                        consumableBatchServiceUsage.ConsumableBatchModelNumber, consumableBatchServiceUsage.ServiceId);

                if (consumableBatchServiceUsageBySerialNumber.RemovalServiceId > 0)
                {
                    consumableBatchServiceUsage.RemovalServiceId = -1;
                    ConsumableBatchServiceUsageOpsBL.UpdateConsumableBatchServiceUsage(consumableBatchServiceUsage);
                }

                //updating sparepart item in db
            }
            catch (RecordNotFoundException e)
            {
                //do nothing if item is not in DB
            }
        }

        public void LoadConsumableBatchRemovals(int serviceId)
        {
            ConsumableRemovals = ConsumableBatchServiceUsageOpsBL
                .GetConsumablesRemovedByServiceId(serviceId);
        }

        public void SaveAddedConsumableBatchRemovals(IServiceProcessor serviceProcessor)
        {
            if (ConsumableRemovals == null) return;
            //setting the service details for each item
            foreach (var consumableBatchServiceUsage in ConsumableRemovals)
            {
                consumableBatchServiceUsage.RemovalServiceId = serviceProcessor.Service.Id;
                ConsumableBatchServiceUsageOpsBL.UpdateConsumableBatchServiceUsage(consumableBatchServiceUsage);
            }
        }

        public ConsumableBatchServiceUsage FindConsumableBatchRemoval(string shipmentNumber, string modelNumber, int serviceId)
        {
            var consumableBatchServiceUsage = new ConsumableBatchServiceUsage()
            {
                ConsumbaleBatchShipmentPONumber = shipmentNumber,
                ConsumableBatchModelNumber = modelNumber,
                ServiceId = serviceId
            };

            var searchResult = ConsumableRemovals.BinarySearch(consumableBatchServiceUsage);
            if (searchResult >= 0)
            {
                var sparePartRemoval = ConsumableRemovals[searchResult];
                if (sparePartRemoval.RemovalServiceId > 0)
                {
                    return sparePartRemoval;
                }
                else
                {
                    throw new RecordNotFoundException("ConsumableBatch removal PO"
                                                      + shipmentNumber + " model "+modelNumber+", service "+serviceId
                                                      +" not found in sparePart removed processor list");
                }
            }
            else
            {
                throw new RecordNotFoundException("ConsumableBatch removal PO"
                                                  + shipmentNumber + " model " + modelNumber + ", service " + serviceId
                                                  + " not found in sparePart removed processor list");
            }
        }
    }
}
