using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;

namespace BusinessLayer
{
    public class SparePartUsageHealthCalculator
    {
        public List<SparePartUsageHealth> CalculateSparePartUsageHealths(List<SparePartUsage> sparePartUsages)
        {
            List<SparePartUsageHealth> sparePartUsageHealths = new List<SparePartUsageHealth>();
            foreach (var sparePartUsage in sparePartUsages)
            {
                SparePartUsageHealth sparePartHealth = new SparePartUsageHealth {SparePartUsage = sparePartUsage};
                loadAllSparePartInfo(sparePartUsage);

                var serviceId = sparePartUsage.ServiceId;
                var sparePartInstallDate = ServiceOpsBL.GetServiceById(serviceId).Date;
                var today = DateTime.Now;
                var sparePartLifeSpanMonths = sparePartUsage.SparePartItem.SparePart.LifeSpanMonths;

                var sparePartExpiryDate = sparePartInstallDate.AddMonths(sparePartLifeSpanMonths);
                sparePartHealth.ExpiryDate = sparePartExpiryDate;

                double totalDaysInLife = (sparePartExpiryDate - sparePartInstallDate).Days;
                double remainingDaysInLife = (sparePartExpiryDate - today).Days;

                if (remainingDaysInLife < 1)
                {
                    sparePartHealth.HealthPercentage = 0;
                }
                else
                {
                    var healthPercentage = (remainingDaysInLife / totalDaysInLife) * 100;
                    sparePartHealth.HealthPercentage = Math.Round(healthPercentage, 2);
                }

                sparePartUsageHealths.Add(sparePartHealth);
            }

            return sparePartUsageHealths;
        }


        private SparePartUsage loadAllSparePartInfo(SparePartUsage sparePartUsage)
        {
            var sparePartItem = SparePartItemOpsBL
                .GetSparePartItemBySerialNumber(sparePartUsage.SparePartItemSerialNumber);
            var sparePart = SparePartOpsBL.GetSparePartByModel(sparePartItem.SparePartModelNumber);
            sparePartItem.SparePart = sparePart;
            sparePartUsage.Service = ServiceOpsBL.GetServiceById(sparePartUsage.ServiceId);
            sparePartUsage.SparePartItem = sparePartItem;

            return sparePartUsage;
        }
//        private int GetSparePartLifeSpan(string sparePartItemSerialNumber)
//        {
//            var sparePartItem = SparePartItemOpsBL
//                .GetSparePartItemBySerialNumber(sparePartItemSerialNumber);
//            var sparePart = SparePartOpsBL.GetSparePartByModel(sparePartItem.SparePartModelNumber);
//            return sparePart.LifeSpanMonths;
//        }
    }
}