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
    public partial class ServiceResponseProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = Request.QueryString["id"];
            if (id == null) Response.Redirect("404.aspx");
            lblServiceResponseId.Text = id;
            LoadServiceResponseDetails(id);
        }

        private void LoadServiceResponseDetails(string id)
        {
            try
            {
                var serviceResponse = ServiceResponseOpsBL.GetServiceResponseById(Convert.ToInt32(id));
                DisplayResponseDetails(serviceResponse);
                DisplayRequestDetails(serviceResponse.ServiceRequestId);
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

        private void DisplayRequestDetails(int serviceRequestId)
        {
            var serviceRequest = ServiceRequestOpsBL.GetServiceRequestById(serviceRequestId);

            lblServiceRequestId2.Text = serviceRequest.Id.ToString();
            var itemType = serviceRequest.ServiceItem.GetItemType();
            lblServiceItemType.Text = itemType.ToString();
            switch (itemType)
            {
                case ItemType.Equipment:
                    var equipmentItem = (EquipmentItem)serviceRequest.ServiceItem;
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

        private void DisplayResponseDetails(ServiceResponse serviceResponse)
        {
            lblServiceResponseId2.Text = serviceResponse.Id.ToString();
            lblDate.Text = serviceResponse.Date.ToLongDateString();
            lblServiceDescription.Text = serviceResponse.ServiceDescription;
            lblCoveredByWarranty.Text = serviceResponse.CoveredByWarranty;
            lblChargesPaid.Text = serviceResponse.ChargesPaid.ToString();

            if (serviceResponse.ReplacementItem == null)
            {
                hlReplacementItem.Text = "No Replacement";
            }
            else
            {
                var itemType = serviceResponse.ReplacementItem.GetItemType();
                switch (itemType)
                {
                    case ItemType.Equipment:
                        var equipmentItem = (EquipmentItem)serviceResponse.ReplacementItem;
                        hlReplacementItem.Text = equipmentItem.SerialNumber;
                        hlReplacementItem.NavigateUrl = "EquipmentItemProfile.aspx?serialnumber=" + equipmentItem.SerialNumber;
                        break;
                    case ItemType.SparePart:
                        var sparePartItem = (SparePartItem)serviceResponse.ReplacementItem;
                        hlReplacementItem.Text = sparePartItem.SerialNumber;
                        hlReplacementItem.NavigateUrl = "SparePartItemProfile.aspx?serialnumber=" + sparePartItem.SerialNumber;
                        break;
                    default:
                        break;
                }
            }
            
        }
    }
}