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
    public partial class ConsumableProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var modelNumber = Request.QueryString["modelnumber"];
            if (modelNumber == null)  Response.Redirect("404.aspx");
            PopulateDropDown();
            lblConsumableModelNumber.Text = modelNumber;
            LoadConsumableDetails(modelNumber);
            LoadConsumableBatches(modelNumber);
            LoadConsumableUsages(modelNumber);
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
        private void LoadConsumableUsages(string modelNumber)
        {
            lvConsumableBatchUsages.DataSource = ConsumableBatchServiceUsageOpsBL.GetConsumableServiceUsagesByModel(modelNumber);
            lvConsumableBatchUsages.DataBind();
        }

        private void LoadConsumableDetails(string modelNumber)
        {
            try
            {
                var consumable = ConsumableOpsBL.GetConsumableByModelNumber(modelNumber);
                DisplayConsumableDetails(consumable);
            }
            catch (RecordNotFoundException e)
            {
                Response.Redirect("404.aspx");
            }
        }

        private void DisplayConsumableDetails(Consumable consumable)
        {
            lblConsumableModelNumber2.Text = consumable.ModelNumber;
            lblMake.Text = consumable.Make;
            lblDescription.Text = consumable.Description;
            lblLifeSpan.Text = consumable.LifeSpanDays.ToString();
        }

        private void LoadConsumableBatches(string modelNumber)
        {
            string query = "SELECT [shipmentpo_number], [consumablemodel_number], [quantity], [price], SUM([consumable_batch_service_usage].[quantity_used]) as [quantity_used] FROM [dbo].[consumable_batch] LEFT JOIN [consumable_batch_service_usage] ON [consumable_batch].[consumablemodel_number] = [consumable_batch_service_usage].[consumable_batchconsumablemodel_number] AND [consumable_batch].[shipmentpo_number] = [consumable_batch_service_usage].[consumable_batchshipmentpo_number] " +
                           "WHERE [consumable_batch].[consumablemodel_number] = '"+modelNumber+"' GROUP BY [shipmentpo_number], [consumablemodel_number], [quantity], [price];";
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
                            int quantityRemaining = -1;
                            int quantityUsed = 0;
                            if (sdr["quantity"] != DBNull.Value)
                            {
                                
                                if (sdr["quantity_used"] != DBNull.Value)
                                {
                                    quantityUsed = Convert.ToInt32(sdr["quantity_used"]);
                                }

                                  quantityRemaining =
                                    Convert.ToInt32(sdr["quantity"]) - quantityUsed;
                            }


                            var consumableBatch = new
                            {
                                ModelNumber = sdr["consumablemodel_number"],
                                Price = sdr["price"],
                                ShipmentPoNumber = sdr["shipmentpo_number"],
                                Quantity = sdr["quantity"],
                                QuantityUsed = quantityUsed.ToString(),
                                QuantityRemaining = (quantityRemaining == -1) ? "" : quantityRemaining.ToString()
                            };


                            data.Add(consumableBatch);
                        }
                    }

                    con.Close();
                    lvConsumableBatches.DataSource = data;
                    lvConsumableBatches.DataBind();
                }
            }
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
                using (SqlCommand cmd = new SqlCommand("usp_consumable_batch_service_usageSelectMonthlyUsage"))
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