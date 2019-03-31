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
    public class SparePartUsageOpsDAL
    {
        public static void AddNewSparePartUsage(SparePartUsage sparePartUsage)
        {
            //making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //calling sp
            SqlCommand sqlCommand = new SqlCommand("usp_spare_part_usageInsert", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@spare_part_itemserial_number",
                sparePartUsage.SparePartItemSerialNumber);
            sqlCommand.Parameters.AddWithValue("@serviceid", sparePartUsage.ServiceId);
            sqlCommand.Parameters.AddWithValue("@warranty_period_months", sparePartUsage.WarrantyPeriodMonths);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }

        public static List<SparePartUsage> GetAllSparePartUsages()
        {
            List<SparePartUsage> sparePartUsages = new List<SparePartUsage>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_spare_part_usageSelectAll", sqlConnection);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating sparePartUsage list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var sparePartUsage = new SparePartUsage()
                {
                    SparePartItemSerialNumber = dataRow["spare_part_itemserial_number"].ToString(),
                    WarrantyPeriodMonths = Convert.ToInt32(dataRow["warranty_period_months"].ToString()),
                    ServiceId = Convert.ToInt32(dataRow["serviceid"].ToString()),
                    RemovalServiceId = dataRow["removal_serviceid"] == DBNull.Value
                        ? 0
                        : Convert.ToInt32(dataRow["removal_serviceid"])
                };
                sparePartUsages.Add(sparePartUsage);
            }

            return sparePartUsages;
        }

        public static SparePartUsage GetSparePartUsageBySerialNumber(string sparePartSerialNumber)
        {
            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_spare_part_usageSelect", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@spare_part_itemserial_number",
                sparePartSerialNumber);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //close connection
            connection.CloseSqlConnection(sqlConnection);

            //if sparePartUsage is not found, throw exception across the business layer to the web layer and then display error
            if (dataTable.Rows.Count <= 0)
            {
                throw new RecordNotFoundException("SparePartUsage " + sparePartSerialNumber + " was not found");
            }

            var sparePartUsage = new SparePartUsage()
            {
                SparePartItemSerialNumber = dataTable.Rows[0]["spare_part_itemserial_number"].ToString(),
                WarrantyPeriodMonths = Convert.ToInt32(dataTable.Rows[0]["warranty_period_months"].ToString()),
                ServiceId = Convert.ToInt32(dataTable.Rows[0]["serviceid"].ToString()),
                RemovalServiceId = dataTable.Rows[0]["removal_serviceid"] == DBNull.Value
                    ? 0
                    : Convert.ToInt32(dataTable.Rows[0]["removal_serviceid"])
            };

            return sparePartUsage;
        }


        public static void UpdateSparePartUsage(SparePartUsage sparePartUsage)
        {
            //make connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);
            SqlCommand sqlCommand = new SqlCommand("usp_spare_part_usageUpdate", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            //call sp
            sqlCommand.Parameters.AddWithValue("@spare_part_itemserial_number",
                sparePartUsage.SparePartItemSerialNumber);
            sqlCommand.Parameters.AddWithValue("@serviceid", sparePartUsage.ServiceId);
            sqlCommand.Parameters.AddWithValue("@warranty_period_months", sparePartUsage.WarrantyPeriodMonths);
            if (sparePartUsage.RemovalServiceId > 0)
            {
                sqlCommand.Parameters.AddWithValue("@removal_serviceid", sparePartUsage.RemovalServiceId);
            }
            else
            {
                sqlCommand.Parameters.AddWithValue("@removal_serviceid", DBNull.Value);
            }

            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }

        public static void DeleteSparePartUsage(SparePartUsage sparePartUsage)
        {
            //make connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);
            SqlCommand sqlCommand = new SqlCommand("usp_spare_part_usageDelete", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            //call sp
            sqlCommand.Parameters.AddWithValue("@spare_part_itemserial_number",
                sparePartUsage.SparePartItemSerialNumber);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }

        public static List<SparePartUsage> GetSparePartUsagesByServiceId(int serviceId)
        {
            List<SparePartUsage> sparePartUsages = new List<SparePartUsage>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_spare_part_usageSelectByService", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@serviceid", serviceId);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating sparePartUsage list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var sparePartUsage = new SparePartUsage()
                {
                    SparePartItemSerialNumber = dataRow["spare_part_itemserial_number"].ToString(),
                    WarrantyPeriodMonths = Convert.ToInt32(dataRow["warranty_period_months"].ToString()),
                    ServiceId = Convert.ToInt32(dataRow["serviceid"].ToString()),
                    RemovalServiceId = dataRow["removal_serviceid"] == DBNull.Value
                        ? 0
                        : Convert.ToInt32(dataRow["removal_serviceid"])
                };
                sparePartUsages.Add(sparePartUsage);
            }

            return sparePartUsages;
        }

        public static List<SparePartUsage> GetRemovableSparePartsByEquipment(int currentServiceId,
            string equipmentItemSerialNumber)
        {
            List<SparePartUsage> removableSparePartsByEquipment = new List<SparePartUsage>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_spare_part_usageSelectRemovables", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@current_serviceid", currentServiceId);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@equipment_item_serial", equipmentItemSerialNumber);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating sparePartUsage list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var sparePartUsage = new SparePartUsage()
                {
                    SparePartItemSerialNumber = dataRow["spare_part_itemserial_number"].ToString(),
                    WarrantyPeriodMonths = Convert.ToInt32(dataRow["warranty_period_months"].ToString()),
                    ServiceId = Convert.ToInt32(dataRow["serviceid"].ToString()),
                    RemovalServiceId = dataRow["removal_serviceid"] == DBNull.Value
                        ? 0
                        : Convert.ToInt32(dataRow["removal_serviceid"])
                };
                removableSparePartsByEquipment.Add(sparePartUsage);
            }

            return removableSparePartsByEquipment;
        }

        public static List<SparePartUsage> GetSparePartsRemovedByServiceId(int serviceId)
        {
            List<SparePartUsage> sparePartUsages = new List<SparePartUsage>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_spare_part_usageSelectRemovedInService", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@serviceid", serviceId);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating sparePartUsage list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var sparePartUsage = new SparePartUsage()
                {
                    SparePartItemSerialNumber = dataRow["spare_part_itemserial_number"].ToString(),
                    WarrantyPeriodMonths = Convert.ToInt32(dataRow["warranty_period_months"].ToString()),
                    ServiceId = Convert.ToInt32(dataRow["serviceid"].ToString()),
                    RemovalServiceId = dataRow["removal_serviceid"] == DBNull.Value
                        ? 0
                        : Convert.ToInt32(dataRow["removal_serviceid"])
                };
                sparePartUsages.Add(sparePartUsage);
            }

            return sparePartUsages;
        }

        public static List<SparePartUsage> GetSparePartUsagesByEquipment(string equipmentItemSerialNumber)
        {
            List<SparePartUsage> removableSparePartsByEquipment = new List<SparePartUsage>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_spare_part_usageSelectUsagesByEquipment", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@equipment_item_serial", equipmentItemSerialNumber);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating sparePartUsage list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var sparePartUsage = new SparePartUsage()
                {
                    SparePartItemSerialNumber = dataRow["spare_part_itemserial_number"].ToString(),
                    WarrantyPeriodMonths = Convert.ToInt32(dataRow["warranty_period_months"].ToString()),
                    ServiceId = Convert.ToInt32(dataRow["serviceid"].ToString()),
                    RemovalServiceId = dataRow["removal_serviceid"] == DBNull.Value
                        ? 0
                        : Convert.ToInt32(dataRow["removal_serviceid"])
                };
                removableSparePartsByEquipment.Add(sparePartUsage);
            }

            return removableSparePartsByEquipment;
        }
    }
}