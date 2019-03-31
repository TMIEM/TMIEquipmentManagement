using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using EntityLayer;

namespace BusinessLayer
{
    public class ServiceOpsBL
    {
        public static Service AddNewService(Service service)
        {
            return ServiceOpsDAL.AddNewService(service);
        }

        public static List<Service> GetAllServices()
        {
            return ServiceOpsDAL.GetAllServices();
        }

        public static Service GetServiceById(int servicePoNumber)
        {
            try
            {
                return ServiceOpsDAL.GetServiceById(servicePoNumber);
            }
            catch (RecordNotFoundException e)
            {
                throw;
            }
        }

        public static void UpdateService(Service service)
        {
            ServiceOpsDAL.UpdateService(service);
        }

        public static List<Service> EagerLoad(List<Service> services)
        {
            return ServiceOpsDAL.EagerLoad(services);
        }

        public static Service EagerLoad(Service service)
        {
            return ServiceOpsDAL.EagerLoad(service);
        }

        public static Service GetLastServiceForEquipment(string equipmentItemSerial)
        {
            try
            {
                return ServiceOpsDAL.GetLastServiceForEquipment(equipmentItemSerial);
            }
            catch (RecordNotFoundException e)
            {
                throw;
            }
        }

        public static Service GetLastServiceForEquipmentByDate(string equipmentItemSerial)
        {
            return ServiceOpsDAL.GetLastServiceForEquipmentByDate(equipmentItemSerial);
        }

        public static List<Service> GetAllServicesForInstallation(EquipmentInstallation equipmentInstallation)
        {
            return ServiceOpsDAL.GetAllServicesForInstallation(equipmentInstallation);
        }
    }
}
