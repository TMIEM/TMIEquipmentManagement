<%@ Page Title="Shipment Management" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ShipmentManagement.aspx.cs" Inherits="TMIEquipmentManagement.ShipmentManagement" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- DataTables -->
    <link rel="stylesheet" href="./bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css">
    <link rel="stylesheet" href="./dist/css/base/jquery-ui-1.9.2.custom.min.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContentPlaceHolder" runat="server">
    <section class="content-header">
        <h1>Shipment Management
            <small>View, Add and Update Shipments</small>
        </h1>
        <ol class="breadcrumb">
            <li>
                <a href="index.aspx"><i class="fa fa-dashboard"></i>Dashboard</a>
            </li>
            <li class="active"><i class="fa fa-user"></i>Shipment Management</li>
        </ol>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="col-md-12">
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label ID="lblAddEditHeading" runat="server" Text="Add Shipment"></asp:Label>
                </h3>
            </div>
            <!-- /.box-header -->
            <!-- form start -->
            <div class="box-body">
                <asp:HiddenField ID="hfEditingShipmentPoNumber" Value="" runat="server" />
                <div class="col-md-12">
                    <div class="row">
                        <div class="box box-primary box-solid">
                            <div class="box-header with-border">
                                <h3 class="box-title">Shipment Details</h3>

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
                                    <div class="row">
                                        <div class="form-group col-md-12">
                                            <label>
                                                PO Number<br />

                                                <asp:Label ID="lblPoNumberWarning" runat="server" ForeColor="Orange" Text="Please enter with care, PO number cannot be updated once shipment is added to the system."></asp:Label>
                                            </label>
                                            <asp:TextBox runat="server" CssClass="form-control"
                                                ID="txtShipmentPoNumber"
                                                MaxLength="49"
                                                placeholder="Enter Shipment PO Number">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                ID="rfvShipmentPoNumber"
                                                ValidationGroup="vgShipmentDetails"
                                                ControlToValidate="txtShipmentPoNumber"
                                                ErrorMessage="Shipment model number is required">
                                            </asp:RequiredFieldValidator>
                                            <asp:Label ID="lblDuplicatePoNumberAlert" runat="server" Visible="False" CssClass="text-danger"
                                                Text="A shipment with the PO number entered exists in the system, please enter a different PO number or update the existing shipment">
                                            </asp:Label>

                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group col-md-12">
                                            <label>Supplier (choose from suppliers table)</label>
                                            <asp:TextBox runat="server" CssClass="form-control"
                                                ID="txtShipmentSupplierId"
                                                Enabled="False"
                                                MaxLength="29"
                                                placeholder="Choose shipment supplier ID">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                ID="rfvShipmentSupplierId"
                                                ValidationGroup="vgShipmentDetails"
                                                ControlToValidate="txtShipmentSupplierId"
                                                ErrorMessage="Shipment supplier ID is required">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group col-md-12">
                                            <label>Supplier Invoice Number</label>
                                            <asp:TextBox runat="server" CssClass="form-control"
                                                ID="txtShipmentSuppplierInvoiceNumber"
                                                MaxLength="254"
                                                placeholder="Enter Supplier Invoice Number">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                ID="rfvShipmentInvoiceNumber"
                                                ValidationGroup="vgShipmentDetails"
                                                ControlToValidate="txtShipmentSuppplierInvoiceNumber"
                                                ErrorMessage="Shipment supplier invoice number is required">
                                            </asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="form-group col-md-12">
                                            <label>Date of Arrival</label>
                                            <asp:TextBox runat="server" CssClass="form-control"
                                                ID="txtShipmentDate"
                                                MaxLength="29"
                                                placeholder="Enter Shipment Date">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                ID="rfvShipmentDate"
                                                ValidationGroup="vgShipmentDetails"
                                                ControlToValidate="txtShipmentDate"
                                                ErrorMessage="Shipment arrival date is required">
                                            </asp:RequiredFieldValidator>
                                            <asp:CompareValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                ID="cvShipmentDate"
                                                ValidationGroup="vgShipmentDetails"
                                                ControlToValidate="txtShipmentDate"
                                                Operator="DataTypeCheck"
                                                Type="Date"
                                                ErrorMessage="Please enter a valid arrival date">
                                            </asp:CompareValidator>
                                        </div>
                                    </div>

                                </div>
                                <!-- /.left side input fields-->
                                <!-- right side input fields-->
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="box box-primary box-solid">
                                            <div class="box-header with-border">
                                                <h3 class="box-title">View and search suppliers</h3>

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
                                                <asp:ListView ID="lvShipmentSuppliers" runat="server" ItemPlaceholderID="shipmentSupplierItemPlaceHolder">
                                                    <EmptyDataTemplate>
                                                        No Suppliers Found
                                                    </EmptyDataTemplate>
                                                    <LayoutTemplate>
                                                        <table id="SuppliersTable" class="table table-hover">
                                                            <thead>
                                                                <tr>
                                                                    <th>ID</th>
                                                                    <th>Name</th>
                                                                    <th>Address</th>
                                                                    <th>Select Supplier</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <asp:PlaceHolder runat="server" ID="shipmentSupplierItemPlaceHolder"></asp:PlaceHolder>
                                                            </tbody>
                                                        </table>
                                                    </LayoutTemplate>
                                                    <ItemTemplate>
                                                        <tr>
                                                            <td>
                                                                <%# Eval("Id").ToString() %>
                                                            </td>
                                                            <td>
                                                                <a href='../SupplierProfile.aspx?id=<%# Eval("Id").ToString() %>' target="_blank">
                                                                    <%# Eval("Name").ToString() %>
                                                                </a>
                                                            </td>
                                                            <td>
                                                                <%# Eval("Address").ToString() %>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton runat="server" CommandArgument='<%# Eval("Id") %>' CssClass="text-primary" CausesValidation="False" OnClick="btnChooseSupplier_OnClick">Choose</asp:LinkButton>
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

                <!-- Shipment equipments -->
                <div class="col-md-12">
                    <div class="row">
                        <div class="box box-primary box-solid">
                            <div class="box-header with-border">
                                <h3 class="box-title">Equipments received</h3>

                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                                <!-- /.box-tools -->
                            </div>
                            <!-- /.box-header -->
                            <div class="box-body">
                                <!-- Equipment input fields-->
                                <div class="col-md-6">
                                    <div class="form-group col-md-12">
                                        <label>
                                            Equipment Model Number (choose from table)<br />
                                        </label>
                                        <asp:TextBox runat="server" CssClass="form-control"
                                            ID="txtEquipmentModelNumber"
                                            Enabled="False"
                                            MaxLength="49"
                                            placeholder="Select Equipment Model Number">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                            ID="rfvEquipmentModelNumber"
                                            ValidationGroup="vgEquipmentItem"
                                            ControlToValidate="txtEquipmentModelNumber"
                                            ErrorMessage="Equipment model number is required">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-md-12">
                                        <label>
                                            Price (LKR)<br />
                                        </label>
                                        <asp:TextBox runat="server" CssClass="form-control"
                                            ID="txtEquipmentPrice"
                                            min="1"
                                            MaxLength="49"
                                            placeholder="Enter Price (LKR)">
                                        </asp:TextBox>
                                        <asp:RangeValidator 
                                            ID="rvEquipmentPrice" ControlToValidate="txtEquipmentPrice" 
                                                            ValidationGroup="vgEquipmentItem" CssClass="text-danger" runat="server" 
                                                            ErrorMessage="Please enter a price greater than 0"
                                            MinimumValue="1" MaximumValue="9999999999">

                                        </asp:RangeValidator>
                                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                            ID="rfvEquipmentPrice"
                                            ValidationGroup="vgEquipmentItem"
                                            ControlToValidate="txtEquipmentPrice"
                                            ErrorMessage="Equipment price is required">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-md-12">
                                        <label>
                                            Serial Number
                                            <br />
                                            <asp:Label ID="lblEquipmentSerialWarning" runat="server" ForeColor="Orange"
                                                Text="Please enter with care, serial number cannot be updated once item is added to the system.">
                                            </asp:Label>
                                        </label>
                                        <asp:TextBox runat="server" CssClass="form-control"
                                            ID="txtEquipmentSerialNumber"
                                            MaxLength="49"
                                            placeholder="Enter Serial Number">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                            ID="rfvEquipmentSerialNumber"
                                            ValidationGroup="vgEquipmentItem"
                                            ControlToValidate="txtEquipmentSerialNumber"
                                            ErrorMessage="Equipment serial number is required">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-md-12">
                                        <asp:HiddenField ID="hfEditingEquipmentItemserial" Value="" runat="server" />
                                        <asp:Button ID="btnAddEquipment" CssClass="btn btn-primary" OnClick="btnSubmitEquipment_OnClick" runat="server" ValidationGroup="vgEquipmentItem" Text="Add Equipment" />
                                        <asp:Button ID="btnCancelEquipmentUpdate" CssClass="btn btn-warning" CausesValidation="False" OnClick="btnCancelEquipmentUpdate_OnClick" runat="server" Visible="False" Text="Cancel Update" />
                                        <asp:Button ID="btnDeleteEquipmentItem" CssClass="btn btn-danger" OnClick="btnDeleteEquipmentItem_OnClick" runat="server" ValidationGroup="vgEquipmentItem" Text="Remove Equipment" Visible="False" />
                                        <br />
                                        <br />
                                        <div runat="server" visible="False" id="DivAlertEquipmentItemSuccess" class="alert alert-success alert-dismissible">
                                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>
                                            <h4><i class="icon fa fa-check"></i>Operation Successful!</h4>
                                            <asp:Label ID="lblEquipmentItemSuccess" runat="server" Text=""></asp:Label>
                                        </div>
                                        <div runat="server" visible="False" id="DivAlertEquipmentItemFailure" class="alert alert-danger alert-dismissible">
                                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>
                                            <h4><i class="icon fa fa-ban"></i>Operation Failed!</h4>
                                            <asp:Label ID="lblEquipmentItemFailure" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <!-- /.Equipment input fields -->
                                <!-- Equipment model selection-->
                                <div class="col-md-6">
                                    <div class="box box-primary box-solid">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">View and search equipments</h3>

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
                                            <asp:ListView ID="lvEquipments" runat="server" ItemPlaceholderID="equipmentItemPlaceHolder">
                                                <EmptyDataTemplate>
                                                    No Equipments Added
                                                </EmptyDataTemplate>
                                                <LayoutTemplate>
                                                    <table id="EquipmentsTable" class="table table-hover">
                                                        <thead>
                                                            <tr>
                                                                <th>Model Number</th>
                                                                <th>Make</th>
                                                                <th>Version</th>
                                                                <th>Software Version</th>
                                                                <th>Select Model</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:PlaceHolder runat="server" ID="equipmentItemPlaceHolder"></asp:PlaceHolder>
                                                        </tbody>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <a href='../EquipmentProfile.aspx?modelnumber=<%# Eval("ModelNumber").ToString() %>' target="_blank">
                                                                <%# Eval("ModelNumber").ToString() %>
                                                            </a>
                                                        </td>
                                                        <td>
                                                            <%# Eval("Make").ToString() %>
                                                        </td>
                                                        <td>
                                                            <%# Eval("Version").ToString() %>
                                                        </td>
                                                        <td>
                                                            <%# Eval("SoftwareVersion").ToString() %>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton runat="server" CommandArgument='<%# Eval("ModelNumber") %>' CssClass="text-primary" CausesValidation="False" OnClick="btnChooseEquipmentModel_OnClick">Choose</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>
                                        <!-- /.box-body -->
                                    </div>
                                </div>
                                <!-- /.Equipment model selection-->
                                <!-- Selected equipment items-->
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">Equipment Items</h3>

                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                    <i class="fa fa-minus"></i>
                                                </button>
                                            </div>
                                            <!-- /.box-tools -->
                                        </div>
                                        <!-- /.box-header -->
                                        <div class="box-body">
                                            <asp:ListView ID="lvEquipmentItems" runat="server" ItemPlaceholderID="equipmentItemItemPlaceHolder">
                                                <EmptyDataTemplate>
                                                    No Equipments Added
                                                </EmptyDataTemplate>
                                                <LayoutTemplate>
                                                    <table id="EquipmentItemsTable" class="table table-hover">
                                                        <thead>
                                                            <tr>
                                                                <th>Model Number</th>
                                                                <th>Serial Number</th>
                                                                <th>Price</th>
                                                                <th>Actions</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:PlaceHolder runat="server" ID="equipmentItemItemPlaceHolder"></asp:PlaceHolder>
                                                        </tbody>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <a href='../EquipmentProfile.aspx?modelnumber=<%# Eval("EquipmentModelNumber").ToString() %>' target="_blank">
                                                                <%# Eval("EquipmentModelNumber").ToString() %>
                                                            </a>
                                                        </td>
                                                        <td>
                                                            <%# Eval("SerialNumber").ToString() %>
                                                        </td>
                                                        <td>
                                                            <%# Eval("Price", "{0:n}")%>
                                                        </td>

                                                        <td>
                                                            <asp:LinkButton runat="server" CommandArgument='<%# Eval("SerialNumber") %>' CssClass="text-primary" CausesValidation="False" OnClick="btnEditEquipmentItem_OnClick">Edit</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>
                                        <!-- /.box-body -->
                                    </div>


                                </div>
                                <!-- /.selected equipment items-->

                            </div>
                            <!-- /.box-body -->
                        </div>
                    </div>
                </div>
                <!-- /.Shipment equipments -->
                <div class="col-md-12">
                    <div class="row">
                        <div class="box box-primary box-solid">
                            <div class="box-header with-border">
                                <h3 class="box-title">SpareParts received</h3>

                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                                <!-- /.box-tools -->
                            </div>
                            <!-- /.box-header -->
                            <div class="box-body">
                                <!-- SparePart input fields-->
                                <div class="col-md-6">
                                    <div class="form-group col-md-12">
                                        <label>
                                            SparePart Model Number (choose from table)<br />
                                        </label>
                                        <asp:TextBox runat="server" CssClass="form-control"
                                            ID="txtSparePartModelNumber"
                                            Enabled="False"
                                            MaxLength="49"
                                            placeholder="Select SparePart Model Number">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                            ID="rfvSparePartModelNumber"
                                            ValidationGroup="vgSparePartItem"
                                            ControlToValidate="txtSparePartModelNumber"
                                            ErrorMessage="SparePart model number is required">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-md-12">
                                        <label>
                                            Price (LKR)<br />
                                        </label>
                                        <asp:TextBox runat="server" CssClass="form-control"
                                            ID="txtSparePartPrice"
                                            min="1"
                                            MaxLength="49"
                                            placeholder="Enter Price (LKR)">
                                        </asp:TextBox>
                                        <asp:RangeValidator 
                                            ID="rvSparePartPrice" ControlToValidate="txtSparePartPrice" 
                                            ValidationGroup="vgSparePartItem" CssClass="text-danger" runat="server" 
                                            ErrorMessage="Please enter a price greater than 0"
                                            MinimumValue="1" MaximumValue="9999999999">

                                        </asp:RangeValidator>
                                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                            ID="rfvSparePartPrice"
                                            ValidationGroup="vgSparePartItem"
                                            ControlToValidate="txtSparePartPrice"
                                            ErrorMessage="SparePart price is required">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-md-12">
                                        <label>
                                            Serial Number
            <br />
                                            <asp:Label ID="lblSparePartserialWarning" runat="server" ForeColor="Orange"
                                                Text="Please enter with care, serial number cannot be updated once item is added to the system.">
                                            </asp:Label>
                                        </label>
                                        <asp:TextBox runat="server" CssClass="form-control"
                                            ID="txtSparePartSerialNumber"
                                            MaxLength="49"
                                            placeholder="Enter Serial Number">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                            ID="rfvSparePartserialNumber"
                                            ValidationGroup="vgSparePartItem"
                                            ControlToValidate="txtSparePartSerialNumber"
                                            ErrorMessage="SparePart serial number is required">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-md-12">
                                        <asp:HiddenField ID="hfEditingSparePartItemserial" Value="" runat="server" />
                                        <asp:Button ID="btnAddSparePart" CssClass="btn btn-primary" OnClick="btnSubmitSparePart_OnClick" runat="server" ValidationGroup="vgSparePartItem" Text="Add SparePart" />
                                        <asp:Button ID="btnCancelSparePartUpdate" CssClass="btn btn-warning" CausesValidation="False" OnClick="btnCancelSparePartUpdate_OnClick" runat="server" Visible="False" Text="Cancel Update" />
                                        <asp:Button ID="btnDeleteSparePartItem" CssClass="btn btn-danger" OnClick="btnDeleteSparePartItem_OnClick" runat="server" ValidationGroup="vgSparePartItem" Text="Remove SparePart" Visible="False" />
                                        <br />
                                        <br />
                                        <div runat="server" visible="False" id="DivAlertSparePartItemSuccess" class="alert alert-success alert-dismissible">
                                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>
                                            <h4><i class="icon fa fa-check"></i>Operation Successful!</h4>
                                            <asp:Label ID="lblSparePartItemSuccess" runat="server" Text=""></asp:Label>
                                        </div>
                                        <div runat="server" visible="False" id="DivAlertSparePartItemFailure" class="alert alert-danger alert-dismissible">
                                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>
                                            <h4><i class="icon fa fa-ban"></i>Operation Failed!</h4>
                                            <asp:Label ID="lblSparePartItemFailure" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <!-- /.SparePart input fields -->
                                <!-- SparePart model selection-->
                                <div class="col-md-6">
                                    <div class="box box-primary box-solid">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">View and search SpareParts</h3>

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
                                            <asp:ListView ID="lvSpareParts" runat="server" ItemPlaceholderID="SparePartItemPlaceHolder">
                                                <EmptyDataTemplate>
                                                    No SpareParts Added
                                                </EmptyDataTemplate>
                                                <LayoutTemplate>
                                                    <table id="SparePartsTable" class="table table-hover">
                                                        <thead>
                                                            <tr>
                                                                <th>Model Number</th>
                                                                <th>Make</th>
                                                                <th>Version</th>
                                                                <th>Software Version</th>
                                                                <th>Select Model</th>
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
                                                            <a href='../SparePartProfile.aspx?modelnumber=<%# Eval("ModelNumber").ToString() %>' target="_blank">
                                                                <%# Eval("ModelNumber").ToString() %>
                                                            </a>
                                                        </td>
                                                        <td>
                                                            <%# Eval("Make").ToString() %>
                                                        </td>
                                                        <td>
                                                            <%# Eval("Description").ToString() %>
                                                        </td>
                                                        <td>
                                                            <%# Eval("LifeSpanMonths").ToString() %>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton runat="server" CommandArgument='<%# Eval("ModelNumber") %>' CssClass="text-primary" CausesValidation="False" OnClick="btnChooseSparePartModel_OnClick">Choose</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>
                                        <!-- /.box-body -->
                                    </div>
                                </div>
                                <!-- /.SparePart model selection-->
                                <!-- Selected SparePart items-->
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">SparePart Items</h3>

                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                    <i class="fa fa-minus"></i>
                                                </button>
                                            </div>
                                            <!-- /.box-tools -->
                                        </div>
                                        <!-- /.box-header -->
                                        <div class="box-body">
                                            <asp:ListView ID="lvSparePartItems" runat="server" ItemPlaceholderID="SparePartItemItemPlaceHolder">
                                                <EmptyDataTemplate>
                                                    No SpareParts Added
                                                </EmptyDataTemplate>
                                                <LayoutTemplate>
                                                    <table id="SparePartItemsTable" class="table table-hover">
                                                        <thead>
                                                            <tr>
                                                                <th>Model Number</th>
                                                                <th>Serial Number</th>
                                                                <th>Price</th>
                                                                <th>Actions</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:PlaceHolder runat="server" ID="SparePartItemItemPlaceHolder"></asp:PlaceHolder>
                                                        </tbody>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <a href='../SparePartProfile.aspx?modelnumber=<%# Eval("SparePartModelNumber").ToString() %>' target="_blank">
                                                                <%# Eval("SparePartModelNumber").ToString() %>
                                                            </a>
                                                        </td>
                                                        <td>
                                                            <%# Eval("SerialNumber").ToString() %>
                                                        </td>
                                                        <td>
                                                            <%# Eval("Price", "{0:n}")%>
                                                        </td>

                                                        <td>
                                                            <asp:LinkButton runat="server" CommandArgument='<%# Eval("SerialNumber") %>' CssClass="text-primary" CausesValidation="False" OnClick="btnEditSparePartItem_OnClick">Edit</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>
                                        <!-- /.box-body -->
                                    </div>


                                </div>
                                <!-- /.selected SparePart items-->

                            </div>
                            <!-- /.box-body -->
                        </div>
                    </div>
                </div>
                <!-- /.End Shipment Spare par items -->
                <!--Consumables received-->
                <div class="col-md-12">
                    <div class="row">
                        <div class="box box-primary box-solid">
                            <div class="box-header with-border">
                                <h3 class="box-title">Consumables Batches received</h3>

                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                                <!-- /.box-tools -->
                            </div>
                            <!-- /.box-header -->
                            <div class="box-body">
                                <!-- ConsumableBatch input fields-->
                                <div class="col-md-6">
                                    <div class="form-group col-md-12">
                                        <label>
                                            Consumable Batch Model Number (choose from table)<br />
                                        </label>
                                        <asp:TextBox runat="server" CssClass="form-control"
                                            ID="txtConsumableBatchModelNumber"
                                            Enabled="False"
                                            MaxLength="49"
                                            placeholder="Select Consumable Batch Model Number">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                            ID="rfvConsumableBatchModelNumber"
                                            ValidationGroup="vgConsumableBatchItem"
                                            ControlToValidate="txtConsumableBatchModelNumber"
                                            ErrorMessage="Consumable Batch model number is required">
                                        </asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group col-md-12">
                                        <label>
                                            Quantity<br />
                                        </label>
                                        <asp:TextBox runat="server" CssClass="form-control"
                                            ID="txtConsumableBatchQuantity"
                                            TextMode="Number"
                                            min="1"
                                            MaxLength="49"
                                            placeholder="Enter Quantity">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                            ID="rfvConsumableBatchQuantity"
                                            ValidationGroup="vgConsumableBatchItem"
                                            ControlToValidate="txtConsumableBatchQuantity"
                                            ErrorMessage="Consumable Batch quantity is required">
                                        </asp:RequiredFieldValidator>
                                        <asp:RangeValidator runat="server"
                                            CssClass="text-danger" Display="Dynamic" runat="server"
                                            ValidationGroup="vgConsumableBatchItem"
                                            ControlToValidate="txtConsumableBatchQuantity"
                                            MinimumValue="1"
                                            MaximumValue="99999999"
                                            ErrorMessage="Please enter a quantity greater than 0">

                                        </asp:RangeValidator>
                                    </div>
                                    <div class="form-group col-md-12">
                                        <label>
                                            Price per unit<br />
                                        </label>
                                        <asp:TextBox runat="server" CssClass="form-control"
                                                     ID="txtConsumableBatchPrice"
                                                     min="1"
                                                     MaxLength="49"
                                                     placeholder="Enter Price">
                                        </asp:TextBox>
                                        <asp:RangeValidator 
                                            ID="rvConsumablePrice" ControlToValidate="txtConsumableBatchPrice" 
                                            ValidationGroup="vgConsumableBatchItem" CssClass="text-danger" runat="server" 
                                            ErrorMessage="Please enter a price greater than 0"
                                            MinimumValue="1" MaximumValue="9999999999">

                                        </asp:RangeValidator>
                                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                                    ID="RequiredFieldValidator1"
                                                                    ValidationGroup="vgConsumableBatchItem"
                                                                    ControlToValidate="txtConsumableBatchPrice"
                                                                    ErrorMessage="Consumable Batch price per unit is required">
                                        </asp:RequiredFieldValidator>
                                        <asp:RangeValidator runat="server"
                                                            CssClass="text-danger" Display="Dynamic" runat="server"
                                                            ValidationGroup="vgConsumableBatchItem"
                                                            ControlToValidate="txtConsumableBatchPrice"
                                                            MinimumValue="1"
                                                            MaximumValue="99999999"
                                                            ErrorMessage="Please enter a quantity greater than 0">

                                        </asp:RangeValidator>
                                    </div>
                                    <div class="form-group col-md-12">
                                        <asp:HiddenField ID="hfEditingConsumableBatchItemserial" Value="" runat="server" />
                                        <asp:Button ID="btnAddConsumableBatch" CssClass="btn btn-primary" OnClick="btnSubmitConsumableBatch_OnClick" runat="server" ValidationGroup="vgConsumableBatchItem" Text="Add Consumable Batch" />
                                        <asp:Button ID="btnCancelConsumableBatchUpdate" CssClass="btn btn-warning" CausesValidation="False" OnClick="btnCancelConsumableBatchUpdate_OnClick" runat="server" Visible="False" Text="Cancel Update" />
                                        <asp:Button ID="btnDeleteConsumableBatchItem" CssClass="btn btn-danger" OnClick="btnDeleteConsumableBatchItem_OnClick" runat="server" ValidationGroup="vgConsumableBatchItem" Text="Remove Consumable Batch" Visible="False" />
                                        <br />
                                        <br />
                                        <div runat="server" visible="False" id="DivAlertConsumableBatchItemSuccess" class="alert alert-success alert-dismissible">
                                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>
                                            <h4><i class="icon fa fa-check"></i>Operation Successful!</h4>
                                            <asp:Label ID="lblConsumableBatchItemSuccess" runat="server" Text=""></asp:Label>
                                        </div>
                                        <div runat="server" visible="False" id="DivAlertConsumableBatchItemFailure" class="alert alert-danger alert-dismissible">
                                            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>
                                            <h4><i class="icon fa fa-ban"></i>Operation Failed!</h4>
                                            <asp:Label ID="lblConsumableBatchItemFailure" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <!-- /.ConsumableBatch input fields -->
                                <!-- ConsumableBatch model selection-->
                                <div class="col-md-6">
                                    <div class="box box-primary box-solid">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">View and search Consumables</h3>

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
                                            <asp:ListView ID="lvConsumableModels" runat="server" ItemPlaceholderID="ConsumableItemPlaceHolder">
                                                <EmptyDataTemplate>
                                                    No Consumables Added
                                                </EmptyDataTemplate>
                                                <LayoutTemplate>
                                                    <table id="ConsumableBatchsTable" class="table table-hover">
                                                        <thead>
                                                            <tr>
                                                                <th>Model Number</th>
                                                                <th>Make</th>
                                                                <th>Description</th>
                                                                <th>Life Span (Days)</th>
                                                                <th>Select Model</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:PlaceHolder runat="server" ID="ConsumableItemPlaceHolder"></asp:PlaceHolder>
                                                        </tbody>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <a href='../ConsumableProfile.aspx?modelnumber=<%# Eval("ModelNumber").ToString() %>' target="_blank">
                                                                <%# Eval("ModelNumber").ToString() %>
                                                            </a>
                                                        </td>
                                                        <td>
                                                            <%# Eval("Make").ToString() %>
                                                        </td>
                                                        <td>
                                                            <%# Eval("Description").ToString() %>
                                                        </td>
                                                        <td>
                                                            <%# Eval("LifeSpanDays").ToString() %>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton runat="server" CommandArgument='<%# Eval("ModelNumber") %>' CssClass="text-primary" CausesValidation="False" OnClick="btnChooseConsumableBatchModel_OnClick">Choose</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>
                                        <!-- /.box-body -->
                                    </div>
                                </div>
                                <!-- /.ConsumableBatch model selection-->
                                <!-- Selected ConsumableBatch items-->
                                <div class="col-md-12">
                                    <div class="box box-primary">
                                        <div class="box-header with-border">
                                            <h3 class="box-title">ConsumableBatch Items</h3>

                                            <div class="box-tools pull-right">
                                                <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                                    <i class="fa fa-minus"></i>
                                                </button>
                                            </div>
                                            <!-- /.box-tools -->
                                        </div>
                                        <!-- /.box-header -->
                                        <div class="box-body">
                                            <asp:ListView ID="lvConsumableBatchItems" runat="server" ItemPlaceholderID="ConsumableBatchItemItemPlaceHolder">
                                                <EmptyDataTemplate>
                                                    No ConsumableBatchs Added
                                                </EmptyDataTemplate>
                                                <LayoutTemplate>
                                                    <table id="ConsumableBatchItemsTable" class="table table-hover">
                                                        <thead>
                                                            <tr>
                                                                <th>Model Number</th>
                                                                <th>Quantity</th>
                                                                <th>Price</th>
                                                                <th>Actions</th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:PlaceHolder runat="server" ID="ConsumableBatchItemItemPlaceHolder"></asp:PlaceHolder>
                                                        </tbody>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td>
                                                            <a href='../ConsumableProfile.aspx?modelnumber=<%# Eval("ConsumableModelNumber").ToString() %>' target="_blank">
                                                                <%# Eval("ConsumableModelNumber").ToString() %>
                                                            </a>
                                                        </td>
                                                        <td>
                                                            <%# Eval("Quantity").ToString() %>
                                                        </td>
                                                        <td>
                                                            <%# Eval("Price", "{0:n}").ToString() %>
                                                        </td>

                                                        <td>
                                                            <asp:LinkButton runat="server" CommandArgument='<%# Eval("ConsumableModelNumber") %>' CssClass="text-primary" CausesValidation="False" OnClick="btnEditConsumableBatchItem_OnClick">Edit</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </div>
                                        <!-- /.box-body -->
                                    </div>


                                </div>
                                <!-- /.selected ConsumableBatch items-->

                            </div>
                            <!-- /.box-body -->
                        </div>
                    </div>
                </div>
                
                <!--/.Consumables received-->
            </div>
            <!-- /.box-body -->
            <div class="box-footer">
                <asp:Button ID="btnSubmit" CssClass="btn btn-primary" OnClick="btnSubmit_OnClick" runat="server" ValidationGroup="vgShipmentDetails" Text="Add Shipment" />
                <asp:Button ID="btnCancelUpdate" CssClass="btn btn-danger" CausesValidation="False" OnClick="btnCancelUpdate_OnClick" runat="server" Visible="False" Text="Cancel Update" />
            </div>
        </div>
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">View & Search Shipments
                </h3>
            </div>
            <!-- /.box-header -->
            <!-- form start -->

            <div class="box-body">
                <asp:ListView ID="lvShipments" runat="server" ItemPlaceholderID="shipmentItemPlaceHolder">
                    <EmptyDataTemplate>
                        No Shipments Added
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="ShipmentsTable" class="table table-hover">
                            <thead>
                                <tr>
                                    <th>PO Number</th>
                                    <th>Supplier Invoice Number</th>
                                    <th>Date of Arrival</th>
                                    <th>Supplier</th>
                                    <th>Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder runat="server" ID="shipmentItemPlaceHolder"></asp:PlaceHolder>
                            </tbody>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <a href='../ShipmentProfile.aspx?ponumber=<%# Eval("PoNumber").ToString() %>' target="_blank">
                                    <%# Eval("PoNumber").ToString() %>
                                </a>
                            </td>
                            <td>
                                <%# Eval("SupplierInvoiceNumber").ToString() %>
                            </td>
                            <td>
                                <%# Convert.ToDateTime(Eval("DateOfArrival")).ToShortDateString() %>
                            </td>
                            <td>
                                <a href='../SupplierProfile.aspx?id=<%# Eval("SupplierId").ToString() %>' target="_blank">
                                    <%# Eval("SupplierId").ToString() %>
                                </a>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" CommandArgument='<%# Eval("PoNumber") %>' CssClass="text-danger" CausesValidation="False" OnClick="btnEditShipment_OnClick">Edit</asp:LinkButton>
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
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContentPlaceHolder" runat="server">
    <script src="http://digitalbush.com/wp-content/uploads/2013/01/jquery.maskedinput-1.3.1.min_.js"></script>
    
    <script src="http://code.jquery.com/jquery-1.10.2.js"></script>
    <script src="http://code.jquery.com/ui/1.11.0/jquery-ui.js"></script>

    <!-- DataTables -->
    <script src="/bower_components/datatables.net/js/jquery.dataTables.min.js"></script>
    <script src="/bower_components/datatables.net-bs/js/dataTables.bootstrap.min.js"></script>

    <script>
        var t = $('#ShipmentsTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script>
    <script>
        var t = $('#SuppliersTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script>
    <script>
        var t = $('#EquipmentsTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script>
<%--     <script> --%>
<%--         var t = $('#EquipmentItemsTable').DataTable({ --%>
<%--             "scrollX": true, --%>
<%--             "scrollCollapse": true, --%>
<%--             "fixedColumns": true, --%>
<%--             "autoWidth": false --%>
<%--         }); --%>
<%--         t.columns.adjust(); --%>
<%--     </script> --%>
<%--     <script> --%>
<%--         var t = $('#SparePartItemsTable').DataTable({ --%>
<%--             "scrollX": true, --%>
<%--             "scrollCollapse": true, --%>
<%--             "fixedColumns": true, --%>
<%--             "autoWidth": false --%>
<%--         }); --%>
<%--         t.columns.adjust(); --%>
<%--     </script>  --%>
    <script>
        var t = $('#SparePartsTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script> 
    <script>
        var t = $('#ConsumableBatchsTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script>
    <script>
        $(function () {
            $("#MainContentPlaceHolder_txtShipmentDate").datepicker({
                changeMonth: true,
                changeYear: true
            });
        });
    </script>

</asp:Content>
