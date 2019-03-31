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
    public partial class EquipmentManagement : System.Web.UI.Page
    {
        private List<Equipment> _allEquipments;
        private Equipment _editingEquipment;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Equipment Management";
            if (!IsPostBack)
            {
                LoadAllEquipments();
                DisplayAllEquipments(_allEquipments);
            }
        }

        private void LoadAllEquipments()
        {
            _allEquipments = EquipmentOpsBL.GetAllEquipments();
        }

        private void DisplayAllEquipments(Object equipmentsList)
        {
            lvEquipments.DataSource = equipmentsList;
            lvEquipments.DataBind();
        }

        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            if (hfEditingEquipmentModel.Value != "")
            {
                //updating equipment
                FindEditingEquipment(false);
                _editingEquipment.ModelNumber = txtEquipmentModel.Text;
                _editingEquipment.Make = txtEquipmentMake.Text;
                _editingEquipment.Description = txtEquipmentDescription.Text;
                _editingEquipment.Version = txtEquipmentVersion.Text;
                _editingEquipment.SoftwareVersion = txtEquipmentSoftwareVersion.Text;
                _editingEquipment.MinimumServicePeriodMonths = Convert.ToInt32(txtEquipmentMinServicePeriod.Text);
                EquipmentOpsBL.UpdateEquipment(_editingEquipment);

                Response.Redirect("EquipmentManagement.aspx");
            }
            else
            {
                //adding equipment
                //checking if the model number is already used
                try
                {
                    var equipmentByModel = EquipmentOpsBL.GetEquipmentByModel(txtEquipmentModel.Text);
                    if (equipmentByModel != null)
                    {
                        //Displaying message to the user to change the model number
                        lblDuplicateModelNumberAlert.Visible = true;
                    }
                }
                catch (RecordNotFoundException recordNotFoundException)
                {
                    //if a record with the same model number is not found, continue to add the new equipment to the DB
                    Equipment equipment = new Equipment
                    {
                        ModelNumber = txtEquipmentModel.Text,
                        Make = txtEquipmentMake.Text,
                        Description = txtEquipmentDescription.Text,
                        Version = txtEquipmentVersion.Text,
                        SoftwareVersion = txtEquipmentSoftwareVersion.Text,
                        MinimumServicePeriodMonths = Convert.ToInt32(txtEquipmentMinServicePeriod.Text)
                    };
                    EquipmentOpsBL.AddNewEquipment(equipment);
                    Response.Redirect("EquipmentManagement.aspx");
                }

                
            }

            
        }

        protected void btnEditEquipment_OnClick(object sender, EventArgs e)
        {
            var equipmentIdString = ((sender as LinkButton)?.CommandArgument);
            if (equipmentIdString == null) return;
            //setting the current editing event to hidden field for reading the ID after postbacks
            hfEditingEquipmentModel.Value = equipmentIdString;
            FindEditingEquipment(true);
            btnSubmit.Text = "Update Equipment";
            lblAddEditHeading.Text = "Edit Equipment";
            btnCancelUpdate.Visible = true;
        }


        private void FindEditingEquipment(bool displayAfterFind)
        {
            try
            {
                _editingEquipment = EquipmentOpsBL.GetEquipmentByModel(hfEditingEquipmentModel.Value);
                if(displayAfterFind) DisplayEditingEquipment();
            }
            catch (RecordNotFoundException e)
            {
                Console.WriteLine(e);
            }
        }

        private void DisplayEditingEquipment()
        {
            lblModelNumberWarning.Visible = false;
            txtEquipmentModel.Enabled = false;
            txtEquipmentModel.Text = _editingEquipment.ModelNumber;
            txtEquipmentMake.Text = _editingEquipment.Make;
            txtEquipmentDescription.Text = _editingEquipment.Description;
            txtEquipmentVersion.Text = _editingEquipment.Version;
            txtEquipmentSoftwareVersion.Text = _editingEquipment.SoftwareVersion;
            txtEquipmentMinServicePeriod.Text = _editingEquipment.MinimumServicePeriodMonths.ToString();
        }

        protected void btnCancelUpdate_OnClick(object sender, EventArgs e)
        {
            hfEditingEquipmentModel.Value = "";
            Response.Redirect("EquipmentManagement.aspx");
        }
    }
}