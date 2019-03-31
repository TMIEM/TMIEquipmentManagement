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
    public partial class CustomerManagement : System.Web.UI.Page
    {
        private List<Customer> _allCustomers;
        private Customer _editingCustomer;
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Customer Management";
            LoadAllCustomers();
            DisplayAllCustomers(_allCustomers);

            if (!IsPostBack)
            {
                if (hfEditingCustomerId.Value != "")
                {
                    FindEditingCustomer(true);
                }
                
            }
        }

        private void LoadAllCustomers()
        {
            _allCustomers = CustomerOpsBL.GetAllCustomers();
        }

        private void DisplayAllCustomers(Object customersList)
        {
            lvCustomers.DataSource = customersList;
            lvCustomers.DataBind();
        }

        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            if (hfEditingCustomerId.Value != "")
            {
                //updating customer
                FindEditingCustomer(false);
                _editingCustomer.Name = txtCustomerName.Text;
                _editingCustomer.Address = txtCustomerAddress.Text;
                _editingCustomer.Telephone = txtCustomerTelephone.Text;
                _editingCustomer.Fax = txtCustomerFax.Text;
                _editingCustomer.ContactPerson = txtCustomerContactPerson.Text;
                _editingCustomer.ContactPersonTelephone = txtCustomerContactPersonContact.Text;
                CustomerOpsBL.UpdateCustomer(_editingCustomer);

            }
            else
            {
                //adding customer
                Customer customer = new Customer();
                customer.Name = txtCustomerName.Text;
                customer.Address = txtCustomerAddress.Text;
                customer.Telephone = txtCustomerTelephone.Text;
                customer.Fax = txtCustomerFax.Text;
                customer.ContactPerson = txtCustomerContactPerson.Text;
                customer.ContactPersonTelephone = txtCustomerContactPersonContact.Text;
                CustomerOpsBL.AddNewCustomer(customer);
            }

            Response.Redirect("CustomerManagement.aspx");
        }

        protected void btnEditCustomer_OnClick(object sender, EventArgs e)
        {
            var customerIdString = ((sender as LinkButton)?.CommandArgument);
            if (customerIdString == null) return;
            //setting the current editing event to hidden field for reading the ID after postbacks
            hfEditingCustomerId.Value = customerIdString;
            FindEditingCustomer(true);
            btnSubmit.Text = "Update Customer";
            lblAddEditHeading.Text = "Edit Customer";
            btnCancelUpdate.Visible = true;
        }


        private void FindEditingCustomer(bool displayAfterFind)
        {
            try
            {
                _editingCustomer = CustomerOpsBL.GetCustomerById(Convert.ToInt32(hfEditingCustomerId.Value));
                if(displayAfterFind) DisplayEditingCustomer();
            }
            catch (RecordNotFoundException e)
            {
                Console.WriteLine(e);
            }
        }

        private void DisplayEditingCustomer()
        {
            txtCustomerName.Text = _editingCustomer.Name;
            txtCustomerAddress.Text = _editingCustomer.Address;
            txtCustomerTelephone.Text = _editingCustomer.Telephone;
            txtCustomerFax.Text = _editingCustomer.Fax;
            txtCustomerContactPerson.Text = _editingCustomer.ContactPerson;
            txtCustomerContactPersonContact.Text = _editingCustomer.ContactPersonTelephone;
        }

        protected void btnCancelUpdate_OnClick(object sender, EventArgs e)
        {
            hfEditingCustomerId.Value = "";
            Response.Redirect("CustomerManagement.aspx");
        }
    }
}