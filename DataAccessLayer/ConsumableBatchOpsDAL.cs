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
    public class ConsumableBatchOpsDAL
    {
        public static void AddNewConsumableBatch(ConsumableBatch consumableBatch)
        {
            //making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //calling sp
            SqlCommand sqlCommand = new SqlCommand("usp_consumable_batchInsert", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@shipmentpo_number", consumableBatch.ShipmentPoNumber);
            sqlCommand.Parameters.AddWithValue("@consumablemodel_number", consumableBatch.ConsumableModelNumber);
            sqlCommand.Parameters.AddWithValue("@quantity", consumableBatch.Quantity);
            sqlCommand.Parameters.AddWithValue("@price", consumableBatch.Price);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }

        public static List<ConsumableBatch> GetAllConsumableBatchs()
        {
            List<ConsumableBatch> consumableBatchs = new List<ConsumableBatch>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_consumable_batchSelectAll", sqlConnection);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating consumableBatch list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var consumableBatch = new ConsumableBatch()
                {
                    ConsumableModelNumber = dataRow["consumablemodel_number"].ToString(),
                    ShipmentPoNumber = dataRow["shipmentpo_number"].ToString(),
                    Quantity = Convert.ToInt32(dataRow["quantity"].ToString()),
                    Price = Convert.ToDouble(dataRow["price"].ToString())
                };
                consumableBatchs.Add(consumableBatch);
            }

            return consumableBatchs;
        }

        public static ConsumableBatch GetConsumableBatchById(string consumableModelNumber, string shipmentPoNumber)
        {
            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_consumable_batchSelect", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@shipmentpo_number", shipmentPoNumber);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@consumablemodel_number", consumableModelNumber);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //close connection
            connection.CloseSqlConnection(sqlConnection);

            //if consumableBatch is not found, throw exception across the business layer to the web layer and then display error
            if (dataTable.Rows.Count <= 0)
            {
                throw new RecordNotFoundException("ConsumableBatch with model " + consumableModelNumber +
                                                  ", shipment " + shipmentPoNumber + " was not found");
            }

            var consumableBatch = new ConsumableBatch()
            {
                ShipmentPoNumber = dataTable.Rows[0]["shipmentpo_number"].ToString(),
                ConsumableModelNumber = dataTable.Rows[0]["consumablemodel_number"].ToString(),
                Quantity = Convert.ToInt32(dataTable.Rows[0]["quantity"].ToString()),
                Price = Convert.ToDouble(dataTable.Rows[0]["price"].ToString())
            };

            return consumableBatch;
        }


        public static void UpdateConsumableBatch(ConsumableBatch consumableBatch)
        {
            //make connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);
            SqlCommand sqlCommand = new SqlCommand("usp_consumable_batchUpdate", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            //call sp
            sqlCommand.Parameters.AddWithValue("@shipmentpo_number", consumableBatch.ShipmentPoNumber);
            sqlCommand.Parameters.AddWithValue("@consumablemodel_number", consumableBatch.ConsumableModelNumber);
            sqlCommand.Parameters.AddWithValue("@quantity", consumableBatch.Quantity);
            sqlCommand.Parameters.AddWithValue("@price", consumableBatch.Price);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }


        public static List<ConsumableBatch> GetConsumableBatchesByShipment(string shipmentPoNumber)
        {
            List<ConsumableBatch> consumableBatches = new List<ConsumableBatch>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_consumable_batchSelectByShipment", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@shipmentpo_number", shipmentPoNumber);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating consumableBatch list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var consumableBatch = new ConsumableBatch()
                {
                    ConsumableModelNumber = dataRow["consumablemodel_number"].ToString(),
                    ShipmentPoNumber = dataRow["shipmentpo_number"].ToString(),
                    Quantity = Convert.ToInt32(dataRow["quantity"].ToString()),
                    Price = Convert.ToDouble(dataRow["price"].ToString())
                };
                consumableBatches.Add(consumableBatch);
            }

            return consumableBatches;
        }

        public static List<ConsumableBatch> GetAvaialbleConsumableBatchs()
        {
            List<ConsumableBatch> consumableBatchs = new List<ConsumableBatch>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_consumable_batchSelectAvailable", sqlConnection);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating consumableBatch list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var modelNumber = dataRow["consumablemodel_number"].ToString();
                var shipmentPoNumber = dataRow["shipmentpo_number"].ToString();
                var batchQuantity = Convert.ToInt32(dataRow["quantity"].ToString());
                var price = Convert.ToDouble(dataRow["price"].ToString());
                var usedQuantity = 0;

                foreach (var consumableBatchServiceUsage in ConsumableBatchServiceUsageOpsDAL
                    .GetConsumableBatchServiceUsagesByBatch(shipmentPoNumber, modelNumber))
                {
                    usedQuantity += consumableBatchServiceUsage.QuantityUsed;
                }

                var availableQuantity = batchQuantity - usedQuantity;

                if (availableQuantity < 1) continue;

                var consumableBatch = new ConsumableBatch()
                {
                    ConsumableModelNumber = modelNumber,
                    ShipmentPoNumber = shipmentPoNumber,
                    Quantity = availableQuantity,
                    Price = price
                };
                consumableBatchs.Add(consumableBatch);
            }

            return consumableBatchs;
        }

        public static void DeleteConsumableBatch(ConsumableBatch consumableBatch)
        {
            //make connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);
            SqlCommand sqlCommand = new SqlCommand("usp_consumable_batchDelete", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            //call sp
            sqlCommand.Parameters.AddWithValue("@shipmentpo_number", consumableBatch.ShipmentPoNumber);
            sqlCommand.Parameters.AddWithValue("@consumablemodel_number", consumableBatch.ConsumableModelNumber);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }
    }
}