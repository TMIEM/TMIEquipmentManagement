using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Exception;
using EntityLayer;

namespace BusinessLayer
{
    [Serializable()]
    public class SparePartProcessor : ISparePartProcessor
    {
        public List<SparePartItem> SparePartItems { get; set; }

        public void AddSparePartItem(SparePartItem sparePartItem, bool isEditing)
        {
            SparePartItems = SparePartItems ?? new List<SparePartItem>();

            //checking for duplicate serial number
            if (!IsSerialDuplicate(sparePartItem))
            {
                SparePartItems.Add(sparePartItem);
                SparePartItems.Sort();
                //adding the item directly to the DB if an item is added to the shipment when editing
                if (isEditing)
                {
                    SparePartItemOpsBL.AddNewSparePartItem(sparePartItem);
                }
            }
            else
            {
                throw new DuplicateRecordIdentifierException(
                    "Serial " + sparePartItem.SerialNumber + " is already in use.");
            }
        }

        private bool IsSerialDuplicate(SparePartItem sparePartItem)
        {
            //checking in the sparePartItems list
            var binarySearchResult = SparePartItems.BinarySearch(sparePartItem);
            if (binarySearchResult >= 0)
            {
                return true;
            }

            //checking in the database
            try
            {
                var sparePartItemBySerialNumber =
                    SparePartItemOpsBL.GetSparePartItemBySerialNumber(sparePartItem.SerialNumber);
                return true;
            }
            catch (RecordNotFoundException e)
            {
                return false;
            }
        }

        public void UpdateSparePartItem(SparePartItem sparePartItem)
        {
            var itemIndex = SparePartItems.BinarySearch(sparePartItem);
            //if found
            if (itemIndex >= 0)
            {
                //replace existing item with updated item
                SparePartItems[itemIndex] = sparePartItem;
            }

            try
            {
                //checking if item is already in database
                var sparePartItemBySerialNumber =
                    SparePartItemOpsBL.GetSparePartItemBySerialNumber(sparePartItem.SerialNumber);

                //updating sparepart item in db
                SparePartItemOpsBL.UpdateSparePartItem(sparePartItem);
            }
            catch (RecordNotFoundException e)
            {
                //do nothing if item is not in DB
            }
        }

        public void RemoveSparePartItem(SparePartItem sparePartItem)
        {
            //search item in DB
            try
            {
                var sparePartItemBySerialNumber
                    = SparePartItemOpsBL.GetSparePartItemBySerialNumber(sparePartItem.SerialNumber);

                //continue to execute delete operation if sparePart is found
                //deleting an sparePart which is referred to by other tables should not be allowed
                //the operation will result in an exception which is assumed to be the foreign key violation exception
                //in such an event, the exception is thrown to client code, 
                //so that appropriate error message can be displayed on front end
                try
                {
                    SparePartItemOpsBL.DeleteSparePartItem(sparePartItem);
                    //if deletion from DB succeeds, local deletion is also performed
                    RemoveSparePartFromList(sparePartItem);
                }
                catch (SqlException sqlException)
                {
                    //record cannot be removed from DB hence it isn't removed from the local list either
                    throw;
                }
            }
            catch (RecordNotFoundException e)
            {
                //record not found in DB, hence continue to remove from local list
                RemoveSparePartFromList(sparePartItem);
            }
        }

        private void RemoveSparePartFromList(SparePartItem sparePartItem)
        {
            //search and remove sparePart item
            var itemIndex = SparePartItems.BinarySearch(sparePartItem);
            if (itemIndex >= 0)
            {
                SparePartItems.RemoveAt(itemIndex);
            }
        }

        public void LoadSparePartItems(string shipmentPoNumber)
        {
            SparePartItems = SparePartItemOpsBL.GetSparePartItemsByShipmentPoNumber(shipmentPoNumber);
        }

        public void SaveAddedSparePartItems(IShipmentProcessor shipmentProcessor)
        {
            if (SparePartItems == null) return;
            //setting the shipment details for each item
            foreach (var sparePartItem in SparePartItems)
            {
                sparePartItem.ShipmentPoNumber = shipmentProcessor.Shipment.PoNumber;
                SparePartItemOpsBL.AddNewSparePartItem(sparePartItem);
            }
        }

        public SparePartItem FindSparePartItem(string sparePartItemSerial)
        {
            var sparePartItem = new SparePartItem()
            {
                SerialNumber = sparePartItemSerial
            };

            var searchResult = SparePartItems.BinarySearch(sparePartItem);
            if (searchResult >= 0)
            {
                return SparePartItems[searchResult];
            }
            else
            {
                throw new RecordNotFoundException("SparePart item "
                                                  + sparePartItemSerial + " not found in sparePart processor list");
            }
        }
    }
}