using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityLayer;

namespace DataAccessLayer
{
    public class ServiceRequestOpsDAL
    {
        public static void AddNewServiceRequestRequest(ServiceRequest serviceRequest)
        {
            //making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //calling sp
            SqlCommand sqlCommand = new SqlCommand("usp_service_requestInsert", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.AddWithValue("@date", serviceRequest.Date);
            sqlCommand.Parameters.AddWithValue("@current_product_location", serviceRequest.CurrentProductLocation);
            sqlCommand.Parameters.AddWithValue("@type", serviceRequest.Type);
            sqlCommand.Parameters.AddWithValue("@under_warranty", serviceRequest.UnderWarranty);
            sqlCommand.Parameters.AddWithValue("@problem_details", serviceRequest.ProblemDetails);
            sqlCommand.Parameters.AddWithValue("@problem_occurrence_date", serviceRequest.ProblemOccurenceDate);
            sqlCommand.Parameters.AddWithValue("@problem_frequency_details", serviceRequest.ProblemFrequencyDetails);
            sqlCommand.Parameters.AddWithValue("@problem_reproduction_instructions", serviceRequest.ProblemReproductionInstructions);
            var serviceItem = serviceRequest.ServiceItem;
            var itemType = serviceItem.GetItemType();
            switch (itemType)
            {
                case ItemType.SparePart:
                    var sparePartItem = (SparePartItem) serviceItem;
                    sqlCommand.Parameters.AddWithValue("@spare_part_itemserial_number", 
                        sparePartItem.SerialNumber);
                    break;
                case ItemType.Equipment:
                    var equipmentItem = (EquipmentItem) serviceItem;
                    sqlCommand.Parameters.AddWithValue("@equipment_itemserial_number", 
                        equipmentItem.SerialNumber);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            sqlCommand.ExecuteNonQuery();
            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }

        public static List<ServiceRequest> GetAllServiceRequests()
        {
            List<ServiceRequest> services = new List<ServiceRequest>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_service_requestSelectAll", sqlConnection);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating place list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var serviceRequest = new ServiceRequest()
                {
                    Id = Convert.ToInt32(dataRow["id"].ToString()),
                    Date = Convert.ToDateTime(dataRow["date"].ToString()),
                    CurrentProductLocation = dataRow["current_product_location"].ToString(),
                    Type = dataRow["type"].ToString(),
                    UnderWarranty = dataRow["under_warranty"].ToString(),
                    ProblemDetails = dataRow["problem_details"].ToString(),
                    ProblemOccurenceDate = Convert.ToDateTime(dataRow["problem_occurrence_date"].ToString()),
                    ProblemFrequencyDetails = dataRow["problem_frequency_details"].ToString(),
                    ProblemReproductionInstructions = dataRow["problem_reproduction_instructions"].ToString()
                };

                if (dataRow["spare_part_itemserial_number"] != DBNull.Value)
                {
                    var sparePartSerialNumber = dataRow["spare_part_itemserial_number"].ToString();
                    var sparePartItem = SparePartItemOpsDAL.GetSparePartItemBySerialNumber(sparePartSerialNumber);
                    serviceRequest.ServiceItem = sparePartItem;
                }
                else if (dataRow["equipment_itemserial_number"] != DBNull.Value)
                {
                    var equipmentSerialNumber = dataRow["equipment_itemserial_number"].ToString();
                    var equipmentItem = EquipmentItemOpsDAL.GetEquipmentItemBySerialNumber(equipmentSerialNumber);
                    serviceRequest.ServiceItem = equipmentItem;
                }
                services.Add(serviceRequest);
            }


            return services;
        }

        public static ServiceRequest GetServiceRequestById(int serviceRequestId)
        {

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_service_requestSelect", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@id", serviceRequestId);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);
           
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var serviceRequest = new ServiceRequest()
                {
                    Id = Convert.ToInt32(dataRow["id"].ToString()),
                    Date = Convert.ToDateTime(dataRow["date"].ToString()),
                    CurrentProductLocation = dataRow["current_product_location"].ToString(),
                    Type = dataRow["type"].ToString(),
                    UnderWarranty = dataRow["under_warranty"].ToString(),
                    ProblemDetails = dataRow["problem_details"].ToString(),
                    ProblemOccurenceDate = Convert.ToDateTime(dataRow["problem_occurrence_date"].ToString()),
                    ProblemFrequencyDetails = dataRow["problem_frequency_details"].ToString(),
                    ProblemReproductionInstructions = dataRow["problem_reproduction_instructions"].ToString()
                };

                if (dataRow["spare_part_itemserial_number"] != DBNull.Value)
                {
                    var sparePartSerialNumber = dataRow["spare_part_itemserial_number"].ToString();
                    var sparePartItem = SparePartItemOpsDAL.GetSparePartItemBySerialNumber(sparePartSerialNumber);
                    serviceRequest.ServiceItem = sparePartItem;
                }
                else if (dataRow["equipment_itemserial_number"] != DBNull.Value)
                {
                    var equipmentSerialNumber = dataRow["equipment_itemserial_number"].ToString();
                    var equipmentItem = EquipmentItemOpsDAL.GetEquipmentItemBySerialNumber(equipmentSerialNumber);
                    serviceRequest.ServiceItem = equipmentItem;
                }

                return serviceRequest;
            }

