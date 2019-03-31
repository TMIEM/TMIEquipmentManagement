using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TMIEquipmentManagement
{
    public partial class ChartTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static List<object> GetChartData()
        {
            string query = "SELECT [date], COUNT(*) as [ServiceCount], equipment_item.equipmentmodel_number as [Model] FROM service_request LEFT JOIN equipment_item ON [service_request].[equipment_itemserial_number] = equipment_item.serial_number WHERE service_request.equipment_itemserial_number IN(SELECT [equipment_itemserial_number] FROM equipment_item WHERE shipmentpo_number = '1') GROUP BY [equipment_item].[equipmentmodel_number], [date]";
            string constr = ConfigurationManager.ConnectionStrings["tmidbConnectionString"].ConnectionString;
            List<object> chartData = new List<object>();
            string annotation = "{role: \'annotation\'}";
            var annotationObject = new JObject();
            annotationObject.Add("role", "annotation");
            object role = new { };
            chartData.Add(new object[]
            {
                "Date", "Service Request Count", role = new {role = "annotation"}
            });
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            chartData.Add(new object[]
                            {
                                Convert.ToDateTime(sdr["date"]).ToLongDateString(), sdr["ServiceCount"], sdr["Model"]
                            });
                        }
                    }

                    con.Close();
                    return chartData;
                }
            }
        }

        [WebMethod]
        public static List<object> GetChartData2()
        {
            string query = "with CTE_Months as ( select EOMONTH(DATEADD(MONTH, DATEDIFF(MONTH, 0, GETDATE()), 0)) as MonthYear union all select DATEADD(MONTH, -1, MonthYear) from CTE_Months where MonthYear > DATEADD(MONTH, -26, GETDATE()) ), CTE_Count as ( select COUNT(equipment_itemserial_number) AS installation_count, DATEADD(MONTH, DATEDIFF(MONTH, 0, installation_date), 0) as MonthYear from equipment_installation where installation_date >= DATEADD(MONTH, -26, GETDATE()) group by DATEADD(MONTH, DATEDIFF(MONTH, 0, installation_date), 0) ) select installation_count, MonthYear from CTE_Count union all select 0, MonthYear from CTE_Months as m where not exists (select 1 from CTE_Count as c where c.MonthYear = m.MonthYear) order by MonthYear";
            string constr = ConfigurationManager.ConnectionStrings["tmidbConnectionString"].ConnectionString;
            List<object> chartData = new List<object>();
            chartData.Add(new object[]
            {
                "Period", "EquipmentInstallations"
            });
            using (SqlConnection con = new SqlConnection(constr))
            {

                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            chartData.Add(new object[]
                            {
                                Convert.ToDateTime(sdr["MonthYear"]).ToLongDateString(), sdr["installation_count"]
                            });
                        }
                    }
                    con.Close();
                    return chartData;
                }
            }
        }
    }
}