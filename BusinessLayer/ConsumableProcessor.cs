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
	public class ConsumableProcessor : IConsumableProcessor
	{
		public List<ConsumableBatch> ConsumableBatches { get; set; }
		public void AddConsumableBatch(ConsumableBatch consumableBatch, bool isEditing)
		{
			ConsumableBatches = ConsumableBatches ?? new List<ConsumableBatch>();
			consumableBatch.ShipmentPoNumber = consumableBatch.ShipmentPoNumber ?? "";

			//checking for duplicate serial number
			if (!IsBatchDuplicate(consumableBatch))
			{
				ConsumableBatches.Add(consumableBatch);
				ConsumableBatches.Sort();
				//adding the item directly to the DB if an item is added to the shipment when editing
				if (isEditing)
				{
					ConsumableBatchOpsBL.AddNewConsumableBatch(consumableBatch);
				}
			}
			else
			{
				throw new DuplicateRecordIdentifierException("ID " + consumableBatch.ShipmentPoNumber + 
															 ", "+consumableBatch.ConsumableModelNumber+" is already in use.");
			}
		}

		private bool IsBatchDuplicate(ConsumableBatch consumableBatch)
		{
			var binarySearchResult = ConsumableBatches.BinarySearch(consumableBatch);
			if (binarySearchResult >= 0)
			{
				return true;
			}

			//checking in the database
			try
			{ 
				var consumableBatchBySerialNumber =
					ConsumableBatchOpsBL.GetConsumableBatchById(consumableBatch.ConsumableModelNumber, 
						consumableBatch.ShipmentPoNumber ?? "");
				return true;

			}
			catch (RecordNotFoundException e)
			{
				return false;
			}
		}

		public void UpdateConsumableBatch(ConsumableBatch consumableBatch)
		{
			var itemIndex = ConsumableBatches.BinarySearch(consumableBatch);
			//if found
			if (itemIndex >= 0)
			{
				//replace existing item with updated item
				ConsumableBatches[itemIndex] = consumableBatch;
			}

			try
			{
				//checking if item is already in database
				var consumableBatchBySerialNumber =
					ConsumableBatchOpsBL.GetConsumableBatchById(consumableBatch.ConsumableModelNumber, 
						consumableBatch.ShipmentPoNumber);

				//updating equipment item in db
				ConsumableBatchOpsBL.UpdateConsumableBatch(consumableBatch);
			}
			catch (RecordNotFoundException e)
			{
				//do nothing if item is not in DB
			}
		}

		public void RemoveConsumableBatch(ConsumableBatch consumableBatch)
		{
			//search item in DB
			try
			{
				var consumableBatchBySerialNumber
					= ConsumableBatchOpsBL.GetConsumableBatchById(consumableBatch.ConsumableModelNumber, consumableBatch.ShipmentPoNumber);

				//continue to execute delete operation if equipment is found
				//deleting an equipment which is referred to by other tables should not be allowed
				//the operation will result in an exception which is assumed to be the foreign key violation exception
				//in such an event, the exception is thrown to client code, 
				//so that appropriate error message can be displayed on front end
				try
				{
					ConsumableBatchOpsBL.DeleteConsumableBatch(consumableBatch);
					//if deletion from DB succeeds, local deletion is also performed
					RemoveConsumableBatchFromList(consumableBatch);
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
				RemoveConsumableBatchFromList(consumableBatch);
			}
		}

		private void RemoveConsumableBatchFromList(ConsumableBatch consumableBatch)
		{
			//search and remove equipment item
			var itemIndex = ConsumableBatches.BinarySearch(consumableBatch);
			if (itemIndex >= 0)
			{
				ConsumableBatches.RemoveAt(itemIndex);
			}
		}

		public void LoadConsumableBatches(string shipmentPoNumber)
		{
			ConsumableBatches = ConsumableBatchOpsBL.GetConsumableBatchesByShipment(shipmentPoNumber);
		}

		public void SaveAddedConsumableBatchs(IShipmentProcessor shipmentProcessor)
		{
			if (ConsumableBatches == null) return;
			//setting the shipment details for each item
			foreach (var consumableBatch in ConsumableBatches)
			{
				consumableBatch.ShipmentPoNumber = shipmentProcessor.Shipment.PoNumber;
				ConsumableBatchOpsBL.AddNewConsumableBatch(consumableBatch);
			}
		}

		public ConsumableBatch FindConsumableBatch(string consumableModelNumber, string shipmentPoNo)
		{
			var consumableBatch = new ConsumableBatch()
			{
				ConsumableModelNumber = consumableModelNumber,
				ShipmentPoNumber = shipmentPoNo
			};

			var searchResult = ConsumableBatches.BinarySearch(consumableBatch);
			if (searchResult >= 0)
			{
				return ConsumableBatches[searchResult];
			}
			else
			{
				throw new RecordNotFoundException("Consumable Batch "
												  + consumableModelNumber+" not found in consumable processor list");
			}
		}
	}
}
