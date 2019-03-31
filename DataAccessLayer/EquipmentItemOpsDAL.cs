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
    public class EquipmentItemOpsDAL
    {
        public static void AddNewEquipmentItem(EquipmentItem equipmentItem)
        {
            //making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //calling sp
            SqlCommand sqlCommand = new SqlCommand("usp_equipment_itemInsert", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@serial_number", equipmentItem.SerialNumber);
            sqlCommand.Parameters.AddWithValue("@price", equipmentItem.Price);
            sqlCommand.Parameters.AddWithValue("@shipmentpo_number", equipmentItem.ShipmentPoNumber);
            sqlCommand.Parameters.AddWithValue("@equipmentmodel_number", equipmentItem.EquipmentModelNumber);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }

        public static List<EquipmentItem> GetAllEquipmentItems()
        {
            List<EquipmentItem> equipmentItems = new List<EquipmentItem>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_equipment_itemSelectAll", sqlConnection);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating equipmentItem list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var equipmentItem = new EquipmentItem()
                {
                    SerialNumber = dataRow["serial_number"].ToString(),
                    Price = Convert.ToDouble(dataRow["price"].ToString()),
                    ShipmentPoNumber = dataRow["shipmentpo_number"].ToString(),
                    EquipmentModelNumber = dataRow["equipmentmodel_number"].ToString(),
                };
                equipmentItems.Add(equipmentItem);
            }

            return equipmentItems;
        }

        public static EquipmentItem GetEquipmentItemBySerialNumber(string equipmentItemSerialNumber)
        {
            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_equipment_itemSelect", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@serial_number", equipmentItemSerialNumber);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //close connection
            connection.CloseSqlConnection(sqlConnection);

            //if equipmentItem is not found, throw exception across the business layer to the web layer and then display error
            if (dataTable.Rows.Count <= 0)
            {
                throw new RecordNotFoundException("EquipmentItem " + equipmentItemSerialNumber + " was not found");
            }

            var equipmentItem = new EquipmentItem()
            {
                SerialNumber = dataTable.Rows[0]["serial_number"].ToString(),
                Price = Convert.ToDouble(dataTable.Rows[0]["price"].ToString()),
                ShipmentPoNumber = dataTable.Rows[0]["shipmentpo_number"].ToString(),
                EquipmentModelNumber = dataTable.Rows[0]["equipmentmodel_number"].ToString()
            };

            return equipmentItem;
        }


        public static void UpdateEquipmentItem(EquipmentItem equipmentItem)
        {
            //make connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);
            SqlCommand sqlCommand = new SqlCommand("usp_equipment_itemUpdate", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            //call sp
            sqlCommand.Parameters.AddWithValue("@serial_number", equipmentItem.SerialNumber);
            sqlCommand.Parameters.AddWithValue("@price", equipmentItem.Price);
            sqlCommand.Parameters.AddWithValue("@shipmentpo_number", equipmentItem.ShipmentPoNumber);
            sqlCommand.Parameters.AddWithValue("@equipmentmodel_number", equipmentItem.EquipmentModelNumber);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }


        public static List<EquipmentItem> GetEquipmentItemsByShipment(string shipmentPoNumber)
        {
            List<EquipmentItem> equipmentItems = new List<EquipmentItem>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_equipment_itemSelectByShipment", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@shipmentpo_number", shipmentPoNumber);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating equipmentItem list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var equipmentItem = new EquipmentItem()
                {
                    SerialNumber = dataRow["serial_number"].ToString(),
                    Price = Convert.ToDouble(dataRow["price"].ToString()),
                    ShipmentPoNumber = dataRow["shipmentpo_number"].ToString(),
                    EquipmentModelNumber = dataRow["equipmentmodel_number"].ToString(),
                };
                equipmentItems.Add(equipmentItem);
            }

            return equipmentItems;
        }


        public static void DeleteEquipmentItem(EquipmentItem equipmentItem)
        {
            //make connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);
            SqlCommand sqlCommand = new SqlCommand("usp_equipment_itemDelete", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            //call sp
            sqlCommand.Parameters.AddWithValue("@serial_number", equipmentItem.SerialNumber);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }

        public static List<EquipmentItem> GetAvailableEquipmentItems()
        {
            List<EquipmentItem> equipmentItems = new List<EquipmentItem>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_equipment_itemSelectAvailable", sqlConnection);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating equipmentItem list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var equipmentItem = new EquipmentItem()
                {
                    SerialNumber = dataRow["serial_number"].ToString(),
                    Price = Convert.ToDouble(dataRow["price"].ToString()),
                    ShipmentPoNumber = dataRow["shipmentpo_number"].ToString(),
                    EquipmentModelNumber = dataRow["equipmentmodel_number"].ToString(),
                };
                equipmentItems.Add(equipmentItem);
            }

            return equipmentItems;
        }

    }
}
