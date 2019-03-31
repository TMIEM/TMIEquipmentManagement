using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Exception
{
    public class ConsumableUsageQuantityExceededException : SystemException
    {
        public string ErrorMessage { get; set; }
        public string ShipmentNo { get; set; }
        public string ModelNumber { get; set; }

        public ConsumableUsageQuantityExceededException(string errorMessage, string shipmentNo, string modelNumber)
        {
            ErrorMessage = errorMessage;
            ShipmentNo = shipmentNo;
            ModelNumber = modelNumber;
        }
    }
}