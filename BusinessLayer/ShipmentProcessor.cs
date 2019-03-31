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
    public class ShipmentProcessor : IShipmentProcessor
    {
        public IEquipmentProcessor EquipmentProcessor { get; set; }
        public ISparePartProcessor SparePartProcessor { get; set; }
        public IConsumableProcessor ConsumableProcessor { get; set; }
        public Shipment Shipment { get; set; }

        public void AddShipment()
        {
            if (Shipment == null) return;
            //checking if the client code has provided a PO number (primary key) which has been used to identify another shipment
            try
            {
                var shipmentByPoNumber = ShipmentOpsBL.GetShipmentByPoNumber(Shipment.PoNumber);
                if (shipmentByPoNumber != null)
                {
                    throw new DuplicateRecordIdentifierException(
                        "Shipment with PO number " + shipmentByPoNumber.PoNumber + " already exists");
                }
            }
            catch (RecordNotFoundException e)
            {
                ExecuteShipmentTransaction();
            }

        }

        private void ExecuteShipmentTransaction()
        {
            //using a transaction to call the persistance related methods of all items part of the shipment 
            //including the shipment itself
            try
            {
                using (var transactionScope = new TransactionScope())
                {
                    SaveShipmentDetails();
                    EquipmentProcessor.SaveAddedEquipmentItems(this);
                    SparePartProcessor.SaveAddedSparePartItems(this);
                    ConsumableProcessor.SaveAddedConsumableBatchs(this);
                    transactionScope.Complete();
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void SaveShipmentDetails()
        {
            var newShipment = ShipmentOpsBL.AddNewShipment(Shipment);
            this.Shipment = newShipment;
        }


        public void UpdateShipment()
        {
            ShipmentOpsBL.UpdateShipment(Shipment);
        }

        public void LoadShipmentDetails(string poNumber)
        {
            Shipment = ShipmentOpsBL.GetShipmentByPoNumber(poNumber);
            LoadItemDetails();
        }

        //loading the records for the dependant items in the shipment
        private void LoadItemDetails()
        {
            EquipmentProcessor.LoadEquipmentItems(Shipment.PoNumber);
            SparePartProcessor.LoadSparePartItems(Shipment.PoNumber);
            ConsumableProcessor.LoadConsumableBatches(Shipment.PoNumber);
        }
    }
}
