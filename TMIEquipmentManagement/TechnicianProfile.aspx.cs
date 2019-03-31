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
    public partial class TechnicianProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = Request.QueryString["id"];
            if (id == null) Response.Redirect("404.aspx");
            PopulateDropDown();
            lblTechnicianId.Text = id;
            LoadTechnicianDetails(id);
            LoadServicesPerformed(id);
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

        private void LoadServicesPerformed(string technicianId)
        {
            var technicanIdInt = Convert.ToInt32(technicianId);
            var services = ServiceOpsBL.EagerLoad(ServiceOpsBL.GetAllServices());
            var servicesPerformed = new List<Service>();
            foreach (var service in services)
            {
                if (service.TechnicianId == technicanIdInt)
                    servicesPerformed.Add(service);
            }
            
            lvServices.DataSource = servicesPerformed;
            lvServices.DataBind();
        }

        private void LoadTechnicianDetails(string id)
        {
            try
            {
                var technician = TechnicianOpsBL.GetTechnicianById(Convert.ToInt32(id));
                DisplayTechnicianDetails(technician);
            }
            catch (RecordNotFoundException e)
            {
                Response.Redirect("404.aspx");
            }
            catch (FormatException exception)
            {
                Response.Redirect("404.aspx");
            }
        }

        private void DisplayTechnicianDetails(Technician technician)
        {
            lblTechnicianId2.Text = technician.Id.ToString();
            lblName.Text = technician.Name;
            lblContactNumber.Text = technician.ContactNumber;
            lblEmail.Text = technician.Email;
        }


        [WebMethod]
        public static List<object> GetServicePerformedChartData(string technicianid, string year)
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
                using (SqlCommand cmd = new SqlCommand("usp_serviceSelectMonthlyTechnicianServices"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@technicianid", technicianid);
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

        [WebMethodAttribute]
        public static List<object> GetServiceShareChartData(string technicianid)
        {
            var technician = TechnicianOpsBL.GetTechnicianById(Convert.ToInt32(technicianid));
            string constr = ConfigurationManager.ConnectionStrings["tmidbConnectionString"].ConnectionString;
            List<object> chartData = new List<object>();
            object role = new { };
            chartData.Add(new object[]
            {
                "Customer", "Percentage"
            });
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("usp_serviceTechnicianPercentage"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@technicianid", technicianid);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        double technicianPercentage = 0;
                        while (sdr.Read())
                        {
                            technicianPercentage = Convert.ToDouble(sdr["technicianpercentage"]);
                            chartData.Add(new object[]
                            {
                                technician.Name, technicianPercentage
                            });
                        }

                        chartData.Add(new object[]
                        {
                            "Others", 100-technicianPercentage
                        });
                    }


                    con.Close();
                    return chartData;
                }
            }
        }
    }
}