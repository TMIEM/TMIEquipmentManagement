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
    public class ConsumableBatchUsageProcessor : IConsumableBatchUsageProcessor
    {
        public List<ConsumableBatchServiceUsage> ConsumableBatchServiceUsages { get; set; }

        public void AddConsumableBatchServiceUsage(ConsumableBatchServiceUsage consumableBatchServiceUsage,
            bool isEditing)
        {
            ConsumableBatchServiceUsages = ConsumableBatchServiceUsages ?? new List<ConsumableBatchServiceUsage>();

            //checking for duplicate serial number
            if (!IsUsageDuplicate(consumableBatchServiceUsage))
            {
                var availability = ConsumableBatchServiceUsageOpsBL.GetConsumableBatchAvailability(
                    consumableBatchServiceUsage.ConsumbaleBatchShipmentPONumber,
                    consumableBatchServiceUsage.ConsumableBatchModelNumber);

                if (!(consumableBatchServiceUsage.QuantityUsed <= availability))
                {
                    throw new ConsumableUsageQuantityExceededException("Quantity used is higher than available qty",
                        consumableBatchServiceUsage.ConsumbaleBatchShipmentPONumber,
                        consumableBatchServiceUsage.ConsumableBatchModelNumber);
                }

                ConsumableBatchServiceUsages.Add(consumableBatchServiceUsage);
                ConsumableBatchServiceUsages.Sort();
                //adding the item directly to the DB if an item is added to the service when editing
                if (isEditing)
                {
                    ConsumableBatchServiceUsageOpsBL.AddNewConsumableBatchServiceUsage(consumableBatchServiceUsage);
                }
            }
            else
            {
                throw new DuplicateRecordIdentifierException(
                    "Consumable " + consumableBatchServiceUsage.ConsumableBatchModelNumber + ", " +
                    consumableBatchServiceUsage.ConsumbaleBatchShipmentPONumber + "," +
                    consumableBatchServiceUsage.ServiceId + " is already added in service.");
            }
        }

        private bool IsUsageDuplicate(ConsumableBatchServiceUsage consumableBatchServiceUsage)
        {
            //checking in the consumableBatchServiceUsages list
            var binarySearchResult = ConsumableBatchServiceUsages.BinarySearch(consumableBatchServiceUsage);
            if (binarySearchResult >= 0)
            {
                return true;
            }

            //checking in the database
            try
            {
                var consumableBatchServiceUsageBySerialNumber =
                    ConsumableBatchServiceUsageOpsBL.GetConsumableBatchServiceUsageById(
                        consumableBatchServiceUsage.ConsumbaleBatchShipmentPONumber,
                        consumableBatchServiceUsage.ConsumableBatchModelNumber, consumableBatchServiceUsage.ServiceId);
                return true;
            }
            catch (RecordNotFoundException e)
            {
                return false;
            }
        }

        public void UpdateConsumableBatchServiceUsage(ConsumableBatchServiceUsage consumableBatchServiceUsage)
        {
            //checking if the quanity used is actually available
            var availabileQuantity = ConsumableBatchServiceUsageOpsBL.GetConsumableBatchAvailability(
                consumableBatchServiceUsage.ConsumbaleBatchShipmentPONumber,
                consumableBatchServiceUsage.ConsumableBatchModelNumber);


            try
            {
                //checking if item is already in database
                var consumableBatchServiceUsageBySerialNumber =
                    ConsumableBatchServiceUsageOpsBL.GetConsumableBatchServiceUsageById(
                        consumableBatchServiceUsage.ConsumbaleBatchShipmentPONumber,
                        consumableBatchServiceUsage.ConsumableBatchModelNumber, consumableBatchServiceUsage.ServiceId);

                var prevQuantity = consumableBatchServiceUsageBySerialNumber.QuantityUsed;
                var newQuantity = consumableBatchServiceUsage.QuantityUsed;

                if (newQuantity > prevQuantity)
                {
                    if ((newQuantity - prevQuantity) > availabileQuantity)
                    {
                        throw new ConsumableUsageQuantityExceededException("Quantity used is higher than available qty",
                            consumableBatchServiceUsage.ConsumbaleBatchShipmentPONumber,
                            consumableBatchServiceUsage.ConsumableBatchModelNumber);
                    }
                }

                //updating consumablebatchserviceusage in db
                ConsumableBatchServiceUsageOpsBL.UpdateConsumableBatchServiceUsage(consumableBatchServiceUsage);
                //Updating the consumableusages list item
                var itemIndex1 = ConsumableBatchServiceUsages.BinarySearch(consumableBatchServiceUsage);
                ConsumableBatchServiceUsages[itemIndex1] = consumableBatchServiceUsage;

            }
            catch (RecordNotFoundException e)
            {
                //updating the ConsumableUsages list only
                if (!(consumableBatchServiceUsage.QuantityUsed <= availabileQuantity))
                {
                    throw new ConsumableUsageQuantityExceededException("Quantity used is higher than available qty",
                        consumableBatchServiceUsage.ConsumbaleBatchShipmentPONumber,
                        consumableBatchServiceUsage.ConsumableBatchModelNumber);
                }
                var itemIndex2 = ConsumableBatchServiceUsages.BinarySearch(consumableBatchServiceUsage);
                ConsumableBatchServiceUsages[itemIndex2] = consumableBatchServiceUsage;

            }


//            var itemIndex = ConsumableBatchServiceUsages.BinarySearch(consumableBatchServiceUsage);
//            
//            var existingConsumableBatchServiceUsage = ConsumableBatchServiceUsages[itemIndex];
//            int previousQuantity = existingConsumableBatchServiceUsage.QuantityUsed;
//            if (consumableBatchServiceUsage.QuantityUsed > previousQuantity)
//            {
//                if ((consumableBatchServiceUsage.QuantityUsed - previousQuantity) > availabileQuantity)
//                {
//                    throw new ConsumableUsageQuantityExceededException("Quantity used is higher than available qty",
//                        consumableBatchServiceUsage.ConsumbaleBatchShipmentPONumber,
//                        consumableBatchServiceUsage.ConsumableBatchModelNumber);
//                }
//            }
//
//            //replace existing item with updated item
//            ConsumableBatchServiceUsages[itemIndex] = consumableBatchServiceUsage;
        }

        public void RemoveConsumableBatchServiceUsage(ConsumableBatchServiceUsage consumableBatchServiceUsage)
        {
            //search and remove consumableBatchServiceUsage
            var itemIndex = ConsumableBatchServiceUsages.BinarySearch(consumableBatchServiceUsage);
            if (itemIndex >= 0)
            {
                ConsumableBatchServiceUsages.RemoveAt(itemIndex);
            }
        }

        public void LoadConsumableBatchServiceUsages(int serviceId)
        {
            ConsumableBatchServiceUsages =
                ConsumableBatchServiceUsageOpsBL.GetConsumableBatchServiceUsagesByServiceId(serviceId);
        }

        public void SaveAddedConsumableBatchServiceUsages(IServiceProcessor serviceProcessor)
        {
            if (ConsumableBatchServiceUsages == null) return;
            //setting the service details for each item
            foreach (var consumableBatchServiceUsage in ConsumableBatchServiceUsages)
            {
                //checking if the quanity used is actually available
                var availability = ConsumableBatchServiceUsageOpsBL.GetConsumableBatchAvailability(
                    consumableBatchServiceUsage.ConsumbaleBatchShipmentPONumber,
                    consumableBatchServiceUsage.ConsumableBatchModelNumber);

                if (!(consumableBatchServiceUsage.QuantityUsed <= availability))
                {
                    throw new ConsumableUsageQuantityExceededException("Quantity used is higher than available qty",
                        consumableBatchServiceUsage.ConsumbaleBatchShipmentPONumber,
                        consumableBatchServiceUsage.ConsumableBatchModelNumber);
                }

                consumableBatchServiceUsage.ServiceId = serviceProcessor.Service.Id;
                ConsumableBatchServiceUsageOpsBL.AddNewConsumableBatchServiceUsage(consumableBatchServiceUsage);
            }
        }

        public ConsumableBatchServiceUsage FindConsumableBatchServiceUsage(string consumableModelNumber,
            string shipmentPoNo, int serviceId)
        {
            var consumableBatchServiceUsage = new ConsumableBatchServiceUsage()
            {
                ConsumbaleBatchShipmentPONumber = shipmentPoNo,
                ConsumableBatchModelNumber = consumableModelNumber,
                ServiceId = serviceId
            };

            var searchResult = ConsumableBatchServiceUsages.BinarySearch(consumableBatchServiceUsage);
            if (searchResult >= 0)
            {
                return ConsumableBatchServiceUsages[searchResult];
            }
            else
            {
                throw new RecordNotFoundException("ConsumableBatchServiceUsage item "
                                                  + shipmentPoNo + ", " + consumableModelNumber + ", " + serviceId +
                                                  " not found in sparePart processor list");
            }
        }
    }
}