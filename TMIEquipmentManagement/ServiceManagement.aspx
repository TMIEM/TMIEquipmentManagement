<%@ Page Title="Service Management" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ServiceManagement.aspx.cs" Inherits="TMIEquipmentManagement.ServiceManagement" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- DataTables -->
    <link rel="stylesheet" href="./bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css">
    <link rel="stylesheet" href="./dist/css/base/jquery-ui-1.9.2.custom.min.css">
    <script type="text/javascript" language="javascript">
//        function helloWorld(){
//            alert("welcome to codepedia.info");	
//        }

    </script>
    <script>
        function hello() {
            var healthBars = document.getElementsByClassName("progress-bar"); // preferred`
            for (var i = 0; i < healthBars.length; i++) {
                var healthBar = healthBars[i];
                var health = healthBar.getAttribute('data-healthpercent');
                var healthFloat = parseFloat(health);
                var progressBarClass = "progress-bar-info";
                if (healthFloat < 70 && healthFloat > 40) {
                    progressBarClass = "progress-bar-warning";
                }
                else if (healthFloat < 40) {
                    progressBarClass = "progress-bar-danger";
                } else {
                    progressBarClass = "progress-bar-success";
                }

                healthBar.classList.remove('progress-bar-info');
                healthBar.classList.add(progressBarClass);
                healthBar.setAttribute('aria-valuenow', healthFloat);
                healthBar.style.width = healthFloat + '%';
                //$(healthBars[i].id).attr('aria-valuenow', healthFloat).css('width', healthFloat);
            }
        }
        
        function openModal() {
            $('#removals-modal-danger').modal('show');
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContentPlaceHolder" runat="server">
    <section class="content-header">
        <h1>Service Management
            <small>View, Add and Update Services</small>
        </h1>
        <ol class="breadcrumb">
            <li>
                <a href="index.aspx"><i class="fa fa-dashboard"></i>Dashboard</a>
            </li>
            <li class="active"><i class="fa fa-user"></i>Service Management</li>
        </ol>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="col-md-12">
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label ID="lblAddEditHeading" runat="server" Text="Add Service"></asp:Label>
                </h3>
            </div>
            <!-- /.box-header -->
            <!-- form start -->
            <div class="box-body">
                <asp:HiddenField ID="hfEditingServiceId" Value="" runat="server" />
                <div class="col-md-12">
                    <div class="row">
                        <div class="box box-primary box-solid">
                            <div class="box-header with-border">
                                <h3 class="box-title">Service Details</h3>

                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                                <!-- /.box-tools -->
                            </div>
                            <!-- /.box-header -->
                            <div class="box-body">
                                <!-- left side input fields -->
                                <div class="col-md-6">
                                    <div runat="server" ID="DivServiceIdRow" Visible="False" class="row">
                                        <div class="form-group col-md-12">
                                            <label>
                                                Service ID
                                            </label>
                                            <asp:TextBox runat="server" CssClass="form-control"
                                                         ID="txtServiceId"
                                                         MaxLength="49"
                                                         Enabled="False"
                                                         placeholder="Service ID">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group col-md-12">
                                            <label>
                                                Equipment Serial Number (Choose from table)
                                            </label>
                                            <asp:TextBox runat="server" CssClass="form-control"
                                                ID="txtServiceEquipmentSerial"
                                                MaxLength="49"
                                                Enabled="False"
                                                placeholder="Choose Equipment Serial Number">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                ID="rfvEquipmentSerial"
                                                ValidationGroup="vgServiceDetails"
                                                ControlToValidate="txtServiceEquipmentSerial"
                                                ErrorMessage="Service model number is required">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group col-md-12">
                                            <label>
                                                Technician ID (Choose from table)
                                            </label>
                                            <asp:TextBox runat="server" CssClass="form-control"
                                                ID="txtServiceTechnicianId"
                                                MaxLength="49"
                                                Enabled="False"
                                                placeholder="Choose Technician ID">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                ID="rfvTechnicianId"
                                                ValidationGroup="vgServiceDetails"
                                                ControlToValidate="txtServiceEquipmentSerial"
                                                ErrorMessage="Technician ID is required">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group col-md-12">
                                            <label>Service Date</label>
                                            <asp:TextBox runat="server" CssClass="form-control"
                                                ID="txtServiceDate"
                                                MaxLength="29"
                                                placeholder="Enter Service Date" OnTextChanged="txtServiceDate_OnTextChanged">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                ID="rfvServiceDate"
                                                ValidationGroup="vgServiceDetails"
                                                ControlToValidate="txtServiceDate"
                                                ErrorMessage="Service date is required">
                                            </asp:RequiredFieldValidator>
                                            <asp:CompareValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                ID="cvServiceDate"
                                                ValidationGroup="vgServiceDetails"
                                                ControlToValidate="txtServiceDate"
                                                Operator="DataTypeCheck"
                                                Type="Date"
                                                ErrorMessage="Please enter a valid service date">
                                            </asp:CompareValidator>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group col-md-12">
                                            <label>Problem Description</label>
                                            <asp:TextBox runat="server" CssClass="form-control"
                                                ID="txtServiceProblemDescription"
                                                MaxLength="4999"
                                                TextMode="MultiLine"
                                                Columns="50"
                                                Rows="20"
                                                placeholder="Enter Problem Description">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                ID="rfvProblemDescription"
                                                ControlToValidate="txtServiceProblemDescription"
                                                ErrorMessage="Problem description is required">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group col-md-12">
                                            <label>Service Description</label>
                                            <asp:TextBox runat="server" CssClass="form-control"
                                                ID="txtServiceDescription"
                                                MaxLength="4999"
                                                TextMode="MultiLine"
                                                Columns="50"
                                                Rows="20"
                                                placeholder="Enter Equipment Description">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                ID="rfvServiceDescription"
                                                ControlToValidate="txtServiceDescription"
                                                ErrorMessage="Service description is required">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group col-md-12">
                                            <label>Special Note</label>
                                            <asp:TextBox runat="server" CssClass="form-control"
                                                ID="txtSpecialNote"
                                                MaxLength="2999"
                                                TextMode="MultiLine"
                                                Columns="50"
                                                Rows="20"
                                                placeholder="Enter Special Note">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <!-- /.left side input fields-->
                                <!-- right side input fields-->
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="box box-primary box-solid">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">View and search installed equipments</h3>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i class="fa fa-minus"></i>
                                                    </button>
                                                </div>
                                                <!-- /.box-tools -->
                                            </div>
                                            <!-- /.box-header -->
                                            <!-- form start -->

                                            <div class="box-body">
                                                <asp:ListView ID="lvInstalledEquipments" runat="server" ItemPlaceholderID="installedEquipmentItemPlaceHolder">
                                                    <EmptyDataTemplate>
                                                        No Installed Equipments Found
                                                    </EmptyDataTemplate>
                                                    <LayoutTemplate>
                                                        <table id="InstalledEquipmentsTable" class="table table-hover">
                                                            <thead>
                                                                <tr>
                                                                    <th>Serial Number</th>
                                                                    <th>Model Number</th>
                                                                    <th>Customer</th>
                                                                    <th>Installation Date</th>
                                                                    <th>Warranty (Months)</th>
                                                                    <th>Service Agreement (Months)</th>
                                                                    <th>Select</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <asp:PlaceHolder runat="server" ID="installedEquipmentItemPlaceHolder"></asp:PlaceHolder>
                                                            </tbody>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <a href='../EquipmentItemProfile.aspx?serialnumber=<%# Eval("EquipmentItemSerialNumber").ToString() %>' target="_blank">
                                                                    <%# Eval("EquipmentItemSerialNumber").ToString() %>
                                                                </a>
                                                            </td>
                                                            <td>
                                                                <a href='../EquipmentProfile.aspx?modelnumber=<%# Eval("EquipmentItem.EquipmentModelNumber").ToString() %>' target="_blank">
                                                                    <%# Eval("EquipmentItem.EquipmentModelNumber").ToString() %>
                                                                </a>
                                                            </td>
                                                            <td>
                                                                <a href='../CustomerProfile.aspx?id=<%# Eval("Customer.Id").ToString() %>' target="_blank">
                                                                    <%# Eval("Customer.Name").ToString() %>
                                                                </a>
                                                            </td>
                                                            <td>
                                                                <%# Convert.ToDateTime(Eval("InstallationDate")).ToShortDateString()%>
                                                            </td>
                                                            <td>
                                                                <%# Eval("WarrantyPeriodMonths").ToString() %>
                                                            </td>
                                                            <td>
                                                                <%# Eval("ServiceAgreementPeriodMonths").ToString() %>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton runat="server" CommandArgument='<%# Eval("EquipmentItemSerialNumber") %>' CssClass="text-primary" CausesValidation="False" OnClick="btnChooseEquipmentInstallation_OnClick">Choose</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </div>
                                            <!-- /.box-body -->
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="box box-primary box-solid">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">View and search technicians</h3>

                                                <div class="box-tools pull-right">
                                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                        <i class="fa fa-minus"></i>
                                                    </button>
                                                </div>
                                                <!-- /.box-tools -->
                                            </div>
                                            <!-- /.box-header -->
                                            <!-- form start -->

                                            <div class="box-body">
                                                <asp:ListView ID="lvTechnicians" runat="server" ItemPlaceholderID="technicianItemPlaceHolder">
                                                    <EmptyDataTemplate>
                                                        No Technicians Added
                                                    </EmptyDataTemplate>
                                                    <LayoutTemplate>
                                                        <table id="TechniciansTable" class="table table-hover">
                                                            <thead>
                                                                <tr>
                                                                    <th>ID</th>
                                                                    <th>Name</th>
                                                                    <th>Contact</th>
                                                                    <th>Email</th>
                                                                    <th>Select Technician</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <asp:PlaceHolder runat="server" ID="technicianItemPlaceHolder"></asp:PlaceHolder>
                                                            </tbody>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <a href='../TechnicianProfile.aspx?id=<%# Eval("Id").ToString() %>' target="_blank">
                                                                    <%# Eval("Id").ToString() %>
                                                                </a>
                                                            </td>
                                                            <td>
                                                                <%# Eval("Name").ToString() %>
                                                            </td>
                                                            <td>
                                                                <%# Eval("ContactNumber").ToString() %>
                                                            </td>
                                                            <td>
                                                                <%# Eval("Email").ToString() %>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton runat="server" CommandArgument='<%# Eval("Id") %>' CssClass="text-danger" CausesValidation="False" OnClick="btnChooseTechnician_OnClick">Choose</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:ListView>
                                            </div>
                                            <!-- /.box-body -->
                                        </div>
                                    </div>
                                </div>
                                <!-- /.right side input fields-->
                            </div>
                            <!-- /.box-body -->
                        </div>
                    </div>
                </div>

                <!-- /.Service equipments -->
                <!--Spare parts used-->
                <div class="col-md-12">
                    <div class="row">
                        <div class="box box-primary box-solid">
                            <div class="box-header with-border">
                                <h3 class="box-title">Spare Parts Used</h3>

                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                                <!-- /.box-tools -->
                            </div>
                            <!-- /.box-header -->
                            <div class="box-body">
                                <!-- SparePartUsage input fields-->
                                <div class="col-md-6">
                                    <div class="form-group col-md-12">
                                        <label>
                                            Spare Part Serial Number (choose from table)<br />
                                        </label>
                                        <asp:TextBox runat="server" CssClass="form-control"
                                            ID="txtSparePartUsageSerialNumber"
                                            Enabled="False"
                                            MaxLength="49"
                                            placeholder="Select Spare Part Serial Number">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                            ID="rfvSparePartUsageModelNumber"
                                            ValidationGroup="vgSparePartUsage"
                                            ControlToValidate="txtSparePartUsageSerialNumber"
                                            ErrorMessage="Spare part serial number is required">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-md-12">
                                        <label>
                                            Warranty Period (Months)<br />
                                        </label>
                                        <asp:TextBox runat="server" CssClass="form-control"
                                            ID="txtSparePartUsageWarrantyPeriod"
                                            TextMode="Number"
                                            Min="0"
                                            MaxLength="49"
                                            placeholder="Enter warranty period (Number of Months)">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                            ID="rfvSparePartUsageWarrantyPeriod"
                                            ValidationGroup="vgSparePartUsage"
                                            ControlToValidate="txtSparePartUsageWarrantyPeriod"
                                            ErrorMessage="Warranty period for spare part is required">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-md-12">
                                        <asp:HiddenField ID="hfEditingSparePartUsageItemserial" Value="" runat="server" />
                                        <asp:Button ID="btnAddSparePartUsage" CssClass="btn btn-primary" OnClick="btnSubmitSparePartUsage_OnClick" runat="server" ValidationGroup="vgSparePartUsage" Text="Add Spare Part Usage" />
                                        <asp:Button ID="btnCancelSparePartUsageUpdate" CssClass="btn btn-warning" CausesValidation="False" OnClick="btnCancelSparePartUsageUpdate_OnClick" runat="server" Visible="False" Text="Cancel Update" />
                                        <asp:Button ID="btnDeleteSparePartUsage" CssClass="btn btn-danger" OnClick="btnDeleteSparePartUsageItem_OnClick" runat="server" ValidationGroup="vgSparePartUsage" Text="Remove Spare Part Usage" Visible="False" />
                                        <br />
                                        <br />
                                        <div runat="server" visible="False" id="DivAlertSparePartUsageSuccess" class="alert alert-success alert-dismissible">
                                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>
                                            <h4><i class="icon fa fa-check"></i>Operation Successful!</h4>
                                            <asp:Label ID="lblSparePartUsageSuccess" runat="server" Text=""></asp:Label>
                                        </div>
                                        <div runat="server" visible="False" id="DivAlertSparePartUsageFailure" class="alert alert-danger alert-dismissible">
                                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>
                                            <h4><i class="icon fa fa-ban"></i>Operation Failed!</h4>
                                            <asp:Label ID="lblSparePartUsageFailure" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <!-- /.SparePartUsage input fields -->
                                <!-- SparePartUsage model selection-->
                                <div class="col-md-6">
                                    <div class="box box-primary box-solid">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">View and search available spare parts</h3>

                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                    <i class="fa fa-minus"></i>
                                                </button>
                                            </div>
                                            <!-- /.box-tools -->
                                        </div>
                                        <!-- /.box-header -->
                                        <!-- form start -->

                                        <div class="box-body">
                                            <asp:ListView ID="lvSparePartItems" runat="server" ItemPlaceholderID="SparePartItemPlaceHolder">
                                                <EmptyDataTemplate>
                                                    No Available Spare Parts Found
                                                </EmptyDataTemplate>
                                                <LayoutTemplate>
                                                    <table id="SparePartItemsTable" class="table table-hover">
                                                        <thead>
                                                            <tr>
                                                                <th>Serial Number</th>                                                                
                                                                <th>Model Number</th>
                                                                <th>Price</th>
                                                                <th>Select Spare Part</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:PlaceHolder runat="server" ID="SparePartItemPlaceHolder"></asp:PlaceHolder>
                                                        </tbody>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <a href='../SparePartItemProfile.aspx?serialnumber=<%# Eval("SerialNumber").ToString() %>' target="_blank">
                                                                <%# Eval("SerialNumber").ToString() %>
                                                            </a>
                                                        </td>
                                                        <td>
                                                            <a href='../SparePartProfile.aspx?modelnumber=<%# Eval("SparePartModelNumber").ToString() %>' target="_blank">
                                                                <%# Eval("SparePartModelNumber").ToString() %>
                                                            </a>
                                                        </td>
                                                        <td>
                                                            <%# Eval("Price", "{0:n}")%>
                                                        </td>

                                                        <td>
                                                            <asp:LinkButton runat="server" CommandArgument='<%# Eval("SerialNumber") %>' CssClass="text-primary" CausesValidation="False" OnClick="btnChooseSparePartItem_OnClick">Choose</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>
                                        <!-- /.box-body -->
                                    </div>
                                </div>
                                <!-- /.SparePartUsage model selection-->
                                <!-- Selected SparePartUsage items-->
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">Spare Parts Used</h3>

                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                    <i class="fa fa-minus"></i>
                                                </button>
                                            </div>
                                            <!-- /.box-tools -->
                                        </div>
                                        <!-- /.box-header -->
                                        <div class="box-body">
                                            <asp:ListView ID="lvSparePartUsages" runat="server" ItemPlaceholderID="SparePartUsageItemPlaceHolder">
                                                <EmptyDataTemplate>
                                                    No Spare Parts Used
                                                </EmptyDataTemplate>
                                                <LayoutTemplate>
                                                    <table id="SparePartUsageItemsTable" class="table table-hover">
                                                        <thead>
                                                            <tr>
                                                                <th>Serial Number</th>
                                                                <th>Warranty Period (Months)</th>
                                                                <th>Actions</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:PlaceHolder runat="server" ID="SparePartUsageItemPlaceHolder"></asp:PlaceHolder>
                                                        </tbody>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <a href='../SparePartItemProfile.aspx?serialnumber=<%# Eval("SparePartItemSerialNumber").ToString() %>' target="_blank">
                                                                <%# Eval("SparePartItemSerialNumber").ToString() %>
                                                            </a>
                                                        </td>
                                                        <td>
                                                            <%# Eval("WarrantyPeriodMonths").ToString() %>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton runat="server" CommandArgument='<%# Eval("SparePartItemSerialNumber") %>' CssClass="text-primary" CausesValidation="False" OnClick="btnEditSparePartUsage_OnClick">Edit</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>
                                        <!-- /.box-body -->
                                    </div>


                                </div>
                                <!-- /.selected SparePartUsage items-->

                            </div>
                            <!-- /.box-body -->
                        </div>
                    </div>
                </div>
                <!-- Service Spare parts used -->
                
                <div class="col-md-12">
                    <div class="row">
                        <div class="box box-primary box-solid">
                            <div class="box-header with-border">
                                <h3 class="box-title">Consumables Used</h3>

                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                                <!-- /.box-tools -->
                            </div>
                            <!-- /.box-header -->
                            <div class="box-body">
                                <!-- ConsumableBatchUsage input fields-->
                                <div class="col-md-6">
                                    <div class="form-group col-md-12">
                                        <label>
                                            Consumable Batch Model Number (choose from table)<br />
                                        </label>
                                        <asp:TextBox runat="server" CssClass="form-control"
                                            ID="txtConsumableBatchUsageModelNumber"
                                            Enabled="False"
                                            MaxLength="49"
                                            placeholder="Select Consumable Batch Model Number">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                            ID="rfvConsumableBatchUsageModelNumber"
                                            ValidationGroup="vgConsumableBatchUsage"
                                            ControlToValidate="txtConsumableBatchUsageModelNumber"
                                            ErrorMessage="Consumable batch model number is required">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-md-12">
                                        <label>
                                            Consumable Batch Shipment PO number (choose from table)<br />
                                        </label>
                                        <asp:TextBox runat="server" CssClass="form-control"
                                            ID="txtConsumableBatchUsageShipmentPoNumber"
                                            Enabled="False"
                                            MaxLength="49"
                                            placeholder="Enter consumable shipment PO Number">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                            ID="rfvConsumableBatchUsageWarrantyPeriod"
                                            ValidationGroup="vgConsumableBatchUsage"
                                            ControlToValidate="txtConsumableBatchUsageShipmentPoNumber"
                                            ErrorMessage="Consumable shipment PO number is required">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-md-12">
                                        <label>
                                            Quantity Used<br />
                                        </label>
                                        <asp:TextBox runat="server" CssClass="form-control"
                                            ID="txtConsumableBatchUsageQuantityUsed"
                                            TextMode="Number"
                                                     Min="1"
                                            MaxLength="49"
                                            placeholder="Enter quantity used">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                            ID="rfvConsumableBatchQuantityUsed"
                                            ValidationGroup="vgConsumableBatchUsage"
                                            ControlToValidate="txtConsumableBatchUsageQuantityUsed"
                                            ErrorMessage="Consumable usage quantity is required">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-md-12">
                                        <asp:HiddenField ID="hfEditingConsumableBatchUsageItemserial" Value="" runat="server" />
                                        <asp:Button ID="btnAddConsumableBatchUsage" CssClass="btn btn-primary" OnClick="btnSubmitConsumableBatchUsage_OnClick" runat="server" ValidationGroup="vgConsumableBatchUsage" Text="Add Consumable Usage" />
                                        <asp:Button ID="btnCancelConsumableBatchUsageUpdate" CssClass="btn btn-warning" CausesValidation="False" OnClick="btnCancelConsumableBatchUsageUpdate_OnClick" runat="server" Visible="False" Text="Cancel Update" />
                                        <asp:Button ID="btnDeleteConsumableBatchUsage" CssClass="btn btn-danger" OnClick="btnDeleteConsumableBatchUsageItem_OnClick" runat="server" ValidationGroup="vgConsumableBatchUsage" Text="Remove Consumable Usage" Visible="False" />
                                        <br />
                                        <br />
                                        <div runat="server" visible="False" id="DivAlertConsumableBatchUsageSuccess" class="alert alert-success alert-dismissible">
                                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>
                                            <h4><i class="icon fa fa-check"></i>Operation Successful!</h4>
                                            <asp:Label ID="lblConsumableBatchUsageSuccess" runat="server" Text=""></asp:Label>
                                        </div>
                                        <div runat="server" visible="False" id="DivAlertConsumableBatchUsageFailure" class="alert alert-danger alert-dismissible">
                                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>
                                            <h4><i class="icon fa fa-ban"></i>Operation Failed!</h4>
                                            <asp:Label ID="lblConsumableBatchUsageFailure" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <!-- /.ConsumableBatchUsage input fields -->
                                <!-- ConsumableBatchUsage model selection-->
                                <div class="col-md-6">
                                    <div class="box box-primary box-solid">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">View and search available consumable batches</h3>

                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                    <i class="fa fa-minus"></i>
                                                </button>
                                            </div>
                                            <!-- /.box-tools -->
                                        </div>
                                        <!-- /.box-header -->
                                        <!-- form start -->

                                        <div class="box-body">
                                            <asp:ListView ID="lvConsumableBatches" runat="server" ItemPlaceholderID="ConsumableBatchItemPlaceHolder">
                                                <EmptyDataTemplate>
                                                    No Consumable Batches Found
                                                </EmptyDataTemplate>
                                                <LayoutTemplate>
                                                    <table id="ConsumableItemsTable" class="table table-hover">
                                                        <thead>
                                                            <tr>
                                                                <th>Shipment PO Number</th>                                                                
                                                                <th>Model Number</th>
                                                                <th>Quantity Available</th>
                                                                <th>Select Consumable</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:PlaceHolder runat="server" ID="ConsumableBatchItemPlaceHolder"></asp:PlaceHolder>
                                                        </tbody>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <a href='../ShipmentProfile.aspx?ponumber=<%# Eval("ShipmentPoNumber").ToString()%>' target="_blank">
                                                                <%# Eval("ShipmentPoNumber").ToString() %>
                                                            </a>
                                                        </td>
                                                        <td>
                                                            <a href='../ConsumableProfile.aspx?modelnumber=<%# Eval("ConsumableModelNumber").ToString() %>' target="_blank">
                                                                <%# Eval("ConsumableModelNumber").ToString() %>
                                                            </a>
                                                        </td>
                                                        <td>
                                                            <%# Eval("Quantity").ToString() %>
                                                        </td>

                                                        <td>
                                                            <asp:LinkButton runat="server" CommandArgument='<%# Eval("ShipmentPoNumber")+"|"+ Eval("ConsumableModelNumber") %>' CssClass="text-primary" CausesValidation="False" OnClick="btnChooseConsumableBatch_OnClick">Choose</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>
                                        <!-- /.box-body -->
                                    </div>
                                </div>
                                <!-- /.ConsumableBatchUsage model selection-->
                                <!-- Selected ConsumableBatchUsage items-->
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">Consumable Batches (items) Used</h3>

                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                    <i class="fa fa-minus"></i>
                                                </button>
                                            </div>
                                            <!-- /.box-tools -->
                                        </div>
                                        <!-- /.box-header -->
                                        <div class="box-body">
                                            <asp:ListView ID="lvConsumableBatchUsages" runat="server" ItemPlaceholderID="ConsumableBatchUsageItemPlaceHolder">
                                                <EmptyDataTemplate>
                                                    No Consumable Batches Items Used
                                                </EmptyDataTemplate>
                                                <LayoutTemplate>
                                                    <table id="ConsumableBatchUsageItemsTable" class="table table-hover">
                                                        <thead>
                                                            <tr>
                                                                <th>Shipment PO Number</th>
                                                                <th>Model Number</th>
                                                                <th>Quantity Used</th>
                                                                <th>Actions</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:PlaceHolder runat="server" ID="ConsumableBatchUsageItemPlaceHolder"></asp:PlaceHolder>
                                                        </tbody>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <a href='../ShipmentProfile.aspx?ponumber=<%# Eval("ConsumbaleBatchShipmentPONumber").ToString() %>' target="_blank">
                                                                <%# Eval("ConsumbaleBatchShipmentPONumber").ToString() %>
                                                            </a>
                                                        </td>
                                                        <td>
                                                            <a href='../ConsumableProfile.aspx?modelnumber=<%# Eval("ConsumableBatchModelNumber").ToString() %>' target="_blank">
                                                                <%# Eval("ConsumableBatchModelNumber").ToString() %>
                                                            </a>
                                                        </td>
                                                        <td>
                                                            <%# Eval("QuantityUsed").ToString() %>
                                                        </td>

                                                        <td>
                                                            <asp:LinkButton runat="server" CommandArgument='<%# Eval("ConsumbaleBatchShipmentPONumber")+"|"+ Eval("ConsumableBatchModelNumber") %>' CssClass="text-primary" CausesValidation="False" OnClick="btnEditConsumableBatchUsage_OnClick">Edit</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>
                                        <!-- /.box-body -->
                                    </div>


                                </div>
                                <!-- /.selected ConsumableBatchUsage items-->

                            </div>
                            <!-- /.box-body -->
                        </div>
                    </div>
                </div>

                <!--Removable spare part usages-->
                <div class="col-md-12">
                    <div class="row">
                        <div class="box box-primary box-solid">
                            <div class="box-header with-border">
                                <h3 class="box-title">Spare Parts Removed From Equipment During Service</h3>

                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                                <!-- /.box-tools -->
                            </div>
                            <!-- /.box-header -->
                            <div class="box-body">
                                <div runat="server" visible="False" id="DivAlertSparePartRemovalSuccess" class="alert alert-success alert-dismissible">
                                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>
                                    <h4><i class="icon fa fa-check"></i>Operation Successful!</h4>
                                    <asp:Label ID="lblSparePartRemovalSuccess" runat="server" Text=""></asp:Label>
                                </div>
                                <div runat="server" visible="False" id="DivAlertSparePartRemovalFail" class="alert alert-danger alert-dismissible">
                                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>
                                    <h4><i class="icon fa fa-ban"></i>Operation Failed!</h4>
                                    <asp:Label ID="lblSparePartRemovalFail" runat="server" Text=""></asp:Label>
                                </div>
                                <!-- ConsumableBatchUsage input fields-->
                                
                                <!-- /.ConsumableBatchUsage input fields -->
                                <!-- ConsumableBatchUsage model selection-->
                                <div class="col-md-12">
                                    <div class="box box-primary ">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">View and search removable spare part usages</h3>

                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                    <i class="fa fa-minus"></i>
                                                </button>
                                            </div>
                                            <!-- /.box-tools -->
                                        </div>
                                        <!-- /.box-header -->
                                        <!-- form start -->

                                        <div class="box-body">
                                            <asp:ListView OnDataBound="runJSript" ID="lvRemovableSparePartUsages" runat="server" ItemPlaceholderID="RemovableSparePartItemPlaceHolder">
                                                <EmptyDataTemplate>
                                                    No Spare Part Usages Found
                                                </EmptyDataTemplate>
                                                <LayoutTemplate>
                                                    <table id="RemovableSparePartItemsTable" class="table table-hover">
                                                        <thead>
                                                            <tr>
                                                                <th>Serial Number</th>                                                                
                                                                <th>Model Number</th>
                                                                <th>Installed in Service</th>
                                                                <th>Installation Date</th>
                                                                <th>Expiry Date</th>
                                                                <th>Health (%)</th>
                                                                <th>Choose Part Removed</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:PlaceHolder runat="server" ID="RemovableSparePartItemPlaceHolder"></asp:PlaceHolder>
                                                        </tbody>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <a href='../SparePartItemProfile.aspx?serialnumber=<%# Eval("SparePartUsage.SparePartItemSerialNumber").ToString()%>' target="_blank">
                                                                <%# Eval("SparePartUsage.SparePartItemSerialNumber").ToString() %>
                                                            </a>
                                                        </td>
                                                        <td>
                                                            <a href='../SparePartProfile.aspx?modelNumber=<%# Eval("SparePartUsage.SparePartItem.SparePartModelNumber").ToString()%>' target="_blank">
                                                                <%# Eval("SparePartUsage.SparePartItem.SparePartModelNumber").ToString() %>
                                                            </a>
                                                        </td>
                                                       
                                                        <td>
                                                            <a href='../ServiceProfile.aspx?id=<%# Eval("SparePartUsage.Service.Id").ToString() %>' target="_blank">
                                                                <%# Eval("SparePartUsage.Service.Id").ToString() %>
                                                            </a>
                                                        </td>
                                                        <td>
                                                            <%# Convert.ToDateTime(Eval("SparePartUsage.Service.Date")).ToShortDateString()%>
                                                        </td>
                                                        <td>
                                                            <%#Convert.ToDateTime(Eval("ExpiryDate")).ToShortDateString()%>
                                                        </td>
                                                        <td style="min-width: 10vw">
                                                            <span style="display: none"><%# Eval("HealthPercentage").ToString() %></span>
                                                           <div class="progress">
                                                               <div id="<%# Eval("SparePartUsage.SparePartItemSerialNumber").ToString() %>" class="progress-bar progress-bar-info progress-bar-striped active" role="progressbar"
                                                                    aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" data-healthpercent="<%# Eval("HealthPercentage").ToString() %>" style="width: 100%; min-width: 8%">
                                                                   <%# Eval("HealthPercentage").ToString() %>%
                                                               </div>
                                                           </div>
                                                            
                                                        </td>

                                                        <td>
                                                            <asp:LinkButton runat="server" CommandArgument='<%# Eval("SparePartUsage.SparePartItemSerialNumber") %>' CssClass="text-primary" CausesValidation="False" OnClick="btnChooseRemovedSparePart_OnClick">Choose</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>
                                        <!-- /.box-body -->
                                    </div>
                                </div>
                                <!-- /.ConsumableBatchUsage model selection-->
                                <!-- Selected ConsumableBatchUsage items-->
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">Spare Parts Removed</h3>

                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                    <i class="fa fa-minus"></i>
                                                </button>
                                            </div>
                                            <!-- /.box-tools -->
                                        </div>
                                        <!-- /.box-header -->
                                        <div class="box-body">
                                            <asp:ListView ID="lvSparePartRemovals" runat="server" ItemPlaceholderID="SparePartsRemovedItemPlaceHolder">
                                                <EmptyDataTemplate>
                                                    No Spare Parts Removed
                                                </EmptyDataTemplate>
                                                <LayoutTemplate>
                                                    <table id="SparePartsRemovedItemsTable" class="table table-hover">
                                                        <thead>
                                                            <tr>
                                                                <th>Serial Number</th>                                                                
                                                                <th>Model Number</th>
                                                                <th>Installed in Service</th>
                                                                <th>Installation Date</th>
                                                                <th>Actions</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:PlaceHolder runat="server" ID="SparePartsRemovedItemPlaceHolder"></asp:PlaceHolder>
                                                        </tbody>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <a href='../SparePartItemProfile.aspx?serialnumber=<%# Eval("SparePartItemSerialNumber").ToString()%>' target="_blank">
                                                                <%# Eval("SparePartItemSerialNumber").ToString() %>
                                                            </a>
                                                        </td>
                                                        <td>
                                                            <a href='../SparePartProfile.aspx?modelNumber=<%# Eval("SparePartItem.SparePartModelNumber").ToString() %>' target="_blank">
                                                                <%# Eval("SparePartItem.SparePartModelNumber").ToString() %>
                                                            </a>
                                                        </td>
                                                       
                                                        <td>
                                                            <a href='../ServiceProfile.aspx?id=<%# Eval("Service.Id").ToString() %>' target="_blank">
                                                                <%# Eval("Service.Id").ToString() %>
                                                            </a>
                                                        </td>
                                                        <td>
                                                            <%# Convert.ToDateTime(Eval("Service.Date")).ToShortDateString()%>
                                                        </td>

                                                        <td>
                                                            <asp:LinkButton runat="server" CommandArgument='<%# Eval("SparePartItemSerialNumber") %>' CssClass="text-primary" CausesValidation="False" OnClick="btnRemoveSparePartRemoval_OnClick">Remove</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>
                                        <!-- /.box-body -->
                                    </div>


                                </div>
                                <!-- /.selected ConsumableBatchUsage items-->

                            </div>
                            <!-- /.box-body -->
                        </div>
                    </div>
                </div>

                <!--/.Removable spare part usages-->
                <!-- Removable consumable usages -->
                <div class="col-md-12">
                    <div class="row">
                        <div class="box box-primary box-solid">
                            <div class="box-header with-border">
                                <h3 class="box-title">Consumables Removed From Equipment During Service</h3>

                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                                <!-- /.box-tools -->
                            </div>
                            <!-- /.box-header -->
                            <div class="box-body">
                                <div runat="server" visible="False" id="DivAlertConsumableRemovalSuccess" class="alert alert-success alert-dismissible">
                                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>
                                    <h4><i class="icon fa fa-check"></i>Operation Successful!</h4>
                                    <asp:Label ID="lblConsumableRemovalSuccess" runat="server" Text=""></asp:Label>
                                </div>
                                <div runat="server" visible="False" id="DivAlertConsumableRemovalFail" class="alert alert-danger alert-dismissible">
                                    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>
                                    <h4><i class="icon fa fa-ban"></i>Operation Failed!</h4>
                                    <asp:Label ID="lblConsumableRemovalFail" runat="server" Text=""></asp:Label>
                                </div>
                                <!-- ConsumableBatchUsage input fields-->
                                
                                <!-- /.ConsumableBatchUsage input fields -->
                                <!-- ConsumableBatchUsage model selection-->
                                <div class="col-md-12">
                                    <div class="box box-primary ">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">View and search removable consumable usages</h3>

                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                    <i class="fa fa-minus"></i>
                                                </button>
                                            </div>
                                            <!-- /.box-tools -->
                                        </div>
                                        <!-- /.box-header -->
                                        <!-- form start -->

                                        <div class="box-body">
                                            <asp:ListView OnDataBound="runJSript" ID="lvRemovableConsumableUsages" runat="server" ItemPlaceholderID="RemovableConsumableItemPlaceHolder">
                                                <EmptyDataTemplate>
                                                    No Removable Consumable Usages Found
                                                </EmptyDataTemplate>
                                                <LayoutTemplate>
                                                    <table id="RemovableConsumableItemsTable" class="table table-hover">
                                                        <thead>
                                                            <tr>
                                                                <th>Shipment PO Number</th>                                                                
                                                                <th>Model Number</th>
                                                                <th>Installed in Service</th>
                                                                <th>Installation Date</th>
                                                                <th>Expiry Date</th>
                                                                <th>Health (%)</th>
                                                                <th>Choose Consumable Removed</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:PlaceHolder runat="server" ID="RemovableConsumableItemPlaceHolder"></asp:PlaceHolder>
                                                        </tbody>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <a href='../ShipmentProfile.aspx?ponumber=<%# Eval("ConsumableBatchUsage.ConsumbaleBatchShipmentPONumber").ToString() %>' target="_blank">
                                                                <%# Eval("ConsumableBatchUsage.ConsumbaleBatchShipmentPONumber").ToString() %>
                                                            </a>
                                                        </td>
                                                        <td>
                                                            <a href='../ConsumableProfile.aspx?modelnumber=<%# Eval("ConsumableBatchUsage.ConsumableBatchModelNumber").ToString() %>' target="_blank">
                                                                <%# Eval("ConsumableBatchUsage.ConsumableBatchModelNumber").ToString() %>
                                                            </a>
                                                        </td>
                                                       
                                                        <td>
                                                            <a href='../ServiceProfile.aspx?id=<%# Eval("ConsumableBatchUsage.Service.Id").ToString()%>' target="_blank">
                                                                <%# Eval("ConsumableBatchUsage.Service.Id").ToString() %>
                                                            </a>
                                                        </td>
                                                        <td>
                                                            <%# Convert.ToDateTime(Eval("ConsumableBatchUsage.Service.Date")).ToShortDateString()%>
                                                        </td>
                                                        <td>
                                                            <%#Convert.ToDateTime(Eval("ExpiryDate")).ToShortDateString()%>
                                                        </td>
                                                        <td style="min-width: 10vw">
                                                            <span style="display: none"><%# Eval("HealthPercentage").ToString() %></span>
                                                           <div class="progress">
                                                               <div class="progress-bar progress-bar-info progress-bar-striped active" role="progressbar"
                                                                    aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" data-healthpercent="<%# Eval("HealthPercentage").ToString() %>" style="width: 100%; min-width: 8%">
                                                                   <%# Eval("HealthPercentage").ToString() %>%
                                                               </div>
                                                           </div>
                                                            
                                                        </td>

                                                        <td>
                                                            <asp:LinkButton runat="server" CommandArgument='<%# Eval("ConsumableBatchUsage.ConsumbaleBatchShipmentPONumber")+"|"+ Eval("ConsumableBatchUsage.ConsumableBatchModelNumber")+"|"+ Eval("ConsumableBatchUsage.Service.Id") %>' CssClass="text-primary" CausesValidation="False" OnClick="btnChooseRemovedConsumable_OnClick">Choose</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>
                                        <!-- /.box-body -->
                                    </div>
                                </div>
                                <!-- /.ConsumableBatchUsage model selection-->
                                <!-- Selected ConsumableBatchUsage items-->
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">Consumables Removed</h3>

                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                    <i class="fa fa-minus"></i>
                                                </button>
                                            </div>
                                            <!-- /.box-tools -->
                                        </div>
                                        <!-- /.box-header -->
                                        <div class="box-body">
                                            <asp:ListView ID="lvConsumableRemovals" runat="server" ItemPlaceholderID="ConsumablesRemovedItemPlaceHolder">
                                                <EmptyDataTemplate>
                                                    No Consumables Removed
                                                </EmptyDataTemplate>
                                                <LayoutTemplate>
                                                    <table id="ConsumablesRemovedItemsTable" class="table table-hover">
                                                        <thead>
                                                            <tr>
                                                                <th>Shipment PO Number</th>                                                                
                                                                <th>Model Number</th>
                                                                <th>Installed in Service</th>
                                                                <th>Installation Date</th>
                                                                <th>Actions</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:PlaceHolder runat="server" ID="ConsumablesRemovedItemPlaceHolder"></asp:PlaceHolder>
                                                        </tbody>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <a href='../ShipmentProfile.aspx?ponumber=<%# Eval("ConsumbaleBatchShipmentPONumber").ToString() %>' target="_blank">
                                                                <%# Eval("ConsumbaleBatchShipmentPONumber").ToString() %>
                                                            </a>
                                                        </td>
                                                        <td>
                                                            <a href='../ConsumableProfile.aspx?modelnumber=<%# Eval("ConsumableBatchModelNumber").ToString() %>' target="_blank">
                                                                <%# Eval("ConsumableBatchModelNumber").ToString() %>
                                                            </a>
                                                        </td>
                                                       
                                                        <td>
                                                            <a href='../ServiceProfile.aspx?id=<%# Eval("Service.Id").ToString() %>' target="_blank">
                                                                <%# Eval("Service.Id").ToString() %>
                                                            </a>
                                                        </td>
                                                        <td>
                                                            <%# Convert.ToDateTime(Eval("Service.Date")).ToShortDateString()%>
                                                        </td>

                                                        <td>
                                                            <asp:LinkButton runat="server" CommandArgument='<%# Eval("ConsumbaleBatchShipmentPONumber")+"|"+ Eval("ConsumableBatchModelNumber")+"|"+ Eval("Service.Id") %>' CssClass="text-primary" CausesValidation="False" OnClick="btnRemoveConsumableRemoval_OnClick">Remove</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>
                                        <!-- /.box-body -->
                                    </div>


                                </div>
                                <!-- /.selected ConsumableBatchUsage items-->

                            </div>
                            <!-- /.box-body -->
                        </div>
                    </div>
                </div>   
                <!-- /.Removable consumable usages -->
            </div>
            

            <!-- /.box-body -->
            <div class="box-footer">
                <asp:Button ID="btnSubmit" CssClass="btn btn-primary" OnClick="btnSubmit_OnClick" runat="server" ValidationGroup="vgServiceDetails" Text="Add Service" />
                <asp:Button ID="btnCancelUpdate" CssClass="btn btn-danger" CausesValidation="False" OnClick="btnCancelUpdate_OnClick" runat="server" Visible="False" Text="Cancel Update" />
            </div>
        </div>
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">View & Search Services
                </h3>
            </div>
            <!-- /.box-header -->
            <!-- form start -->

            <div class="box-body">
                <asp:ListView ID="lvServices" runat="server" ItemPlaceholderID="serviceItemPlaceHolder">
                    <EmptyDataTemplate>
                        No Services Added
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="ServicesTable" class="table table-hover">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Date</th>
                                    <th>Technician</th>
                                    <th>Equipment Serial</th>
                                    <th>Equipment Item</th>
                                    <th>Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder runat="server" ID="serviceItemPlaceHolder"></asp:PlaceHolder>
                            </tbody>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <a href='../ServiceProfile.aspx?id=<%# Eval("Id").ToString() %>' target="_blank">
                                    <%# Eval("Id").ToString() %>
                                </a>
                            </td>
                            <td>
                                <%# Convert.ToDateTime(Eval("Date")).ToShortDateString()%>
                            </td>
                            <td>
                                <a href='../TechnicianProfile.aspx?id=<%# Eval("TechnicianId").ToString() %>' target="_blank">
                                    <%# Eval("Technician.Name").ToString() %>
                                </a>
                                
                            </td>
                            <td>
                                <a href='../EquipmentItemProfile.aspx?serialnumber=<%# Eval("InstalledEquipmentSerialNumber").ToString() %>' target="_blank">
                                    <%# Eval("InstalledEquipmentSerialNumber").ToString() %>
                                </a>
                            </td>
                            <td>
                                <a href='../EquipmentProfile.aspx?modelnumber=<%# Eval("InstalledEquipmentSerialNumber").ToString() %>' target="_blank">
                                    <%# Eval("EquipmentInstallation.EquipmentItem.EquipmentModelNumber").ToString() %>
                                </a>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" CommandArgument='<%# Eval("Id") %>' CssClass="text-danger" CausesValidation="False" OnClick="btnEditService_OnClick">Edit</asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            <!-- /.box-body -->
            <div class="box-footer">
            </div>
        </div>
    </div>

    <%-- <div class="modal modal-danger fade in" id="removals-modal-danger" style="display: block; padding-right: 17px;"> --%>
    <%--     <div class="modal-dialog"> --%>
    <%--         <div class="modal-content"> --%>
    <%--             <div class="modal-header"> --%>
    <%--                  --%>
    <%--                 <asp:Button ID="btnRemovalModalClose2" data-dismiss="modal" runat="server" Text="x" OnClick="btnCancelDateChange_OnClick"/>                         --%>
    <%-- --%>
    <%--                 <h4 class="modal-title">Warning !</h4> --%>
    <%--             </div> --%>
    <%--             <div class="modal-body"> --%>
    <%--                 <p> --%>
    <%--                     Changing the service date will result in deletion of removal records (Consumables and Spare Parts)  --%>
    <%--                     if the date of the service is changed to a date which is prior to the installation date of the consumables and spare parts --%>
    <%--                 </p> --%>
    <%--             </div> --%>
    <%--             <div class="modal-footer"> --%>
    <%--                 <asp:Button ID="btnRemovalModalClose" CssClass="btn btn-outline pull-left" data-dismiss="modal" runat="server" Text="Close" OnClick="btnCancelDateChange_OnClick"/> --%>
    <%--                 <asp:Button ID="btnRemovalModalChangeDate" CssClass="btn btn-outline" runat="server" Text="Change Date" OnClick="btnConfirmDateChange_OnClick"/> --%>
    <%-- --%>
    <%--             </div> --%>
    <%--         </div> --%>
    <%--         <!-- /.modal-content --> --%>
    <%--     </div> --%>
    <%--     <!-- /.modal-dialog --> --%>
    <%-- </div> --%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContentPlaceHolder" runat="server">
    <script src="http://digitalbush.com/wp-content/uploads/2013/01/jquery.maskedinput-1.3.1.min_.js"></script>

    <script src="http://code.jquery.com/jquery-1.10.2.js"></script>
    <script src="http://code.jquery.com/ui/1.11.0/jquery-ui.js"></script>

    <!-- DataTables -->
    <script src="/bower_components/datatables.net/js/jquery.dataTables.min.js"></script>
    <script src="/bower_components/datatables.net-bs/js/dataTables.bootstrap.min.js"></script>

    <script>
        var t = $('#ServicesTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script>
    <script>
        var t = $('#InstalledEquipmentsTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script>
    <script>
        var t = $('#TechniciansTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script>
    <script>
        var t = $('#SparePartItemsTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script>
    <script>
        var t = $('#SparePartUsageItemsTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script>
    <script>
        var t = $('#ConsumableItemsTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script>
    <script>
        var t = $('#RemovableSparePartItemsTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script>
    <script>
        $(function () {
            $("#MainContentPlaceHolder_txtServiceDate").datepicker({
                changeMonth: true,
                changeYear: true
            });
        });
    </script>
    <script>
        function styleHealthProgressBar(element, healthPercentage) {
            
            $(element.id).attr('aria-valuenow', healthPercentage).css('width', healthPercentage);
        }

    </script>
   
    
</asp:Content>
