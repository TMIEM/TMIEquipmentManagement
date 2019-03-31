<%@ Page Title="Technician Management" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="TechnicianManagement.aspx.cs" Inherits="TMIEquipmentManagement.TechnicianManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- DataTables -->
    <link rel="stylesheet" href="../../bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContentPlaceHolder" runat="server">
    <section class="content-header">
        <h1>
            Technician Management
            <small>View, Add and Update Technicians</small>
        </h1>
        <ol class="breadcrumb">
            <li>
                <a href="index.aspx"><i class="fa fa-dashboard"></i> Dashboard</a>
            </li>
            <li class="active"><i class="fa fa-user"></i> Technician Management</li>
        </ol>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="col-md-12">
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label ID="lblAddEditHeading" runat="server" Text="Add Technician"></asp:Label>
                </h3>
            </div>
            <!-- /.box-header -->
            <!-- form start -->

            <div class="box-body">
                <asp:HiddenField ID="hfEditingTechnicianId" Value="" runat="server"/>
                <div class="row">
                    <div class="form-group col-md-6">
                        <label>Name</label>
                        <asp:TextBox runat="server" CssClass="form-control"
                                     ID="txtTechnicianName"
                                     MaxLength="254"
                                     placeholder="Enter Technician Name">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                    ID="rfvTechnicianName"
                                                    ControlToValidate="txtTechnicianName"
                                                    ErrorMessage="Technician name is required">
                        </asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Contact Number (+XX XX XXX XXXX)</label>
                        <asp:TextBox runat="server" CssClass="form-control"
                                     ID="txtTechnicianContact"
                                     MaxLength="24"
                                     TextMode="Phone"
                                     placeholder="Enter Technician Contact Number">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                    ID="rfvTechnicianContact"
                                                    ControlToValidate="txtTechnicianContact"
                                                    ErrorMessage="Technician contact number is required">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-md-6">
                        <label>Email</label>
                        <asp:TextBox runat="server" CssClass="form-control"
                                     ID="txtTechnicianEmail"
                                     MaxLength="254"
                                     TextMode="Email"
                                     placeholder="Enter Technician Email Address">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                    ID="rfvTechnicianEmail"
                                                    ControlToValidate="txtTechnicianEmail"
                                                    ErrorMessage="Technician email is required">
                        </asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="revTechnicianEmail" runat="server" 
                                                        ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                                        ControlToValidate="txtTechnicianEmail" CssClass="text-danger" 
                                                        ErrorMessage="Please enter a valid email address">
                                                
                        </asp:RegularExpressionValidator>
                    </div>
                </div>

            </div>
            <!-- /.box-body -->
            <div class="box-footer">
                <asp:Button ID="btnSubmit" CssClass="btn btn-primary" OnClick="btnSubmit_OnClick" runat="server" Text="Add Technician"/>
                <asp:Button ID="btnCancelUpdate" CssClass="btn btn-danger" CausesValidation="False" OnClick="btnCancelUpdate_OnClick" runat="server" Visible="False" Text="Cancel Update"/>
            </div>
        </div>
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">
                    View & Search Technicians
                </h3>
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
                                <th>Actions</th>
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
                                <%# Eval("Id").ToString() %>
                            </td>
                            <td>
                                <a href='../TechnicianProfile.aspx?id=<%# Eval("Id").ToString() %>' target="_blank">
                                    <%# Eval("Name").ToString() %>
                                </a>
                            </td>
                            <td>
                                <%# Eval("ContactNumber").ToString() %>
                            </td>
                            <td>
                                <%# Eval("Email").ToString() %>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" CommandArgument='<%# Eval("Id") %>' CssClass="text-danger" CausesValidation="False" OnClick="btnEditTechnician_OnClick">Edit</asp:LinkButton>
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
            $("#MainContentPlaceHolder_txtTechnicianContact").mask("~99 99 999 9999");
            $("#MainContentPlaceHolder_txtTechnicianFax").mask("~99 99 999 9999");
        });

    </script>
    <script>
        $(document).ready(function() {
            var t = $('#TechniciansTable').DataTable({
                "scrollX": true,
                "scrollCollapse": true,
                "fixedColumns": true,
                "autoWidth": false
            });
            t.columns.adjust();
        });
    </script>


</asp:Content>