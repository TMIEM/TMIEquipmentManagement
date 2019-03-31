using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using EntityLayer;
using Convert = System.Convert;

namespace TMIEquipmentManagement
{
    public partial class ServiceResponseManagement : System.Web.UI.Page
    {
        private const string ViewStateVarServiceReplacementItem = "ServiceReplacementItem";
        protected void Page_Load(object sender, EventArgs e)
        {
            DisplayAllServiceRequests();
            DisplayAllEquipmentItems();
            DisplayAllSparePartItems();
            DisplayAllServiceResponses();
            DisplayReplacementSelection();
            SetResponseItem();
        }

        private void DisplayAllServiceResponses()
        {
            lvServiceResponses.DataSource = ServiceResponseOpsBL.GetAllServiceResponses();
            lvServiceResponses.DataBind();
        }

        private void SetResponseItem()
        {
            var serviceItem = ViewState[ViewStateVarServiceReplacementItem];
            if (serviceItem == null) return;
            var item = (ServiceItem)serviceItem;
            txtServiceItemType.Text = item.GetItemType().ToString();
        }

        private void DisplayAllSparePartItems()
        {
            lvSparePartItems.DataSource = SparePartItemOpsBL.GetAllSparePartItems();
            lvSparePartItems.DataBind();
        }

        private void DisplayAllEquipmentItems()
        {
            lvEquipmentItems.DataSource = EquipmentItemOpsBL.GetAllEquipmentItems();
            lvEquipmentItems.DataBind();
        }

        private void DisplayAllServiceRequests()
        {
            lvServiceRequests.DataSource = ServiceRequestOpsBL.GetAllServiceRequests();
            lvServiceRequests.DataBind();
        }

        protected void btnChooseEquipmentItem_OnClick(object sender, EventArgs e)
        {
            var equipmentSerialNumber = ((sender as LinkButton)?.CommandArgument);
            if (equipmentSerialNumber == null) return;
            ServiceItem serviceItem = EquipmentItemOpsBL.GetEquipmentItemBySerialNumber(equipmentSerialNumber);
            ViewState[ViewStateVarServiceReplacementItem] = serviceItem;
            divReplacedItemCallout.Visible = true;
            lblChosenServiceResponseItem.Text = "Equipment Item " + equipmentSerialNumber;
            SetResponseItem();
            
        }

        protected void btnChooseSparePartItem_OnClick(object sender, EventArgs e)
        {
            var chosenSparePartItemSerial = ((sender as LinkButton)?.CommandArgument);
            if (chosenSparePartItemSerial == null) return;
            ServiceItem serviceItem = SparePartItemOpsBL.GetSparePartItemBySerialNumber(chosenSparePartItemSerial);
            ViewState[ViewStateVarServiceReplacementItem] = serviceItem;
            divReplacedItemCallout.Visible = true;
            lblChosenServiceResponseItem.Text = "Spare Part " + chosenSparePartItemSerial;
            SetResponseItem();
        }

        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            if (hfEditingServiceResponseId.Value != "")
            {
                //updating service request
                var editingSrId = hfEditingServiceResponseId.Value;
                var serviceResponse = ServiceResponseOpsBL.GetServiceResponseById(Convert.ToInt32(editingSrId));
                serviceResponse.Date = Convert.ToDateTime(txtServiceResponseDate.Text);
                serviceResponse.ServiceDescription = txtServiceDescription.Text;
                serviceResponse.CoveredByWarranty = DropDownListCoveredByWarranty.SelectedValue;
                serviceResponse.ChargesPaid = Convert.ToDouble(txtChargesPaid.Text);
                serviceResponse.ServiceRequestId = Convert.ToInt32(txtServiceRequestId.Text);
                serviceResponse.ReplacementItem = (ServiceItem)ViewState[ViewStateVarServiceReplacementItem];
                ServiceResponseOpsBL.UpdateServiceResponse(serviceResponse);
            }
            else
            {
                //adding service request
                ServiceResponse serviceResponse = new ServiceResponse()
                {
                    Date = Convert.ToDateTime(txtServiceResponseDate.Text),
                    ServiceDescription = txtServiceDescription.Text,
                    CoveredByWarranty = DropDownListCoveredByWarranty.SelectedValue,
                    ChargesPaid = Convert.ToDouble(txtChargesPaid.Text),
                    ServiceRequestId = Convert.ToInt32(txtServiceRequestId.Text),
                    ReplacementItem = (ServiceItem) ViewState[ViewStateVarServiceReplacementItem]
                };

                ServiceResponseOpsBL.AddNewServiceResponse(serviceResponse);
            }

