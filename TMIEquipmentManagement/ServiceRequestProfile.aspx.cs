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
    public partial class ServiceRequestProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = Request.QueryString["id"];
            if (id == null) Response.Redirect("404.aspx");
            lblServiceRequestId.Text = id;
            LoadServiceRequestDetails(id);
            LoadResponses(id);
        }

        private void LoadResponses(string id)
        {
            lvServiceResponses.DataSource = ServiceResponseOpsBL.GetAllServiceResponsesForRequest(Convert.ToInt32(id));
            lvServiceResponses.DataBind();
        }

        private void LoadServiceRequestDetails(string id)
        {
            try
            {
                var serviceRequest = ServiceRequestOpsBL.GetServiceRequestById(Convert.ToInt32(id));
                DisplaySrDetails(serviceRequest);
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

        private void DisplaySrDetails(ServiceRequest serviceRequest)
        {
            lblServiceRequestId2.Text = serviceRequest.Id.ToString();
            var itemType = serviceRequest.ServiceItem.GetItemType();
            lblServiceItemType.Text = itemType.ToString();
            switch (itemType)
            {
                case ItemType.Equipment:
                    var equipmentItem = (EquipmentItem) serviceRequest.ServiceItem;
                    hlServiceItem.Text = equipmentItem.SerialNumber;
                    hlServiceItem.NavigateUrl = "EquipmentItemProfile.aspx?serialnumber=" + equipmentItem.SerialNumber;
                    break;
                case ItemType.SparePart:
                    var sparePartItem = (SparePartItem)serviceRequest.ServiceItem;
                    hlServiceItem.Text = sparePartItem.SerialNumber;
                    hlServiceItem.NavigateUrl = "SparePartItemProfile.aspx?serialnumber=" + sparePartItem.SerialNumber;
                    break;
                default:
                    break;
            }

            lblDate.Text = serviceRequest.Date.ToLongDateString();
            lblProductLocation.Text = serviceRequest.CurrentProductLocation;
            lblType.Text = serviceRequest.Type;
            lblUnderWarranty.Text = serviceRequest.UnderWarranty;
            lblProblemDetails.Text = serviceRequest.ProblemDetails;
            lblProblemDate.Text = serviceRequest.ProblemOccurenceDate.ToLongDateString();
            lblProblemFrequency.Text = serviceRequest.ProblemFrequencyDetails;
            lblProblemReproduction.Text = serviceRequest.ProblemReproductionInstructions;

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