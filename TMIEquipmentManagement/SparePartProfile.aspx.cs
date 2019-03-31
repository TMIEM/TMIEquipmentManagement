using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using EntityLayer;

namespace TMIEquipmentManagement
{
    public partial class SparePartProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var modelNumber = Request.QueryString["modelnumber"];
            if (modelNumber == null) Response.Redirect("404.aspx");
            lblSparePartModelNumber.Text = modelNumber;
           
            LoadSparePartDetails(modelNumber);
            LoadAvailableSpareParts(modelNumber);
            LoadAllSparePartItems(modelNumber);
            PopulateDropDown();
        }

        private void PopulateDropDown()
        {
            var currentYear = DateTime.Today.Year;
            for (int i = currentYear - 20; i < currentYear + 20; i++)
            {
                ddlYear.Items.Add(i.ToString());
            }

            ddlYear.SelectedValue = (currentYear + 1).ToString();
        }

        private void LoadAvailableSpareParts(string modelNumber)
        {
            string query =
                "SELECT [serial_number], [price], [shipmentpo_number], [spare_partmodel_number] FROM [dbo].[spare_part_item] WHERE [serial_number] NOT IN (SELECT [spare_part_itemserial_number] FROM [dbo].[spare_part_usage]) AND [spare_part_item].[spare_partmodel_number] =" +
                " '" + modelNumber + "'";

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
                            object sparePartItem = new
                            {
                                SerialNumber = sdr["serial_number"],
                                ModelNumber = sdr["spare_partmodel_number"],
                                Price = sdr["price"],
                                ShipmentPoNumber = sdr["shipmentpo_number"]
                            };

                            data.Add(sparePartItem);
                        }
                    }

                    con.Close();
                    lvAvailableSpareParts.DataSource = data;
                    lvAvailableSpareParts.DataBind();
                }
            }
        }


        private void LoadAllSparePartItems(string modelNumber)
        {
            string query =
                "SELECT [serial_number], [price], [shipmentpo_number], [spare_partmodel_number], CASE WHEN [spare_part_usage].[spare_part_itemserial_number] IS NULL then 'No' ELSE 'Yes' END AS [IsInstalled] FROM [dbo].[spare_part_item] LEFT JOIN [spare_part_usage] ON [spare_part_item].[serial_number] = [spare_part_usage].[spare_part_itemserial_number] WHERE [spare_part_item].[spare_partmodel_number]  " +
                "= '" + modelNumber + "'";

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
                                ModelNumber = sdr["spare_partmodel_number"],
                                Price = sdr["price"],
                                ShipmentPoNumber = sdr["shipmentpo_number"],
                                IsInstalled = sdr["IsInstalled"]
                            };

                            data.Add(equipmentItem);
                        }
                    }

                    con.Close();
                    lvAllSpareParts.DataSource = data;
                    lvAllSpareParts.DataBind();
                }
            }
        }


        private void LoadSparePartDetails(string modelNumber)
        {
            try
            {
                var sparePart = SparePartOpsBL.GetSparePartByModel(modelNumber);
                DisplaySparePartDetails(sparePart);
            }
            catch (RecordNotFoundException e)
            {
                Response.Redirect("404.aspx");
            }
        }

        private void DisplaySparePartDetails(SparePart sparePart)
        {
            lblSparePartModelNumber2.Text = sparePart.ModelNumber;
            lblDescription.Text = sparePart.Description;
            lblMake.Text = sparePart.Make;
            lblLifeSpan.Text = sparePart.LifeSpanMonths.ToString();
        }

        [WebMethod]
        public static List<object> GetUsageChartData(string modelnumber, string year)
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
                using (SqlCommand cmd = new SqlCommand("usp_spare_part_usageSelectMonthlyUsage"))
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