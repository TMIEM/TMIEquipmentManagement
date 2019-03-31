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
    public partial class EquipmentInstallationManagement : System.Web.UI.Page
    {
        private List<EquipmentInstallation> _allEquipmentInstallations;
        private EquipmentInstallation _editingEquipmentInstallation;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "EquipmentInstallation Management";
            if (!IsPostBack)
            {
                LoadAllEquipmentInstallations();
                DisplayAllEquipmentInstallations(_allEquipmentInstallations);
                DisplayAvailabelEquipment();
                DisplayCustomers();
            }
        }

        private void DisplayCustomers()
        {
            lvCustomers.DataSource = CustomerOpsBL.GetAllCustomers();
            lvCustomers.DataBind();
        }

        private void DisplayAvailabelEquipment()
        {
            lvAvailableEquipmentItems.DataSource = EquipmentItemOpsBL.GetAvailableEquipmentItems();
            lvAvailableEquipmentItems.DataBind();
        }

        private void LoadAllEquipmentInstallations()
        {
            _allEquipmentInstallations = EquipmentInstallationOpsBL.GetAllEquipmentInstallations();
        }

        private void DisplayAllEquipmentInstallations(Object equipmentInstallationsList)
        {
            lvEquipmentInstallations.DataSource = equipmentInstallationsList;
            lvEquipmentInstallations.DataBind();
        }

        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            if (hfEditingEquipmentInstallationSerial.Value != "")
            {
                //updating equipmentInstallation
                FindEditingEquipmentInstallation(false);
                _editingEquipmentInstallation.EquipmentItemSerialNumber = txtEquipmentSerialNumber.Text;
                _editingEquipmentInstallation.InstallationDate = Convert.ToDateTime(txtEquipmentInstallationDate.Text);
                _editingEquipmentInstallation.WarrantyPeriodMonths = Convert.ToInt32(txtEquipmentInstallationWarrantyPeriod.Text);
                _editingEquipmentInstallation.ServiceAgreementPeriodMonths = Convert.ToInt32(txtEquipmentInstallationSAPeriod.Text);
                _editingEquipmentInstallation.InvoiceId = txtEquipmentInstallationInvoiceId.Text;
                _editingEquipmentInstallation.CustomerId = Convert.ToInt32(txtEquipmentInstallationCustomerId.Text);
                EquipmentInstallationOpsBL.UpdateEquipmentInstallation(_editingEquipmentInstallation);
                Response.Redirect("EquipmentInstallationManagement.aspx");
            }
            else
            {
                //adding equipmentInstallation
                //checking if the serial number is already installed
                try
                {
                    var equipmentInstallationByModel = EquipmentInstallationOpsBL.GetEquipmentInstallationBySerial(txtEquipmentSerialNumber.Text);
                    if (equipmentInstallationByModel != null)
                    {
                        //Displaying message to the user to change the model number
                        //lblD.Visible = true;
                    }
                }
                catch (RecordNotFoundException recordNotFoundException)
                {
                    //if a record with the same model number is not found, continue to add the new equipmentInstallation to the DB
                    EquipmentInstallation equipmentInstallation = new EquipmentInstallation
                    {
                        EquipmentItemSerialNumber = txtEquipmentSerialNumber.Text,
                        InstallationDate = Convert.ToDateTime(txtEquipmentInstallationDate.Text),
                        WarrantyPeriodMonths = Convert.ToInt32(txtEquipmentInstallationWarrantyPeriod.Text),
                        ServiceAgreementPeriodMonths = Convert.ToInt32(txtEquipmentInstallationSAPeriod.Text),
                        InvoiceId = txtEquipmentInstallationInvoiceId.Text,
                        CustomerId = Convert.ToInt32(txtEquipmentInstallationCustomerId.Text)
                    };
                    EquipmentInstallationOpsBL.AddNewEquipmentInstallation(equipmentInstallation);
                    Response.Redirect("EquipmentInstallationManagement.aspx");
                }

                
            }

            
        }

        protected void btnEditEquipmentInstallation_OnClick(object sender, EventArgs e)
        {
            var equipmentInstallationIdString = ((sender as LinkButton)?.CommandArgument);
            if (equipmentInstallationIdString == null) return;
            //setting the current editing event to hidden field for reading the ID after postbacks
            hfEditingEquipmentInstallationSerial.Value = equipmentInstallationIdString;
            FindEditingEquipmentInstallation(true);
            btnSubmit.Text = "Update Equipment Installation";
            lblAddEditHeading.Text = "Edit Equipment Installation";
            divAvailableEquipments.Visible = false;
            btnCancelUpdate.Visible = true;
        }


        private void FindEditingEquipmentInstallation(bool displayAfterFind)
        {
            try
            {
                _editingEquipmentInstallation = EquipmentInstallationOpsBL.GetEquipmentInstallationBySerial(hfEditingEquipmentInstallationSerial.Value);
                if(displayAfterFind) DisplayEditingEquipmentInstallation();
            }
            catch (RecordNotFoundException e)
            {
                Console.WriteLine(e);
            }
        }

        private void DisplayEditingEquipmentInstallation()
        {
            lblSerialNumberWarning.Visible = false;
            txtEquipmentSerialNumber.Enabled = false;
            txtEquipmentSerialNumber.Text = _editingEquipmentInstallation.EquipmentItemSerialNumber;
            txtEquipmentInstallationDate.Text = _editingEquipmentInstallation.InstallationDate.ToShortDateString();
            txtEquipmentInstallationWarrantyPeriod.Text = _editingEquipmentInstallation.WarrantyPeriodMonths.ToString();
            txtEquipmentInstallationSAPeriod.Text = _editingEquipmentInstallation.ServiceAgreementPeriodMonths.ToString();
            txtEquipmentInstallationInvoiceId.Text = _editingEquipmentInstallation.InvoiceId;
            txtEquipmentInstallationCustomerId.Text = _editingEquipmentInstallation.CustomerId.ToString();
        }

        protected void btnCancelUpdate_OnClick(object sender, EventArgs e)
        {
            hfEditingEquipmentInstallationSerial.Value = "";
            Response.Redirect("EquipmentInstallationManagement.aspx");
        }

        protected void btnChooseEquipmentItem_OnClick(object sender, EventArgs e)
        {
            var equipmentSerialNumber = ((sender as LinkButton)?.CommandArgument);
            if (equipmentSerialNumber == null) return;
            txtEquipmentSerialNumber.Text = equipmentSerialNumber;
        }

        protected void btnChooseCustomer_OnClick(object sender, EventArgs e)
        {
            var customerId = ((sender as LinkButton)?.CommandArgument);
            if (customerId == null) return;
            txtEquipmentInstallationCustomerId.Text = customerId;
        }
    }
}