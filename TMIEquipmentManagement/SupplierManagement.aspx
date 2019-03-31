<%@ Page Title="Supplier Management" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SupplierManagement.aspx.cs" Inherits="TMIEquipmentManagement.SupplierManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- DataTables -->
    <link rel="stylesheet" href="../../bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContentPlaceHolder" runat="server">
    <section class="content-header">
        <h1>
            Supplier Management
            <small>View, Add and Update Suppliers</small>
        </h1>
        <ol class="breadcrumb">
            <li>
                <a href="index.aspx"><i class="fa fa-dashboard"></i> Dashboard</a>
            </li>
            <li class="active"><i class="fa fa-user"></i> Supplier Management</li>
        </ol>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="col-md-12">
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label ID="lblAddEditHeading" runat="server" Text="Add Supplier"></asp:Label>
                </h3>
            </div>
            <!-- /.box-header -->
            <!-- form start -->

            <div class="box-body">
                <asp:HiddenField ID="hfEditingSupplierId" Value="" runat="server"/>
                <div class="row">
                    <div class="form-group col-md-6">
                        <label>Name</label>
                        <asp:TextBox runat="server" CssClass="form-control"
                                     ID="txtSupplierName"
                                     MaxLength="254"
                                     placeholder="Enter Supplier Name">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                    ID="rfvSupplierName"
                                                    ControlToValidate="txtSupplierName"
                                                    ErrorMessage="Supplier name is required">
                        </asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Address</label>
                        <asp:TextBox runat="server" CssClass="form-control"
                                     ID="txtSupplierAddress"
                                     MaxLength="599"
                                     placeholder="Enter Supplier Address">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                    ID="rfvSupplierAddress"
                                                    ControlToValidate="txtSupplierAddress"
                                                    ErrorMessage="Supplier address is required">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-md-6">
                        <label>Telephone (+XX XX XXX XXXX)</label>
                        <asp:TextBox runat="server" CssClass="form-control"
                                     ID="txtSupplierTelephone"
                                     MaxLength="24"
                                     TextMode="Phone"
                                     placeholder="Enter Supplier Telephone Number">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                    ID="rfvSupplierTelephone"
                                                    ControlToValidate="txtSupplierTelephone"
                                                    ErrorMessage="Supplier telephone is required">
                        </asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Fax (+XX XX XXX XXXX)</label>
                        <asp:TextBox runat="server" CssClass="form-control"
                                     ID="txtSupplierFax"
                                     MaxLength="29"
                                     TextMode="Phone"
                                     placeholder="Enter Supplier Fax Number">
                        </asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-md-6">
                        <label>Email</label>
                        <asp:TextBox runat="server" CssClass="form-control"
                                     ID="txtSupplierEmail"
                                     MaxLength="254"
                                     TextMode="Email"
                                     placeholder="Enter Supplier Email Address">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                    ID="rfvSupplierEmail"
                                                    ControlToValidate="txtSupplierEmail"
                                                    ErrorMessage="Supplier email is required">
                        </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revSupplierEmail" runat="server" 
                                                        ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                                        ControlToValidate="txtSupplierEmail" CssClass="text-danger" 
                                                        ErrorMessage="Please enter a valid email address">
                                                
                        </asp:RegularExpressionValidator>
                    </div>
                </div>

            </div>
            <!-- /.box-body -->
            <div class="box-footer">
                <asp:Button ID="btnSubmit" CssClass="btn btn-primary" OnClick="btnSubmit_OnClick" runat="server" Text="Add Supplier"/>
                <asp:Button ID="btnCancelUpdate" CssClass="btn btn-danger" CausesValidation="False" OnClick="btnCancelUpdate_OnClick" runat="server" Visible="False" Text="Cancel Update"/>
            </div>
        </div>
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">
                    View & Search Suppliers
                </h3>
            </div>
            <!-- /.box-header -->
            <!-- form start -->

            <div class="box-body">
                <asp:ListView ID="lvSuppliers" runat="server" ItemPlaceholderID="supplierItemPlaceHolder">
                    <EmptyDataTemplate>
                        No Suppliers Added
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="SuppliersTable" class="table table-hover">
                            <thead>
                            <tr>
                                <th>ID</th>
                                <th>Name</th>
                                <th>Address</th>
                                <th>Telephone</th>
                                <th>Fax</th>
                                <th>Email</th>
                                <th>Actions</th>
                            </tr>
                            </thead>
                            <tbody>
                            <asp:PlaceHolder runat="server" ID="supplierItemPlaceHolder"></asp:PlaceHolder>
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
                                <%# Eval("Telephone").ToString() %>
                            </td>
                            <td>
                                <%# Eval("Fax").ToString() %>
                            </td>
                            <td>
                                <%# Eval("Email").ToString() %>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" CommandArgument='<%# Eval("Id") %>' CssClass="text-danger" CausesValidation="False" OnClick="btnEditSupplier_OnClick">Edit</asp:LinkButton>
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
            $("#MainContentPlaceHolder_txtSupplierTelephone").mask("~99 99 999 9999");
            $("#MainContentPlaceHolder_txtSupplierFax").mask("~99 99 999 9999");
        });

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


</asp:Content>