            throw new RecordNotFoundException("Service request " + serviceRequestId + " not found");
        }

        public static void UpdateServiceRequest(ServiceRequest serviceRequest)
        {
            //make connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);
            SqlCommand sqlCommand = new SqlCommand("usp_service_requestUpdate", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            //call sp
            sqlCommand.Parameters.AddWithValue("@id", serviceRequest.Id);
            sqlCommand.Parameters.AddWithValue("@date", serviceRequest.Date);
            sqlCommand.Parameters.AddWithValue("@current_product_location", serviceRequest.CurrentProductLocation);
            sqlCommand.Parameters.AddWithValue("@type", serviceRequest.Type);
            sqlCommand.Parameters.AddWithValue("@under_warranty", serviceRequest.UnderWarranty);
            sqlCommand.Parameters.AddWithValue("@problem_details", serviceRequest.ProblemDetails);
            sqlCommand.Parameters.AddWithValue("@problem_occurrence_date", serviceRequest.ProblemOccurenceDate);
            sqlCommand.Parameters.AddWithValue("@problem_frequency_details", serviceRequest.ProblemFrequencyDetails);
            sqlCommand.Parameters.AddWithValue("@problem_reproduction_instructions", serviceRequest.ProblemReproductionInstructions);

            var serviceItem = serviceRequest.ServiceItem;
            var itemType = serviceItem.GetItemType();
            switch (itemType)
            {
                case ItemType.SparePart:
                    var sparePartItem = (SparePartItem)serviceItem;
                    sqlCommand.Parameters.AddWithValue("@spare_part_itemserial_number",
                        sparePartItem.SerialNumber);
                    break;
                case ItemType.Equipment:
                    var equipmentItem = (EquipmentItem)serviceItem;
                    sqlCommand.Parameters.AddWithValue("@equipment_itemserial_number",
                        equipmentItem.SerialNumber);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }



        public static List<ServiceRequest> GetAllServiceRequestsForEquipment(string serialNumber)
        {
            List<ServiceRequest> services = new List<ServiceRequest>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_service_requestSelectForEquipment", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@equipment_itemserial_number", serialNumber);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating place list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var serviceRequest = new ServiceRequest()
                {
                    Id = Convert.ToInt32(dataRow["id"].ToString()),
                    Date = Convert.ToDateTime(dataRow["date"].ToString()),
                    CurrentProductLocation = dataRow["current_product_location"].ToString(),
                    Type = dataRow["type"].ToString(),
                    UnderWarranty = dataRow["under_warranty"].ToString(),
                    ProblemDetails = dataRow["problem_details"].ToString(),
                    ProblemOccurenceDate = Convert.ToDateTime(dataRow["problem_occurrence_date"].ToString()),
                    ProblemFrequencyDetails = dataRow["problem_frequency_details"].ToString(),
                    ProblemReproductionInstructions = dataRow["problem_reproduction_instructions"].ToString()
                };

                if (dataRow["spare_part_itemserial_number"] != DBNull.Value)
                {
                    var sparePartSerialNumber = dataRow["spare_part_itemserial_number"].ToString();
                    var sparePartItem = SparePartItemOpsDAL.GetSparePartItemBySerialNumber(sparePartSerialNumber);
                    serviceRequest.ServiceItem = sparePartItem;
                }
                else if (dataRow["equipment_itemserial_number"] != DBNull.Value)
                {
                    var equipmentSerialNumber = dataRow["equipment_itemserial_number"].ToString();
                    var equipmentItem = EquipmentItemOpsDAL.GetEquipmentItemBySerialNumber(equipmentSerialNumber);
                    serviceRequest.ServiceItem = equipmentItem;
                }
                services.Add(serviceRequest);
            }


            return services;
        }

        public static List<ServiceRequest> GetAllServiceRequestsForSparePart(string serialNumber)
        {
            List<ServiceRequest> services = new List<ServiceRequest>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_service_requestSelectForSparePart", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@spare_part_itemserial_number", serialNumber);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating place list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var serviceRequest = new ServiceRequest()
                {
                    Id = Convert.ToInt32(dataRow["id"].ToString()),
                    Date = Convert.ToDateTime(dataRow["date"].ToString()),
                    CurrentProductLocation = dataRow["current_product_location"].ToString(),
                    Type = dataRow["type"].ToString(),
                    UnderWarranty = dataRow["under_warranty"].ToString(),
                    ProblemDetails = dataRow["problem_details"].ToString(),
                    ProblemOccurenceDate = Convert.ToDateTime(dataRow["problem_occurrence_date"].ToString()),
                    ProblemFrequencyDetails = dataRow["problem_frequency_details"].ToString(),
                    ProblemReproductionInstructions = dataRow["problem_reproduction_instructions"].ToString()
                };

                if (dataRow["spare_part_itemserial_number"] != DBNull.Value)
                {
                    var sparePartSerialNumber = dataRow["spare_part_itemserial_number"].ToString();
                    var sparePartItem = SparePartItemOpsDAL.GetSparePartItemBySerialNumber(sparePartSerialNumber);
                    serviceRequest.ServiceItem = sparePartItem;
                }
                else if (dataRow["equipment_itemserial_number"] != DBNull.Value)
                {
                    var equipmentSerialNumber = dataRow["equipment_itemserial_number"].ToString();
                    var equipmentItem = EquipmentItemOpsDAL.GetEquipmentItemBySerialNumber(equipmentSerialNumber);
                    serviceRequest.ServiceItem = equipmentItem;
                }
                services.Add(serviceRequest);
            }


            return services;
        }

    }
}
