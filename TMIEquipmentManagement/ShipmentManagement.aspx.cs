using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using BusinessLayer;
using BusinessLayer.Exception;
using EntityLayer;

namespace TMIEquipmentManagement
{
    public partial class ShipmentManagement : System.Web.UI.Page
    {
        private const string ViewStateVarCurrentShipmentProcessor = "CurrentShipmentProcessor";
        private const string ViewStateVarEditingEquipmentItem = "EditingEquipmentItem";
        private const string ViewStateVarEditingSparePartItem = "EditingSparePartItem";
        private const string ViewStateVarEditingConsumableBatch = "EditingConsumableBatch";

        private List<Shipment> _allShipments;
        private Shipment _editingShipment;
        private IShipmentProcessor _shipmentProcessor;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Shipment Management";


            if (!IsPostBack)
            {
                LoadAllShipments();
                DisplayAllShipments(_allShipments);
                DisplaySuppliers();
                DisplayEquipmentModels();
                DisplaySparePartModels();
                DisplayConsumableModels();
                //DisplayAddedEquipmentItems();
                //                if (hfEditingShipmentModel.Value != "")
                //                {
                //                    FindEditingShipment(true);
                //                    DisplayEditingEquipmentItem();
                //                }
            }
        }


        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);
            InitShipmentProcessor();
        }

        protected override object SaveViewState()
        {
            ViewState[ViewStateVarCurrentShipmentProcessor] = _shipmentProcessor;
            return base.SaveViewState();
        }
//
//        protected override void OnUnload(EventArgs e)
//        {
//            base.OnUnload(e);
//        }


        /*---------------------------Shipment related functions------------------------------*/

        //Displays the suppliers in the supplier list view (table)
        private void DisplaySuppliers()
        {
            lvShipmentSuppliers.DataSource = SupplierOpsBL.GetAllSuppliers();
            lvShipmentSuppliers.DataBind();
        }

        //retrieves all shipment records from DB and holds them locally
        private void LoadAllShipments()
        {
            _allShipments = ShipmentOpsBL.GetAllShipments();
        }

        //Displays the shipment records stored locally
        private void DisplayAllShipments(Object shipmentsList)
        {
            lvShipments.DataSource = shipmentsList;
            lvShipments.DataBind();
        }

        //initializes/restores the shipmentprocessor object used to manipulate the current shipment being handled
        private void InitShipmentProcessor()
        {
            if (ViewState[ViewStateVarCurrentShipmentProcessor] == null)
            {
                _shipmentProcessor = _shipmentProcessor ?? new ShipmentProcessor();
                _shipmentProcessor.EquipmentProcessor =
                    _shipmentProcessor.EquipmentProcessor ?? new EquipmentProcessor();
                _shipmentProcessor.SparePartProcessor =
                    _shipmentProcessor.SparePartProcessor ?? new SparePartProcessor();
                _shipmentProcessor.ConsumableProcessor =
                    _shipmentProcessor.ConsumableProcessor ?? new ConsumableProcessor();
            }
            else
            {
                _shipmentProcessor = (IShipmentProcessor) ViewState[ViewStateVarCurrentShipmentProcessor];
            }
        }

