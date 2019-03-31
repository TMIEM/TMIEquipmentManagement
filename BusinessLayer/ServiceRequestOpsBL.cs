using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using EntityLayer;

namespace BusinessLayer
{
    public class ServiceRequestOpsBL
    {
        public static void AddNewServiceRequestRequest(ServiceRequest serviceRequest)
        {
            ServiceRequestOpsDAL.AddNewServiceRequestRequest(serviceRequest);
        }

        public static List<ServiceRequest> GetAllServiceRequests()
        {
            return ServiceRequestOpsDAL.GetAllServiceRequests();
        }

        public static ServiceRequest GetServiceRequestById(int serviceRequestId)
        {
            return ServiceRequestOpsDAL.GetServiceRequestById(serviceRequestId);
        }

        public static void UpdateServiceRequest(ServiceRequest serviceRequest)
        {
            ServiceRequestOpsDAL.UpdateServiceRequest(serviceRequest);
        }

        public static List<ServiceRequest> GetAllServiceRequestsForEquipment(string serialNumber)
        {
            return ServiceRequestOpsDAL.GetAllServiceRequestsForEquipment(serialNumber);
        }

        public static List<ServiceRequest> GetAllServiceRequestsForSparePart(string serialNumber)
        {
            return ServiceRequestOpsDAL.GetAllServiceRequestsForSparePart(serialNumber);
        }
    }
}
