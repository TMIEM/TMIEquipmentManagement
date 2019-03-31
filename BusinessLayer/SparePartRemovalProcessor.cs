using System;
using System.Collections;
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
    public class SparePartRemovalProcessor : ISparePartRemovalProcessor
    {
        public List<SparePartUsage> SparePartRemovals { get; set; }

        public SparePartRemovalProcessor()
        {
            SparePartRemovals = new List<SparePartUsage>();
        }

        public void AddSparePartRemoval(SparePartUsage sparePartUsage, bool isEditing)
        {
            SparePartRemovals = SparePartRemovals ?? new List<SparePartUsage>();

            //checking for duplicate serial number
            if (!IsSparePartRemovedAlready(sparePartUsage))
            {
                SparePartRemovals.Add(sparePartUsage);
                SparePartRemovals.Sort();
                //adding the item directly to the DB if an item is added to the service when editing
                if (isEditing)
                {
                    SparePartUsageOpsBL.UpdateSparePartUsage(sparePartUsage);
                }
            }
            else
            {
                throw new DuplicateRecordIdentifierException(
                    "Serial " + sparePartUsage.SparePartItemSerialNumber + " is already in use.");
            }
        }

        private bool IsSparePartRemovedAlready(SparePartUsage sparePartUsage)
        {
            //checking in the sparePartUsages list
            var binarySearchResult = SparePartRemovals.BinarySearch(sparePartUsage);
            if (binarySearchResult >= 0)
            {
                return true;
            }

            //checking in the database
            try
            {
                var sparePartUsageBySerialNumber =
                    SparePartUsageOpsBL.GetSparePartUsageBySerialNumber(sparePartUsage.SparePartItemSerialNumber);
                return sparePartUsageBySerialNumber.RemovalServiceId > 0;
            }
            catch (RecordNotFoundException e)
            {
                return false;
            }
        }

        public void RemoveSparePartRemoval(SparePartUsage sparePartUsage)
        {
            var itemIndex = SparePartRemovals.BinarySearch(sparePartUsage);
            //if found
            if (itemIndex >= 0)
            {
                //replace existing item with updated item
                SparePartRemovals.RemoveAt(itemIndex);
            }

            try
            {
                //checking if item is already in database
                var sparePartUsageBySerialNumber =
                    SparePartUsageOpsBL.GetSparePartUsageBySerialNumber(sparePartUsage.SparePartItemSerialNumber);
                if (sparePartUsageBySerialNumber.RemovalServiceId > 0)
                {
                    sparePartUsage.RemovalServiceId = -1;
                    SparePartUsageOpsBL.UpdateSparePartUsage(sparePartUsage);
                }

                //updating sparepart item in db
            }
            catch (RecordNotFoundException e)
            {
                //do nothing if item is not in DB
            }
        }

        public void LoadSparePartRemovals(int serviceId)
        {
            SparePartRemovals = SparePartUsageOpsBL.GetSparePartsRemovedByServiceId(serviceId);
        }

        public void SaveAddedSparePartRemovals(IServiceProcessor serviceProcessor)
        {
            if (SparePartRemovals == null) return;
            //setting the service details for each item
            foreach (var sparePartUsage in SparePartRemovals)
            {
                sparePartUsage.RemovalServiceId = serviceProcessor.Service.Id;
                SparePartUsageOpsBL.UpdateSparePartUsage(sparePartUsage);
            }
        }

        public SparePartUsage FindSparePartRemoval(string sparePartSerialNumber)
        {
            var sparePartUsage = new SparePartUsage()
            {
                SparePartItemSerialNumber = sparePartSerialNumber
            };

            var searchResult = SparePartRemovals.BinarySearch(sparePartUsage);
            if (searchResult >= 0)
            {
                var sparePartRemoval = SparePartRemovals[searchResult];
                if (sparePartRemoval.RemovalServiceId > 0)
                {
                    return sparePartRemoval;
                }
                else
                {
                    throw new RecordNotFoundException("SparePart removal "
                                                      + sparePartSerialNumber + " not found in sparePart removed processor list");
                }
            }
            else
            {
                throw new RecordNotFoundException("SparePart removal "
                                                  + sparePartSerialNumber + " not found in sparePart removed processor list");
            }
        }
    }
}