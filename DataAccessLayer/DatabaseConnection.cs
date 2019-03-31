using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DatabaseConnection
    {
        //test
        private static DatabaseConnection _singleInstance = new DatabaseConnection();

        public static DatabaseConnection getInstance()
        {
            return _singleInstance;
        }

        SqlConnection Obj_sqlcon =
            new SqlConnection(); //making the instance of sqlconnection

        SqlConnectionStringBuilder Obj_sqnbuild =
            new SqlConnectionStringBuilder();

        private DatabaseConnection()
        {
            SqlConnectionStringBuilder Obj_sqnbuild = new SqlConnectionStringBuilder();
            Obj_sqnbuild.InitialCatalog = ConfigurationManager.AppSettings["DatabaseName"];
            Obj_sqnbuild.DataSource = ConfigurationManager.AppSettings["DataSource"];
            Obj_sqnbuild.UserID = ConfigurationManager.AppSettings["SQLUserId"];
            Obj_sqnbuild.Password = ConfigurationManager.AppSettings["SQLUserPassword"];
            Obj_sqnbuild.Add("Max pool size", 1500);
            Obj_sqnbuild.Add("Min pool size", 20);
            Obj_sqnbuild.Add("Pooling", true);
            Obj_sqlcon.ConnectionString = Obj_sqnbuild.ConnectionString;
        }

        public void OpenSqlConnection(SqlConnection connection)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
        }

        public void CloseSqlConnection(SqlConnection connection)
        {
            if (connection.State != ConnectionState.Closed)
                connection.Close();
        }

        public string ConnectionString
        {
            get => Obj_sqlcon.ConnectionString;
            set => ConnectionString = value;
        }

        public SqlConnection GetSqlConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
