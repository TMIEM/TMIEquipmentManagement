using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using EntityLayer;

namespace BusinessLayer
{
    public class ServiceResponseOpsBL
    {
        public static void AddNewServiceResponse(ServiceResponse serviceResponse)
        {
            ServiceResponseOpsDAL.AddNewServiceResponseRequest(serviceResponse);
        }

        public static List<ServiceResponse> GetAllServiceResponses()
        {
            return ServiceResponseOpsDAL.GetAllServiceResponses();
        }

        public static ServiceResponse GetServiceResponseById(int serviceResponseId)
        {
            return ServiceResponseOpsDAL.GetServiceResponseById(serviceResponseId);
        }

        public static void UpdateServiceResponse(ServiceResponse serviceResponse)
        {
            ServiceResponseOpsDAL.UpdateServiceResponse(serviceResponse);
        }

        public static List<ServiceResponse> GetAllServiceResponsesForEquipment(string serialNumber)
        {
            return ServiceResponseOpsDAL.GetAllServiceResponsesForEquipment(serialNumber);
        }

        public static List<ServiceResponse> GetAllServiceResponsesForSparePart(string serialNumber)
        {
            return ServiceResponseOpsDAL.GetAllServiceResponsesForSparePart(serialNumber);
        }

        public static List<ServiceResponse> GetAllServiceResponsesForRequest(int serviceRequestId)
        {
            return ServiceResponseOpsDAL.GetAllServiceResponsesForRequest(serviceRequestId);
        }
    }
}