//        private void FindEditingShipment(bool displayAfterFind)
//        {
//            try
//            {
//                _editingShipment = ShipmentOpsBL.GetShipmentByPoNumber(hfEditingShipmentPoNumber.Value);
//                if (displayAfterFind) DisplayEditingShipment();
//            }
//            catch (RecordNotFoundException e)
//            {
//                Console.WriteLine(e);
//            }
//        }


        private void DisplayEditingShipment(Shipment shipment)
        {
            lblPoNumberWarning.Visible = false;
            txtShipmentPoNumber.Enabled = false;
            txtShipmentPoNumber.Text = shipment.PoNumber;
            txtShipmentSuppplierInvoiceNumber.Text = shipment.SupplierInvoiceNumber;
            txtShipmentDate.Text = shipment.DateOfArrival.ToShortDateString();
            txtShipmentSupplierId.Text = shipment.SupplierId.ToString();
        }

        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            if (hfEditingShipmentPoNumber.Value != "")
            {
                //updating shipment
                //                //FindEditingShipment(false);
                //                _editingShipment.PoNumber = txtShipmentPoNumber.Text;
                //                _editingShipment.SupplierInvoiceNumber = txtShipmentSuppplierInvoiceNumber.Text;
                //                _editingShipment.DateOfArrival = Convert.ToDateTime(txtShipmentDate.Text);
                //                _editingShipment.SupplierId = Convert.ToInt32(txtShipmentSupplier.Text);
                //                ShipmentOpsBL.UpdateShipment(_editingShipment);

                _shipmentProcessor.Shipment.PoNumber = txtShipmentPoNumber.Text;
                _shipmentProcessor.Shipment.SupplierInvoiceNumber = txtShipmentSuppplierInvoiceNumber.Text;
                _shipmentProcessor.Shipment.DateOfArrival = Convert.ToDateTime(txtShipmentDate.Text);
                _shipmentProcessor.Shipment.SupplierId = Convert.ToInt32(txtShipmentSupplierId.Text);
                _shipmentProcessor.UpdateShipment();
                Response.Redirect("ShipmentManagement.aspx");
            }
            else
            {
                //Adding a new shipment
                try
                {
                    _shipmentProcessor.Shipment = new Shipment()
                    {
                        PoNumber = txtShipmentPoNumber.Text,
                        SupplierInvoiceNumber = txtShipmentSuppplierInvoiceNumber.Text,
                        DateOfArrival = Convert.ToDateTime(txtShipmentDate.Text),
                        SupplierId = Convert.ToInt32(txtShipmentSupplierId.Text)
                    };
                    _shipmentProcessor.AddShipment();
                    Response.Redirect("ShipmentManagement.aspx");
                }
                catch (DuplicateRecordIdentifierException duplicateRecordIdentifierException)
                {
                    lblDuplicatePoNumberAlert.Visible = true;
                    //throw;
                }

                //adding shipment
                //checking if the PO number is already used
//                try
//                {
//                    var shipmentByPoNumber = ShipmentOpsBL.GetShipmentByPoNumber(txtShipmentPoNumber.Text);
//                    if (shipmentByPoNumber != null)
//                    {
//                        //Displaying message to the user to change the model number
//                        lblDuplicatePoNumberAlert.Visible = true;
//                    }
//                }
//                catch (RecordNotFoundException recordNotFoundException)
//                {
//                    //if a record with the same model number is not found, continue to add the new shipment to the DB
//                    Shipment shipment = new Shipment
//                    {
//                        PoNumber = txtShipmentPoNumber.Text,
//                        SupplierInvoiceNumber = txtShipmentSuppplierInvoiceNumber.Text,
//                        DateOfArrival = Convert.ToDateTime(txtShipmentDate.Text),
//                        SupplierId = Convert.ToInt32(txtShipmentSupplierId.Text)
//                    };
//                    ShipmentOpsBL.AddNewShipment(shipment);
//                    Response.Redirect("ShipmentManagement.aspx");
//                }
            }
        }

        protected void btnEditShipment_OnClick(object sender, EventArgs e)
        {
            var shipmentPoNumber = ((sender as LinkButton)?.CommandArgument);
            if (shipmentPoNumber == null) return;
            //setting the current editing event to hidden field for reading the ID after postbacks
            hfEditingShipmentPoNumber.Value = shipmentPoNumber;
            _shipmentProcessor.LoadShipmentDetails(shipmentPoNumber);
            LoadEditingShipmentFields();
            btnSubmit.Text = "Update Shipment";
            lblAddEditHeading.Text = "Edit Shipment";
            btnCancelUpdate.Visible = true;
        }

        private void LoadEditingShipmentFields()
        {
            //method call to display shipment related input fields
            DisplayEditingShipment(_shipmentProcessor.Shipment);

            //displaying equipments in editing shipment
            DisplayAddedEquipmentItems();

            //displaying spare parts in editing shipment
            DisplayAddedSparePartItems();

            //displaying consumables in editing shipment
            DisplayAddedConsumableBatches();
        }


        protected void btnCancelUpdate_OnClick(object sender, EventArgs e)
        {
            hfEditingShipmentPoNumber.Value = "";
            Response.Redirect("ShipmentManagement.aspx");
        }

        protected void btnChooseSupplier_OnClick(object sender, EventArgs e)
        {
            var supplierIdString = ((sender as LinkButton)?.CommandArgument);
            if (supplierIdString == null) return;
            txtShipmentSupplierId.Text = supplierIdString;
        }

        /*---------------------------/.Shipment related functions------------------------------*/


        /*---------------------------Equipment related functions------------------------------*/


        private void DisplayEditingEquipmentItem()
        {
            if (ViewState[ViewStateVarEditingEquipmentItem] != null)
            {
                EquipmentItem editingEquipmentItem = (EquipmentItem) ViewState[ViewStateVarEditingEquipmentItem];
                txtEquipmentModelNumber.Text = editingEquipmentItem.EquipmentModelNumber;
                txtEquipmentPrice.Text = editingEquipmentItem.Price.ToString();
                txtEquipmentSerialNumber.Text = editingEquipmentItem.SerialNumber;
                txtEquipmentSerialNumber.Enabled = false;
            }
        }


        private void DisplayAddedEquipmentItems()
        {
            if (_shipmentProcessor.EquipmentProcessor.EquipmentItems == null) return;
            lvEquipmentItems.DataSource = _shipmentProcessor.EquipmentProcessor.EquipmentItems;
            lvEquipmentItems.DataBind();
        }


        private void DisplayEquipmentModels()
        {
            lvEquipments.DataSource = EquipmentOpsBL.GetAllEquipments();
            lvEquipments.DataBind();
        }


        protected void btnChooseEquipmentModel_OnClick(object sender, EventArgs e)
        {
            var equipmentModel = ((sender as LinkButton)?.CommandArgument);
            if (equipmentModel == null) return;
            txtEquipmentModelNumber.Text = equipmentModel;
        }

        protected void btnSubmitEquipment_OnClick(object sender, EventArgs e)
        {
            ClearEquipmentAlerts();
            //if an item is being edited
            if (ViewState[ViewStateVarEditingEquipmentItem] != null)
            {
                var editingItem = (EquipmentItem) ViewState[ViewStateVarEditingEquipmentItem];
                editingItem.SerialNumber = txtEquipmentSerialNumber.Text;
                editingItem.EquipmentModelNumber = txtEquipmentModelNumber.Text;
                editingItem.Price = Convert.ToDouble(txtEquipmentPrice.Text);
                _shipmentProcessor.EquipmentProcessor.UpdateEquipmentItem(editingItem);
                ClearEditingEquipmentItem();
                lblEquipmentItemSuccess.Text = "Equipment updated successfully.";
                DivAlertEquipmentItemSuccess.Visible = true;
            }
            else
            {
                //adding equipment
                var newEquipment = new EquipmentItem()
                {
                    SerialNumber = txtEquipmentSerialNumber.Text,
                    EquipmentModelNumber = txtEquipmentModelNumber.Text,
                    Price = Convert.ToDouble(txtEquipmentPrice.Text)
                };

                try
                {
                    var isEditing = hfEditingShipmentPoNumber.Value != "" ? true : false;
                    //setting the PoNumber for the new item being added to the editing shipment since PO number isn't set
                    //for the item at any other point (unlike adding an item to a new shipment where PO numbers is given before saving)
                    if (isEditing)
                    {
                        newEquipment.ShipmentPoNumber = _shipmentProcessor.Shipment.PoNumber;
                    }

                    _shipmentProcessor.EquipmentProcessor.AddEquipmentItem(newEquipment, isEditing);
                    lblEquipmentItemSuccess.Text = "Equipment added to shipment successfully.";
                    DivAlertEquipmentItemSuccess.Visible = true;
                }
                catch (DuplicateRecordIdentifierException exception)
                {
                    Console.WriteLine(exception);
                    lblEquipmentItemFailure.Text =
                        "The serial number entered is already in use for another equipment, please enter a different serial number";
                    DivAlertEquipmentItemFailure.Visible = true;
                }
            }

            DisplayAddedEquipmentItems();
        }


        protected void btnEditEquipmentItem_OnClick(object sender, EventArgs e)
        {
            ClearEquipmentAlerts();
            var equipmentItemSerial = ((sender as LinkButton)?.CommandArgument);
            if (equipmentItemSerial == null) return;
            var editingEquipmentItem
                = _shipmentProcessor.EquipmentProcessor.FindEquipmentItem(equipmentItemSerial);

            //The editing item is held in a session variable so that it can survive postbacks
            ViewState[ViewStateVarEditingEquipmentItem] = editingEquipmentItem;
            DisplayEditingEquipmentItem();


            btnAddEquipment.Text = "Update Equipment";
            btnCancelEquipmentUpdate.Visible = true;
            btnDeleteEquipmentItem.Visible = true;
        }

        private void ClearEquipmentAlerts()
        {
            DivAlertEquipmentItemSuccess.Visible = false;
            DivAlertEquipmentItemFailure.Visible = false;
        }

        protected void btnCancelEquipmentUpdate_OnClick(object sender, EventArgs e)
        {
            ClearEditingEquipmentItem();
        }

        private void ClearEditingEquipmentItem()
        {
            ViewState[ViewStateVarEditingEquipmentItem] = null;
            txtEquipmentModelNumber.Text = "";
            txtEquipmentPrice.Text = "";
            txtEquipmentSerialNumber.Text = "";
            txtEquipmentSerialNumber.Enabled = true;
            btnAddEquipment.Text = "Add Equipment";
            btnCancelEquipmentUpdate.Visible = false;
            btnDeleteEquipmentItem.Visible = false;
            DivAlertEquipmentItemFailure.Visible = false;
            DivAlertEquipmentItemSuccess.Visible = false;
        }

        protected void btnDeleteEquipmentItem_OnClick(object sender, EventArgs e)
        {
            ClearEquipmentAlerts();

            if (ViewState[ViewStateVarEditingEquipmentItem] == null) return;
            EquipmentItem equipmentItem = (EquipmentItem) ViewState[ViewStateVarEditingEquipmentItem];
            try
            {
                _shipmentProcessor.EquipmentProcessor.RemoveEquipmentItem(equipmentItem);
                ClearEditingEquipmentItem();
                lblEquipmentItemSuccess.Text = "Equipment removed from shipment successfully.";
                DivAlertEquipmentItemSuccess.Visible = true;
                DisplayAddedEquipmentItems();
            }
            catch (SqlException sqlException)
            {
                lblEquipmentItemFailure.Text =
                    "The equipment has already been associated with other records, " +
                    "it cannot be deleted until the associated records have been deleted";
                DivAlertEquipmentItemFailure.Visible = true;
                Console.WriteLine(sqlException);
            }
        }

        /*---------------------------/.Equipment related functions------------------------------*/


        /*---------------------------/Spare parts related functions------------------------------*/
        private void DisplayEditingSparePartItem()
        {
            if (ViewState[ViewStateVarEditingSparePartItem] != null)
            {
                SparePartItem editingSparePartItem = (SparePartItem) ViewState[ViewStateVarEditingSparePartItem];
                txtSparePartModelNumber.Text = editingSparePartItem.SparePartModelNumber;
                txtSparePartPrice.Text = editingSparePartItem.Price.ToString();
                txtSparePartSerialNumber.Text = editingSparePartItem.SerialNumber;
                txtSparePartSerialNumber.Enabled = false;
            }
        }


        private void DisplayAddedSparePartItems()
        {
            if (_shipmentProcessor.SparePartProcessor.SparePartItems == null) return;
            lvSparePartItems.DataSource = _shipmentProcessor.SparePartProcessor.SparePartItems;
            lvSparePartItems.DataBind();
        }


        private void DisplaySparePartModels()
        {
            lvSpareParts.DataSource = SparePartOpsBL.GetAllSpareParts();
            lvSpareParts.DataBind();
        }


        protected void btnChooseSparePartModel_OnClick(object sender, EventArgs e)
        {
            var sparePartModel = ((sender as LinkButton)?.CommandArgument);
            if (sparePartModel == null) return;
            txtSparePartModelNumber.Text = sparePartModel;
        }

        protected void btnSubmitSparePart_OnClick(object sender, EventArgs e)
        {
            ClearSparePartAlerts();
            //if an item is being edited
            if (ViewState[ViewStateVarEditingSparePartItem] != null)
            {
                var editingItem = (SparePartItem) ViewState[ViewStateVarEditingSparePartItem];
                editingItem.SerialNumber = txtSparePartSerialNumber.Text;
                editingItem.SparePartModelNumber = txtSparePartModelNumber.Text;
                editingItem.Price = Convert.ToDouble(txtSparePartPrice.Text);
                _shipmentProcessor.SparePartProcessor.UpdateSparePartItem(editingItem);
                ClearEditingSparePartItem();
                lblSparePartItemSuccess.Text = "SparePart updated successfully.";
                DivAlertSparePartItemSuccess.Visible = true;
            }
            else
            {
                //adding sparePart
                var newSparePart = new SparePartItem()
                {
                    SerialNumber = txtSparePartSerialNumber.Text,
                    SparePartModelNumber = txtSparePartModelNumber.Text,
                    Price = Convert.ToDouble(txtSparePartPrice.Text)
                };

                try
                {
                    var isEditing = hfEditingShipmentPoNumber.Value != "" ? true : false;
                    //setting the PoNumber for the new item being added to the editing shipment since PO number isn't set
                    //for the item at any other point (unlike adding an item to a new shipment where PO numbers is given before saving)
                    if (isEditing)
                    {
                        newSparePart.ShipmentPoNumber = _shipmentProcessor.Shipment.PoNumber;
                    }

                    _shipmentProcessor.SparePartProcessor.AddSparePartItem(newSparePart, isEditing);
                    lblSparePartItemSuccess.Text = "SparePart added to shipment successfully.";
                    DivAlertSparePartItemSuccess.Visible = true;
                }
                catch (DuplicateRecordIdentifierException exception)
                {
                    Console.WriteLine(exception);
                    lblSparePartItemFailure.Text =
                        "The serial number entered is already in use for another sparePart, please enter a different serial number";
                    DivAlertSparePartItemFailure.Visible = true;
                }
            }

            DisplayAddedSparePartItems();
        }

        private void ClearEditingSparePartItem()
        {
            ViewState[ViewStateVarEditingSparePartItem] = null;
            txtSparePartModelNumber.Text = "";
            txtSparePartPrice.Text = "";
            txtSparePartSerialNumber.Text = "";
            txtSparePartSerialNumber.Enabled = true;
            btnAddSparePart.Text = "Add SparePart";
            btnCancelSparePartUpdate.Visible = false;
            btnDeleteSparePartItem.Visible = false;
            DivAlertSparePartItemFailure.Visible = false;
            DivAlertSparePartItemSuccess.Visible = false;
        }

        private void ClearSparePartAlerts()
        {
            DivAlertSparePartItemSuccess.Visible = false;
            DivAlertSparePartItemFailure.Visible = false;
        }

        protected void btnCancelSparePartUpdate_OnClick(object sender, EventArgs e)
        {
            ClearEditingSparePartItem();
        }

        protected void btnDeleteSparePartItem_OnClick(object sender, EventArgs e)
        {
            ClearSparePartAlerts();

            if (ViewState[ViewStateVarEditingSparePartItem] == null) return;
            SparePartItem sparePartItem = (SparePartItem) ViewState[ViewStateVarEditingSparePartItem];
            try
            {
                _shipmentProcessor.SparePartProcessor.RemoveSparePartItem(sparePartItem);
                ClearEditingSparePartItem();
                lblSparePartItemSuccess.Text = "SparePart removed from shipment successfully.";
                DivAlertSparePartItemSuccess.Visible = true;
                DisplayAddedSparePartItems();
            }
            catch (SqlException sqlException)
            {
                lblSparePartItemFailure.Text =
                    "The sparePart has already been associated with other records, " +
                    "it cannot be deleted until the associated records have been deleted";
                DivAlertSparePartItemFailure.Visible = true;
                Console.WriteLine(sqlException);
            }
        }

        protected void btnEditSparePartItem_OnClick(object sender, EventArgs e)
        {
            ClearSparePartAlerts();
            var sparePartItemSerial = ((sender as LinkButton)?.CommandArgument);
            if (sparePartItemSerial == null) return;
            var editingSparePartItem
                = _shipmentProcessor.SparePartProcessor.FindSparePartItem(sparePartItemSerial);

            //The editing item is held in a session variable so that it can survive postbacks
            ViewState[ViewStateVarEditingSparePartItem] = editingSparePartItem;
            DisplayEditingSparePartItem();


            btnAddSparePart.Text = "Update SparePart";
            btnCancelSparePartUpdate.Visible = true;
            btnDeleteSparePartItem.Visible = true;
        }

        /*-----------------Consumable related functions------------------*/
        private void DisplayConsumableModels()
        {
            lvConsumableModels.DataSource = ConsumableOpsBL.GetAllConsumables();
            lvConsumableModels.DataBind();
        }

        private void DisplayAddedConsumableBatches()
        {
            if (_shipmentProcessor.ConsumableProcessor.ConsumableBatches == null) return;
            lvConsumableBatchItems.DataSource = _shipmentProcessor.ConsumableProcessor.ConsumableBatches;
            lvConsumableBatchItems.DataBind();
        }

        private void ClearEditingConsumableBatch()
        {
            ViewState[ViewStateVarEditingConsumableBatch] = null;
            txtConsumableBatchModelNumber.Text = "";
            txtConsumableBatchQuantity.Text = "";
            btnAddConsumableBatch.Text = "Add Batch";
            btnCancelConsumableBatchUpdate.Visible = false;
            btnDeleteConsumableBatchItem.Visible = false;
            DivAlertConsumableBatchItemSuccess.Visible = false;
            DivAlertConsumableBatchItemFailure.Visible = false;
        }


        protected void btnSubmitConsumableBatch_OnClick(object sender, EventArgs e)
        {
            ClearConsumableAlerts();
            //if an item is being edited
            if (ViewState[ViewStateVarEditingConsumableBatch] != null)
            {
                var editingConsumableBatch = (ConsumableBatch)ViewState[ViewStateVarEditingConsumableBatch];
                editingConsumableBatch.ConsumableModelNumber = txtConsumableBatchModelNumber.Text;
                editingConsumableBatch.Quantity = Convert.ToInt32(txtConsumableBatchQuantity.Text);
                editingConsumableBatch.Price = Convert.ToDouble(txtConsumableBatchPrice.Text);
                _shipmentProcessor.ConsumableProcessor.UpdateConsumableBatch(editingConsumableBatch);
                ClearEditingConsumableBatch();
                lblConsumableBatchItemSuccess.Text = "SparePart updated successfully.";
                DivAlertConsumableBatchItemSuccess.Visible = true;
            }
            else
            {
                //adding sparePart
                var consumableBatch = new ConsumableBatch()
                {
                    ConsumableModelNumber = txtConsumableBatchModelNumber.Text,
                    Quantity = Convert.ToInt32(txtConsumableBatchQuantity.Text),
                    Price = Convert.ToDouble(txtConsumableBatchPrice.Text)
                };

                try
                {
                    var isEditing = hfEditingShipmentPoNumber.Value != "" ? true : false;
                    //setting the PoNumber for the new item being added to the editing shipment since PO number isn't set
                    //for the item at any other point (unlike adding an item to a new shipment where PO numbers is given before saving)
                    if (isEditing)
                    {
                        consumableBatch.ShipmentPoNumber = _shipmentProcessor.Shipment.PoNumber;
                    }

                    _shipmentProcessor.ConsumableProcessor.AddConsumableBatch(consumableBatch, isEditing);
                    lblConsumableBatchItemSuccess.Text = "Consumable Batch added to shipment successfully.";
                    DivAlertConsumableBatchItemSuccess.Visible = true;
                }
                catch (DuplicateRecordIdentifierException exception)
                {
                    Console.WriteLine(exception);
                    lblConsumableBatchItemFailure.Text =
                        "A batch for the selected consumable model number is already added to the shipment, " +
                        "please update the quantity for the existing batch or choose a different model";
                    DivAlertConsumableBatchItemFailure.Visible = true;
                }
            }

            DisplayAddedConsumableBatches();
        }

        
        private void ClearConsumableAlerts()
        {
            DivAlertConsumableBatchItemSuccess.Visible = false;
            DivAlertConsumableBatchItemFailure.Visible = false;
        }

        protected void btnCancelConsumableBatchUpdate_OnClick(object sender, EventArgs e)
        {
            ClearEditingConsumableBatch();
        }

        protected void btnDeleteConsumableBatchItem_OnClick(object sender, EventArgs e)
        {
            ClearConsumableAlerts();

            if (ViewState[ViewStateVarEditingConsumableBatch] == null) return;
            ConsumableBatch consumableBatch = (ConsumableBatch)ViewState[ViewStateVarEditingConsumableBatch];
            try
            {
                _shipmentProcessor.ConsumableProcessor.RemoveConsumableBatch(consumableBatch);
                ClearEditingConsumableBatch();
                lblConsumableBatchItemSuccess.Text = "Consumable batch removed from shipment successfully.";
                DivAlertConsumableBatchItemSuccess.Visible = true;
                DisplayAddedConsumableBatches();
            }
            catch (SqlException sqlException)
            {
                lblConsumableBatchItemFailure.Text =
                    "The consumable batch has already been associated with other records, " +
                    "it cannot be deleted until the associated records have been deleted";
                DivAlertConsumableBatchItemFailure.Visible = true;
                Console.WriteLine(sqlException);
            }
        }

        protected void btnChooseConsumableBatchModel_OnClick(object sender, EventArgs e)
        {
            var consumableModel = ((sender as LinkButton)?.CommandArgument);
            if (consumableModel == null) return;
            txtConsumableBatchModelNumber.Text = consumableModel;
        }

        protected void btnEditConsumableBatchItem_OnClick(object sender, EventArgs e)
        {
            ClearConsumableAlerts();
            var consumableModel = ((sender as LinkButton)?.CommandArgument);
            if (consumableModel == null) return;
            var editingConsumbaleBatch
                = _shipmentProcessor.ConsumableProcessor.FindConsumableBatch(consumableModel, hfEditingShipmentPoNumber.Value);

            //The editing item is held in a session variable so that it can survive postbacks
            ViewState[ViewStateVarEditingConsumableBatch] = editingConsumbaleBatch;
            DisplayEditingConsumableBatch();


            btnAddConsumableBatch.Text = "Update Batch";
            btnCancelConsumableBatchUpdate.Visible = true;
            btnDeleteConsumableBatchItem.Visible = true;
        }

        private void DisplayEditingConsumableBatch()
        {
            if (ViewState[ViewStateVarEditingConsumableBatch] != null)
            {
                ConsumableBatch editingConsumableBatch = (ConsumableBatch)ViewState[ViewStateVarEditingConsumableBatch];
                txtConsumableBatchModelNumber.Text = editingConsumableBatch.ConsumableModelNumber;
                txtConsumableBatchQuantity.Text = editingConsumableBatch.Quantity.ToString();
                txtConsumableBatchPrice.Text = editingConsumableBatch.Price.ToString();
                txtConsumableBatchModelNumber.Enabled = false;
            }
        }
    }
}