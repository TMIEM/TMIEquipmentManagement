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
    public partial class ServiceRequestManagement : System.Web.UI.Page
    {
        private const string ViewStateVarServiceRequestItem = "ServiceRequestItem";

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Service Request Management";
            LoadAllEquipmentInstallations();
            LoadSpareParts();
            DisplayAllServiceRequests();
            SwitchServiceItemTab();
        }


        private void SwitchServiceItemTab()
        {
            var serviceItem = ViewState[ViewStateVarServiceRequestItem];
            if (serviceItem == null) return;
            var item = (ServiceItem) serviceItem;
            txtServiceItemType.Text = item.GetItemType().ToString();
        }

        private void DisplayAllServiceRequests()
        {
            lvServiceRequests.DataSource = ServiceRequestOpsBL.GetAllServiceRequests();
            lvServiceRequests.DataBind();
        }

        private void LoadSpareParts()
        {
            lvSparePartItems.DataSource = SparePartItemOpsBL.GetAllSparePartItems();
            lvSparePartItems.DataBind();
        }

        private void LoadAllEquipmentInstallations()
        {
            lvEquipmentItems.DataSource = EquipmentItemOpsBL.GetAllEquipmentItems();
            lvEquipmentItems.DataBind();
        }

        protected void btnChooseEquipmentItem_OnClick(object sender, EventArgs e)
        {
            var equipmentSerialNumber = ((sender as LinkButton)?.CommandArgument);
            if (equipmentSerialNumber == null) return;
            ServiceItem serviceItem = EquipmentItemOpsBL.GetEquipmentItemBySerialNumber(equipmentSerialNumber);
            ViewState[ViewStateVarServiceRequestItem] = serviceItem;
            lblChosenServiceRequestItem.Text = "Equipment Item " + equipmentSerialNumber;
            SwitchServiceItemTab();
        }

        protected void btnChooseSparePartItem_OnClick(object sender, EventArgs e)
        {
            var chosenSparePartItemSerial = ((sender as LinkButton)?.CommandArgument);
            if (chosenSparePartItemSerial == null) return;
            ServiceItem serviceItem = SparePartItemOpsBL.GetSparePartItemBySerialNumber(chosenSparePartItemSerial);
            ViewState[ViewStateVarServiceRequestItem] = serviceItem;
            lblChosenServiceRequestItem.Text = "Spare Part " + chosenSparePartItemSerial;
            SwitchServiceItemTab();
        }

        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            if (hfEditingServiceRequestId.Value != "")
            {
                //updating service request
                var editingSrId = hfEditingServiceRequestId.Value;
                var serviceRequest = ServiceRequestOpsBL.GetServiceRequestById(Convert.ToInt32(editingSrId));
                serviceRequest.Date = Convert.ToDateTime(txtServiceRequestDate.Text);
                serviceRequest.CurrentProductLocation = txtServiceRequestCurrentProductLocation.Text;
                serviceRequest.Type = txtServiceRequestType.Text;
                serviceRequest.UnderWarranty = DropDownListUnderWarranty.SelectedValue;
                serviceRequest.ProblemDetails = txtProblemDetails.Text;
                serviceRequest.ProblemOccurenceDate = Convert.ToDateTime(txtProblemOccurenceDate.Text);
                serviceRequest.ProblemFrequencyDetails = txtProblemFrequencyDetails.Text;
                serviceRequest.ProblemReproductionInstructions = txtProblemReprodcutionInstructions.Text;
                serviceRequest.ServiceItem = (ServiceItem) ViewState[ViewStateVarServiceRequestItem];
                ServiceRequestOpsBL.UpdateServiceRequest(serviceRequest);
            }
            else
            {
                //adding service request
                ServiceRequest serviceRequest = new ServiceRequest()
                {
                    Date = Convert.ToDateTime(txtServiceRequestDate.Text),
                    CurrentProductLocation = txtServiceRequestCurrentProductLocation.Text,
                    Type = txtServiceRequestType.Text,
                    UnderWarranty = DropDownListUnderWarranty.SelectedValue,
                    ProblemDetails = txtProblemDetails.Text,
                    ProblemOccurenceDate = Convert.ToDateTime(txtProblemOccurenceDate.Text),
                    ProblemFrequencyDetails = txtProblemFrequencyDetails.Text,
                    ProblemReproductionInstructions = txtProblemReprodcutionInstructions.Text,
                    ServiceItem = (ServiceItem) ViewState[ViewStateVarServiceRequestItem]
                };

                ServiceRequestOpsBL.AddNewServiceRequestRequest(serviceRequest);
            }

            Response.Redirect("ServiceRequestManagement.aspx");
        }

        protected void btnCancelUpdate_OnClick(object sender, EventArgs e)
        {
            hfEditingServiceRequestId.Value = "";
            Response.Redirect("ServiceRequestManagement.aspx");
        }

        protected void btnEditSR_OnClick(object sender, EventArgs e)
        {
            var serviceReqId = ((sender as LinkButton)?.CommandArgument);
            if (serviceReqId == null) return;
            hfEditingServiceRequestId.Value = serviceReqId;
            var serviceRequest = ServiceRequestOpsBL.GetServiceRequestById(Convert.ToInt32(serviceReqId));
            DisplayServiceRequestFields(serviceRequest);

            btnSubmit.Text = "Update Service Request";
            btnCancelUpdate.Visible = true;
        }

        private void DisplayServiceRequestFields(ServiceRequest serviceRequest)
        {
            txtServiceRequestDate.Text = serviceRequest.Date.ToShortDateString();
            txtServiceRequestCurrentProductLocation.Text = serviceRequest.CurrentProductLocation;
            txtServiceRequestType.Text = serviceRequest.Type;
            DropDownListUnderWarranty.SelectedIndex = serviceRequest.UnderWarranty == "Yes" ? 0 : 1;
            txtProblemDetails.Text = serviceRequest.ProblemDetails;
            txtProblemOccurenceDate.Text = serviceRequest.ProblemOccurenceDate.ToShortDateString();
            txtProblemFrequencyDetails.Text = serviceRequest.ProblemFrequencyDetails;
            txtProblemReprodcutionInstructions.Text = serviceRequest.ProblemReproductionInstructions;

            var serviceItem = serviceRequest.ServiceItem;
            if (serviceItem.GetItemType() == ItemType.Equipment)
            {
                ViewState[ViewStateVarServiceRequestItem] = serviceItem;
                lblChosenServiceRequestItem.Text = "Equipment Item " + ((EquipmentItem) serviceItem).SerialNumber;
            }
            else if (serviceItem.GetItemType() == ItemType.SparePart)
            {
                ViewState[ViewStateVarServiceRequestItem] = serviceItem;
                lblChosenServiceRequestItem.Text = "Spare Part Item " + ((SparePartItem) serviceItem).SerialNumber;
            }

            SwitchServiceItemTab();
        }

        protected string CreateItemLink(ServiceItem serviceItem)
        {
            if (serviceItem.GetItemType() == ItemType.Equipment)
            {
                var equipmentItem = (EquipmentItem) serviceItem;
                return "<a href='../EquipmentItemProfile.aspx?serialnumber=" + equipmentItem.SerialNumber + "' target=\"_blank\">" +
                       equipmentItem.SerialNumber + "</a> ";
            }
            else if (serviceItem.GetItemType() == ItemType.SparePart)
            {
                var sparePartItem = (SparePartItem) serviceItem;
                return "<a href='../SparePartItemProfile.aspx?serialnumber=" + sparePartItem.SerialNumber + "' target=\"_blank\">" +
                       sparePartItem.SerialNumber + "</a> ";
            }

            return "";
        }
    }
}