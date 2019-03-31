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
    public class SparePartOpsDAL
    {
        public static void AddNewSparePart(SparePart sparePart)
        {
            //making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //calling sp
            SqlCommand sqlCommand = new SqlCommand("usp_spare_partInsert", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@model_number", sparePart.ModelNumber);
            sqlCommand.Parameters.AddWithValue("@make", sparePart.Make);
            sqlCommand.Parameters.AddWithValue("@description", sparePart.Description);
            sqlCommand.Parameters.AddWithValue("@life_span_months", sparePart.LifeSpanMonths);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }

        public static List<SparePart> GetAllSpareParts()
        {
            List<SparePart> spareParts = new List<SparePart>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_spare_partSelectAll", sqlConnection);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating place list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var sparePart = new SparePart()
                {
                    ModelNumber = dataRow["model_number"].ToString(),
                    Make = dataRow["make"].ToString(),
                    Description = dataRow["description"].ToString(),
                    LifeSpanMonths = Convert.ToInt32(dataRow["life_span_months"].ToString())
                };
                spareParts.Add(sparePart);
            }

            return spareParts;
        }

        public static SparePart GetSparePartByModel(string sparePartModel)
        {
            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_spare_partSelect", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@model_number", sparePartModel);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //close connection
            connection.CloseSqlConnection(sqlConnection);

            //if spare part is not found, throw exception across the business layer to the web layer and then display error
            if (dataTable.Rows.Count <= 0)
            {
                throw new RecordNotFoundException("SparePart " + sparePartModel + " was not found");
            }

            var sparePart = new SparePart()
            {
                ModelNumber = dataTable.Rows[0]["model_number"].ToString(),
                Make = dataTable.Rows[0]["make"].ToString(),
                Description = dataTable.Rows[0]["description"].ToString(),
                LifeSpanMonths = Convert.ToInt32(dataTable.Rows[0]["life_span_months"].ToString())

            };

            return sparePart;
        }


        public static void UpdateSparePart(SparePart sparePart)
        {
            //make connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);
            SqlCommand sqlCommand = new SqlCommand("usp_spare_partUpdate", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            //call sp
            sqlCommand.Parameters.AddWithValue("@model_number", sparePart.ModelNumber);
            sqlCommand.Parameters.AddWithValue("@make", sparePart.Make);
            sqlCommand.Parameters.AddWithValue("@description", sparePart.Description);
            sqlCommand.Parameters.AddWithValue("@life_span_months", sparePart.LifeSpanMonths);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }
    }
}
