using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using EntityLayer;

namespace TMISparePartManagement
{
    public partial class SparePartManagement : System.Web.UI.Page
    {
        private List<SparePart> _allSpareParts;
        private SparePart _editingSparePart;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "SparePart Management";
            LoadAllSpareParts();
            DisplayAllSpareParts(_allSpareParts);

            if (!IsPostBack)
            {
                if (hfEditingSparePartModel.Value != "")
                {
                    FindEditingSparePart(true);
                }
                
            }
        }

        private void LoadAllSpareParts()
        {
            _allSpareParts = SparePartOpsBL.GetAllSpareParts();
        }

        private void DisplayAllSpareParts(Object sparePartsList)
        {
            lvSpareParts.DataSource = sparePartsList;
            lvSpareParts.DataBind();
        }

        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            if (hfEditingSparePartModel.Value != "")
            {
                //updating sparePart
                FindEditingSparePart(false);
                _editingSparePart.ModelNumber = txtSparePartModel.Text;
                _editingSparePart.Make = txtSparePartMake.Text;
                _editingSparePart.Description = txtSparePartDescription.Text;
                _editingSparePart.LifeSpanMonths  = Convert.ToInt32(txtSparePartLifeSpan.Text);
                SparePartOpsBL.UpdateSparePart(_editingSparePart);

                Response.Redirect("SparePartManagement.aspx");
            }
            else
            {
                //adding sparePart

                //checking if the model number is already used
                try
                {
                    var sparePartByModel = SparePartOpsBL.GetSparePartByModel(txtSparePartModel.Text);
                    if (sparePartByModel != null)
                    {
                        //Displaying message to the user to change the model number
                        lblDuplicateModelNumberAlert.Visible = true;
                    }
                }
                catch (RecordNotFoundException recordNotFoundException)
                {
                    //if a record with the same model number is not found, continue to add the new sparePart to the DB
                    SparePart sparePart = new SparePart
                    {
                        ModelNumber = txtSparePartModel.Text,
                        Make = txtSparePartMake.Text,
                        Description = txtSparePartDescription.Text,
                        LifeSpanMonths = Convert.ToInt32(txtSparePartLifeSpan.Text)
                    };
                    SparePartOpsBL.AddNewSparePart(sparePart);
                    Response.Redirect("SparePartManagement.aspx");
                }

                
            }

            
        }

        protected void btnEditSparePart_OnClick(object sender, EventArgs e)
        {
            var sparePartIdString = ((sender as LinkButton)?.CommandArgument);
            if (sparePartIdString == null) return;
            //setting the current editing event to hidden field for reading the ID after postbacks
            hfEditingSparePartModel.Value = sparePartIdString;
            FindEditingSparePart(true);
            btnSubmit.Text = "Update SparePart";
            lblAddEditHeading.Text = "Edit SparePart";
            btnCancelUpdate.Visible = true;
        }


        private void FindEditingSparePart(bool displayAfterFind)
        {
            try
            {
                _editingSparePart = SparePartOpsBL.GetSparePartByModel(hfEditingSparePartModel.Value);
                if(displayAfterFind) DisplayEditingSparePart();
            }
            catch (RecordNotFoundException e)
            {
                Console.WriteLine(e);
            }
        }

        private void DisplayEditingSparePart()
        {
            lblModelNumberWarning.Visible = false;
            txtSparePartModel.Enabled = false;
            txtSparePartModel.Text = _editingSparePart.ModelNumber;
            txtSparePartMake.Text = _editingSparePart.Make;
            txtSparePartDescription.Text = _editingSparePart.Description;
            txtSparePartLifeSpan.Text = _editingSparePart.LifeSpanMonths.ToString();
        }

        protected void btnCancelUpdate_OnClick(object sender, EventArgs e)
        {
            hfEditingSparePartModel.Value = "";
            Response.Redirect("SparePartManagement.aspx");
        }
    }
}