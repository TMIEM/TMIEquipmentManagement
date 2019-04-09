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
    public partial class EquipmentItemProfile : System.Web.UI.Page
    {
        private EquipmentInstallation _equipmentInstallation;

        protected void Page_Load(object sender, EventArgs e)
        {
            var serialNumber = Request.QueryString["serialnumber"];
            if (serialNumber == null) Response.Redirect("404.aspx");
            lblEquipmentItemSerialNumber.Text = serialNumber;
            LoadEquipmentDetails(serialNumber);
            LoadServiceDetails(serialNumber);
            LoadServiceRequests(serialNumber);
            LoadServiceResponses(serialNumber);
            LoadUsedSpares(serialNumber);
            LoadUsedConsumables(serialNumber);
        }

        private void LoadServiceResponses(string serialNumber)
        {
            var serviceResponses = ServiceResponseOpsBL.GetAllServiceResponsesForEquipment(serialNumber);
            lvServiceResponses.DataSource = serviceResponses;
            lvServiceResponses.DataBind();
        }

        private void LoadServiceRequests(string serialNumber)
        {
            var serviceRequests = ServiceRequestOpsBL.GetAllServiceRequestsForEquipment(serialNumber);
            lvServiceRequests.DataSource = serviceRequests;
            lvServiceRequests.DataBind();
        }

        private void LoadServiceDetails(string serialNumber)
        {
            var services = ServiceOpsBL.GetAllServicesForInstallation(new EquipmentInstallation()
            {
                EquipmentItemSerialNumber = serialNumber
            });
            foreach (var service in services)
            {
                service.Technician = TechnicianOpsBL.GetTechnicianById(service.TechnicianId);
            }

            lvServices.DataSource = services;
            lvServices.DataBind();
        }

        private void LoadEquipmentDetails(string serialNumber)
        {
            try
            {
                var equipmentItem = EquipmentItemOpsBL.GetEquipmentItemBySerialNumber(serialNumber);
                DisplayEquipmentItemDetails(equipmentItem);
                var equipment = EquipmentOpsBL.GetEquipmentByModel(equipmentItem.EquipmentModelNumber);
                DisplayEquipmentDetails(equipment);
                LoadInstallationDetails(equipmentItem);
            }
            catch (RecordNotFoundException e)
            {
                Response.Redirect("404.aspx");
            }
        }

        private void LoadInstallationDetails(EquipmentItem equipmentItem)
        {
            try
            {
                var equipmentInstallation = EquipmentInstallationOpsBL
                    .GetEquipmentInstallationBySerial(equipmentItem.SerialNumber);

                _equipmentInstallation = equipmentInstallation;
                DisplayEquipmentInstallationDetails(equipmentInstallation);
                lblInstalled.Text = "Yes";
            }
            catch (RecordNotFoundException exception)
            {
                divInstallationdetails.Visible = false;
                lblInstalled.Text = "No";
            }
        }

        private void DisplayEquipmentInstallationDetails(EquipmentInstallation equipmentInstallation)
        {
            lblInstallationDate.Text = equipmentInstallation.InstallationDate.ToLongDateString();
            lblWarranty.Text = equipmentInstallation.WarrantyPeriodMonths.ToString();
            lblServiceAgreement.Text = equipmentInstallation.ServiceAgreementPeriodMonths.ToString();
            lblInvoiceId.Text = equipmentInstallation.InvoiceId;

            try
            {
                var lastServiceForEquipment = ServiceOpsBL
                    .GetLastServiceForEquipmentByDate(equipmentInstallation.EquipmentItemSerialNumber);
                lblLastServiceDate.Text = lastServiceForEquipment.Date.ToLongDateString();
            }
            catch (RecordNotFoundException exception)
            {
                lblLastServiceDate.Text = "No services done";
            }
            

            var customer = CustomerOpsBL.GetCustomerById(equipmentInstallation.CustomerId);
            hlCustomer.NavigateUrl = "CustomerProfile.aspx?id=" + customer.Id;
            lblCustomerName.Text = customer.Name;
            var isCommissioned = equipmentInstallation.CommissioningDate != DateTime.MinValue;

            if (isCommissioned)
            {
                lblCommissioningDate.Text = equipmentInstallation.CommissioningDate.ToLongDateString();
                btnCommissionEquipment.Text = "Update Commissioning Date";
            }
            else
            {
                lblCommissioningDate.Text = "Not Commissioned";
                btnCommissionEquipment.Text = "Commission Equipment";
            }

            
            LoadHealthInfo(equipmentInstallation);

        }

        private void LoadHealthInfo(EquipmentInstallation equipmentInstallation)
        {
            EquipmentItemHealthCalculator calculator = new EquipmentItemHealthCalculator();
            var equipmentInstallations = new List<EquipmentInstallation>();
            equipmentInstallations.Add(equipmentInstallation);
            var equipmentItemHealths = calculator.CalcuEquipmentItemHealths(equipmentInstallations);
            var equipmentItemHealth = equipmentItemHealths[0];

            var healthPercentage = equipmentItemHealth.HealthPercentage;

            string css = "progress-bar progress-bar-striped active ";
            if (healthPercentage < 70 && healthPercentage > 40)
            {
                css += "progress-bar-warning";
            }
            else if (healthPercentage < 40)
            {
                css += "progress-bar-danger";
            }
            else
            {
                css += "progress-bar-success";
            }

            lblHealthPercent.Text = healthPercentage + "%";
            divHealthBar.Attributes["class"] = css;
            divHealthBar.Attributes["aria-valuenow"] = healthPercentage.ToString();
            divHealthBar.Attributes["data-healthpercent"] = healthPercentage.ToString();
            divHealthBar.Attributes["style"] = "min-width: 8%;width:" + healthPercentage+"%;";

        }

        private void DisplayEquipmentDetails(Equipment equipment)
        {
            lblModelNumber.Text = equipment.ModelNumber;
            lblDescription.Text = equipment.Description;
            lblMake.Text = equipment.Make;
            lblVersion.Text = equipment.Version;
            lblSoftwareVersion.Text = equipment.SoftwareVersion;
            lblMinServicePeriod.Text = equipment.MinimumServicePeriodMonths.ToString();
        }

        private void DisplayEquipmentItemDetails(EquipmentItem equipmentItem)
        {
            lblSerialNumber.Text = equipmentItem.SerialNumber;
            lblPrice.Text = equipmentItem.Price.ToString();
            lblShipmentPoNumber.Text = equipmentItem.ShipmentPoNumber;
        }

        protected string CreateItemLink(ServiceItem serviceItem)
        {
            if (serviceItem == null)
            {
                return "No Replacement";
            }

            if (serviceItem.GetItemType() == ItemType.Equipment)
            {
                var equipmentItem = (EquipmentItem)serviceItem;
                return "<a href='../EquipmentItemProfile.aspx?serialnumber=" + equipmentItem.SerialNumber + "' target=\"_blank\">" +
                       equipmentItem.SerialNumber + "</a> ";
            }
            else if (serviceItem.GetItemType() == ItemType.SparePart)
            {
                var sparePartItem = (SparePartItem)serviceItem;
                return "<a href='../SparePartItem.aspx?id=" + sparePartItem.SerialNumber + "' target=\"_blank\">" +
                       sparePartItem.SerialNumber + "</a> ";
            }

            return "";
        }


        [WebMethod]
        public static List<object> GetServiceChartData(string equipmentserialnumber)
        {
            
            List<object> chartData = new List<object>();
            object role = new { };
            chartData.Add(new object[]
            {
                "Date", "Service ID"
            });

            var services 
                = ServiceOpsBL.GetAllServicesForInstallation(new EquipmentInstallation(){EquipmentItemSerialNumber 
                    = equipmentserialnumber });
            foreach (var service in services)
            {
                chartData.Add(new object[]
                {
                    service.Date.ToShortDateString(), service.Id
                });
            }

            return chartData;
        }

        private void LoadUsedSpares(string equipmentSerial)
        {
            lvSparePartUsages.DataSource =
                SparePartUsageOpsBL.GetSparePartUsagesByEquipment(equipmentSerial);
            lvSparePartUsages.DataBind();
        }

        private void LoadUsedConsumables(string equipmentSerial)
        {
            lvConsumableBatchUsages.DataSource =
                ConsumableBatchServiceUsageOpsBL.GetConsumableServiceUsagesByEquipment(equipmentSerial);
            lvConsumableBatchUsages.DataBind();
        }

        protected void btnCommissionEquipment_OnClick(object sender, EventArgs e)
        {
            var commissioningDate = Convert.ToDateTime(txtCommissioningDate.Text.ToString());
            _equipmentInstallation.CommissioningDate = commissioningDate;
            EquipmentInstallationOpsBL.UpdateEquipmentCommissioningDate(_equipmentInstallation);
            LoadInstallationDetails(_equipmentInstallation.EquipmentItem);

        }
    }
}