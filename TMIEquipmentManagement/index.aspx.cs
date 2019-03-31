using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using EntityLayer;
using Newtonsoft.Json.Linq;

namespace TMIEquipmentManagement
{
    public partial class index : System.Web.UI.Page
    {
        private int _goodHealth;
        private int _okayHealth;
        private int _lowHealth;

        protected void Page_Load(object sender, EventArgs e)
        {
            _goodHealth = 0;
            _okayHealth = 0;
            _lowHealth = 0;

            LoadSparePartHealth();
            LoadEquipmentHealth();
            LoadConsumableHealth();
            DisplayHealthCounts();
            LoadAllEquipmentItems();
            LoadAllSparePartItems();
            LoadConsumableBatches();
            ClientScript.RegisterStartupScript(GetType(), "Javascript1", "javascript:setHealthBars(); ", true);
        }


        private void DisplayHealthCounts()
        {
            lblGoodHealth.Text = _goodHealth.ToString();
            lblOkayHealth.Text = _okayHealth.ToString();
            lblLowHealth.Text = _lowHealth.ToString();
        }

        private void LoadConsumableHealth()
        {
            var serviceUsages = ConsumableBatchServiceUsageOpsBL.GetAllConsumableBatchServiceUsages();
            ConsumableBatchHealthCalculator calculator = new ConsumableBatchHealthCalculator();
            var healths = calculator.CalculateConsumableBatchHealths(serviceUsages);
            Sort(healths);
            lvConsumableHealths.DataSource = healths;
            lvConsumableHealths.DataBind();
            foreach (var health in healths)
            {
                IncrementHealthCount(health.HealthPercentage);
            }
        }

        private void LoadEquipmentHealth()
        {
            var installations = EquipmentInstallationOpsBL.GetAllEquipmentInstallations();
            EquipmentItemHealthCalculator calculator = new EquipmentItemHealthCalculator();
            var healths = calculator.CalcuEquipmentItemHealths(installations);
            Sort(healths);
            lvEquipmentHealths.DataSource = healths;
            lvEquipmentHealths.DataBind();
            foreach (var health in healths)
            {
                IncrementHealthCount(health.HealthPercentage);
            }
        }

        private void LoadSparePartHealth()
        {
            var sparePartUsages = SparePartUsageOpsBL.GetAllSparePartUsages();
            SparePartUsageHealthCalculator calculator = new SparePartUsageHealthCalculator();
            var healths = calculator.CalculateSparePartUsageHealths(sparePartUsages);
            Sort(healths);
            lvSparePartsHealth.DataSource = healths;
            lvSparePartsHealth.DataBind();
            foreach (var health in healths)
            {
                IncrementHealthCount(health.HealthPercentage);
            }
        }


        private static void Sort(List<SparePartUsageHealth> list)
        {
            int size = list.Count;
            for (int i = 1; i < size; i++)
            {
                for (int j = 0; j < (size - i); j++)
                {
                    if (list[j].HealthPercentage > list[j + 1].HealthPercentage)
                    {
                        SparePartUsageHealth temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }
        }

        private static void Sort(List<EquipmentItemHealth> list)
        {
            int size = list.Count;
            for (int i = 1; i < size; i++)
            {
                for (int j = 0; j < (size - i); j++)
                {
                    if (list[j].HealthPercentage > list[j + 1].HealthPercentage)
                    {
                        var temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }
        }

        private static void Sort(List<ConsumableBatchHealth> list)
        {
            int size = list.Count;
            for (int i = 1; i < size; i++)
            {
                for (int j = 0; j < (size - i); j++)
                {
                    if (list[j].HealthPercentage > list[j + 1].HealthPercentage)
                    {
                        var temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }
        }

        private void IncrementHealthCount(double healthPercentage)
        {
            if (healthPercentage < 70 && healthPercentage > 40)
            {
                _okayHealth++;
            }
            else if (healthPercentage < 40)
            {
                _lowHealth++;
            }
            else
            {
                _goodHealth++;
            }
        }


        [WebMethod]
        public static List<object> GetServiceChartData()
        {
            List<object> chartData = new List<object>();
            object role = new { };
            chartData.Add(new object[]
            {
                "Date", "Service ID"
            });

            var services
                = ServiceOpsBL.GetAllServices();

            foreach (var service in services)
            {
                chartData.Add(new object[]
                {
                    service.Date.ToShortDateString(), service.Id
                });
            }

            return chartData;
        }

        [WebMethodAttribute]
        public static List<object> GetConsumableChart()
        {
            string constr = ConfigurationManager.ConnectionStrings["tmidbConnectionString"].ConnectionString;
            List<object> chartData = new List<object>();
            object role = new { };
            chartData.Add(new object[]
            {
                "Model", "Percentage"
            });
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("usp_consumable_batch_service_usageSelectTop3UsedModels"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();
                    double totalUsedPercentage = 0;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            var usagePercentage = Convert.ToDouble(sdr["usage_count"]);
                            chartData.Add(new object[]
                            {
                                sdr["consumable_batchconsumablemodel_number"], usagePercentage
                            });
                            totalUsedPercentage += usagePercentage;
                        }

                        chartData.Add(new object[]
                        {
                            "Others", 100 - totalUsedPercentage
                        });
                    }


                    con.Close();
                    return chartData;
                }
            }
        }

        [WebMethodAttribute]
        public static List<object> GetEquipmentChart()
        {
            string constr = ConfigurationManager.ConnectionStrings["tmidbConnectionString"].ConnectionString;
            List<object> chartData = new List<object>();
            object role = new { };
            chartData.Add(new object[]
            {
                "Model", "Percentage"
            });
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("usp_equipment_installationSelectTop3InstalledModels"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();
                    double totalUsedPercentage = 0;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            var usagePercentage = Convert.ToDouble(sdr["usage_count"]);
                            chartData.Add(new object[]
                            {
                                sdr["equipmentmodel_number"], usagePercentage
                            });
                            totalUsedPercentage += usagePercentage;
                        }

                        chartData.Add(new object[]
                        {
                            "Others", 100 - totalUsedPercentage
                        });
                    }


                    con.Close();
                    return chartData;
                }
            }
        }

