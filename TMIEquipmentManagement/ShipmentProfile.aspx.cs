using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using EntityLayer;
using Newtonsoft.Json.Linq;

namespace TMIEquipmentManagement
{
    public partial class ShipmentProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var poNumberString = Request.QueryString["ponumber"];
            if(poNumberString == null) Response.Redirect("404.aspx");
            
            lblShipmentPoNumber.Text = poNumberString;
            LoadShipmentDetails(poNumberString);
        }

        private void LoadShipmentDetails(string poNumberString)
        {
            try
            {
                var shipment = ShipmentOpsBL.GetShipmentByPoNumber(poNumberString);
                shipment.Supplier = SupplierOpsBL.GetSupplierById(shipment.SupplierId);
                DisplayShipmentDetails(shipment);
                LoadShipmentEquipmentItems(poNumberString);
                LoadShipmentSpareParts(poNumberString);
                LoadConsumableItems(poNumberString);
            }
            catch (RecordNotFoundException e)
            {
                Response.Redirect("404.aspx");
            }
            
        }

        private void LoadConsumableItems(string poNumberString)
        {
            var consumableBatches = ConsumableBatchOpsBL.GetConsumableBatchesByShipment(poNumberString);
            lvConsumableBatchItems.DataSource = consumableBatches;
            lvConsumableBatchItems.DataBind();
        }

        private void LoadShipmentSpareParts(string poNumberString)
        {
            var sparePartItems = SparePartItemOpsBL.GetSparePartItemsByShipmentPoNumber(poNumberString);
            lvSparePartItems.DataSource = sparePartItems;
            lvSparePartItems.DataBind();
        }

        private void LoadShipmentEquipmentItems(string poNumberString)
        {
            var equipmentItems = EquipmentItemOpsBL.GetEquipmentItemByShipmentPoNumber(poNumberString);
            lvEquipmentItems.DataSource = equipmentItems;
            lvEquipmentItems.DataBind();
        }

        private void DisplayShipmentDetails(Shipment shipment)
        {
            lblDetailsShipmentPoNumber.Text = shipment.PoNumber;
            lblDetailsDateOfArrival.Text = shipment.DateOfArrival.ToShortDateString();
            lblDetailsInvoiceNumber.Text = shipment.SupplierInvoiceNumber;
            lblDetailsSupplier.Text = "<a href='../Supplier.aspx?id="+shipment.Supplier.Id+"' target=\"_blank\">" 
                                      + shipment.Supplier.Name + "</a>";
        }


        [WebMethod]
        public static List<object> GetEquipmentsChartData(string shipmentPoNumber)
        {
            string query = "SELECT [date], COUNT(*) as [ServiceCount], equipment_item.equipmentmodel_number as [Model] FROM service_request LEFT JOIN equipment_item ON [service_request].[equipment_itemserial_number] = equipment_item.serial_number WHERE service_request.equipment_itemserial_number IN(SELECT [serial_number] FROM equipment_item WHERE shipmentpo_number = '"+shipmentPoNumber+"') GROUP BY [equipment_item].[equipmentmodel_number], [date]";
            string constr = ConfigurationManager.ConnectionStrings["tmidbConnectionString"].ConnectionString;
            List<object> chartData = new List<object>();
            object role = new { };
            chartData.Add(new object[]
            {
                "Date", "Service Request Count", role = new {role = "annotation"}
            });
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            chartData.Add(new object[]
                            {
                                Convert.ToDateTime(sdr["date"]).ToLongDateString(), sdr["ServiceCount"], sdr["Model"]
                            });
                        }
                    }
                    con.Close();
                    return chartData;
                }
            }
        }


        [WebMethod]
        public static List<object> GetSparePartsChartData(string shipmentPoNumber)
        {
            string query = "SELECT [date], COUNT(*) as [ServiceCount], spare_part_item.spare_partmodel_number as [Model] FROM service_request LEFT JOIN spare_part_item ON [service_request].[spare_part_itemserial_number] = spare_part_item.serial_number WHERE service_request.spare_part_itemserial_number IN(SELECT [spare_part_itemserial_number] FROM spare_part_item WHERE shipmentpo_number = '"+shipmentPoNumber+"') GROUP BY [spare_part_item].[spare_partmodel_number], [date]";
            string constr = ConfigurationManager.ConnectionStrings["tmidbConnectionString"].ConnectionString;
            List<object> chartData = new List<object>();
            object role = new { };
            chartData.Add(new object[]
            {
                "Date", "Service Request Count", role = new {role = "annotation"}
            });
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            chartData.Add(new object[]
                            {
                                Convert.ToDateTime(sdr["date"]).ToLongDateString(), sdr["ServiceCount"], sdr["Model"]
                            });
                        }
                    }
                    con.Close();
                    return chartData;
                }
            }
        }
    }
}