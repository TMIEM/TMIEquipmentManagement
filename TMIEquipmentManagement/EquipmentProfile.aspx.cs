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
using BusinessLayer;
using EntityLayer;

namespace TMIEquipmentManagement
{
    public partial class EquipmentProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var modelNumber = Request.QueryString["modelnumber"];
            if (modelNumber == null) Response.Redirect("404.aspx");

            lblEquipmentModelNumber.Text = modelNumber;
            PopulateDropDown();
            LoadEquipmentDetails(modelNumber);
            LoadAvailableEquipments(modelNumber);
            LoadAllEquipmentItems(modelNumber);
        }

        private void PopulateDropDown()
        {
            var currentYear = DateTime.Today.Year;
            for (int i = currentYear-20; i < currentYear+20; i++)
            {
                ddlYear.Items.Add(i.ToString());    
            }

            ddlYear.SelectedValue = (currentYear+1).ToString();
        }


        private void LoadAvailableEquipments(string modelNumber)
        {
            string query = "SELECT [serial_number], [price], [shipmentpo_number], [equipmentmodel_number] FROM [dbo].[equipment_item] WHERE [serial_number] NOT IN (SELECT [equipment_itemserial_number] FROM [dbo].[equipment_installation]) AND [equipment_item].[equipmentmodel_number] = '"+
                           modelNumber+"'";

            string constr = ConfigurationManager.ConnectionStrings["tmidbConnectionString"].ConnectionString;
            List<object> data = new List<object>();
            

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
                            object equipmentItem = new
                            {
                                SerialNumber = sdr["serial_number"],
                                ModelNumber = sdr["equipmentmodel_number"],
                                Price = sdr["price"],
                                ShipmentPoNumber = sdr["shipmentpo_number"]
                            };

                            data.Add(equipmentItem);
                        }
                    }
                    con.Close();
                    lvAvailableEquipmentItems.DataSource = data;
                    lvAvailableEquipmentItems.DataBind();
                }
            }
        }

        private void LoadAllEquipmentItems(string modelNumber)
        {
            string query = "SELECT [serial_number], [price], [shipmentpo_number], [equipmentmodel_number], CASE WHEN [equipment_installation].[equipment_itemserial_number] IS NULL then 'No' ELSE 'Yes' END AS [IsInstalled] FROM [dbo].[equipment_item] LEFT JOIN [equipment_installation] ON [equipment_item].[serial_number] = [equipment_installation].[equipment_itemserial_number] WHERE [equipment_item].[equipmentmodel_number] " +
                           "= '"+modelNumber+"'";

            string constr = ConfigurationManager.ConnectionStrings["tmidbConnectionString"].ConnectionString;
            List<object> data = new List<object>();


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
                            object equipmentItem = new
                            {
                                SerialNumber = sdr["serial_number"],
                                ModelNumber = sdr["equipmentmodel_number"],
                                Price = sdr["price"],
                                ShipmentPoNumber = sdr["shipmentpo_number"],
                                IsInstalled = sdr["IsInstalled"]
                            };

                            data.Add(equipmentItem);
                        }
                    }
                    con.Close();
                    lvAllEquipmentItems.DataSource = data;
                    lvAllEquipmentItems.DataBind();
                }
            }
        }


        private void LoadEquipmentDetails(string modelNumber)
        {
            try
            {
                var equipment = EquipmentOpsBL.GetEquipmentByModel(modelNumber);
                DisplayEquipmentDetails(equipment);
            }
            catch (RecordNotFoundException e)
            {
                Response.Redirect("404.aspx");
            }

        }

        private void DisplayEquipmentDetails(Equipment equipment)
        {
            lblEquipmentModelNumber2.Text = equipment.ModelNumber;
            lblDescription.Text = equipment.Description;
            lblMake.Text = equipment.Make;
            lblVersion.Text = equipment.Version;
            lblSoftwareVersion.Text = equipment.SoftwareVersion;
            lblMinServicePeriod.Text = equipment.MinimumServicePeriodMonths.ToString();
        }


        [WebMethod]
        public static List<object> GetEquipmentUsageChartData(string modelnumber, string year)
        {
            string constr = ConfigurationManager.ConnectionStrings["tmidbConnectionString"].ConnectionString;
            List<object> chartData = new List<object>();
            object role = new { };
            chartData.Add(new object[]
            {
                "Date", "Installation Count"
            });
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("usp_equipment_installationMonthlyInstalls"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@modelnumber", modelnumber);
                    cmd.Parameters.AddWithValue("@Date", year);
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