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
    public class EquipmentInstallationOpsDAL
    {
        public static void AddNewEquipmentInstallation(EquipmentInstallation equipmentInstallation)
        {
            //making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //calling sp
            SqlCommand sqlCommand = new SqlCommand("usp_equipment_installationInsert", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.Parameters.AddWithValue("@equipment_itemserial_number",
                equipmentInstallation.EquipmentItemSerialNumber);
            sqlCommand.Parameters.AddWithValue("@installation_date", equipmentInstallation.InstallationDate);
            sqlCommand.Parameters.AddWithValue("@warranty_period_months", equipmentInstallation.WarrantyPeriodMonths);
            sqlCommand.Parameters.AddWithValue("@service_agreement_period_months",
                equipmentInstallation.ServiceAgreementPeriodMonths);
            sqlCommand.Parameters.AddWithValue("@invoice_id", equipmentInstallation.InvoiceId);
            sqlCommand.Parameters.AddWithValue("@customerid", equipmentInstallation.CustomerId);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }

        public static List<EquipmentInstallation> GetAllEquipmentInstallations()
        {
            List<EquipmentInstallation> equipmentInstallations = new List<EquipmentInstallation>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_equipment_installationSelectAll", sqlConnection);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating place list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var equipmentInstallation = new EquipmentInstallation()
                {
                    EquipmentItemSerialNumber = dataRow["equipment_itemserial_number"].ToString(),
                    InstallationDate = Convert.ToDateTime(dataRow["installation_date"].ToString()),
                    WarrantyPeriodMonths = Convert.ToInt32(dataRow["warranty_period_months"].ToString()),
                    ServiceAgreementPeriodMonths =
                        Convert.ToInt32(dataRow["service_agreement_period_months"].ToString()),
                    InvoiceId = dataRow["invoice_id"].ToString(),
                    CustomerId = Convert.ToInt32(dataRow["customerid"].ToString())
                };
                equipmentInstallations.Add(equipmentInstallation);
            }


            return equipmentInstallations;
        }

        public static EquipmentInstallation GetEquipmentInstallationBySerial(string equipmentInstallationSerialNumber)
        {
            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_equipment_installationSelect", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@equipment_itemserial_number",
                equipmentInstallationSerialNumber);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //close connection
            connection.CloseSqlConnection(sqlConnection);

            //if supplier is not found, throw exception across the business layer to the web layer and then display error
            if (dataTable.Rows.Count <= 0)
            {
                throw new RecordNotFoundException("EquipmentInstallation " + equipmentInstallationSerialNumber +
                                                  " was not found");
            }

            var equipmentInstallation = new EquipmentInstallation()
            {
                EquipmentItemSerialNumber = dataTable.Rows[0]["equipment_itemserial_number"].ToString(),
                InstallationDate = Convert.ToDateTime(dataTable.Rows[0]["installation_date"].ToString()),
                WarrantyPeriodMonths = Convert.ToInt32(dataTable.Rows[0]["warranty_period_months"].ToString()),
                ServiceAgreementPeriodMonths =
                    Convert.ToInt32(dataTable.Rows[0]["service_agreement_period_months"].ToString()),
                InvoiceId = dataTable.Rows[0]["invoice_id"].ToString(),
                CustomerId = Convert.ToInt32(dataTable.Rows[0]["customerid"].ToString())
            };

            return equipmentInstallation;
        }


        public static void UpdateEquipmentInstallation(EquipmentInstallation equipmentInstallation)
        {
            //make connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);
            SqlCommand sqlCommand = new SqlCommand("usp_equipment_installationUpdate", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            //call sp
            sqlCommand.Parameters.AddWithValue("@equipment_itemserial_number",
                equipmentInstallation.EquipmentItemSerialNumber);
            sqlCommand.Parameters.AddWithValue("@installation_date", equipmentInstallation.InstallationDate);
            sqlCommand.Parameters.AddWithValue("@warranty_period_months", equipmentInstallation.WarrantyPeriodMonths);
            sqlCommand.Parameters.AddWithValue("@service_agreement_period_months",
                equipmentInstallation.ServiceAgreementPeriodMonths);
            sqlCommand.Parameters.AddWithValue("@invoice_id", equipmentInstallation.InvoiceId);
            sqlCommand.Parameters.AddWithValue("@customerid", equipmentInstallation.CustomerId);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }

        public static List<EquipmentInstallation> EagerLoad(List<EquipmentInstallation> equipmentInstallations)
        {
            foreach (var equipmentInstallation in equipmentInstallations)
            {
                equipmentInstallation.Customer = CustomerOpsDAL.GetCustomerById(equipmentInstallation.CustomerId);
                equipmentInstallation.EquipmentItem =
                    EquipmentItemOpsDAL.GetEquipmentItemBySerialNumber(equipmentInstallation.EquipmentItemSerialNumber);
                equipmentInstallation.Services = ServiceOpsDAL.GetAllServicesForInstallation(equipmentInstallation);
            }

            return equipmentInstallations;
        }

        public static EquipmentInstallation EagerLoad(EquipmentInstallation equipmentInstallation)
        {
            equipmentInstallation.Customer = CustomerOpsDAL.GetCustomerById(equipmentInstallation.CustomerId);
            equipmentInstallation.EquipmentItem =
                EquipmentItemOpsDAL.GetEquipmentItemBySerialNumber(equipmentInstallation.EquipmentItemSerialNumber);
            equipmentInstallation.Services = ServiceOpsDAL.GetAllServicesForInstallation(equipmentInstallation);


            return equipmentInstallation;
        }

        public static List<EquipmentInstallation> GetAllEquipmentInstallationsForCustomer(int customerId)
        {
            List<EquipmentInstallation> equipmentInstallations = new List<EquipmentInstallation>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_equipment_installationSelectByCustomer", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@customerid",
                customerId);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating place list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var equipmentInstallation = new EquipmentInstallation()
                {
                    EquipmentItemSerialNumber = dataRow["equipment_itemserial_number"].ToString(),
                    InstallationDate = Convert.ToDateTime(dataRow["installation_date"].ToString()),
                    WarrantyPeriodMonths = Convert.ToInt32(dataRow["warranty_period_months"].ToString()),
                    ServiceAgreementPeriodMonths =
                        Convert.ToInt32(dataRow["service_agreement_period_months"].ToString()),
                    InvoiceId = dataRow["invoice_id"].ToString(),
                    CustomerId = Convert.ToInt32(dataRow["customerid"].ToString())
                };
                equipmentInstallations.Add(equipmentInstallation);
            }


            return equipmentInstallations;
        }

    }
}