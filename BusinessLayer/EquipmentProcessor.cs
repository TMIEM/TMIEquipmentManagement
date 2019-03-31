using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Exception;
using EntityLayer;

namespace BusinessLayer
{
    [Serializable()]
    public class EquipmentProcessor : IEquipmentProcessor
    {
        public List<EquipmentItem> EquipmentItems { get; set; }

        public void AddEquipmentItem(EquipmentItem equipmentItem, bool isEditing)
        {
            EquipmentItems = EquipmentItems ?? new List<EquipmentItem>();

            //checking for duplicate serial number
            if (!IsSerialDuplicate(equipmentItem))
            {
                EquipmentItems.Add(equipmentItem);
                EquipmentItems.Sort();
                //adding the item directly to the DB if an item is added to the shipment when editing
                if (isEditing)
                {
                    EquipmentItemOpsBL.AddNewEquipmentItem(equipmentItem);
                }
            }
            else
            {
                throw new DuplicateRecordIdentifierException("Serial "+equipmentItem.SerialNumber+" is already in use.");
            }

        }

        //returns false if no duplicates are being used
        private bool IsSerialDuplicate(EquipmentItem equipmentItem)
        {
            //checking in the equipmentItems list
            var binarySearchResult = EquipmentItems.BinarySearch(equipmentItem);
            if (binarySearchResult >= 0)
            {
                return true;
            }

            //checking in the database
            try
            {
                var equipmentItemBySerialNumber =
                    EquipmentItemOpsBL.GetEquipmentItemBySerialNumber(equipmentItem.SerialNumber);
                return true;

            }
            catch (RecordNotFoundException e)
            {
                return false;
            }
        }

        //if record is already present in database, updates the database record, else it updates the local object only
        public void UpdateEquipmentItem(EquipmentItem equipmentItem)
        {
            var itemIndex = EquipmentItems.BinarySearch(equipmentItem);
            //if found
            if (itemIndex >= 0)
            {
                //replace existing item with updated item
                EquipmentItems[itemIndex] = equipmentItem;
            }

            try
            {
                //checking if item is already in database
                var equipmentItemBySerialNumber =
                    EquipmentItemOpsBL.GetEquipmentItemBySerialNumber(equipmentItem.SerialNumber);
                
                //updating equipment item in db
                EquipmentItemOpsBL.UpdateEquipmentItem(equipmentItem);
            }
            catch (RecordNotFoundException e)
            {
                //do nothing if item is not in DB
            }
        }

        //removes equipment item from local list and
        //from database only if the item is not referred to by another table
        public void RemoveEquipmentItem(EquipmentItem equipmentItem)
        {
            
            //search item in DB
            try
            {
                var equipmentItemBySerialNumber
                    = EquipmentItemOpsBL.GetEquipmentItemBySerialNumber(equipmentItem.SerialNumber);

                //continue to execute delete operation if equipment is found
                //deleting an equipment which is referred to by other tables should not be allowed
                //the operation will result in an exception which is assumed to be the foreign key violation exception
                //in such an event, the exception is thrown to client code, 
                //so that appropriate error message can be displayed on front end
                try
                {
                    EquipmentItemOpsBL.DeleteEquipmentItem(equipmentItem);
                    //if deletion from DB succeeds, local deletion is also performed
                    RemoveEquipmentFromList(equipmentItem);
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
                RemoveEquipmentFromList(equipmentItem);
            }

        }


        private void RemoveEquipmentFromList(EquipmentItem equipmentItem)
        {
            //search and remove equipment item
            var itemIndex = EquipmentItems.BinarySearch(equipmentItem);
            if (itemIndex >= 0)
            {
                EquipmentItems.RemoveAt(itemIndex);
            }
        }

        public void LoadEquipmentItems(string shipmentPoNumber)
        {
            EquipmentItems = EquipmentItemOpsBL.GetEquipmentItemByShipmentPoNumber(shipmentPoNumber);
        }

        //to be called when addding the records for the first time only
        public void SaveAddedEquipmentItems(IShipmentProcessor shipmentProcessor)
        {
            if(EquipmentItems == null) return;
            //setting the shipment details for each item
            foreach (var equipmentItem in EquipmentItems)
            {
                equipmentItem.ShipmentPoNumber = shipmentProcessor.Shipment.PoNumber;
                EquipmentItemOpsBL.AddNewEquipmentItem(equipmentItem);
            }


        }

        //looks for an equipment item with the provided serial in the local list
        public EquipmentItem FindEquipmentItem(string equipmentItemSerial)
        {
            var equipmentItem = new EquipmentItem()
            {
                SerialNumber = equipmentItemSerial
            };

            var searchResult = EquipmentItems.BinarySearch(equipmentItem);
            if (searchResult >= 0)
            {
                return EquipmentItems[searchResult];
            }
            else
            {
                throw new RecordNotFoundException("Equipment item "
                    +equipmentItemSerial+" not found in equipment processor list");
            }
        }
    }
}
