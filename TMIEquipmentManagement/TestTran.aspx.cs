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
    public partial class TestTran : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_OnClick(object sender, EventArgs e)
        {
            try
            {
                TestTransaction.Test();
            }
            catch (Exception ex)
            {
                Label1.Text = ex.Message;
            }
        }

        protected void Button2_OnClick(object sender, EventArgs e)
        {
            Label2.Text = TestTransaction.TestSearch().ToString();
        }

        protected void Button3_OnClick(object sender, EventArgs e)
        {
            try
            {
                EquipmentItemOpsBL.DeleteEquipmentItem(new EquipmentItem() { SerialNumber = "EI001" });

            }
            catch (Exception exception)
            {
                Label3.Text = exception.Message;
                Console.WriteLine(exception);
            }
        }

        protected void Button4_OnClick(object sender, EventArgs e)
        {
            var addNewShipment = ShipmentOpsBL.AddNewShipment(new Shipment()
            {
                PoNumber = "999",
                DateOfArrival = DateTime.Now,
                SupplierId = 1,
                SupplierInvoiceNumber = "1234"
            });

            Label4.Text = addNewShipment.PoNumber;
        }
    }
}