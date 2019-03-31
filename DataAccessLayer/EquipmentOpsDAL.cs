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
    public class EquipmentOpsDAL
    {
        public static void AddNewEquipment(Equipment equipment)
        {
            //making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //calling sp
            SqlCommand sqlCommand = new SqlCommand("usp_equipmentInsert", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@model_number", equipment.ModelNumber);
            sqlCommand.Parameters.AddWithValue("@make", equipment.Make);
            sqlCommand.Parameters.AddWithValue("@description", equipment.Description);
            sqlCommand.Parameters.AddWithValue("@version", equipment.Version);
            sqlCommand.Parameters.AddWithValue("@software_version", equipment.SoftwareVersion);
            sqlCommand.Parameters.AddWithValue("@minimum_service_period_months", equipment.MinimumServicePeriodMonths);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }

        public static List<Equipment> GetAllEquipments()
        {
            List<Equipment> equipments = new List<Equipment>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_equipmentSelectAll", sqlConnection);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            
            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating place list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var equipment = new Equipment()
                {
                    ModelNumber = dataRow["model_number"].ToString(),
                    Make = dataRow["make"].ToString(),
                    Description = dataRow["description"].ToString(),
                    Version = dataRow["version"].ToString(),
                    SoftwareVersion = dataRow["software_version"].ToString(),
                    MinimumServicePeriodMonths = Convert.ToInt32(dataRow["minimum_service_period_months"].ToString())
                };
                equipments.Add(equipment);
            }

            return equipments;
        }

        public static Equipment GetEquipmentByModel(string equipmentModel)
        {
            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_equipmentSelect", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@model_number", equipmentModel);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //close connection
            connection.CloseSqlConnection(sqlConnection);

            //if supplier is not found, throw exception across the business layer to the web layer and then display error
            if (dataTable.Rows.Count <= 0)
            {
                throw new RecordNotFoundException("Equipment " + equipmentModel + " was not found");
            }

            var equipment = new Equipment()
            {
                ModelNumber = dataTable.Rows[0]["model_number"].ToString(),
                Make = dataTable.Rows[0]["make"].ToString(),
                Description = dataTable.Rows[0]["description"].ToString(),
                Version = dataTable.Rows[0]["version"].ToString(),
                SoftwareVersion = dataTable.Rows[0]["software_version"].ToString(),
                MinimumServicePeriodMonths = Convert.ToInt32(dataTable.Rows[0]["minimum_service_period_months"].ToString())

            };

            return equipment;
        }


        public static void UpdateEquipment(Equipment equipment)
        {
            //make connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);
            SqlCommand sqlCommand = new SqlCommand("usp_equipmentUpdate", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            //call sp
            sqlCommand.Parameters.AddWithValue("@model_number", equipment.ModelNumber);
            sqlCommand.Parameters.AddWithValue("@make", equipment.Make);
            sqlCommand.Parameters.AddWithValue("@description", equipment.Description);
            sqlCommand.Parameters.AddWithValue("@version", equipment.Version);
            sqlCommand.Parameters.AddWithValue("@software_version", equipment.SoftwareVersion);
            sqlCommand.Parameters.AddWithValue("@minimum_service_period_months", equipment.MinimumServicePeriodMonths);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }
    }
}
