using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessLayer;
using BusinessLayer.Exception;
using EntityLayer;

namespace TMIEquipmentManagement
{
    public partial class ServiceManagement : System.Web.UI.Page
    {
        private const string ViewStateVarCurrentServiceProcessor = "CurrentServiceProcessor";
        private const string ViewStateVarEditingEquipmentItem = "EditingEquipmentItem";
        private const string ViewStateVarEditingSparePartUsage = "EditingSparePartUsage";
        private const string ViewStateVarEditingConsumableBatchServiceUsage = "EditingConsumableBatchUsage";

        private List<Service> _allServices;
        private Service _editingService;
        private IServiceProcessor _serviceProcessor;


        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "Service Management";
            ClientScript.RegisterStartupScript(GetType(), "Javascript1", "javascript:hello(); ", true);

            if (IsPostBack) return;
            LoadAllServices();
            DisplayAllServices(_allServices);
            DisplayEquipmentInstallations();
            DisplayTechnicians();
            DisplayAvailableSparePartItems();
            DisplayAvailableConsumables();
        }


        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);
            InitServiceProcessor();
        }

        protected override object SaveViewState()
        {
            ViewState[ViewStateVarCurrentServiceProcessor] = _serviceProcessor;
            return base.SaveViewState();
        }


        /*---------------------------Service related functions------------------------------*/

        //Displays the installed equipments in the installed equipments list view (table)
        private void DisplayEquipmentInstallations()
        {
            lvInstalledEquipments.DataSource = EquipmentInstallationOpsBL
                .EagerLoad(EquipmentInstallationOpsBL.GetAllEquipmentInstallations());
            lvInstalledEquipments.DataBind();
        }

        //retrieves all service records from DB and holds them locally
        private void LoadAllServices()
        {
            _allServices = ServiceOpsBL.EagerLoad(ServiceOpsBL.GetAllServices());
        }

        //Displays the service records stored locally
        private void DisplayAllServices(Object servicesList)
        {
            lvServices.DataSource = servicesList;
            lvServices.DataBind();
        }

        //initializes/restores the serviceprocessor object used to manipulate the current service being handled
        private void InitServiceProcessor()
        {
            if (ViewState[ViewStateVarCurrentServiceProcessor] == null)
            {
                _serviceProcessor = _serviceProcessor ?? new ServiceProcessor();
                _serviceProcessor.SparePartUsageProcessor =
                    _serviceProcessor.SparePartUsageProcessor ?? new SparePartUsageProcessor();
                _serviceProcessor.ConsumableBatchUsageProcessor =
                    _serviceProcessor.ConsumableBatchUsageProcessor ?? new ConsumableBatchUsageProcessor();
                _serviceProcessor.SparePartRemovalProcessor =
                    _serviceProcessor.SparePartRemovalProcessor ?? new SparePartRemovalProcessor();
                _serviceProcessor.ConsumableRemovalProcessor =
                    _serviceProcessor.ConsumableRemovalProcessor ?? new ConsumableRemovalProcessor();
            }
            else
            {
                _serviceProcessor = (IServiceProcessor) ViewState[ViewStateVarCurrentServiceProcessor];
            }
        }

        //Displays technicians to choose for the service
        private void DisplayTechnicians()
        {
            lvTechnicians.DataSource = TechnicianOpsBL.GetAllTechnicians();
            lvTechnicians.DataBind();
        }

        private void DisplayEditingService(Service service)
        {
            DivServiceIdRow.Visible = true;
            txtServiceId.Text = service.Id.ToString();
            txtServiceEquipmentSerial.Text = service.InstalledEquipmentSerialNumber;
            txtServiceTechnicianId.Text = service.TechnicianId.ToString();
            txtServiceDate.Text = service.Date.ToShortDateString();
            txtServiceProblemDescription.Text = service.ProblemDescription;
            txtServiceDescription.Text = service.ServiceDescription;
            txtSpecialNote.Text = service.SpecialNote;
        }

        protected void btnSubmit_OnClick(object sender, EventArgs e)
        {
            if (hfEditingServiceId.Value != "")
            {
                //updating service record
                _serviceProcessor.Service.Id = Convert.ToInt32(hfEditingServiceId.Value);
                _serviceProcessor.Service.Date = Convert.ToDateTime(txtServiceDate.Text);
                _serviceProcessor.Service.ProblemDescription = txtServiceProblemDescription.Text;
                _serviceProcessor.Service.ServiceDescription = txtServiceDescription.Text;
                _serviceProcessor.Service.SpecialNote = txtSpecialNote.Text;
                _serviceProcessor.Service.InstalledEquipmentSerialNumber = txtServiceEquipmentSerial.Text;
                _serviceProcessor.Service.TechnicianId = Convert.ToInt32(txtServiceTechnicianId.Text);
                _serviceProcessor.UpdateService();
                Response.Redirect("ServiceManagement.aspx");
            }
            else
            {
                //Adding a new service
                _serviceProcessor.Service = new Service()
                {
                    Date = Convert.ToDateTime(txtServiceDate.Text),
                    ProblemDescription = txtServiceProblemDescription.Text,
                    ServiceDescription = txtServiceDescription.Text,
                    SpecialNote = txtSpecialNote.Text,
                    InstalledEquipmentSerialNumber = txtServiceEquipmentSerial.Text,
                    TechnicianId = Convert.ToInt32(txtServiceTechnicianId.Text)
                };
                _serviceProcessor.AddService();
                Response.Redirect("ServiceManagement.aspx");
            }
        }

        protected void btnEditService_OnClick(object sender, EventArgs e)
        {
            var serviceId = ((sender as LinkButton)?.CommandArgument);
            if (serviceId == null) return;
            //setting the current editing event to hidden field for reading the ID after postbacks
            hfEditingServiceId.Value = serviceId;
            _serviceProcessor.LoadServiceDetails(Convert.ToInt32(serviceId));
            LoadEditingServiceFields();
            btnSubmit.Text = "Update Service";
            lblAddEditHeading.Text = "Edit Service";
            btnCancelUpdate.Visible = true;
        }

        private void LoadEditingServiceFields()
        {
            //method call to display service related input fields
            DisplayEditingService(_serviceProcessor.Service);


            //displaying spare part usages in editing service
            DisplaySparePartUsages();

            //display consumable usages in editing service
            DisplayConsumableBatchServiceUsages();

            LoadRemovables(txtServiceEquipmentSerial.Text);

            DisplayRemovedSpareParts();
            DisplayRemovedConsumables();
        }


        protected void btnCancelUpdate_OnClick(object sender, EventArgs e)
        {
            hfEditingServiceId.Value = "";
            Response.Redirect("ServiceManagement.aspx");
        }

        protected void btnChooseEquipmentInstallation_OnClick(object sender, EventArgs e)
        {
            var installedEquipmentSerial = ((sender as LinkButton)?.CommandArgument);
            if (installedEquipmentSerial == null) return;
            txtServiceEquipmentSerial.Text = installedEquipmentSerial;
            LoadRemovables(installedEquipmentSerial);
        }


        protected void btnChooseTechnician_OnClick(object sender, EventArgs e)
        {
            var technicianId = ((sender as LinkButton)?.CommandArgument);
            if (technicianId == null) return;
            txtServiceTechnicianId.Text = technicianId;
        }

        /*---------------------------/.Service related functions------------------------------*/


        /*---------------------------Spare parts related functions------------------------------*/
        private void DisplayEditingSparePartUsage()
        {
            if (ViewState[ViewStateVarEditingSparePartUsage] == null) return;

            SparePartUsage editingSparePartUsage = (SparePartUsage) ViewState[ViewStateVarEditingSparePartUsage];
            txtSparePartUsageSerialNumber.Text = editingSparePartUsage.SparePartItemSerialNumber;
            txtSparePartUsageWarrantyPeriod.Text = editingSparePartUsage.WarrantyPeriodMonths.ToString();
            txtSparePartUsageSerialNumber.Enabled = false;
        }


        private void DisplaySparePartUsages()
        {
            if (_serviceProcessor.SparePartUsageProcessor.SparePartUsages == null) return;
            lvSparePartUsages.DataSource = _serviceProcessor.SparePartUsageProcessor.SparePartUsages;
            lvSparePartUsages.DataBind();
        }


        private void DisplayAvailableSparePartItems()
        {
            lvSparePartItems.DataSource = SparePartItemOpsBL.GetAvailableSparePartItems();
            lvSparePartItems.DataBind();
        }


        protected void btnChooseSparePartItem_OnClick(object sender, EventArgs e)
        {
            if (ViewState[ViewStateVarEditingSparePartUsage] != null)
            {
                lblSparePartUsageFailure.Text =
                    "The serial number of a used spare part cannot be updated once used in a service." +
                    " Please remove the current spare part usage and add the required spare part";
                DivAlertSparePartUsageFailure.Visible = true;
                return;
            }

            var chosenSparePartItemSerial = ((sender as LinkButton)?.CommandArgument);
            if (chosenSparePartItemSerial == null) return;
            txtSparePartUsageSerialNumber.Text = chosenSparePartItemSerial;
        }

        protected void btnSubmitSparePartUsage_OnClick(object sender, EventArgs e)
        {
            ClearSparePartAlerts();
            //if an item is being edited
            if (ViewState[ViewStateVarEditingSparePartUsage] != null)
            {
                var editingSparePartUsage = (SparePartUsage) ViewState[ViewStateVarEditingSparePartUsage];
                editingSparePartUsage.SparePartItemSerialNumber = txtSparePartUsageSerialNumber.Text;
                editingSparePartUsage.WarrantyPeriodMonths = Convert.ToInt32(txtSparePartUsageWarrantyPeriod.Text);
                _serviceProcessor.SparePartUsageProcessor.UpdateSparePartUsage(editingSparePartUsage);
                ClearEditingSparePartUsage();
                lblSparePartUsageSuccess.Text = "SparePart updated successfully.";
                DivAlertSparePartUsageSuccess.Visible = true;
            }
            else
            {
                //adding sparePart
                var newSparePartUsage = new SparePartUsage()
                {
                    SparePartItemSerialNumber = txtSparePartUsageSerialNumber.Text,
                    WarrantyPeriodMonths = Convert.ToInt32(txtSparePartUsageWarrantyPeriod.Text),
                };

                try
                {
                    var isEditing = hfEditingServiceId.Value != "" ? true : false;
                    //setting the service ID for the new item being added to the editing service since service ID isn't set
                    //for the item at any other point (unlike adding an item to a new service where service ID is given before saving)
                    if (isEditing)
                    {
                        newSparePartUsage.ServiceId = _serviceProcessor.Service.Id;
                    }

                    _serviceProcessor.SparePartUsageProcessor.AddSparePartUsage(newSparePartUsage, isEditing);
                    lblSparePartUsageSuccess.Text = "Spare part usage added to service successfully.";
                    DivAlertSparePartUsageSuccess.Visible = true;
                }
                catch (DuplicateRecordIdentifierException exception)
                {
                    Console.WriteLine(exception);
                    lblSparePartUsageFailure.Text =
                        "The spare part item is already used, please choose an item with a different serial number";
                    DivAlertSparePartUsageFailure.Visible = true;
                }
            }

            DisplayAvailableSparePartItems();
            DisplaySparePartUsages();
        }

        private void ClearEditingSparePartUsage()
        {
            ViewState[ViewStateVarEditingSparePartUsage] = null;
            txtSparePartUsageSerialNumber.Text = "";
            txtSparePartUsageWarrantyPeriod.Text = "";
            btnAddSparePartUsage.Text = "Add Spare Part Usage";
            btnCancelSparePartUsageUpdate.Visible = false;
            btnDeleteSparePartUsage.Visible = false;
            ClearSparePartAlerts();
        }

        private void ClearSparePartAlerts()
        {
            DivAlertSparePartUsageSuccess.Visible = false;
            DivAlertSparePartUsageFailure.Visible = false;
        }

        protected void btnCancelSparePartUsageUpdate_OnClick(object sender, EventArgs e)
        {
            ClearEditingSparePartUsage();
        }

        protected void btnDeleteSparePartUsageItem_OnClick(object sender, EventArgs e)
        {
            ClearSparePartAlerts();

            if (ViewState[ViewStateVarEditingSparePartUsage] == null) return;
            SparePartUsage sparePartUsage = (SparePartUsage) ViewState[ViewStateVarEditingSparePartUsage];
            try
            {
                _serviceProcessor.SparePartUsageProcessor.RemoveSparePartUsage(sparePartUsage);
                ClearEditingSparePartUsage();
                lblSparePartUsageSuccess.Text = "Spare part usage removed from service successfully.";
                DivAlertSparePartUsageSuccess.Visible = true;
                DisplaySparePartUsages();
                DisplayAvailableSparePartItems();
            }
            catch (SqlException sqlException)
            {
                lblSparePartUsageFailure.Text =
                    "The spare part usage has already been associated with other records, " +
                    "it cannot be deleted until the associated records have been deleted";
                DivAlertSparePartUsageFailure.Visible = true;
                Console.WriteLine(sqlException);
            }
        }

        protected void btnEditSparePartUsage_OnClick(object sender, EventArgs e)
        {
            ClearSparePartAlerts();
            var sparePartUsageSerial = ((sender as LinkButton)?.CommandArgument);
            if (sparePartUsageSerial == null) return;
            var editingSparePartUsage
                = _serviceProcessor.SparePartUsageProcessor.FindSparePartUsage(sparePartUsageSerial);

            //The editing item is held in a session variable so that it can survive postbacks
            ViewState[ViewStateVarEditingSparePartUsage] = editingSparePartUsage;
            DisplayEditingSparePartUsage();

            btnAddSparePartUsage.Text = "Update Spare Part Usage";
            btnCancelSparePartUsageUpdate.Visible = true;
            btnDeleteSparePartUsage.Visible = true;
        }


        /*--------------------------Consumable usage related functions------------------------------*/
        private void DisplayAvailableConsumables()
        {
            lvConsumableBatches.DataSource = ConsumableBatchOpsBL.GetAvailableConsumableBatches();
            lvConsumableBatches.DataBind();
        }

        private void ClearEditingConsumableBatchServiceUsage()
        {
            ViewState[ViewStateVarEditingConsumableBatchServiceUsage] = null;
            txtConsumableBatchUsageModelNumber.Text = "";
            txtConsumableBatchUsageShipmentPoNumber.Text = "";
            txtConsumableBatchUsageQuantityUsed.Text = "";
            btnAddConsumableBatchUsage.Text = "Add Consumable Usage";
            btnCancelConsumableBatchUsageUpdate.Visible = false;
            btnDeleteConsumableBatchUsage.Visible = false;
            ClearConsumableUsageAlerts();
        }

        private void ClearConsumableUsageAlerts()
        {
            DivAlertConsumableBatchUsageSuccess.Visible = false;
            DivAlertConsumableBatchUsageFailure.Visible = false;
        }

        protected void btnSubmitConsumableBatchUsage_OnClick(object sender, EventArgs e)
        {
            ClearConsumableUsageAlerts();
            //if an item is being edited
            if (ViewState[ViewStateVarEditingConsumableBatchServiceUsage] != null)
            {
                var editingConsumableBatchServiceUsage =
                    (ConsumableBatchServiceUsage) ViewState[ViewStateVarEditingConsumableBatchServiceUsage];
                editingConsumableBatchServiceUsage.ConsumbaleBatchShipmentPONumber =
                    txtConsumableBatchUsageShipmentPoNumber.Text;
                editingConsumableBatchServiceUsage.ConsumableBatchModelNumber = txtConsumableBatchUsageModelNumber.Text;
                editingConsumableBatchServiceUsage.QuantityUsed =
                    Convert.ToInt32(txtConsumableBatchUsageQuantityUsed.Text);
                try
                {
                    _serviceProcessor.ConsumableBatchUsageProcessor.UpdateConsumableBatchServiceUsage(
                        editingConsumableBatchServiceUsage);
                    ClearEditingConsumableBatchServiceUsage();
                    lblConsumableBatchUsageSuccess.Text = "Consumable usage updated successfully.";
                    DivAlertConsumableBatchUsageSuccess.Visible = true;
                }
                catch (ConsumableUsageQuantityExceededException exceededException1)
                {
                    lblConsumableBatchUsageFailure.Text =
                        "The used quantity is larger than the quantity available, please adjust the quantity and try again.";
                    DivAlertConsumableBatchUsageFailure.Visible = true;
                }
            }
            else
            {
                //adding consumable usage
                var newConsumableBatchServiceUsage = new ConsumableBatchServiceUsage()
                {
                    ConsumbaleBatchShipmentPONumber = txtConsumableBatchUsageShipmentPoNumber.Text,
                    ConsumableBatchModelNumber = txtConsumableBatchUsageModelNumber.Text,
                    QuantityUsed = Convert.ToInt32(txtConsumableBatchUsageQuantityUsed.Text),
                };

                try
                {
                    var isEditing = hfEditingServiceId.Value != "" ? true : false;
                    //setting the service ID for the new item being added to the editing service since service ID isn't set
                    //for the item at any other point (unlike adding an item to a new service where service ID is given before saving)
                    if (isEditing)
                    {
                        newConsumableBatchServiceUsage.ServiceId = _serviceProcessor.Service.Id;
                    }

                    try
                    {
                        _serviceProcessor.ConsumableBatchUsageProcessor.AddConsumableBatchServiceUsage(
                            newConsumableBatchServiceUsage, isEditing);

                        lblConsumableBatchUsageSuccess.Text = "Consumable usage added to service successfully.";
                        DivAlertConsumableBatchUsageSuccess.Visible = true;
                    }
                    catch (ConsumableUsageQuantityExceededException exceededException)
                    {
                        lblConsumableBatchUsageFailure.Text =
                            "The used quantity is larger than the quantity available, please adjust the quantity and try again.";
                        DivAlertConsumableBatchUsageFailure.Visible = true;
                    }
                }
                catch (DuplicateRecordIdentifierException exception)
                {
                    Console.WriteLine(exception);
                    lblConsumableBatchUsageFailure.Text =
                        "The consumable item is already added to the service, please choose an item with a different serial number";
                    DivAlertConsumableBatchUsageFailure.Visible = true;
                }
            }

            DisplayAvailableConsumables();
            DisplayConsumableBatchServiceUsages();
        }

        private void DisplayConsumableBatchServiceUsages()
        {
            if (_serviceProcessor.ConsumableBatchUsageProcessor.ConsumableBatchServiceUsages == null) return;
            lvConsumableBatchUsages.DataSource =
                _serviceProcessor.ConsumableBatchUsageProcessor.ConsumableBatchServiceUsages;
            lvConsumableBatchUsages.DataBind();
        }


        protected void btnCancelConsumableBatchUsageUpdate_OnClick(object sender, EventArgs e)
        {
            ClearEditingConsumableBatchServiceUsage();
        }

        protected void btnDeleteConsumableBatchUsageItem_OnClick(object sender, EventArgs e)
        {
            ClearConsumableUsageAlerts();

            if (ViewState[ViewStateVarEditingConsumableBatchServiceUsage] == null) return;
            ConsumableBatchServiceUsage consumableBatchServiceUsage =
                (ConsumableBatchServiceUsage) ViewState[ViewStateVarEditingConsumableBatchServiceUsage];
            try
            {
                _serviceProcessor.ConsumableBatchUsageProcessor.RemoveConsumableBatchServiceUsage(
                    consumableBatchServiceUsage);
                ClearEditingConsumableBatchServiceUsage();
                lblConsumableBatchUsageSuccess.Text = "Consumable usage removed from service successfully.";
                DivAlertConsumableBatchUsageSuccess.Visible = true;
                DisplayConsumableBatchServiceUsages();
                DisplayAvailableConsumables();
            }
            catch (SqlException sqlException)
            {
                lblConsumableBatchUsageFailure.Text =
                    "The consumable usage has already been associated with other records, " +
                    "it cannot be deleted until the associated records have been deleted";
                DivAlertConsumableBatchUsageFailure.Visible = true;
                Console.WriteLine(sqlException);
            }
        }


        protected void btnChooseConsumableBatch_OnClick(object sender, EventArgs e)
        {
            if (ViewState[ViewStateVarEditingConsumableBatchServiceUsage] != null)
            {
                lblConsumableBatchUsageFailure.Text =
                    "The model number and shipment number of a used consumable cannot be updated once used in a service." +
                    " Please remove the current consumable usage and add the required consumable usage";
                DivAlertConsumableBatchUsageFailure.Visible = true;
                return;
            }

            var chosenConsumableBatch = ((sender as LinkButton)?.CommandArgument);
            if (chosenConsumableBatch == null) return;
            var consumableBatchIdArray = chosenConsumableBatch.Split('|');
            var shipmentPoNumber = consumableBatchIdArray[0];
            var modelNumber = consumableBatchIdArray[1];

            txtConsumableBatchUsageShipmentPoNumber.Text = shipmentPoNumber;
            txtConsumableBatchUsageModelNumber.Text = modelNumber;
        }

        protected void btnEditConsumableBatchUsage_OnClick(object sender, EventArgs e)
        {
            ClearConsumableUsageAlerts();
            var consumableBatchServiceUsageId = ((sender as LinkButton)?.CommandArgument);
            if (consumableBatchServiceUsageId == null) return;

            var consumableUsageIdArray = consumableBatchServiceUsageId.Split('|');
            var shipmentPo = consumableUsageIdArray[0];
            var modelNumber = consumableUsageIdArray[1];
            int serviceId = 0;
            if (hfEditingServiceId.Value != "") serviceId = Convert.ToInt32(hfEditingServiceId.Value);

            var editingconsumableBatchServiceUsage
                = _serviceProcessor.ConsumableBatchUsageProcessor.FindConsumableBatchServiceUsage(modelNumber,
                    shipmentPo, serviceId);

            //The editing item is held in a session variable so that it can survive postbacks
            ViewState[ViewStateVarEditingConsumableBatchServiceUsage] = editingconsumableBatchServiceUsage;
            DisplayEditingconsumableBatchServiceUsage();

            btnAddConsumableBatchUsage.Text = "Update Consumable Usage";
            btnCancelConsumableBatchUsageUpdate.Visible = true;
            btnDeleteConsumableBatchUsage.Visible = true;
        }

        private void DisplayEditingconsumableBatchServiceUsage()
        {
            if (ViewState[ViewStateVarEditingConsumableBatchServiceUsage] == null) return;

            ConsumableBatchServiceUsage editingConsumableBatchServiceUsage =
                (ConsumableBatchServiceUsage) ViewState[ViewStateVarEditingConsumableBatchServiceUsage];
            txtConsumableBatchUsageShipmentPoNumber.Text =
                editingConsumableBatchServiceUsage.ConsumbaleBatchShipmentPONumber;
            txtConsumableBatchUsageModelNumber.Text = editingConsumableBatchServiceUsage.ConsumableBatchModelNumber;
            txtConsumableBatchUsageQuantityUsed.Text = editingConsumableBatchServiceUsage.QuantityUsed.ToString();
            txtConsumableBatchUsageShipmentPoNumber.Enabled = false;
            txtConsumableBatchUsageModelNumber.Enabled = false;
        }


        //removal related methods

        

        private void LoadRemovables(string installedEquipmentSerial)
        {
            LoadSparePartRemovables(installedEquipmentSerial);
            LoadConsumableRemovables(installedEquipmentSerial);
        }

        private void LoadConsumableRemovables(string installedEquipmentSerial)
        {
            Service lastServiceForEquipment;
            try
            {
                lastServiceForEquipment = ServiceOpsBL.GetLastServiceForEquipment(installedEquipmentSerial);
            }
            catch (RecordNotFoundException e)
            {
                return;
            }
//
//            var removableConsumables = ConsumableBatchServiceUsageOpsBL.GetRemovableConsumableServiceUsagesByEquipment(
//                lastServiceForEquipment.Id + 1, installedEquipmentSerial);

            var removableConsumables
                = ConsumableBatchServiceUsageOpsBL.GetConsumableServiceUsagesByEquipment(
                    installedEquipmentSerial);

            //if the spare part has been removed, do not load in the 
            foreach (var consumableRemoval in _serviceProcessor.ConsumableRemovalProcessor.ConsumableRemovals)
            {
                foreach (var removableConsumable in removableConsumables.ToList())
                {
                    if (consumableRemoval.CompareTo(removableConsumable) == 0)
                    {
                        removableConsumables.Remove(removableConsumable);
                    }
                }
            }
            
            ConsumableBatchHealthCalculator batchHealthCalculator = new ConsumableBatchHealthCalculator();
            var consumableUsageHealths = batchHealthCalculator.CalculateConsumableBatchHealths(removableConsumables);

            lvRemovableConsumableUsages.DataSource = consumableUsageHealths;
            lvRemovableConsumableUsages.DataBind();
        }

        private void LoadSparePartRemovables(string installedEquipmentSerial)
        {
            Service lastServiceForEquipment;
            try
            {
                lastServiceForEquipment = ServiceOpsBL.GetLastServiceForEquipment(installedEquipmentSerial);
            }
            catch (RecordNotFoundException e)
            {
                return;
            }

//            var removableSparePartUsages = SparePartUsageOpsBL.GetRemovableSparePartUsages(
//                lastServiceForEquipment.Id + 1,
//                installedEquipmentSerial);

            var removableSparePartUsages = SparePartUsageOpsBL.GetSparePartUsagesByEquipment(installedEquipmentSerial);
            //if the spare part has been removed, do not load in the list
            foreach (var sparePartRemoval in _serviceProcessor.SparePartRemovalProcessor.SparePartRemovals)
            {
                foreach (var removableSparePartUsage in removableSparePartUsages.ToList())
                {
                    if (sparePartRemoval.SparePartItemSerialNumber == removableSparePartUsage.SparePartItemSerialNumber)
                    {
                        removableSparePartUsages.Remove(removableSparePartUsage);
                    }
                }
            }

            SparePartUsageHealthCalculator healthCalculator = new SparePartUsageHealthCalculator();
            var sparePartUsageHealths = healthCalculator.CalculateSparePartUsageHealths(removableSparePartUsages);

            lvRemovableSparePartUsages.DataSource = sparePartUsageHealths;
            lvRemovableSparePartUsages.DataBind();
        }

        //remove used spare parts related methods


        protected void btnChooseRemovedSparePart_OnClick(object sender, EventArgs e)
        {
            ClearSparePartRemovalAlerts();

            //spare part removals are still a spare part usage, the only difference is that a removal_service ID is set
            var sparePartItemSerialNumber = ((sender as LinkButton)?.CommandArgument);
            if (sparePartItemSerialNumber == null) return;
            var sparePartUsage
                = SparePartUsageOpsBL.GetSparePartUsageBySerialNumber(sparePartItemSerialNumber);

            //checking if the service is being edited or added for the first time
            var isEditing = hfEditingServiceId.Value != "" ? true : false;
            if (isEditing)
            {
                sparePartUsage.RemovalServiceId = _serviceProcessor.Service.Id;
            }

            _serviceProcessor.SparePartRemovalProcessor.AddSparePartRemoval(sparePartUsage, isEditing);
            lblSparePartRemovalSuccess.Text = "Spare part removal record added successfully!";
            DivAlertSparePartRemovalSuccess.Visible = true;

            LoadRemovables(txtServiceEquipmentSerial.Text);
            DisplayRemovedSpareParts();
        }

        private void ClearSparePartRemovalAlerts()
        {
            DivAlertSparePartRemovalSuccess.Visible = false;
            DivAlertSparePartRemovalFail.Visible = false;
        }

        private void DisplayRemovedSpareParts()
        {
            foreach (var sparePartUsage in _serviceProcessor.SparePartRemovalProcessor.SparePartRemovals)
            {
                var sparePartItem =
                    SparePartItemOpsBL.GetSparePartItemBySerialNumber(sparePartUsage.SparePartItemSerialNumber);
                var service = ServiceOpsBL.GetServiceById(sparePartUsage.ServiceId);
                sparePartUsage.SparePartItem = sparePartItem;
                sparePartUsage.Service = service;
            }


            lvSparePartRemovals.DataSource = _serviceProcessor.SparePartRemovalProcessor.SparePartRemovals;
            lvSparePartRemovals.DataBind();
        }

        protected void btnRemoveSparePartRemoval_OnClick(object sender, EventArgs e)
        {
            var sparePartItemSerialNumber = ((sender as LinkButton)?.CommandArgument);
            if (sparePartItemSerialNumber == null) return;
            SparePartUsage removedSparePartRemoval = null;

            foreach (var sparePartRemoval in _serviceProcessor.SparePartRemovalProcessor.SparePartRemovals)
            {
                if (sparePartRemoval.SparePartItemSerialNumber == sparePartItemSerialNumber)
                {
                    removedSparePartRemoval = sparePartRemoval;
                }
            }

            _serviceProcessor.SparePartRemovalProcessor.RemoveSparePartRemoval(removedSparePartRemoval);
            LoadRemovables(txtServiceEquipmentSerial.Text);
            DisplayRemovedSpareParts();
        }

        protected void runJSript(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(GetType(), "Javascript1", "javascript:hello(); ", true);
        }


        protected void btnChooseRemovedConsumable_OnClick(object sender, EventArgs e)
        {
            ClearConsumableRemovalAlerts();

            //spare part removals are still a spare part usage, the only difference is that a removal_service ID is set
            var chosenConsumableBatch = ((sender as LinkButton)?.CommandArgument);
            if (chosenConsumableBatch == null) return;
            var consumableBatchIdArray = chosenConsumableBatch.Split('|');
            var shipmentPoNumber = consumableBatchIdArray[0];
            var modelNumber = consumableBatchIdArray[1];
            var serviceId = consumableBatchIdArray[2];

            var consumableRemoval = ConsumableBatchServiceUsageOpsBL.GetConsumableBatchServiceUsageById(shipmentPoNumber, modelNumber,
                Convert.ToInt32(serviceId));

            //checking if the service is being edited or added for the first time
            var isEditing = hfEditingServiceId.Value != "" ? true : false;
            if (isEditing)
            {
                consumableRemoval.RemovalServiceId = _serviceProcessor.Service.Id;
            }

            _serviceProcessor.ConsumableRemovalProcessor.AddConsumableRemoval(consumableRemoval, isEditing);
            lblSparePartRemovalSuccess.Text = "Consumable removal record added successfully!";
            DivAlertSparePartRemovalSuccess.Visible = true;

            LoadRemovables(txtServiceEquipmentSerial.Text);
            DisplayRemovedConsumables();
        }

        private void DisplayRemovedConsumables()
        {
            foreach (var removal in _serviceProcessor.ConsumableRemovalProcessor.ConsumableRemovals)
            {
                var service = ServiceOpsBL.GetServiceById(removal.ServiceId);
                var consumableBatch = ConsumableBatchOpsBL.GetConsumableBatchById(removal.ConsumableBatchModelNumber,
                    removal.ConsumbaleBatchShipmentPONumber);
                removal.Service = service;
                removal.ConsumableBatch = consumableBatch;
            }
           

            lvConsumableRemovals.DataSource = _serviceProcessor.ConsumableRemovalProcessor.ConsumableRemovals;
            lvConsumableRemovals.DataBind();
        }

        private void ClearConsumableRemovalAlerts()
        {
            DivAlertConsumableRemovalSuccess.Visible = false;
            DivAlertConsumableRemovalFail.Visible = false;
        }

        protected void btnRemoveConsumableRemoval_OnClick(object sender, EventArgs e)
        {
            var chosenConsumableBatch = ((sender as LinkButton)?.CommandArgument);
            if (chosenConsumableBatch == null) return;
            var consumableBatchIdArray = chosenConsumableBatch.Split('|');
            var shipmentPoNumber = consumableBatchIdArray[0];
            var modelNumber = consumableBatchIdArray[1];
            var serviceId = consumableBatchIdArray[2];

            ConsumableBatchServiceUsage removedConsumableRemoval = null;
            foreach (var consumableRemoval in _serviceProcessor.ConsumableRemovalProcessor.ConsumableRemovals)
            {
                if (consumableRemoval.ConsumbaleBatchShipmentPONumber == shipmentPoNumber &&
                    consumableRemoval.ConsumableBatchModelNumber == modelNumber &&
                    consumableRemoval.ServiceId == Convert.ToInt32(serviceId))
                {
                    removedConsumableRemoval = consumableRemoval;
                }
            }

            _serviceProcessor.ConsumableRemovalProcessor.RemoveConsumableRemoval(removedConsumableRemoval);
            LoadRemovables(txtServiceEquipmentSerial.Text);
            DisplayRemovedConsumables();
        }

        protected void txtServiceDate_OnTextChanged(object sender, EventArgs e)
        {
            var isEditing = hfEditingServiceId.Value != "" ? true : false;
            if (isEditing)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "openModal();", true);
            }

        }

        protected void btnCancelDateChange_OnClick(object sender, EventArgs e)
        {
            
        }

        protected void btnConfirmDateChange_OnClick(object sender, EventArgs e)
        {
            
        }

       
    }
}