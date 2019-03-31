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
    public class TechnicianOpsDAL
    {
        public static void AddNewTechnician(Technician technician)
        {
            //making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //calling sp
            SqlCommand sqlCommand = new SqlCommand("usp_technicianInsert", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@name", technician.Name);
            sqlCommand.Parameters.AddWithValue("@contact_number", technician.ContactNumber);
            sqlCommand.Parameters.AddWithValue("@email", technician.Email);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }

        public static List<Technician> GetAllTechnicians()
        {
            List<Technician> technicians = new List<Technician>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_technicianSelectAll", sqlConnection);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating place list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var technician = new Technician()
                {
                    Id = Convert.ToInt32(dataRow["id"].ToString()),
                    Name = dataRow["name"].ToString(),
                    ContactNumber = dataRow["contact_number"].ToString(),
                    Email = dataRow["email"].ToString(),
                };
                technicians.Add(technician);
            }

            return technicians;
        }

        public static Technician GetTechnicianById(int technicianId)
        {
            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_technicianSelect", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@id", technicianId);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //close connection
            connection.CloseSqlConnection(sqlConnection);

            //if supplier is not found, throw exception across the business layer to the web layer and then display error
            if (dataTable.Rows.Count <= 0)
            {
                throw new RecordNotFoundException("Technician " + technicianId + " was not found");
            }

            var technician = new Technician()
            {
                Id = Convert.ToInt32(dataTable.Rows[0]["id"].ToString()),
                Name = dataTable.Rows[0]["name"].ToString(),
                ContactNumber = dataTable.Rows[0]["contact_number"].ToString(),
                Email = dataTable.Rows[0]["email"].ToString(),
            };

            return technician;
        }


        public static void UpdateTechnician(Technician technician)
        {
            //make connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);
            SqlCommand sqlCommand = new SqlCommand("usp_technicianUpdate", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            //call sp
            sqlCommand.Parameters.AddWithValue("@id", technician.Id);
            sqlCommand.Parameters.AddWithValue("@name", technician.Name);
            sqlCommand.Parameters.AddWithValue("@contact_number", technician.ContactNumber);
            sqlCommand.Parameters.AddWithValue("@email", technician.Email);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);

        }
    }
}
