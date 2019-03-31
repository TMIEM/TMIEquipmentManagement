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
    public partial class TechnicianManagement : System.Web.UI.Page
    {
        private List<Technician> _allTechnicians;
        private Technician _editingTechnician;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Technician Management";
            LoadAllTechnicians();
            DisplayAllTechnicians(_allTechnicians);

            if (!IsPostBack)
            {
                if (hfEditingTechnicianId.Value != "")
                {
                    FindEditingTechnician(true);
                }
                
            }
        }

        private void LoadAllTechnicians()
        {
            _allTechnicians = TechnicianOpsBL.GetAllTechnicians();
        }

        private void DisplayAllTechnicians(Object techniciansList)
        {
            lvTechnicians.DataSource = techniciansList;
            lvTechnicians.DataBind();
        }

        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            if (hfEditingTechnicianId.Value != "")
            {
                //updating technician
                FindEditingTechnician(false);
                _editingTechnician.Name = txtTechnicianName.Text;
                _editingTechnician.ContactNumber = txtTechnicianContact.Text;
                _editingTechnician.Email = txtTechnicianEmail.Text;
                TechnicianOpsBL.UpdateTechnician(_editingTechnician);

            }
            else
            {
                //adding technician
                Technician technician = new Technician();
                technician.Name = txtTechnicianName.Text;
                technician.ContactNumber = txtTechnicianContact.Text;
                technician.Email = txtTechnicianEmail.Text;
                TechnicianOpsBL.AddNewTechnician(technician);
            }

            Response.Redirect("TechnicianManagement.aspx");
        }

        protected void btnEditTechnician_OnClick(object sender, EventArgs e)
        {
            var technicianIdString = ((sender as LinkButton)?.CommandArgument);
            if (technicianIdString == null) return;
            //setting the current editing event to hidden field for reading the ID after postbacks
            hfEditingTechnicianId.Value = technicianIdString;
            FindEditingTechnician(true);
            btnSubmit.Text = "Update Technician";
            lblAddEditHeading.Text = "Edit Technician";
            btnCancelUpdate.Visible = true;
        }


        private void FindEditingTechnician(bool displayAfterFind)
        {
            try
            {
                _editingTechnician = TechnicianOpsBL.GetTechnicianById(Convert.ToInt32(hfEditingTechnicianId.Value));
                if(displayAfterFind) DisplayEditingTechnician();
            }
            catch (RecordNotFoundException e)
            {
                Console.WriteLine(e);
            }
        }

        private void DisplayEditingTechnician()
        {
            txtTechnicianName.Text = _editingTechnician.Name;
            txtTechnicianContact.Text = _editingTechnician.ContactNumber;
            txtTechnicianEmail.Text = _editingTechnician.Email;
        }

        protected void btnCancelUpdate_OnClick(object sender, EventArgs e)
        {
            hfEditingTechnicianId.Value = "";
            Response.Redirect("TechnicianManagement.aspx");
        }
    }
}