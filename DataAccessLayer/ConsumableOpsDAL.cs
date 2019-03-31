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
    public class ConsumableOpsDAL
    {
        public static void AddNewConsumable(Consumable consumable)
        {
            //making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //calling sp
            SqlCommand sqlCommand = new SqlCommand("usp_consumableInsert", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@model_number", consumable.ModelNumber);
            sqlCommand.Parameters.AddWithValue("@make", consumable.Make);
            sqlCommand.Parameters.AddWithValue("@description", consumable.Description);
            sqlCommand.Parameters.AddWithValue("@life_span_days", consumable.LifeSpanDays);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }

        public static List<Consumable> GetAllConsumables()
        {
            List<Consumable> consumables = new List<Consumable>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_consumableSelectAll", sqlConnection);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating place list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var consumable = new Consumable()
                {
                    ModelNumber = dataRow["model_number"].ToString(),
                    Make = dataRow["make"].ToString(),
                    Description = dataRow["description"].ToString(),
                    LifeSpanDays = Convert.ToInt32(dataRow["life_span_days"].ToString())
                };
                consumables.Add(consumable);
            }

            return consumables;
        }

        public static Consumable GetConsumableByModelNumber(string modelNumber)
        {
            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_consumableSelect", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@model_number", modelNumber);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //close connection
            connection.CloseSqlConnection(sqlConnection);

            //if consumable is not found, throw exception across the business layer to the web layer and then display error
            if (dataTable.Rows.Count <= 0)
            {
                throw new RecordNotFoundException("Consumable " + modelNumber + " was not found");
            }

            var consumable = new Consumable()
            {
                ModelNumber = dataTable.Rows[0]["model_number"].ToString(),
                Make = dataTable.Rows[0]["make"].ToString(),
                Description = dataTable.Rows[0]["description"].ToString(),
                LifeSpanDays = Convert.ToInt32(dataTable.Rows[0]["life_span_days"].ToString())
            };

            return consumable;
        }


        public static void UpdateConsumable(Consumable consumable)
        {
            //make connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);
            SqlCommand sqlCommand = new SqlCommand("usp_consumableUpdate", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            //call sp
            sqlCommand.Parameters.AddWithValue("@model_number", consumable.ModelNumber);
            sqlCommand.Parameters.AddWithValue("@make", consumable.Make);
            sqlCommand.Parameters.AddWithValue("@description", consumable.Description);
            sqlCommand.Parameters.AddWithValue("@life_span_days", consumable.LifeSpanDays);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }


        public static void DeleteConsumable(Consumable consumable)
        {
            //make connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);
            SqlCommand sqlCommand = new SqlCommand("usp_consumableDelete", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            //call sp
            sqlCommand.Parameters.AddWithValue("@model_number", consumable.ModelNumber);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }

    }


}
