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
    public class ServiceResponseOpsDAL
    {
        public static void AddNewServiceResponseRequest(ServiceResponse serviceResponse)
        {
            //making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //calling sp
            SqlCommand sqlCommand = new SqlCommand("usp_service_responseInsert", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            sqlCommand.Parameters.AddWithValue("@date", serviceResponse.Date);
            sqlCommand.Parameters.AddWithValue("@service_description", serviceResponse.ServiceDescription);
            sqlCommand.Parameters.AddWithValue("@covered_by_warranty", serviceResponse.CoveredByWarranty);
            sqlCommand.Parameters.AddWithValue("@charges_paid", serviceResponse.ChargesPaid);
            sqlCommand.Parameters.AddWithValue("@service_requestid", serviceResponse.ServiceRequestId);
            var replacementItem = serviceResponse.ReplacementItem;
            if (replacementItem != null)
            {
                var itemType = replacementItem.GetItemType();
                switch (itemType)
                {
                    case ItemType.SparePart:
                        var sparePartItem = (SparePartItem)replacementItem;
                        sqlCommand.Parameters.AddWithValue("@replacement_spare_part_itemserial_number",
                            sparePartItem.SerialNumber);
                        break;
                    case ItemType.Equipment:
                        var equipmentItem = (EquipmentItem)replacementItem;
                        sqlCommand.Parameters.AddWithValue("@replacement_equipment_itemserial_number",
                            equipmentItem.SerialNumber);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

            }

            sqlCommand.ExecuteNonQuery();
            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }

        public static List<ServiceResponse> GetAllServiceResponses()
        {
            List<ServiceResponse> allServiceResponses = new List<ServiceResponse>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_service_responseSelectAll", sqlConnection);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating place list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var serviceResponse = new ServiceResponse()
                {
                    Id = Convert.ToInt32(dataRow["id"].ToString()),
                    Date = Convert.ToDateTime(dataRow["date"].ToString()),
                    ServiceDescription = dataRow["service_description"].ToString(),
                    CoveredByWarranty = dataRow["covered_by_warranty"].ToString(),
                    ChargesPaid = Convert.ToDouble(dataRow["charges_paid"].ToString()),
                    ServiceRequestId = Convert.ToInt32(dataRow["service_requestid"].ToString())
                };

                if (dataRow["replacement_spare_part_itemserial_number"] != DBNull.Value)
                {
                    var sparePartSerialNumber = dataRow["replacement_spare_part_itemserial_number"].ToString();
                    var sparePartItem = SparePartItemOpsDAL.GetSparePartItemBySerialNumber(sparePartSerialNumber);
                    serviceResponse.ReplacementItem = sparePartItem;
                }
                else if (dataRow["replacement_equipment_itemserial_number"] != DBNull.Value)
                {
                    var equipmentSerialNumber = dataRow["replacement_equipment_itemserial_number"].ToString();
                    var equipmentItem = EquipmentItemOpsDAL.GetEquipmentItemBySerialNumber(equipmentSerialNumber);
                    serviceResponse.ReplacementItem = equipmentItem;
                }
                allServiceResponses.Add(serviceResponse);
            }


            return allServiceResponses;
        }

        public static ServiceResponse GetServiceResponseById(int serviceResponseId)
        {

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_service_responseSelect", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@id", serviceResponseId);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            foreach (DataRow dataRow in dataTable.Rows)
            {
                var serviceResponse = new ServiceResponse()
                {
                    Id = Convert.ToInt32(dataRow["id"].ToString()),
                    Date = Convert.ToDateTime(dataRow["date"].ToString()),
                    ServiceDescription = dataRow["service_description"].ToString(),
                    CoveredByWarranty = dataRow["covered_by_warranty"].ToString(),
                    ChargesPaid = Convert.ToDouble(dataRow["charges_paid"].ToString()),
                    ServiceRequestId = Convert.ToInt32(dataRow["service_requestid"].ToString())
                };

                if (dataRow["replacement_spare_part_itemserial_number"] != DBNull.Value)
                {
                    var sparePartSerialNumber = dataRow["replacement_spare_part_itemserial_number"].ToString();
                    var sparePartItem = SparePartItemOpsDAL.GetSparePartItemBySerialNumber(sparePartSerialNumber);
                    serviceResponse.ReplacementItem = sparePartItem;
                }
                else if (dataRow["replacement_equipment_itemserial_number"] != DBNull.Value)
                {
                    var equipmentSerialNumber = dataRow["replacement_equipment_itemserial_number"].ToString();
                    var equipmentItem = EquipmentItemOpsDAL.GetEquipmentItemBySerialNumber(equipmentSerialNumber);
                    serviceResponse.ReplacementItem = equipmentItem;
                }

                return serviceResponse;
            }

            throw new RecordNotFoundException("Service Response " + serviceResponseId + " not found");
        }

        public static void UpdateServiceResponse(ServiceResponse serviceResponse)
        {
            //make connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);
            SqlCommand sqlCommand = new SqlCommand("usp_service_responseUpdate", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            //call sp
            sqlCommand.Parameters.AddWithValue("@id", serviceResponse.Id);
            sqlCommand.Parameters.AddWithValue("@date", serviceResponse.Date);
            sqlCommand.Parameters.AddWithValue("@service_description", serviceResponse.ServiceDescription);
            sqlCommand.Parameters.AddWithValue("@covered_by_warranty", serviceResponse.CoveredByWarranty);
            sqlCommand.Parameters.AddWithValue("@charges_paid", serviceResponse.ChargesPaid);
            sqlCommand.Parameters.AddWithValue("@service_requestid", serviceResponse.ServiceRequestId);

            var serviceItem = serviceResponse.ReplacementItem;
            if (serviceItem != null)
            {
                var itemType = serviceItem.GetItemType();
                switch (itemType)
                {
                    case ItemType.SparePart:
                        var sparePartItem = (SparePartItem)serviceItem;
                        sqlCommand.Parameters.AddWithValue("@replacement_spare_part_itemserial_number",
                            sparePartItem.SerialNumber);

                        break;
                    case ItemType.Equipment:
                        var equipmentItem = (EquipmentItem)serviceItem;
                        sqlCommand.Parameters.AddWithValue("@replacement_equipment_itemserial_number",
                            equipmentItem.SerialNumber);

                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
           
            

            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }


        public static List<ServiceResponse> GetAllServiceResponsesForEquipment(string serialNumber)
        {
            List<ServiceResponse> allServiceResponses = new List<ServiceResponse>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_service_responseSelectForEquipment", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@equipment_itemserial_number", serialNumber);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating place list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var serviceResponse = new ServiceResponse()
                {
                    Id = Convert.ToInt32(dataRow["id"].ToString()),
                    Date = Convert.ToDateTime(dataRow["date"].ToString()),
                    ServiceDescription = dataRow["service_description"].ToString(),
                    CoveredByWarranty = dataRow["covered_by_warranty"].ToString(),
                    ChargesPaid = Convert.ToDouble(dataRow["charges_paid"].ToString()),
                    ServiceRequestId = Convert.ToInt32(dataRow["service_requestid"].ToString())
                };

                if (dataRow["replacement_spare_part_itemserial_number"] != DBNull.Value)
                {
                    var sparePartSerialNumber = dataRow["replacement_spare_part_itemserial_number"].ToString();
                    var sparePartItem = SparePartItemOpsDAL.GetSparePartItemBySerialNumber(sparePartSerialNumber);
                    serviceResponse.ReplacementItem = sparePartItem;
                }
                else if (dataRow["replacement_equipment_itemserial_number"] != DBNull.Value)
                {
                    var equipmentSerialNumber = dataRow["replacement_equipment_itemserial_number"].ToString();
                    var equipmentItem = EquipmentItemOpsDAL.GetEquipmentItemBySerialNumber(equipmentSerialNumber);
                    serviceResponse.ReplacementItem = equipmentItem;
                }
                allServiceResponses.Add(serviceResponse);
            }


            return allServiceResponses;
        }

        public static List<ServiceResponse> GetAllServiceResponsesForSparePart(string serialNumber)
        {
            List<ServiceResponse> allServiceResponses = new List<ServiceResponse>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_service_responseSelectForSparePart", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@sparepart_itemserial_number", serialNumber);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating place list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var serviceResponse = new ServiceResponse()
                {
                    Id = Convert.ToInt32(dataRow["id"].ToString()),
                    Date = Convert.ToDateTime(dataRow["date"].ToString()),
                    ServiceDescription = dataRow["service_description"].ToString(),
                    CoveredByWarranty = dataRow["covered_by_warranty"].ToString(),
                    ChargesPaid = Convert.ToDouble(dataRow["charges_paid"].ToString()),
                    ServiceRequestId = Convert.ToInt32(dataRow["service_requestid"].ToString())
                };

                if (dataRow["replacement_spare_part_itemserial_number"] != DBNull.Value)
                {
                    var sparePartSerialNumber = dataRow["replacement_spare_part_itemserial_number"].ToString();
                    var sparePartItem = SparePartItemOpsDAL.GetSparePartItemBySerialNumber(sparePartSerialNumber);
                    serviceResponse.ReplacementItem = sparePartItem;
                }
                else if (dataRow["replacement_equipment_itemserial_number"] != DBNull.Value)
                {
                    var equipmentSerialNumber = dataRow["replacement_equipment_itemserial_number"].ToString();
                    var equipmentItem = EquipmentItemOpsDAL.GetEquipmentItemBySerialNumber(equipmentSerialNumber);
                    serviceResponse.ReplacementItem = equipmentItem;
                }
                allServiceResponses.Add(serviceResponse);
            }


            return allServiceResponses;
        }

        public static List<ServiceResponse> GetAllServiceResponsesForRequest(int serviceRequestId)
        {
            List<ServiceResponse> allServiceResponses = new List<ServiceResponse>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_service_responseSelectForRequest", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@service_requestid", serviceRequestId);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating place list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var serviceResponse = new ServiceResponse()
                {
                    Id = Convert.ToInt32(dataRow["id"].ToString()),
                    Date = Convert.ToDateTime(dataRow["date"].ToString()),
                    ServiceDescription = dataRow["service_description"].ToString(),
                    CoveredByWarranty = dataRow["covered_by_warranty"].ToString(),
                    ChargesPaid = Convert.ToDouble(dataRow["charges_paid"].ToString()),
                    ServiceRequestId = Convert.ToInt32(dataRow["service_requestid"].ToString())
                };

                if (dataRow["replacement_spare_part_itemserial_number"] != DBNull.Value)
                {
                    var sparePartSerialNumber = dataRow["replacement_spare_part_itemserial_number"].ToString();
                    var sparePartItem = SparePartItemOpsDAL.GetSparePartItemBySerialNumber(sparePartSerialNumber);
                    serviceResponse.ReplacementItem = sparePartItem;
                }
                else if (dataRow["replacement_equipment_itemserial_number"] != DBNull.Value)
                {
                    var equipmentSerialNumber = dataRow["replacement_equipment_itemserial_number"].ToString();
                    var equipmentItem = EquipmentItemOpsDAL.GetEquipmentItemBySerialNumber(equipmentSerialNumber);
                    serviceResponse.ReplacementItem = equipmentItem;
                }
                allServiceResponses.Add(serviceResponse);
            }


            return allServiceResponses;
        }
    }
}
