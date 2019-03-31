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
    public class ConsumableBatchServiceUsageOpsDAL
    {
        public static void AddNewConsumableBatchServiceUsage(ConsumableBatchServiceUsage consumableBatchServiceUsage)
        {
            //making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //calling sp
            SqlCommand sqlCommand = new SqlCommand("usp_consumable_batch_service_usageInsert", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@consumable_batchshipmentpo_number",
                consumableBatchServiceUsage.ConsumbaleBatchShipmentPONumber);
            sqlCommand.Parameters.AddWithValue("@consumable_batchconsumablemodel_number",
                consumableBatchServiceUsage.ConsumableBatchModelNumber);
            sqlCommand.Parameters.AddWithValue("@serviceid", consumableBatchServiceUsage.ServiceId);
            sqlCommand.Parameters.AddWithValue("@quantity_used", consumableBatchServiceUsage.QuantityUsed);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }

        public static List<ConsumableBatchServiceUsage> GetAllConsumableBatchServiceUsages()
        {
            List<ConsumableBatchServiceUsage> consumableBatchServiceUsages = new List<ConsumableBatchServiceUsage>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_consumable_batch_service_usageSelectAll", sqlConnection);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating consumableBatchServiceUsage list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var consumableBatchServiceUsage = new ConsumableBatchServiceUsage()
                {
                    ConsumbaleBatchShipmentPONumber = dataRow["consumable_batchshipmentpo_number"].ToString(),
                    ConsumableBatchModelNumber = dataRow["consumable_batchconsumablemodel_number"].ToString(),
                    ServiceId = Convert.ToInt32(dataRow["serviceid"].ToString()),
                    QuantityUsed = Convert.ToInt32(dataRow["quantity_used"].ToString()),
                    RemovalServiceId =
                        Convert.ToInt32(dataRow["removal_serviceid"] == DBNull.Value
                            ? "0"
                            : dataRow["removal_serviceid"])
                };
                consumableBatchServiceUsages.Add(consumableBatchServiceUsage);
            }

            return consumableBatchServiceUsages;
        }

        public static ConsumableBatchServiceUsage GetConsumableBatchServiceUsageById(string shipmentPoNumber,
            string modelNumber, int serviceId)
        {
            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_consumable_batch_service_usageSelect", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@consumable_batchshipmentpo_number",
                shipmentPoNumber);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@consumable_batchconsumablemodel_number",
                modelNumber);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@serviceid", serviceId);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //close connection
            connection.CloseSqlConnection(sqlConnection);

            //if consumableBatchServiceUsage is not found, throw exception across the business layer to the web layer and then display error
            if (dataTable.Rows.Count <= 0)
            {
                throw new RecordNotFoundException("ConsumableBatchServiceUsage " + shipmentPoNumber + ", " +
                                                  modelNumber + ", " + serviceId +
                                                  " was not found");
            }

            var consumableBatchServiceUsage = new ConsumableBatchServiceUsage()
            {
                ConsumbaleBatchShipmentPONumber = dataTable.Rows[0]["consumable_batchshipmentpo_number"].ToString(),
                ConsumableBatchModelNumber = dataTable.Rows[0]["consumable_batchconsumablemodel_number"].ToString(),
                ServiceId = Convert.ToInt32(dataTable.Rows[0]["serviceid"].ToString()),
                QuantityUsed = Convert.ToInt32(dataTable.Rows[0]["quantity_used"].ToString()),
                RemovalServiceId = Convert.ToInt32(dataTable.Rows[0]["removal_serviceid"] == DBNull.Value
                    ? "0"
                    : dataTable.Rows[0]["removal_serviceid"])
            };

            return consumableBatchServiceUsage;
        }


        public static void UpdateConsumableBatchServiceUsage(ConsumableBatchServiceUsage consumableBatchServiceUsage)
        {
            //make connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);
            SqlCommand sqlCommand = new SqlCommand("usp_consumable_batch_service_usageUpdate", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            //call sp
            sqlCommand.Parameters.AddWithValue("@consumable_batchshipmentpo_number",
                consumableBatchServiceUsage.ConsumbaleBatchShipmentPONumber);
            sqlCommand.Parameters.AddWithValue("@consumable_batchconsumablemodel_number",
                consumableBatchServiceUsage.ConsumableBatchModelNumber);
            sqlCommand.Parameters.AddWithValue("@serviceid", consumableBatchServiceUsage.ServiceId);
            sqlCommand.Parameters.AddWithValue("@quantity_used", consumableBatchServiceUsage.QuantityUsed);
            if (consumableBatchServiceUsage.RemovalServiceId > 0)
            {
                sqlCommand.Parameters.AddWithValue("@removal_serviceid", consumableBatchServiceUsage.RemovalServiceId);
            }
            else
            {
                sqlCommand.Parameters.AddWithValue("@removal_serviceid", DBNull.Value);
            }

            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }

        public static void DeleteConsumableBatchServiceUsage(ConsumableBatchServiceUsage consumableBatchServiceUsage)
        {
            //make connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);
            SqlCommand sqlCommand = new SqlCommand("usp_consumable_batch_service_usageDelete", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            //call sp
            sqlCommand.Parameters.AddWithValue("@consumable_batchshipmentpo_number",
                consumableBatchServiceUsage.ConsumbaleBatchShipmentPONumber);
            sqlCommand.Parameters.AddWithValue("@consumable_batchconsumablemodel_number",
                consumableBatchServiceUsage.ConsumableBatchModelNumber);
            sqlCommand.Parameters.AddWithValue("@serviceid", consumableBatchServiceUsage.ServiceId);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }

        public static List<ConsumableBatchServiceUsage> GetConsumableBatchServiceUsagesByServiceId(int serviceId)
        {
            List<ConsumableBatchServiceUsage> consumableBatchServiceUsages = new List<ConsumableBatchServiceUsage>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_consumable_batch_service_usageSelectByService", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@serviceid", serviceId);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating consumableBatchServiceUsage list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var consumableBatchServiceUsage = new ConsumableBatchServiceUsage()
                {
                    ConsumbaleBatchShipmentPONumber = dataRow["consumable_batchshipmentpo_number"].ToString(),
                    ConsumableBatchModelNumber = dataRow["consumable_batchconsumablemodel_number"].ToString(),
                    ServiceId = Convert.ToInt32(dataRow["serviceid"].ToString()),
                    QuantityUsed = Convert.ToInt32(dataRow["quantity_used"].ToString()),
                    RemovalServiceId =
                        Convert.ToInt32(dataRow["removal_serviceid"] == DBNull.Value
                            ? "0"
                            : dataRow["removal_serviceid"])
                };
                consumableBatchServiceUsages.Add(consumableBatchServiceUsage);
            }

            return consumableBatchServiceUsages;
        }

        public static List<ConsumableBatchServiceUsage> GetConsumableBatchServiceUsagesByBatch(string shipmentPoNumber,
            string modelNumber)
        {
            List<ConsumableBatchServiceUsage> consumableBatchServiceUsages = new List<ConsumableBatchServiceUsage>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_consumable_batch_service_usageSelectByBatch", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@consumable_batchshipmentpo_number",
                shipmentPoNumber);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@consumable_batchconsumablemodel_number",
                modelNumber);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating consumableBatchServiceUsage list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var consumableBatchServiceUsage = new ConsumableBatchServiceUsage()
                {
                    ConsumbaleBatchShipmentPONumber = dataRow["consumable_batchshipmentpo_number"].ToString(),
                    ConsumableBatchModelNumber = dataRow["consumable_batchconsumablemodel_number"].ToString(),
                    ServiceId = Convert.ToInt32(dataRow["serviceid"].ToString()),
                    QuantityUsed = Convert.ToInt32(dataRow["quantity_used"].ToString()),
                    RemovalServiceId =
                        Convert.ToInt32(dataRow["removal_serviceid"] == DBNull.Value
                            ? "0"
                            : dataRow["removal_serviceid"])
                };
                consumableBatchServiceUsages.Add(consumableBatchServiceUsage);
            }

            return consumableBatchServiceUsages;
        }

        public static List<ConsumableBatchServiceUsage> GetRemovableConsumableServiceUsagesByEquipment(
            int currentServiceId,
            string equipmentItemSerialNumber)
        {
            List<ConsumableBatchServiceUsage> consumableBatchServiceUsages = new List<ConsumableBatchServiceUsage>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter =
                new SqlDataAdapter("usp_consumable_batch_service_usageSelectRemovables", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@current_serviceid", currentServiceId);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@equipment_item_serial", equipmentItemSerialNumber);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating consumableBatchServiceUsage list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var consumableBatchServiceUsage = new ConsumableBatchServiceUsage()
                {
                    ConsumbaleBatchShipmentPONumber = dataRow["consumable_batchshipmentpo_number"].ToString(),
                    ConsumableBatchModelNumber = dataRow["consumable_batchconsumablemodel_number"].ToString(),
                    ServiceId = Convert.ToInt32(dataRow["serviceid"].ToString()),
                    QuantityUsed = Convert.ToInt32(dataRow["quantity_used"].ToString()),
                    RemovalServiceId =
                        Convert.ToInt32(dataRow["removal_serviceid"] == DBNull.Value
                            ? "0"
                            : dataRow["removal_serviceid"])
                };
                consumableBatchServiceUsages.Add(consumableBatchServiceUsage);
            }

            return consumableBatchServiceUsages;
        }

        public static List<ConsumableBatchServiceUsage> GetConsumablesRemovedByServiceId(int serviceId)
        {
            List<ConsumableBatchServiceUsage> consumableBatchServiceUsages = new List<ConsumableBatchServiceUsage>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter =
                new SqlDataAdapter("usp_consumable_batch_service_usageSelectRemovedInService", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@serviceid", serviceId);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating consumableBatchServiceUsage list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var consumableBatchServiceUsage = new ConsumableBatchServiceUsage()
                {
                    ConsumbaleBatchShipmentPONumber = dataRow["consumable_batchshipmentpo_number"].ToString(),
                    ConsumableBatchModelNumber = dataRow["consumable_batchconsumablemodel_number"].ToString(),
                    ServiceId = Convert.ToInt32(dataRow["serviceid"].ToString()),
                    QuantityUsed = Convert.ToInt32(dataRow["quantity_used"].ToString()),
                    RemovalServiceId =
                        Convert.ToInt32(dataRow["removal_serviceid"] == DBNull.Value
                            ? "0"
                            : dataRow["removal_serviceid"])
                };
                consumableBatchServiceUsages.Add(consumableBatchServiceUsage);
            }

            return consumableBatchServiceUsages;
        }


        public static List<ConsumableBatchServiceUsage> GetConsumableServiceUsagesByEquipment(
            string equipmentItemSerialNumber)
        {
            List<ConsumableBatchServiceUsage> consumableBatchServiceUsages = new List<ConsumableBatchServiceUsage>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter =
                new SqlDataAdapter("usp_consumable_batch_service_usageSelectUsageByEquipment", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@equipment_item_serial", equipmentItemSerialNumber);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating consumableBatchServiceUsage list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var consumableBatchServiceUsage = new ConsumableBatchServiceUsage()
                {
                    ConsumbaleBatchShipmentPONumber = dataRow["consumable_batchshipmentpo_number"].ToString(),
                    ConsumableBatchModelNumber = dataRow["consumable_batchconsumablemodel_number"].ToString(),
                    ServiceId = Convert.ToInt32(dataRow["serviceid"].ToString()),
                    QuantityUsed = Convert.ToInt32(dataRow["quantity_used"].ToString()),
                    RemovalServiceId =
                        Convert.ToInt32(dataRow["removal_serviceid"] == DBNull.Value
                            ? "0"
                            : dataRow["removal_serviceid"])
                };
                consumableBatchServiceUsages.Add(consumableBatchServiceUsage);
            }

            return consumableBatchServiceUsages;
        }

        public static List<ConsumableBatchServiceUsage> GetConsumableServiceUsagesByModel(
            string consumableModelNumber)
        {
            List<ConsumableBatchServiceUsage> consumableBatchServiceUsages = new List<ConsumableBatchServiceUsage>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter =
                new SqlDataAdapter("usp_consumable_batch_service_usageSelectUsageByModel", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@consumable_batchconsumablemodel_number", consumableModelNumber);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating consumableBatchServiceUsage list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var consumableBatchServiceUsage = new ConsumableBatchServiceUsage()
                {
                    ConsumbaleBatchShipmentPONumber = dataRow["consumable_batchshipmentpo_number"].ToString(),
                    ConsumableBatchModelNumber = dataRow["consumable_batchconsumablemodel_number"].ToString(),
                    ServiceId = Convert.ToInt32(dataRow["serviceid"].ToString()),
                    QuantityUsed = Convert.ToInt32(dataRow["quantity_used"].ToString()),
                    RemovalServiceId =
                        Convert.ToInt32(dataRow["removal_serviceid"] == DBNull.Value
                            ? "0"
                            : dataRow["removal_serviceid"])
                };
                consumableBatchServiceUsages.Add(consumableBatchServiceUsage);
            }

            return consumableBatchServiceUsages;
        }
    }
}