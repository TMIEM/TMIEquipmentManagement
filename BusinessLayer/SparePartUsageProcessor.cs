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
    public class SparePartUsageProcessor : ISparePartUsageProcessor
    {
        public List<SparePartUsage> SparePartUsages { get; set; }
        public void AddSparePartUsage(SparePartUsage sparePartUsage, bool isEditing)
        {
            SparePartUsages = SparePartUsages ?? new List<SparePartUsage>();

            //checking for duplicate serial number
            if (!IsSerialDuplicate(sparePartUsage))
            {
                SparePartUsages.Add(sparePartUsage);
                SparePartUsages.Sort();
                //adding the item directly to the DB if an item is added to the service when editing
                if (isEditing)
                {
                    SparePartUsageOpsBL.AddNewSparePartUsage(sparePartUsage);
                }
            }
            else
            {
                throw new DuplicateRecordIdentifierException(
                    "Serial " + sparePartUsage.SparePartItemSerialNumber + " is already in use.");
            }
        }

        private bool IsSerialDuplicate(SparePartUsage sparePartUsage)
        {
            //checking in the sparePartUsages list
            var binarySearchResult = SparePartUsages.BinarySearch(sparePartUsage);
            if (binarySearchResult >= 0)
            {
                return true;
            }

            //checking in the database
            try
            {
                var sparePartUsageBySerialNumber =
                    SparePartUsageOpsBL.GetSparePartUsageBySerialNumber(sparePartUsage.SparePartItemSerialNumber);
                return true;
            }
            catch (RecordNotFoundException e)
            {
                return false;
            }
        }

        public void UpdateSparePartUsage(SparePartUsage sparePartUsage)
        {
            var itemIndex = SparePartUsages.BinarySearch(sparePartUsage);
            //if found
            if (itemIndex >= 0)
            {
                //replace existing item with updated item
                SparePartUsages[itemIndex] = sparePartUsage;
            }

            try
            {
                //checking if item is already in database
                var sparePartUsageBySerialNumber =
                    SparePartUsageOpsBL.GetSparePartUsageBySerialNumber(sparePartUsage.SparePartItemSerialNumber);

                //updating sparepart item in db
                SparePartUsageOpsBL.UpdateSparePartUsage(sparePartUsage);
            }
            catch (RecordNotFoundException e)
            {
                //do nothing if item is not in DB
            }
        }

        public void RemoveSparePartUsage(SparePartUsage sparePartUsage)
        {
            //search item in DB
            try
            {
                var sparePartUsageBySerialNumber
                    = SparePartUsageOpsBL.GetSparePartUsageBySerialNumber(sparePartUsage.SparePartItemSerialNumber);

                //continue to execute delete operation if sparePart usage is found
                //deleting an sparePartUsage which is referred to by other tables should not be allowed
                //the operation will result in an exception which is assumed to be the foreign key violation exception
                //in such an event, the exception is thrown to client code, 
                //so that appropriate error message can be displayed on front end
                try
                {
                    SparePartUsageOpsBL.DeleteSparePartUsage(sparePartUsage);
                    //if deletion from DB succeeds, local deletion is also performed
                    RemoveSparePartFromList(sparePartUsage);
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
                RemoveSparePartFromList(sparePartUsage);
            }
        }

        private void RemoveSparePartFromList(SparePartUsage sparePartUsage)
        {
            //search and remove sparePartUsage
            var itemIndex = SparePartUsages.BinarySearch(sparePartUsage);
            if (itemIndex >= 0)
            {
                SparePartUsages.RemoveAt(itemIndex);
            }
        }

        public void LoadSparePartUsages(int serviceId)
        {
            SparePartUsages = SparePartUsageOpsBL.GetSparePartUsagesByServiceId(serviceId);

        }

        public void SaveAddedSparePartUsages(IServiceProcessor serviceProcessor)
        {
            if (SparePartUsages == null) return;
            //setting the service details for each item
            foreach (var sparePartUsage in SparePartUsages)
            {
                sparePartUsage.ServiceId = serviceProcessor.Service.Id;
                SparePartUsageOpsBL.AddNewSparePartUsage(sparePartUsage);
            }
        }

        public SparePartUsage FindSparePartUsage(string sparePartSerialNumber)
        {
            var sparePartUsage = new SparePartUsage()
            {
                SparePartItemSerialNumber = sparePartSerialNumber
            };

            var searchResult = SparePartUsages.BinarySearch(sparePartUsage);
            if (searchResult >= 0)
            {
                return SparePartUsages[searchResult];
            }
            else
            {
                throw new RecordNotFoundException("SparePart item "
                                                  + sparePartSerialNumber + " not found in sparePart processor list");
            }
        }
    }
}
