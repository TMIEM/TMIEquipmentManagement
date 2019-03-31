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
    public partial class ServiceProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var serviceId = Request.QueryString["id"];
            if (serviceId == null) Response.Redirect("404.aspx");
            lblServiceId.Text = serviceId;
            LoadServiceDetails(serviceId);
            LoadUsedSpares(serviceId);
            LoadUsedConsumables(serviceId);
            LoadRemovedSpares(serviceId);
            LoadRemovedConsumables(serviceId);
        }

        private void LoadRemovedConsumables(string serviceId)
        {
            var removals =
                ConsumableBatchServiceUsageOpsBL.GetConsumablesRemovedByServiceId(Convert.ToInt32(serviceId));
            foreach (var removal in removals)
            {
                var service = ServiceOpsBL.GetServiceById(removal.ServiceId);
                var consumableBatch = ConsumableBatchOpsBL.GetConsumableBatchById(removal.ConsumableBatchModelNumber,
                    removal.ConsumbaleBatchShipmentPONumber);
                removal.Service = service;
                removal.ConsumableBatch = consumableBatch;
            }


            lvConsumableRemovals.DataSource = removals;
            lvConsumableRemovals.DataBind();
        }

        private void LoadRemovedSpares(string serviceId)
        {
            var removedSpares = SparePartUsageOpsBL.GetSparePartsRemovedByServiceId(Convert.ToInt32(serviceId));
            foreach (var sparePartUsage in removedSpares)
            {
                var sparePartItem =
                    SparePartItemOpsBL.GetSparePartItemBySerialNumber(sparePartUsage.SparePartItemSerialNumber);
                var service = ServiceOpsBL.GetServiceById(sparePartUsage.ServiceId);
                sparePartUsage.SparePartItem = sparePartItem;
                sparePartUsage.Service = service;
            }


            lvSparePartRemovals.DataSource = removedSpares;
            lvSparePartRemovals.DataBind();
        }

        private void LoadUsedConsumables(string serviceId)
        {
            lvConsumableBatchUsages.DataSource =
                ConsumableBatchServiceUsageOpsBL.GetConsumableBatchServiceUsagesByServiceId(Convert.ToInt32(serviceId));
            lvConsumableBatchUsages.DataBind();
        }

        private void LoadUsedSpares(string serviceId)
        {
            lvSparePartUsages.DataSource =
                SparePartUsageOpsBL.GetSparePartUsagesByServiceId(Convert.ToInt32(serviceId));
            lvSparePartUsages.DataBind();
        }

        private void LoadServiceDetails(string serviceId)
        {
            try
            {
                var service = ServiceOpsBL.GetServiceById(Convert.ToInt32(serviceId));
                DisplayServiceDetailst(service);
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

        private void DisplayServiceDetailst(Service service)
        {
            lblServiceId2.Text = service.Id.ToString();
            lblDate.Text = service.Date.ToLongDateString();
            lblProblemDescription.Text = service.ProblemDescription;
            lblServiceDescription.Text = service.ServiceDescription;
            lblSpecialNote.Text = service.SpecialNote;
            hlEquipmentItem.Text = service.InstalledEquipmentSerialNumber;
            hlEquipmentItem.NavigateUrl =
                "EquipmentItemProfile.aspx?serialnumber=" + service.InstalledEquipmentSerialNumber;
            var technician = TechnicianOpsBL.GetTechnicianById(service.TechnicianId);
            hlTechnician.Text = technician.Name;
            hlTechnician.NavigateUrl = "TechnicianProfile.aspx?id=" + technician.Id;
        }
    }
}