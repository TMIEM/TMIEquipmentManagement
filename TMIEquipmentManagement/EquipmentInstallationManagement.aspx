<%@ Page Title="EquipmentInstallation Management" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="EquipmentInstallationManagement.aspx.cs" Inherits="TMIEquipmentManagement.EquipmentInstallationManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="./dist/css/base/jquery-ui-1.9.2.custom.min.css">
    <!-- DataTables -->
    <link rel="stylesheet" href="../../bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContentPlaceHolder" runat="server">
    <section class="content-header">
        <h1>EquipmentInstallation Management
            <small>View, Add and Update Equipment Installations</small>
        </h1>
        <ol class="breadcrumb">
            <li>
                <a href="index.aspx"><i class="fa fa-dashboard"></i>Dashboard</a>
            </li>
            <li class="active"><i class="fa fa-user"></i>Equipment Installation Management</li>
        </ol>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="col-md-12">
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label ID="lblAddEditHeading" runat="server" Text="Add Equipment Installation"></asp:Label>
                </h3>
            </div>
            <!-- /.box-header -->
            <!-- form start -->

            <div class="box-body">
                <asp:HiddenField ID="hfEditingEquipmentInstallationSerial" Value="" runat="server" />

                <!-- left side input fields -->
                <div class="col-md-6">
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label>
                                Equipment Serial Number (Choose from table)<br />
                                <asp:Label ID="lblSerialNumberWarning" runat="server" ForeColor="Orange" Text="Please enter with care, equipment serial number cannot be updated once installation is added to the system."></asp:Label>
                            </label>
                            <asp:TextBox runat="server" CssClass="form-control"
                                ID="txtEquipmentSerialNumber"
                                MaxLength="49"
                                Enabled="False"
                                placeholder="Choose Equipment Serial Number">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                ID="rfvEquipmentInstallationSerial"
                                ControlToValidate="txtEquipmentSerialNumber"
                                ErrorMessage="Serial number of equipment being installed is required">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label>Installation Date</label>
                            <asp:TextBox runat="server" CssClass="form-control"
                                ID="txtEquipmentInstallationDate"
                                MaxLength="254"
                                placeholder="Enter Equipment Installation Date">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                ID="rfvEquipmentInstallationMake"
                                ControlToValidate="txtEquipmentInstallationDate"
                                ErrorMessage="Equipment Installation date is required">
                            </asp:RequiredFieldValidator>
                            <asp:CompareValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                ID="cvEquipmentInstallationDate"
                                ControlToValidate="txtEquipmentInstallationDate"
                                Operator="DataTypeCheck"
                                Type="Date"
                                ErrorMessage="Please enter a valid installation date">
                            </asp:CompareValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label>Equipment Warranty Period (Months)</label>
                            <asp:TextBox runat="server" CssClass="form-control"
                                ID="txtEquipmentInstallationWarrantyPeriod"
                                TextMode="Number"
                                Min="0"
                                placeholder="Enter Equipment Warranty Period">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                ID="rfvEquipmentInstallationWarrantyPeriod"
                                ControlToValidate="txtEquipmentInstallationWarrantyPeriod"
                                ErrorMessage="Equipment warranty period is required">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label>Service Agreement Period (Months)</label>
                            <asp:TextBox runat="server" CssClass="form-control"
                                ID="txtEquipmentInstallationSAPeriod"
                                TextMode="Number"
                                Min="0"
                                placeholder="Enter Service Agreement Period (Months)">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                ID="rfvEquipmentInstallationSAPeriod"
                                ControlToValidate="txtEquipmentInstallationSAPeriod"
                                ErrorMessage="Equipment service agreement period is required">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label>Invoice ID</label>
                            <asp:TextBox runat="server" CssClass="form-control"
                                ID="txtEquipmentInstallationInvoiceId"
                                placeholder="Enter sales invoice number">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                ID="rfvEquipmentInstallationInvoiceId"
                                ControlToValidate="txtEquipmentInstallationInvoiceId"
                                ErrorMessage="EquipmentInstallation invoice ID is required">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label>Customer ID (Choose from table)</label>
                            <asp:TextBox runat="server" CssClass="form-control"
                                ID="txtEquipmentInstallationCustomerId"
                                Enabled="False"
                                placeholder="Choose Customer ID">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                ID="rfvEquipmentInstallationCustomerId"
                                ControlToValidate="txtEquipmentInstallationCustomerId"
                                ErrorMessage="Customer ID is required">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label>Department</label>
                            <asp:TextBox runat="server" CssClass="form-control"
                                         ID="txtDepartment"
                                         placeholder="Enter department">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                        ID="RequiredFieldValidator1"
                                                        ControlToValidate="txtDepartment"
                                                        ErrorMessage="EquipmentInstallation Department is required">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label>Ward</label>
                            <asp:TextBox runat="server" CssClass="form-control"
                                         ID="txtWard"
                                         placeholder="Enter ward">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                        ID="RequiredFieldValidator2"
                                                        ControlToValidate="txtWard"
                                                        ErrorMessage="EquipmentInstallation Ward is required">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label>Contact Person</label>
                            <asp:TextBox runat="server" CssClass="form-control"
                                         ID="txtContactPerson"
                                         placeholder="Enter Contact Person">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                        ID="RequiredFieldValidator3"
                                                        ControlToValidate="txtContactPerson"
                                                        ErrorMessage=" Contact Person is required">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    
                    <div class="row">
                        <div class="form-group col-md-6">
                            <label>Contact Person Contact Number (+XX XX XXX XXXX)</label>
                            <asp:TextBox runat="server" CssClass="form-control"
                                         ID="txtContactPersonContact"
                                         MaxLength="29"
                                         TextMode="Phone"
                                         placeholder="Enter Customer Telephone Number">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                        ID="rfvCustomerTelephone"
                                                        ControlToValidate="txtContactPersonContact"
                                                        ErrorMessage="Contact person contact number is required">
                            </asp:RequiredFieldValidator>
                        </div>

                    </div>

                </div>
                <!-- /.left side input fields-->
                <!-- right side input fields-->
                <div class="col-md-6">
                    <div class="row">
                        <div runat="server" ID="divAvailableEquipments" class="box box-primary box-solid">
                            <div class="box-header with-border">
                                <h3 class="box-title">View & Search Avaialble Items</h3>

                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                                <!-- /.box-tools -->
                            </div>
                            <!-- /.box-header -->
                            <div class="box-body">
                                <div class="form-group col-md-12">
                                    <asp:ListView ID="lvAvailableEquipmentItems" runat="server" ItemPlaceholderID="equipmentItemItemPlaceHolder">
                                        <EmptyDataTemplate>
                                            No Equipments Available In Inventory.
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="AvailableEquipmentItemsTable" class="table table-hover">
                                                <thead>
                                                    <tr>
                                                        <th>Serial Number</th>
                                                        <th>Model Number</th>
                                                        <th>Price</th>
                                                        <th>Select Equipment</th>
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
                                                    <a href='../EquipmentItemProfile.aspx?serialnumber=<%# Eval("SerialNumber").ToString() %>' target="_blank">
                                                        <%# Eval("SerialNumber").ToString() %>
                                                    </a>
                                                </td>
                                                <td>
                                                    <a href='../EquipmentProfile.aspx?modelnumber=<%# Eval("EquipmentModelNumber").ToString() %>' target="_blank">
                                                        <%# Eval("EquipmentModelNumber").ToString() %>
                                                    </a>
                                                </td>

                                                <td>
                                                    <%# Eval("Price", "{0:n}") %>
                                                </td>

                                                <td>
                                                    <asp:LinkButton runat="server" CommandArgument='<%# Eval("SerialNumber") %>' CssClass="text-primary" CausesValidation="False" OnClick="btnChooseEquipmentItem_OnClick">Choose</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </div>
                            </div>
                            <!-- /.box-body -->
                        </div>
                    </div>
                    <div class="row">
                        <div class="box box-primary box-solid">
                            <div class="box-header with-border">
                                <h3 class="box-title">View & Search Customers</h3>

                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                </div>
                                <!-- /.box-tools -->
                            </div>
                            <!-- /.box-header -->
                            <div class="box-body">
                                <div class="form-group col-md-12">
                                    <asp:ListView ID="lvCustomers" runat="server" ItemPlaceholderID="customerItemPlaceHolder">
                                        <EmptyDataTemplate>
                                            No Customers Added
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="CustomersTable" class="table table-hover">
                                                <thead>
                                                    <tr>
                                                        <th>ID</th>
                                                        <th>Name</th>
                                                        <th>Address</th>
                                                        <th>Select Customer</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:PlaceHolder runat="server" ID="customerItemPlaceHolder"></asp:PlaceHolder>
                                                </tbody>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <%# Eval("Id").ToString() %>
                                                </td>
                                                <td>
                                                    <a href='../CustomerProfile.aspx?id=<%# Eval("Id").ToString() %>' target="_blank">
                                                        <%# Eval("Name").ToString() %>
                                                    </a>
                                                </td>
                                                <td>
                                                    <%# Eval("Address").ToString() %>
                                                </td>
                                                <td>
                                                    <asp:LinkButton runat="server" CommandArgument='<%# Eval("Id") %>' CssClass="text-primary" CausesValidation="False" OnClick="btnChooseCustomer_OnClick">Choose</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </div>
                            </div>
                            <!-- /.box-body -->
                        </div>

                    </div>
                </div>
                <!-- /.right side input fields-->


            </div>
            <!-- /.box-body -->
            <div class="box-footer">
                <asp:Button ID="btnSubmit" CssClass="btn btn-primary" OnClick="btnSubmit_OnClick" runat="server" Text="Add EquipmentInstallation" />
                <asp:Button ID="btnCancelUpdate" CssClass="btn btn-danger" CausesValidation="False" OnClick="btnCancelUpdate_OnClick" runat="server" Visible="False" Text="Cancel Update" />
            </div>
        </div>
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">View & Search EquipmentInstallations
                </h3>
            </div>
            <!-- /.box-header -->
            <!-- form start -->

            <div class="box-body">
                <asp:ListView ID="lvEquipmentInstallations" runat="server" ItemPlaceholderID="equipmentInstallationItemPlaceHolder">
                    <EmptyDataTemplate>
                        No Equipment Installations Added
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="EquipmentInstallationsTable" class="table table-hover">
                            <thead>
                                <tr>
                                    <th>Serial Number</th>
                                    <th>Installation Date</th>
                                    <th>Warranty Period (Months)</th>
                                    <th>Service Agreement Period</th>
                                    <th>Invoice ID</th>
                                    <th>Customer ID</th>
                                    <th>Actions</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder runat="server" ID="equipmentInstallationItemPlaceHolder"></asp:PlaceHolder>
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
                                <%# ((DateTime) Eval("InstallationDate")).ToShortDateString() %>
                            </td>
                            <td>
                                <%# Eval("WarrantyPeriodMonths").ToString() %>
                            </td>
                            <td>
                                <%# Eval("ServiceAgreementPeriodMonths").ToString() %>
                            </td>
                            <td>
                                <%# Eval("InvoiceId").ToString() %>
                            </td>
                            <td>
                                <%# Eval("CustomerId").ToString() %>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" CommandArgument='<%# Eval("EquipmentItemSerialNumber") %>' CssClass="text-danger" CausesValidation="False" OnClick="btnEditEquipmentInstallation_OnClick">Edit</asp:LinkButton>
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
    <%-- <script src="http://code.jquery.com/jquery-1.10.2.js"></script> --%>
    <script src="http://code.jquery.com/ui/1.11.0/jquery-ui.js"></script>
    <!-- DataTables -->
    <script src="/bower_components/datatables.net/js/jquery.dataTables.min.js"></script>
    <script src="/bower_components/datatables.net-bs/js/dataTables.bootstrap.min.js"></script>
  
    <script>

        var t = $('#EquipmentInstallationsTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script>
    <script>
        var t = $('#AvailableEquipmentItemsTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script>
    <script>
        var t = $('#CustomersTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script>
    <script>
        $(function () {
            $("#MainContentPlaceHolder_txtEquipmentInstallationDate").datepicker({
                changeMonth: true,
                changeYear: true
            });
        });
    </script>
    
    <script>
        jQuery(function($) {
            $.mask.definitions['~'] = '[+]';
            $("#MainContentPlaceHolder_txtContactPersonContact").mask("~99 99 999 9999");
        });

    </script>

</asp:Content>
