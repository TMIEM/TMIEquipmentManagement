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
    public class SupplierOpsDAL
    {
        public static void AddNewSupplier(Supplier supplier)
        {
            //making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //calling sp
            SqlCommand sqlCommand = new SqlCommand("usp_supplierInsert", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@name", supplier.Name);
            sqlCommand.Parameters.AddWithValue("@address", supplier.Address);
            sqlCommand.Parameters.AddWithValue("@telephone", supplier.Telephone);
            sqlCommand.Parameters.AddWithValue("@fax", supplier.Fax);
            sqlCommand.Parameters.AddWithValue("@email", supplier.Email);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }

        public static List<Supplier> GetAllSuppliers()
        {
            List<Supplier> suppliers = new List<Supplier>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_supplierSelectAll", sqlConnection);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating place list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var supplier = new Supplier()
                {
                    Id = Convert.ToInt32(dataRow["id"].ToString()),
                    Name = dataRow["name"].ToString(),
                    Address = dataRow["address"].ToString(),
                    Telephone = dataRow["telephone"].ToString(),
                    Fax = dataRow["fax"].ToString(),
                    Email = dataRow["email"].ToString(),
                };
                suppliers.Add(supplier);
            }

            return suppliers;
        }

        public static Supplier GetSupplierById(int supplierId)
        {
            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_supplierSelect", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@id", supplierId);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //close connection
            connection.CloseSqlConnection(sqlConnection);

            //if supplier is not found, throw exception across the business layer to the web layer and then display error
            if (dataTable.Rows.Count <= 0)
            {
                throw new RecordNotFoundException("Supplier " + supplierId + " was not found");
            }

            var supplier = new Supplier()
            {
                Id = Convert.ToInt32(dataTable.Rows[0]["id"].ToString()),
                Name = dataTable.Rows[0]["name"].ToString(),
                Address = dataTable.Rows[0]["address"].ToString(),
                Telephone = dataTable.Rows[0]["telephone"].ToString(),
                Fax = dataTable.Rows[0]["fax"].ToString(),
                Email = dataTable.Rows[0]["email"].ToString(),
            };

            return supplier;
        }


        public static void UpdateSupplier(Supplier supplier)
        {
            //make connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);
            SqlCommand sqlCommand = new SqlCommand("usp_supplierUpdate", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            //call sp
            sqlCommand.Parameters.AddWithValue("@id", supplier.Id);
            sqlCommand.Parameters.AddWithValue("@name", supplier.Name);
            sqlCommand.Parameters.AddWithValue("@address", supplier.Address);
            sqlCommand.Parameters.AddWithValue("@telephone", supplier.Telephone);
            sqlCommand.Parameters.AddWithValue("@fax", supplier.Fax);
            sqlCommand.Parameters.AddWithValue("@email", supplier.Email);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }
    }
}
