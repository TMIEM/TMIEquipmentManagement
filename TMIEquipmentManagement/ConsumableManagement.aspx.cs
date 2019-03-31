using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using EntityLayer;

namespace TMIConsumableManagement
{
    public partial class ConsumableManagement : System.Web.UI.Page
    {
        private List<Consumable> _allConsumables;
        private Consumable _editingConsumable;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Consumable Management";
            if (!IsPostBack)
            {
                LoadAllConsumables();
                DisplayAllConsumables(_allConsumables);
            }
        }

        private void LoadAllConsumables()
        {
            _allConsumables = ConsumableOpsBL.GetAllConsumables();
        }

        private void DisplayAllConsumables(Object consumablesList)
        {
            lvConsumables.DataSource = consumablesList;
            lvConsumables.DataBind();
        }

        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            if (hfEditingConsumableModel.Value != "")
            {
                //updating consumable
                FindEditingConsumable(false);
                _editingConsumable.ModelNumber = txtConsumableModel.Text;
                _editingConsumable.Make = txtConsumableMake.Text;
                _editingConsumable.Description = txtConsumableDescription.Text;
                _editingConsumable.LifeSpanDays = Convert.ToInt32(txtConsumableLifeSpan.Text);
                ConsumableOpsBL.UpdateConsumable(_editingConsumable);

                Response.Redirect("ConsumableManagement.aspx");
            }
            else
            {
                //adding consumable
                //checking if the model number is already used
                try
                {
                    var consumableByModel = ConsumableOpsBL.GetConsumableByModelNumber(txtConsumableModel.Text);
                    if (consumableByModel != null)
                    {
                        //Displaying message to the user to change the model number
                        lblDuplicateModelNumberAlert.Visible = true;
                    }
                }
                catch (RecordNotFoundException recordNotFoundException)
                {
                    //if a record with the same model number is not found, continue to add the new consumable to the DB
                    Consumable consumable = new Consumable
                    {
                        ModelNumber = txtConsumableModel.Text,
                        Make = txtConsumableMake.Text,
                        Description = txtConsumableDescription.Text,
                        LifeSpanDays = Convert.ToInt32(txtConsumableLifeSpan.Text)
                    };
                    ConsumableOpsBL.AddNewConsumable(consumable);
                    Response.Redirect("ConsumableManagement.aspx");
                }

                
            }

            
        }

        protected void btnEditConsumable_OnClick(object sender, EventArgs e)
        {
            var consumableIdString = ((sender as LinkButton)?.CommandArgument);
            if (consumableIdString == null) return;
            //setting the current editing event to hidden field for reading the ID after postbacks
            hfEditingConsumableModel.Value = consumableIdString;
            FindEditingConsumable(true);
            btnSubmit.Text = "Update Consumable";
            lblAddEditHeading.Text = "Edit Consumable";
            btnCancelUpdate.Visible = true;
        }


        private void FindEditingConsumable(bool displayAfterFind)
        {
            try
            {
                _editingConsumable = ConsumableOpsBL.GetConsumableByModelNumber(hfEditingConsumableModel.Value);
                if(displayAfterFind) DisplayEditingConsumable();
            }
            catch (RecordNotFoundException e)
            {
                Console.WriteLine(e);
            }
        }

        private void DisplayEditingConsumable()
        {
            lblModelNumberWarning.Visible = false;
            txtConsumableModel.Enabled = false;
            txtConsumableModel.Text = _editingConsumable.ModelNumber;
            txtConsumableMake.Text = _editingConsumable.Make;
            txtConsumableDescription.Text = _editingConsumable.Description;
            txtConsumableLifeSpan.Text = _editingConsumable.LifeSpanDays.ToString();
        }

        protected void btnCancelUpdate_OnClick(object sender, EventArgs e)
        {
            hfEditingConsumableModel.Value = "";
            Response.Redirect("ConsumableManagement.aspx");
        }
    }
}