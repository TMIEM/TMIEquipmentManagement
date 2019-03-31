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
    public partial class SupplierManagement : System.Web.UI.Page
    {
        private List<Supplier> _allSuppliers;
        private Supplier _editingSupplier;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Supplier Management";
            LoadAllSuppliers();
            DisplayAllSuppliers(_allSuppliers);

            if (!IsPostBack)
            {
                if (hfEditingSupplierId.Value != "")
                {
                    FindEditingSupplier(true);
                }
                
            }
        }

        private void LoadAllSuppliers()
        {
            _allSuppliers = SupplierOpsBL.GetAllSuppliers();
        }

        private void DisplayAllSuppliers(Object suppliersList)
        {
            lvSuppliers.DataSource = suppliersList;
            lvSuppliers.DataBind();
        }

        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            if (hfEditingSupplierId.Value != "")
            {
                //updating supplier
                FindEditingSupplier(false);
                _editingSupplier.Name = txtSupplierName.Text;
                _editingSupplier.Address = txtSupplierAddress.Text;
                _editingSupplier.Telephone = txtSupplierTelephone.Text;
                _editingSupplier.Fax = txtSupplierFax.Text;
                _editingSupplier.Email = txtSupplierEmail.Text;
                SupplierOpsBL.UpdateSupplier(_editingSupplier);

            }
            else
            {
                //adding supplier
                Supplier supplier = new Supplier();
                supplier.Name = txtSupplierName.Text;
                supplier.Address = txtSupplierAddress.Text;
                supplier.Telephone = txtSupplierTelephone.Text;
                supplier.Fax = txtSupplierFax.Text;
                supplier.Email = txtSupplierEmail.Text;
                SupplierOpsBL.AddNewSupplier(supplier);
            }

            Response.Redirect("SupplierManagement.aspx");
        }

        protected void btnEditSupplier_OnClick(object sender, EventArgs e)
        {
            var supplierIdString = ((sender as LinkButton)?.CommandArgument);
            if (supplierIdString == null) return;
            //setting the current editing event to hidden field for reading the ID after postbacks
            hfEditingSupplierId.Value = supplierIdString;
            FindEditingSupplier(true);
            btnSubmit.Text = "Update Supplier";
            lblAddEditHeading.Text = "Edit Supplier";
            btnCancelUpdate.Visible = true;
        }


        private void FindEditingSupplier(bool displayAfterFind)
        {
            try
            {
                _editingSupplier = SupplierOpsBL.GetSupplierById(Convert.ToInt32(hfEditingSupplierId.Value));
                if(displayAfterFind) DisplayEditingSupplier();
            }
            catch (RecordNotFoundException e)
            {
                Console.WriteLine(e);
            }
        }

        private void DisplayEditingSupplier()
        {
            txtSupplierName.Text = _editingSupplier.Name;
            txtSupplierAddress.Text = _editingSupplier.Address;
            txtSupplierTelephone.Text = _editingSupplier.Telephone;
            txtSupplierFax.Text = _editingSupplier.Fax;
            txtSupplierEmail.Text = _editingSupplier.Email;
        }

        protected void btnCancelUpdate_OnClick(object sender, EventArgs e)
        {
            hfEditingSupplierId.Value = "";
            Response.Redirect("SupplierManagement.aspx");
        }

    }
}