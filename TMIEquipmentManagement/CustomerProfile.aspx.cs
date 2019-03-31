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
    public partial class CustomerProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = Request.QueryString["id"];
            if (id == null) Response.Redirect("404.aspx");
            lblCustomerId.Text = id;
            LoadCustomerDetails(id);
            LoadEquipmentInstallations(id);
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

        private void LoadEquipmentInstallations(string customerId)
        {
            lvEquipmentInstallations.DataSource =
                EquipmentInstallationOpsBL.GetAllEquipmentInstallationsForCustomer(Convert.ToInt32(customerId));
            lvEquipmentInstallations.DataBind();
        }

        private void LoadCustomerDetails(string id)
        {
            try
            {
                var customer = CustomerOpsBL.GetCustomerById(Convert.ToInt32(id));
                DisplayCustomerDetails(customer);
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

        private void DisplayCustomerDetails(Customer customer)
        {
            lblCustomerId2.Text = customer.Id.ToString();
            lblName.Text = customer.Name;
            lblAddress.Text = customer.Address;
            lblTelephone.Text = customer.Telephone;
            lblFax.Text = customer.Fax;
            lblContactPerson.Text = customer.ContactPerson;
            lblContactPersonTelephone.Text = customer.ContactPersonTelephone;
        }


        [WebMethod]
        public static List<object> GetInstallationsChartData(string customerid, string year)
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
                using (SqlCommand cmd = new SqlCommand("usp_equipment_installationMonthlyInstallsByCustomer"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@customerid", customerid);
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
        public static List<object> GetInstallationShareChartData(string customerid)
        {
            var customer = CustomerOpsBL.GetCustomerById(Convert.ToInt32(customerid));
            string constr = ConfigurationManager.ConnectionStrings["tmidbConnectionString"].ConnectionString;
            List<object> chartData = new List<object>();
            object role = new { };
            chartData.Add(new object[]
            {
                "Customer", "Percentage"
            });
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("usp_equipment_installationCustomerPercentage"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@customerid", customerid);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        double customerPercentage = 0;
                        while (sdr.Read())
                        {
                            customerPercentage = Convert.ToDouble(sdr["customerpercentage"]);
                            chartData.Add(new object[]
                            {
                                customer.Name, customerPercentage
                            });
                        }

                        chartData.Add(new object[]
                        {
                            "Others", 100-customerPercentage
                        });
                    }


                    con.Close();
                    return chartData;
                }
            }
        }
    }
}