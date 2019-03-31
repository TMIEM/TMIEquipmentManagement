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
    public class ServiceOpsDAL
    {
        public static Service AddNewService(Service service)
        {
            //making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //calling sp
            var sqlDataAdapter = new SqlDataAdapter("usp_serviceInsert", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@date", service.Date);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@problem_description", service.ProblemDescription);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@service_description", service.ServiceDescription);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@special_note", service.SpecialNote);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@equipment_installationequipment_itemserial_number",
                service.InstalledEquipmentSerialNumber);
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@technicianid", service.TechnicianId);

            //capturing the returned service
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            var insertedService = new Service()
            {
                Id = Convert.ToInt32(dataTable.Rows[0]["id"].ToString()),
                Date = Convert.ToDateTime(dataTable.Rows[0]["date"].ToString()),
                ProblemDescription = dataTable.Rows[0]["problem_description"].ToString(),
                ServiceDescription = dataTable.Rows[0]["service_description"].ToString(),
                SpecialNote = dataTable.Rows[0]["special_note"].ToString(),
                InstalledEquipmentSerialNumber = dataTable.Rows[0]["equipment_installationequipment_itemserial_number"].ToString(),
                TechnicianId = Convert.ToInt32(dataTable.Rows[0]["technicianid"].ToString())
            };

            //close connection
            connection.CloseSqlConnection(sqlConnection);
            return insertedService;
        }

       

        public static List<Service> GetAllServices()
        {
            List<Service> services = new List<Service>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_serviceSelectAll", sqlConnection);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating place list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var service = new Service()
                {
                    Id = Convert.ToInt32(dataRow["id"].ToString()),
                    Date = Convert.ToDateTime(dataRow["date"].ToString()),
                    ProblemDescription = dataRow["problem_description"].ToString(),
                    ServiceDescription = dataRow["service_description"].ToString(),
                    SpecialNote = dataRow["special_note"].ToString(),
                    InstalledEquipmentSerialNumber = dataRow["equipment_installationequipment_itemserial_number"].ToString(),
                    TechnicianId = Convert.ToInt32(dataRow["technicianid"].ToString())
                };
                services.Add(service);
            }

            return services;
        }

        public static Service GetServiceById(int serviceId)
        {
            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_serviceSelect", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@id", serviceId);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //close connection
            connection.CloseSqlConnection(sqlConnection);

            //if supplier is not found, throw exception across the business layer to the web layer and then display error
            if (dataTable.Rows.Count <= 0)
            {
                throw new RecordNotFoundException("Service " + serviceId + " was not found");
            }

            var service = new Service()
            {
                Id = Convert.ToInt32(dataTable.Rows[0]["id"].ToString()),
                Date = Convert.ToDateTime(dataTable.Rows[0]["date"].ToString()),
                ProblemDescription = dataTable.Rows[0]["problem_description"].ToString(),
                ServiceDescription = dataTable.Rows[0]["service_description"].ToString(),
                SpecialNote = dataTable.Rows[0]["special_note"].ToString(),
                InstalledEquipmentSerialNumber = dataTable.Rows[0]["equipment_installationequipment_itemserial_number"].ToString(),
                TechnicianId = Convert.ToInt32(dataTable.Rows[0]["technicianid"].ToString())

            };

            return service;
        }


        public static void UpdateService(Service service)
        {
            //make connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);
            SqlCommand sqlCommand = new SqlCommand("usp_serviceUpdate", sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;

            //call sp
            sqlCommand.Parameters.AddWithValue("@id", service.Id);
            sqlCommand.Parameters.AddWithValue("@date", service.Date);
            sqlCommand.Parameters.AddWithValue("@problem_description", service.ProblemDescription);
            sqlCommand.Parameters.AddWithValue("@service_description", service.ServiceDescription);
            sqlCommand.Parameters.AddWithValue("@special_note", service.SpecialNote);
            sqlCommand.Parameters.AddWithValue("@equipment_installationequipment_itemserial_number",
                service.InstalledEquipmentSerialNumber);
            sqlCommand.Parameters.AddWithValue("@technicianid", service.TechnicianId);
            sqlCommand.ExecuteNonQuery();

            //close connection
            connection.CloseSqlConnection(sqlConnection);
        }


        public static List<Service> GetAllServicesForInstallation(EquipmentInstallation equipmentInstallation)
        {
            List<Service> services = new List<Service>();

            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_serviceSelectForInstallation", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.
                AddWithValue("@equipment_itemserial_number", equipmentInstallation.EquipmentItemSerialNumber);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //Close connection
            connection.CloseSqlConnection(sqlConnection);

            //populating place list by reading sp result
            foreach (DataRow dataRow in dataTable.Rows)
            {
                var service = new Service()
                {
                    Id = Convert.ToInt32(dataRow["id"].ToString()),
                    Date = Convert.ToDateTime(dataRow["date"].ToString()),
                    ProblemDescription = dataRow["problem_description"].ToString(),
                    ServiceDescription = dataRow["service_description"].ToString(),
                    SpecialNote = dataRow["special_note"].ToString(),
                    InstalledEquipmentSerialNumber = dataRow["equipment_installationequipment_itemserial_number"].ToString(),
                    TechnicianId = Convert.ToInt32(dataRow["technicianid"].ToString())
                };
                services.Add(service);
            }

            return services;
        }


        public static List<Service> EagerLoad(List<Service> services)
        {
            foreach (var service in services)
            {
                service.Technician = TechnicianOpsDAL.GetTechnicianById(service.TechnicianId);
                service.EquipmentInstallation =
                    EquipmentInstallationOpsDAL.EagerLoad(
                        EquipmentInstallationOpsDAL
                        .GetEquipmentInstallationBySerial(service.InstalledEquipmentSerialNumber));
                service.SparePartUsages = SparePartUsageOpsDAL.GetSparePartUsagesByServiceId(service.Id);
            }

            return services;
        }

        public static Service EagerLoad(Service service)
        {
            service.Technician = TechnicianOpsDAL.GetTechnicianById(service.TechnicianId);
            service.EquipmentInstallation =
                EquipmentInstallationOpsDAL.GetEquipmentInstallationBySerial(service
                    .InstalledEquipmentSerialNumber);
            service.SparePartUsages = SparePartUsageOpsDAL.GetSparePartUsagesByServiceId(service.Id);

            return service;
        }


        public static Service GetLastServiceForEquipment(string equipmentItemSerial)
        {
            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_serviceSelectLastServiceForEquipment", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@equipment_installationequipment_itemserial_number", equipmentItemSerial);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //close connection
            connection.CloseSqlConnection(sqlConnection);

            if (dataTable.Rows.Count <= 0)
            {
                throw new RecordNotFoundException("Service for " + equipmentItemSerial + " was not found");
            }

            var service = new Service()
            {
                Id = Convert.ToInt32(dataTable.Rows[0]["id"].ToString()),
                Date = Convert.ToDateTime(dataTable.Rows[0]["date"].ToString()),
                ProblemDescription = dataTable.Rows[0]["problem_description"].ToString(),
                ServiceDescription = dataTable.Rows[0]["service_description"].ToString(),
                SpecialNote = dataTable.Rows[0]["special_note"].ToString(),
                InstalledEquipmentSerialNumber = dataTable.Rows[0]["equipment_installationequipment_itemserial_number"].ToString(),
                TechnicianId = Convert.ToInt32(dataTable.Rows[0]["technicianid"].ToString())

            };

            return service;
        }

        public static Service GetLastServiceForEquipmentByDate(string equipmentItemSerial)
        {
            //Making connection
            DatabaseConnection connection = DatabaseConnection.getInstance();
            SqlConnection sqlConnection = connection.GetSqlConnection();
            connection.OpenSqlConnection(sqlConnection);

            //call SP
            var sqlDataAdapter = new SqlDataAdapter("usp_serviceSelectLastServiceForEquipmentByDate", sqlConnection);
            sqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            sqlDataAdapter.SelectCommand.Parameters.AddWithValue("@equipment_installationequipment_itemserial_number", equipmentItemSerial);
            var dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);

            //close connection
            connection.CloseSqlConnection(sqlConnection);

            if (dataTable.Rows.Count <= 0)
            {
                throw new RecordNotFoundException("Service for " + equipmentItemSerial + " was not found");
            }

            var service = new Service()
            {
                Id = Convert.ToInt32(dataTable.Rows[0]["id"].ToString()),
                Date = Convert.ToDateTime(dataTable.Rows[0]["date"].ToString()),
                ProblemDescription = dataTable.Rows[0]["problem_description"].ToString(),
                ServiceDescription = dataTable.Rows[0]["service_description"].ToString(),
                SpecialNote = dataTable.Rows[0]["special_note"].ToString(),
                InstalledEquipmentSerialNumber = dataTable.Rows[0]["equipment_installationequipment_itemserial_number"].ToString(),
                TechnicianId = Convert.ToInt32(dataTable.Rows[0]["technicianid"].ToString())

            };

            return service;
        }
    }
}
