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
    public class SparePartItemOpsDAL
    {
        public static void AddNewSparePartItem(SparePartItem sparePartItem)
        {
            //making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //calling sp
            SqlCommand sqlCommand = new SqlCommand("usp_spare_part_itemInsert", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@serial_number", sparePartItem.SerialNumber);
            sqlCommand.Parameters.AddWithValue("@price", sparePartItem.Price);
            sqlCommand.Parameters.AddWithValue("@shipmentpo_number", sparePartItem.ShipmentPoNumber);
            sqlCommand.Parameters.AddWithValue("@spare_partmodel_number", sparePartItem.SparePartModelNumber);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }

        public static List<SparePartItem> GetAllSparePartItems()
        {
            List<SparePartItem> sparePartItems = new List<SparePartItem>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_spare_part_itemSelectAll", sqlConnection);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating sparePartItem list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var sparePartItem = new SparePartItem()
                {
                    SerialNumber = dataRow["serial_number"].ToString(),
                    Price = Convert.ToDouble(dataRow["price"].ToString()),
                    ShipmentPoNumber = dataRow["shipmentpo_number"].ToString(),
                    SparePartModelNumber = dataRow["spare_partmodel_number"].ToString(),
                };
                sparePartItems.Add(sparePartItem);
            }

            return sparePartItems;
        }

        public static SparePartItem GetSparePartItemBySerialNumber(string sparePartItemSerialNumber)
        {
            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_spare_part_itemSelect", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@serial_number", sparePartItemSerialNumber);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //close connection
            connection.CloseSqlConnection(sqlConnection);

            //if sparePartItem is not found, throw exception across the business layer to the web layer and then display error
            if (dataTable.Rows.Count <= 0)
            {
                throw new RecordNotFoundException("SparePartItem " + sparePartItemSerialNumber + " was not found");
            }

            var sparePartItem = new SparePartItem()
            {
                SerialNumber = dataTable.Rows[0]["serial_number"].ToString(),
                Price = Convert.ToDouble(dataTable.Rows[0]["price"].ToString()),
                ShipmentPoNumber = dataTable.Rows[0]["shipmentpo_number"].ToString(),
                SparePartModelNumber = dataTable.Rows[0]["spare_partmodel_number"].ToString(),
            };

            return sparePartItem;
        }


        public static void UpdateSparePartItem(SparePartItem sparePartItem)
        {
            //make connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);
            SqlCommand sqlCommand = new SqlCommand("usp_spare_part_itemUpdate", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            //call sp
            sqlCommand.Parameters.AddWithValue("@serial_number", sparePartItem.SerialNumber);
            sqlCommand.Parameters.AddWithValue("@price", sparePartItem.Price);
            sqlCommand.Parameters.AddWithValue("@shipmentpo_number", sparePartItem.ShipmentPoNumber);
            sqlCommand.Parameters.AddWithValue("@spare_partmodel_number", sparePartItem.SparePartModelNumber);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }

        public static void DeleteSparePartItem(SparePartItem sparePartItem)
        {
            //make connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);
            SqlCommand sqlCommand = new SqlCommand("usp_spare_part_itemDelete", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            //call sp
            sqlCommand.Parameters.AddWithValue("@serial_number", sparePartItem.SerialNumber);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }

        public static List<SparePartItem> GetSparePartItemsByShipmentPoNumber(string shipmentPoNumber)
        {
            List<SparePartItem> sparePartItems = new List<SparePartItem>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_spare_part_itemSelectByShipment", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@shipmentpo_number", shipmentPoNumber);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating sparePartItem list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var sparePartItem = new SparePartItem()
                {
                    SerialNumber = dataRow["serial_number"].ToString(),
                    Price = Convert.ToDouble(dataRow["price"].ToString()),
                    ShipmentPoNumber = dataRow["shipmentpo_number"].ToString(),
                    SparePartModelNumber = dataRow["spare_partmodel_number"].ToString(),
                };
                sparePartItems.Add(sparePartItem);
            }

            return sparePartItems;
        }

        public static List<SparePartItem> GetAvailableSparePartItems()
        {
            List<SparePartItem> sparePartItems = new List<SparePartItem>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_spare_part_itemSelectAvailable", sqlConnection);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating sparePartItem list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var sparePartItem = new SparePartItem()
                {
                    SerialNumber = dataRow["serial_number"].ToString(),
                    Price = Convert.ToDouble(dataRow["price"].ToString()),
                    ShipmentPoNumber = dataRow["shipmentpo_number"].ToString(),
                    SparePartModelNumber = dataRow["spare_partmodel_number"].ToString(),
                };
                sparePartItems.Add(sparePartItem);
            }

            return sparePartItems;
        }
    }
}
