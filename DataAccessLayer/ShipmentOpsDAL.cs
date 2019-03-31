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
    public class ShipmentOpsDAL
    {
        public static Shipment AddNewShipment(Shipment shipment)
        {
            //making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //calling sp
            var sqlDataAdapter = new SqlDataAdapter("usp_shipmentInsert", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@po_number", shipment.PoNumber);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@supplier_invoice_number", shipment.SupplierInvoiceNumber);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@date_of_arrival", shipment.DateOfArrival);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@supplierid", shipment.SupplierId);


            //capturing the returned shipment
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            var insertedShipment = new Shipment()
            {
                PoNumber = dataTable.Rows[0]["po_number"].ToString(),
                SupplierInvoiceNumber = dataTable.Rows[0]["supplier_invoice_number"].ToString(),
                DateOfArrival = Convert.ToDateTime(dataTable.Rows[0]["date_of_arrival"].ToString()),
                SupplierId = Convert.ToInt32(dataTable.Rows[0]["supplierid"].ToString())

            };


            //close connection
            connection.CloseSqlConnection(sqlConnection);
            return insertedShipment;
        }

        public static List<Shipment> GetAllShipments()
        {
            List<Shipment> shipments = new List<Shipment>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_shipmentSelectAll", sqlConnection);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating place list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var shipment = new Shipment()
                {
                    PoNumber = dataRow["po_number"].ToString(),
                    SupplierInvoiceNumber = dataRow["supplier_invoice_number"].ToString(),
                    DateOfArrival = Convert.ToDateTime(dataRow["date_of_arrival"].ToString()),
                    SupplierId = Convert.ToInt32(dataRow["supplierid"].ToString())
                };
                shipments.Add(shipment);
            }

            return shipments;
        }

        public static Shipment GetShipmentByPoNumber(string shipmentPoNumber)
        {
            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_shipmentSelect", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@po_number", shipmentPoNumber);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //close connection
            connection.CloseSqlConnection(sqlConnection);

            //if supplier is not found, throw exception across the business layer to the web layer and then display error
            if (dataTable.Rows.Count <= 0)
            {
                throw new RecordNotFoundException("Shipment " + shipmentPoNumber + " was not found");
            }

            var shipment = new Shipment()
            {
                PoNumber = dataTable.Rows[0]["po_number"].ToString(),
                SupplierInvoiceNumber = dataTable.Rows[0]["supplier_invoice_number"].ToString(),
                DateOfArrival = Convert.ToDateTime(dataTable.Rows[0]["date_of_arrival"].ToString()),
                SupplierId = Convert.ToInt32(dataTable.Rows[0]["supplierid"].ToString())

            };

            return shipment;
        }


        public static void UpdateShipment(Shipment shipment)
        {
            //make connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);
            SqlCommand sqlCommand = new SqlCommand("usp_shipmentUpdate", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            //call sp
            sqlCommand.Parameters.AddWithValue("@po_number", shipment.PoNumber);
            sqlCommand.Parameters.AddWithValue("@supplier_invoice_number", shipment.SupplierInvoiceNumber);
            sqlCommand.Parameters.AddWithValue("@date_of_arrival", shipment.DateOfArrival);
            sqlCommand.Parameters.AddWithValue("@supplierid", shipment.SupplierId);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }
    }
}
