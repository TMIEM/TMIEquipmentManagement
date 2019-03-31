using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using EntityLayer;

namespace BusinessLayer
{
    public class ShipmentOpsBL
    {
        public static Shipment AddNewShipment(Shipment shipment)
        {
            return ShipmentOpsDAL.AddNewShipment(shipment);
        }

        public static List<Shipment> GetAllShipments()
        {
            return ShipmentOpsDAL.GetAllShipments();
        }

        public static Shipment GetShipmentByPoNumber(string shipmentPoNumber)
        {
            try
            {
                return ShipmentOpsDAL.GetShipmentByPoNumber(shipmentPoNumber);
            }
            catch (RecordNotFoundException e)
            {
                throw;
            }
        }

        public static void UpdateShipment(Shipment shipment)
        {
            ShipmentOpsDAL.UpdateShipment(shipment);
        }

    }
}
