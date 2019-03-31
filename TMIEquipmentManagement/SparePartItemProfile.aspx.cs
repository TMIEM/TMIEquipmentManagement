using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using EntityLayer;

namespace TMIEquipmentManagement
{
    public partial class SparePartItemProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var serialNumber = Request.QueryString["serialnumber"];
            if (serialNumber == null) Response.Redirect("404.aspx");
            lblSparePartItemSerialNumber.Text = serialNumber;
            LoadSparePartDetails(serialNumber);
            LoadServiceRequests(serialNumber);
            LoadServiceResponses(serialNumber);
        }

        private void LoadServiceResponses(string serialNumber)
        {
            lvServiceResponses.DataSource = ServiceResponseOpsBL.GetAllServiceResponsesForSparePart(serialNumber);
            lvServiceResponses.DataBind();
        }

        private void LoadServiceRequests(string serialNumber)
        {
            lvServiceRequests.DataSource = ServiceRequestOpsBL.GetAllServiceRequestsForSparePart(serialNumber);
            lvServiceRequests.DataBind();
        }

        private void LoadSparePartDetails(string serialNumber)
        {
            try
            {
                var sparePartItem = SparePartItemOpsBL.GetSparePartItemBySerialNumber(serialNumber);
                DisplaySparePartItemDetails(sparePartItem);
                var sparePart = SparePartOpsBL.GetSparePartByModel(sparePartItem.SparePartModelNumber);
                DisplaySparePartDetails(sparePart);
                LoadInstallationDetails(sparePartItem);
            }
            catch (RecordNotFoundException e)
            {
                Response.Redirect("404.aspx");
            }
        }

        private void LoadInstallationDetails(SparePartItem sparePartItem)
        {
            try
            {
                var equipmentInstallation = SparePartUsageOpsBL
                    .GetSparePartUsageBySerialNumber(sparePartItem.SerialNumber);
                DisplaySparePartUsageDetails(equipmentInstallation);
                lblInstalled.Text = "Yes";
            }
            catch (RecordNotFoundException exception)
            {
                divInstallationdetails.Visible = false;
                lblInstalled.Text = "No";
            }
        }

        private void DisplaySparePartUsageDetails(SparePartUsage sparePartUsage)
        {
            var installationService = ServiceOpsBL.GetServiceById(sparePartUsage.ServiceId);
            lblUsageDate.Text = installationService.Date.ToLongDateString();
            hlUsedInService.NavigateUrl = "ServiceProfile.aspx?id="+installationService.Id.ToString();
            lblUsedInService.Text = installationService.Id.ToString();
            lblEquipmentSerial.Text = installationService.InstalledEquipmentSerialNumber;
            hlEquipment.NavigateUrl = "EquipmentItemProfile.aspx?serialnumber=" +
                                      installationService.InstalledEquipmentSerialNumber;
            lblWarranty.Text = sparePartUsage.WarrantyPeriodMonths.ToString();

            if (sparePartUsage.RemovalServiceId != 0)
            {
                var removalService = ServiceOpsBL.GetServiceById(sparePartUsage.RemovalServiceId);
                lblRemovalDate.Text = removalService.Date.ToLongDateString();
                lblRemovedInService.Text = removalService.Id.ToString();
                
                hlRemovedInService.NavigateUrl = "ServiceProfile.aspx?id=" + removalService.Id.ToString();
            }
            else
            {
                lblRemovalDate.Text = "Not Removed";
                lblRemovedInService.Text = "Not Removed";
            }

            LoadHealthInfo(sparePartUsage);
        }

        private void LoadHealthInfo(SparePartUsage sparePartUsage)
        {
            SparePartUsageHealthCalculator calculator = new SparePartUsageHealthCalculator();
            var sparePartUsages = new List<SparePartUsage>();
            sparePartUsages.Add(sparePartUsage);
            var healths = calculator.CalculateSparePartUsageHealths(sparePartUsages);
            var usageHealth = healths[0];

            var healthPercentage = usageHealth.HealthPercentage;

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
            divHealthBar.Attributes["style"] = "min-width: 8%;width:" + healthPercentage + "%;";
        }

        private void DisplaySparePartDetails(SparePart sparePart)
        {
            lblModelNumber.Text = sparePart.ModelNumber;
            lblMake.Text = sparePart.Make;
            lblDescription.Text = sparePart.Description;
            lblLifeSpan.Text = sparePart.LifeSpanMonths.ToString();
        }

        private void DisplaySparePartItemDetails(SparePartItem sparePartItem)
        {
            lblSerialNumber.Text = sparePartItem.SerialNumber;
            lblPrice.Text = sparePartItem.Price.ToString();
            lblShipmentPoNumber.Text = sparePartItem.ShipmentPoNumber;

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
                return "<a href='../SparePartItemProfile.aspx?serialnumber=" + sparePartItem.SerialNumber + "' target=\"_blank\">" +
                       sparePartItem.SerialNumber + "</a> ";
            }

            return "";
        }
    }
}