using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;

namespace BusinessLayer
{
    public class EquipmentItemHealthCalculator
    {
        public List<EquipmentItemHealth> CalcuEquipmentItemHealths(List<EquipmentInstallation> equipmentInstallations)
        {
            List<EquipmentItemHealth> equipmentItemHealths = new List<EquipmentItemHealth>();
            foreach (var equipmentInstallation in equipmentInstallations)
            {
                var installation = LoadAllEquipmentInstallationInfo(equipmentInstallation);
                EquipmentItemHealth equipmentItemHealth =
                    new EquipmentItemHealth() {EquipmentInstallation = installation};

                var startDate = installation.InstallationDate;

                try
                {
                    var lastServiceForEquipment
                        = ServiceOpsBL.GetLastServiceForEquipmentByDate(equipmentInstallation.EquipmentItemSerialNumber);
                    startDate = lastServiceForEquipment.Date;
                }
                catch (RecordNotFoundException exception)
                {
                    //if services have not been performed for the equipment, use the installation date as the start date
                }

                var today = DateTime.Now;
                var lifeSpanMonths = installation.EquipmentItem.Equipment.MinimumServicePeriodMonths;
                var expiryDate = startDate.AddMonths(lifeSpanMonths);
                equipmentItemHealth.ExpiryDate = expiryDate;
                double totalDaysInLife = (expiryDate - startDate).Days;
                double remainingDaysInLife = (expiryDate - today).Days;

                if (remainingDaysInLife < 1)
                {
                    equipmentItemHealth.HealthPercentage = 0;
                }
                else
                {
                    var healthPercentage = (remainingDaysInLife / totalDaysInLife) * 100;
                    equipmentItemHealth.HealthPercentage = Math.Round(healthPercentage, 2);
                }

                equipmentItemHealths.Add(equipmentItemHealth);
            }

            return equipmentItemHealths;
        }

        private EquipmentInstallation LoadAllEquipmentInstallationInfo(EquipmentInstallation equipmentInstallation)
        {
            var equipmentItem = EquipmentItemOpsBL
                .GetEquipmentItemBySerialNumber(equipmentInstallation.EquipmentItemSerialNumber);
            equipmentItem.Equipment = EquipmentOpsBL.GetEquipmentByModel(equipmentItem.EquipmentModelNumber);
            equipmentInstallation.EquipmentItem = equipmentItem;
            return equipmentInstallation;
        }
    }
}