            Response.Redirect("ServiceResponseManagement.aspx");
        }

        protected void btnCancelUpdate_OnClick(object sender, EventArgs e)
        {
            hfEditingServiceResponseId.Value = "";
            Response.Redirect("ServiceResponseManagement.aspx");
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

        protected void btnEditSR_OnClick(object sender, EventArgs e)
        {
            var serviceResponseId = ((sender as LinkButton)?.CommandArgument);
            if (serviceResponseId == null) return;
            hfEditingServiceResponseId.Value = serviceResponseId;
            var serviceResponse = ServiceResponseOpsBL.GetServiceResponseById(Convert.ToInt32(serviceResponseId));
            DisplayServiceResponseFields(serviceResponse);

            btnSubmit.Text = "Update Service Response";
            btnCancelUpdate.Visible = true;
        }

        private void DisplayServiceResponseFields(ServiceResponse serviceResponse)
        {
            txtServiceResponseDate.Text = serviceResponse.Date.ToShortDateString();
            txtServiceDescription.Text = serviceResponse.ServiceDescription;
            DropDownListCoveredByWarranty.SelectedIndex = serviceResponse.CoveredByWarranty == "Yes" ? 0 : 1;
            txtChargesPaid.Text = serviceResponse.ChargesPaid.ToString();
            txtServiceRequestId.Text = serviceResponse.ServiceRequestId.ToString();

            var replacementItem = serviceResponse.ReplacementItem;
            if (replacementItem.GetItemType() == ItemType.Equipment)
            {
                ViewState[ViewStateVarServiceReplacementItem] = replacementItem;
                lblChosenServiceResponseItem.Text = "Equipment Item " + ((EquipmentItem)replacementItem).SerialNumber;
            }
            else if (replacementItem.GetItemType() == ItemType.SparePart)
            {
                ViewState[ViewStateVarServiceReplacementItem] = replacementItem;
                lblChosenServiceResponseItem.Text = "Spare Part Item " + ((SparePartItem)replacementItem).SerialNumber;
            }

            DisplayReplacementSelection();
            SetResponseItem();
        }

        protected string CreateServiceRequestItemLink(ServiceItem serviceItem)
        {
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

        protected void btnChooseSR_OnClick(object sender, EventArgs e)
        {
            var serviceRequestId = ((sender as LinkButton)?.CommandArgument);
            if (serviceRequestId == null) return;
            txtServiceRequestId.Text = serviceRequestId;
            DisplayReplacementSelection();
            ClearReplacedItem();

        }

        private void ClearReplacedItem()
        {
            divReplacedItemCallout.Visible = false;
            txtServiceItemType.Text = "";
            ViewState[ViewStateVarServiceReplacementItem] = null;
        }

        protected void DropDownListReplacementReceived_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayReplacementSelection();
            SetResponseItem();
        }

        private void DisplayReplacementSelection()
        {
            var selectedIndex = DropDownListReplacementReceived.SelectedIndex;
            if (selectedIndex == 0)
            {
                rfvServiceItem.Enabled = true;
                //Yes
                Exception exc;
                ServiceRequest serviceRequest = null;

                try
                {
                    var serviceRequestId = txtServiceRequestId.Text;
                    serviceRequest = ServiceRequestOpsBL.GetServiceRequestById(Convert.ToInt32(serviceRequestId));
                }
                catch (Exception exception)
                {
                    ///do nothing
                }

                if (serviceRequest == null)
                {
                    lblReplacementReceivedError.Text = "Please choose a valid service request ID to continue";
                    lblReplacementReceivedError.Visible = true;
                    divReplacedItemCallout.Visible = false;
                    DropDownListReplacementReceived.SelectedIndex = 1;
                }
                else
                {
                    divReplacedItemCallout.Visible = true;

                    switch (serviceRequest.ServiceItem.GetItemType())
                    {
                        case ItemType.Equipment:
                            divEquipmentItemsTable.Visible = true;
                            divSparePartItemsTable.Visible = false;
                            break;
                        case ItemType.SparePart:
                            divEquipmentItemsTable.Visible = false;
                            divSparePartItemsTable.Visible = true;
                            break;
                        default:
                            break;
                    }
                }

            }
            else
            {
                //No
                ViewState[ViewStateVarServiceReplacementItem] = null;
                rfvServiceItem.Enabled = false;
                divEquipmentItemsTable.Visible = false;
                divSparePartItemsTable.Visible = false;
                divReplacedItemCallout.Visible = false;
                lblReplacementReceivedError.Visible = false;
            }
        }
    }
}