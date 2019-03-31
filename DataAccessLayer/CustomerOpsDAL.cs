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
    public class CustomerOpsDAL
    {
        public static void AddNewCustomer(Customer customer)
        {
            //making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);
            
            //calling sp
            SqlCommand sqlCommand = new SqlCommand("usp_customerInsert", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@name", customer.Name);
            sqlCommand.Parameters.AddWithValue("@address", customer.Address);
            sqlCommand.Parameters.AddWithValue("@telephone", customer.Telephone);
            sqlCommand.Parameters.AddWithValue("@fax", customer.Fax);
            sqlCommand.Parameters.AddWithValue("@contact_person", customer.ContactPerson);
            sqlCommand.Parameters.AddWithValue("@contact_person_telephone", customer.ContactPersonTelephone);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }

        public static List<Customer> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_customerSelectAll", sqlConnection);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating place list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var customer = new Customer()
                {
                    Id = Convert.ToInt32(dataRow["id"].ToString()),
                    Name = dataRow["name"].ToString(),
                    Address = dataRow["address"].ToString(),
                    Telephone = dataRow["telephone"].ToString(),
                    Fax = dataRow["fax"].ToString(),
                    ContactPerson = dataRow["contact_person"].ToString(),
                    ContactPersonTelephone = dataRow["contact_person_telephone"].ToString()
                };
                customers.Add(customer);
            }

            return customers;
        }

        public static Customer GetCustomerById(int customerId)
        {
            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_customerSelect", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@id", customerId);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            
            //close connection
            connection.CloseSqlConnection(sqlConnection);

            //if customer is not found, throw exception across the business layer to the web layer and then display error
            if (dataTable.Rows.Count <= 0)
            {
                throw new RecordNotFoundException("Customer "+customerId+" was not found");
            }

            var customer = new Customer()
            {
                Id = Convert.ToInt32(dataTable.Rows[0]["id"].ToString()),
                Name = dataTable.Rows[0]["name"].ToString(),
                Address = dataTable.Rows[0]["address"].ToString(),
                Telephone = dataTable.Rows[0]["telephone"].ToString(),
                Fax = dataTable.Rows[0]["fax"].ToString(),
                ContactPerson = dataTable.Rows[0]["contact_person"].ToString(),
                ContactPersonTelephone = dataTable.Rows[0]["contact_person_telephone"].ToString()
            };

            return customer;
        }


        public static void UpdateCustomer(Customer customer)
        {
            //make connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);
            SqlCommand sqlCommand = new SqlCommand("usp_customerUpdate", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            //call sp
            sqlCommand.Parameters.AddWithValue("@id", customer.Id);
            sqlCommand.Parameters.AddWithValue("@name", customer.Name);
            sqlCommand.Parameters.AddWithValue("@address", customer.Address);
            sqlCommand.Parameters.AddWithValue("@telephone", customer.Telephone);
            sqlCommand.Parameters.AddWithValue("@fax", customer.Fax);
            sqlCommand.Parameters.AddWithValue("@contact_person", customer.ContactPerson);
            sqlCommand.Parameters.AddWithValue("@contact_person_telephone", customer.ContactPersonTelephone);
            sqlCommand.ExecuteNonQuery();
            
            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }

    }
}
