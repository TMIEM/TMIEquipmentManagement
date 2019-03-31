using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;

namespace BusinessLayer
{
    public class ConsumableBatchHealthCalculator
    {
        public List<ConsumableBatchHealth> CalculateConsumableBatchHealths(List<ConsumableBatchServiceUsage> consumableUsages)
        {
            List<ConsumableBatchHealth> consumableBatchHealths = new List<ConsumableBatchHealth>();
            foreach (var consumableUsage in consumableUsages)
            {
                ConsumableBatchHealth consumableHealth = new ConsumableBatchHealth { ConsumableBatchUsage = consumableUsage };
                loadAllConsumableBatchInfo(consumableUsage);

                var serviceId = consumableUsage.ServiceId;
                var consumableInstallDate = ServiceOpsBL.GetServiceById(serviceId).Date;
                var today = DateTime.Now;
                var consumableLifeSpanDays = consumableUsage.ConsumableBatch.Consumable.LifeSpanDays;

                var consumableExpiryDate = consumableInstallDate.AddDays(consumableLifeSpanDays);
                consumableHealth.ExpiryDate = consumableExpiryDate;

                double totalDaysInLife = (consumableExpiryDate - consumableInstallDate).Days;
                double remainingDaysInLife = (consumableExpiryDate - today).Days;

                if (remainingDaysInLife < 1)
                {
                    consumableHealth.HealthPercentage = 0;
                }
                else
                {
                    var healthPercentage = (remainingDaysInLife / totalDaysInLife) * 100;
                    consumableHealth.HealthPercentage = Math.Round(healthPercentage, 2);
                }

                consumableBatchHealths.Add(consumableHealth);
            }

            return consumableBatchHealths;
        }


        private ConsumableBatchServiceUsage loadAllConsumableBatchInfo(ConsumableBatchServiceUsage consumableBatchServiceUsage)
        {

            consumableBatchServiceUsage.ConsumableBatch = ConsumableBatchOpsBL.GetConsumableBatchById(consumableBatchServiceUsage.ConsumableBatchModelNumber,
                consumableBatchServiceUsage.ConsumbaleBatchShipmentPONumber);
            consumableBatchServiceUsage.Service = ServiceOpsBL.GetServiceById(consumableBatchServiceUsage.ServiceId);
            consumableBatchServiceUsage.Shipment =
                ShipmentOpsBL.GetShipmentByPoNumber(consumableBatchServiceUsage.ConsumbaleBatchShipmentPONumber);
            consumableBatchServiceUsage.ConsumableBatch.Consumable =
                ConsumableOpsBL.GetConsumableByModelNumber(consumableBatchServiceUsage.ConsumableBatch
                    .ConsumableModelNumber);
            return consumableBatchServiceUsage;
        }
    }
}