        [WebMethodAttribute]
        public static List<object> GetSparePartChart()
        {
            string constr = ConfigurationManager.ConnectionStrings["tmidbConnectionString"].ConnectionString;
            List<object> chartData = new List<object>();
            object role = new { };
            chartData.Add(new object[]
            {
                "Model", "Percentage"
            });
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("usp_spare_part_usageSelectTop3UsedModels"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();
                    double totalUsedPercentage = 0;
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            var usagePercentage = Convert.ToDouble(sdr["usage_count"]);
                            chartData.Add(new object[]
                            {
                                sdr["spare_partmodel_number"], usagePercentage
                            });
                            totalUsedPercentage += usagePercentage;
                        }

                        chartData.Add(new object[]
                        {
                            "Others", 100 - totalUsedPercentage
                        });
                    }


                    con.Close();
                    return chartData;
                }
            }
        }

        [WebMethodAttribute]
        public static List<object> GetCustomerLocationData()
        {
            List<object> chartData = new List<object>();
            chartData.Add(new object[]
            {
                "Lat", "Lon", "Customer"
            });

            var allCustomers = CustomerOpsBL.GetAllCustomers();
            foreach (var customer in allCustomers)
            {
                var geoCoordinate = GetGeoCoordinate(customer.Address);
                chartData.Add(new object[]
                {
                    geoCoordinate.Latitude, geoCoordinate.Longitude, customer.Name
                });
                
            }

            return chartData;
        }

        public static GeoLocation GetGeoCoordinate(string address)
        {
            try
            {
                var url = $"https://us1.locationiq.com/v1/search.php?key=efe212549e6ff1&q={address}&format=json";
                WebClient client = new WebClient();
                string response = client.DownloadString(url);
                var jArray = JArray.Parse(response);
                JObject joResponse = JObject.Parse(jArray[0].ToString());

                return new GeoLocation() { Latitude = joResponse["lat"].ToString(), Longitude = joResponse["lon"].ToString() };
            }
            catch (Exception exception)
            {
                return new GeoLocation() { Latitude = "6.8775191", Longitude = "79.8739964" };
            }
           
        }

        private void LoadAllEquipmentItems()
        {
            string query = "SELECT [serial_number], [price], [shipmentpo_number], [equipmentmodel_number], CASE WHEN [equipment_installation].[equipment_itemserial_number] IS NULL then 'No' ELSE 'Yes' END AS [IsInstalled] FROM [dbo].[equipment_item] LEFT JOIN [equipment_installation] ON [equipment_item].[serial_number] = [equipment_installation].[equipment_itemserial_number] ";

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

        private void LoadAllSparePartItems()
        {
            string query =
                "SELECT [serial_number], [price], [shipmentpo_number], [spare_partmodel_number], CASE WHEN [spare_part_usage].[spare_part_itemserial_number] IS NULL then 'No' ELSE 'Yes' END AS [IsInstalled] FROM [dbo].[spare_part_item] LEFT JOIN [spare_part_usage] ON [spare_part_item].[serial_number] = [spare_part_usage].[spare_part_itemserial_number]  ";

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

        private void LoadConsumableBatches()
        {
            string query = "SELECT [shipmentpo_number], [consumablemodel_number], [quantity], [price], SUM([consumable_batch_service_usage].[quantity_used]) as [quantity_used] FROM [dbo].[consumable_batch] LEFT JOIN [consumable_batch_service_usage] ON [consumable_batch].[consumablemodel_number] = [consumable_batch_service_usage].[consumable_batchconsumablemodel_number] AND [consumable_batch].[shipmentpo_number] = [consumable_batch_service_usage].[consumable_batchshipmentpo_number] " +
                           " GROUP BY [shipmentpo_number], [consumablemodel_number], [quantity], [price];";
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
    }
}