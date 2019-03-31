<%@ Page Title="Customer Management" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CustomerManagement.aspx.cs" Inherits="TMIEquipmentManagement.CustomerManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- DataTables -->
    <link rel="stylesheet" href="../../bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContentPlaceHolder" runat="server">
    <section class="content-header">
        <h1>
            Customer Management
            <small>View, Add and Update Customers</small>
        </h1>
        <ol class="breadcrumb">
            <li>
                <a href="index.aspx"><i class="fa fa-dashboard"></i> Dashboard</a>
            </li>
            <li class="active"><i class="fa fa-user"></i> Customer Management</li>
        </ol>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="col-md-12">
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label ID="lblAddEditHeading" runat="server" Text="Add Customer"></asp:Label>
                </h3>
            </div>
            <!-- /.box-header -->
            <!-- form start -->

            <div class="box-body">
                <asp:HiddenField ID="hfEditingCustomerId" Value="" runat="server" />
                <div class="row">
                    <div class="form-group col-md-6">
                        <label>Name</label>
                        <asp:TextBox runat="server" CssClass="form-control"
                                     ID="txtCustomerName"
                                     MaxLength="254"
                                     placeholder="Enter Customer Name">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                    ID="rfvCustomerName"
                                                    ControlToValidate="txtCustomerName"
                                                    ErrorMessage="Customer name is required">
                        </asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Address</label>
                        <asp:TextBox runat="server" CssClass="form-control"
                                     ID="txtCustomerAddress"
                                     MaxLength="699"
                                     placeholder="Enter Customer Address">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                    ID="rfvCustomerAddress"
                                                    ControlToValidate="txtCustomerAddress"
                                                    ErrorMessage="Customer address is required">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-md-6">
                        <label>Telephone (+XX XX XXX XXXX)</label>
                        <asp:TextBox runat="server" CssClass="form-control"
                                     ID="txtCustomerTelephone"
                                     MaxLength="29"
                                     TextMode="Phone"
                                     placeholder="Enter Customer Telephone Number">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                    ID="rfvCustomerTelephone"
                                                    ControlToValidate="txtCustomerTelephone"
                                                    ErrorMessage="Customer telephone is required">
                        </asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Fax (+XX XX XXX XXXX)</label>
                        <asp:TextBox runat="server" CssClass="form-control"
                                     ID="txtCustomerFax"
                                     MaxLength="29"
                                     TextMode="Phone"
                                     placeholder="Enter Customer Fax Number">
                        </asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-md-6">
                        <label>Contact Person Name</label>
                        <asp:TextBox runat="server" CssClass="form-control"
                                     ID="txtCustomerContactPerson"
                                     MaxLength="254"
                                     placeholder="Enter Customer Contact Person Name">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                    ID="rfvCustomerContactPerson"
                                                    ControlToValidate="txtCustomerContactPerson"
                                                    ErrorMessage="Customer contact person name is required">
                        </asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Contact Person's Contact Number (+XX XX XXX XXXX)</label>
                        <asp:TextBox runat="server" CssClass="form-control"
                                     ID="txtCustomerContactPersonContact"
                                     MaxLength="29"
                                     placeholder="Enter Customer Contact Person's Contact Number">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                    ID="rfvCustomerContactPersonContact"
                                                    ControlToValidate="txtCustomerContactPersonContact"
                                                    ErrorMessage="Customer contact person's contact number is required">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>

            </div>
            <!-- /.box-body -->
            <div class="box-footer">
                <asp:Button ID="btnSubmit" CssClass="btn btn-primary" OnClick="btnSubmit_OnClick" runat="server" Text="Add Customer"/>
                <asp:Button ID="btnCancelUpdate" CssClass="btn btn-danger" CausesValidation="False" OnClick="btnCancelUpdate_OnClick" runat="server" Visible="False" Text="Cancel Update"/>
            </div>
        </div>
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">
                    View & Search Customers
                </h3>
            </div>
            <!-- /.box-header -->
            <!-- form start -->

            <div class="box-body">
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
                                    <th>Telephone</th>
                                    <th>Fax</th>
                                    <th>Contact Person</th>
                                    <th>Contact Person Tel</th>
                                    <th>Actions</th>
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
                                <%# Eval("Telephone").ToString() %>
                            </td>
                            <td>
                                <%# Eval("Fax").ToString() %>
                            </td>
                            <td>
                                <%# Eval("ContactPerson").ToString() %>
                            </td>
                            <td>
                                <%# Eval("ContactPersonTelephone").ToString() %>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" CommandArgument='<%# Eval("Id") %>' CssClass="text-danger"  CausesValidation="False" OnClick="btnEditCustomer_OnClick">Edit</asp:LinkButton>
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
    <!-- DataTables -->
    <script src="../../bower_components/datatables.net/js/jquery.dataTables.min.js"></script>
    <script src="../../bower_components/datatables.net-bs/js/dataTables.bootstrap.min.js"></script>

    <script type='text/javascript'>
        jQuery(function($) {
            $.mask.definitions['~'] = '[+]';
            $("#MainContentPlaceHolder_txtCustomerTelephone").mask("~99 99 999 9999");
            $("#MainContentPlaceHolder_txtCustomerFax").mask("~99 99 999 9999");
            $("#MainContentPlaceHolder_txtCustomerContactPersonContact").mask("~99 99 999 9999");
        });

    </script>
    <script>
        $(document).ready(function () {
            var t = $('#CustomersTable').DataTable({
                "scrollX": true,
                "scrollCollapse": true,
                "fixedColumns": true,
                "autoWidth": false
            });
            t.columns.adjust();
        });
    </script>


</asp:Content>