<%@ Page Title="Equipment Management" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="EquipmentManagement.aspx.cs" Inherits="TMIEquipmentManagement.EquipmentManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- DataTables -->
    <link rel="stylesheet" href="../../bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContentPlaceHolder" runat="server">
    <section class="content-header">
        <h1>
            Equipment Management
            <small>View, Add and Update Equipments</small>
        </h1>
        <ol class="breadcrumb">
            <li>
                <a href="index.aspx"><i class="fa fa-dashboard"></i> Dashboard</a>
            </li>
            <li class="active"><i class="fa fa-user"></i> Equipment Management</li>
        </ol>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="col-md-12">
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label ID="lblAddEditHeading" runat="server" Text="Add Equipment"></asp:Label>
                </h3>
            </div>
            <!-- /.box-header -->
            <!-- form start -->

            <div class="box-body">
                <asp:HiddenField ID="hfEditingEquipmentModel" Value="" runat="server"/>
                
                <!-- left side input fields -->
                <div class="col-md-6">
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label>
                                Model Number <br/>
                                <asp:Label ID="lblModelNumberWarning" runat="server" ForeColor="Orange" Text="Please enter with care, model number cannot be updated once equipment is added to the system."></asp:Label>
                            </label>
                            <asp:TextBox runat="server" CssClass="form-control"
                                         ID="txtEquipmentModel"
                                         MaxLength="49"
                                         placeholder="Enter Equipment Model Number">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                        ID="rfvEquipmentModel"
                                                        ControlToValidate="txtEquipmentModel"
                                                        ErrorMessage="Equipment model number is required">
                            </asp:RequiredFieldValidator>
                            <asp:Label ID="lblDuplicateModelNumberAlert" runat="server" Visible="False" CssClass="text-danger" 
                                       Text="An equipment with the model number entered exists in the system, please enter a different model number or update the existing item"></asp:Label>

                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label>Make</label>
                            <asp:TextBox runat="server" CssClass="form-control"
                                         ID="txtEquipmentMake"
                                         MaxLength="254"
                                         placeholder="Enter Equipment Make">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                        ID="rfvEquipmentMake"
                                                        ControlToValidate="txtEquipmentMake"
                                                        ErrorMessage="Equipment make is required">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label>Version</label>
                            <asp:TextBox runat="server" CssClass="form-control"
                                         ID="txtEquipmentVersion"
                                         MaxLength="29"
                                         placeholder="Enter Equipment Version">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                        ID="rfvEquipmentVersion"
                                                        ControlToValidate="txtEquipmentVersion"
                                                        ErrorMessage="Equipment version is required">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label>Software Version</label>
                            <asp:TextBox runat="server" CssClass="form-control"
                                         ID="txtEquipmentSoftwareVersion"
                                         MaxLength="29"
                                         placeholder="Enter Equipment Software Version">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                        ID="rfvEquipmentSoftwareVersion"
                                                        ControlToValidate="txtEquipmentSoftwareVersion"
                                                        ErrorMessage="Equipment software version is required">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label>Minimum Service Period (Months)</label>
                            <asp:TextBox runat="server" CssClass="form-control"
                                         ID="txtEquipmentMinServicePeriod"
                                         TextMode="Number"
                                         placeholder="Enter Equipment Minimum Service Period">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                        ID="rfvEquipmentMinServicePeriod"
                                                        ControlToValidate="txtEquipmentMinServicePeriod"
                                                        ErrorMessage="Equipment minimum service period is required">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <!-- /.left side input fields-->
                <!-- right side input fields-->
                <div class="col-md-6">
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label>Description</label>
                            <asp:TextBox runat="server" CssClass="form-control"
                                         ID="txtEquipmentDescription"
                                         MaxLength="1999"
                                         TextMode="MultiLine"
                                         Columns="50"
                                         Rows="18"
                                         placeholder="Enter Equipment Description">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                        ID="rfvEquipmentTelephone"
                                                        ControlToValidate="txtEquipmentDescription"
                                                        ErrorMessage="Equipment description is required">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <!-- /.right side input fields-->


            </div>
            <!-- /.box-body -->
            <div class="box-footer">
                <asp:Button ID="btnSubmit" CssClass="btn btn-primary" OnClick="btnSubmit_OnClick" runat="server" Text="Add Equipment"/>
                <asp:Button ID="btnCancelUpdate" CssClass="btn btn-danger" CausesValidation="False" OnClick="btnCancelUpdate_OnClick" runat="server" Visible="False" Text="Cancel Update"/>
            </div>
        </div>
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">
                    View & Search Equipments
                </h3>
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
                                <th>Description</th>
                                <th>Version</th>
                                <th>Software Version</th>
                                <th>Min Service Period</th>
                                <th>Actions</th>
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
                                <%# Eval("Description").ToString() %>
                            </td>
                            <td>
                                <%# Eval("Version").ToString() %>
                            </td>
                            <td>
                                <%# Eval("SoftwareVersion").ToString() %>
                            </td>
                            <td>
                                <%# Eval("MinimumServicePeriodMonths").ToString() %>
                            </td>
                            <td>
                                <asp:LinkButton runat="server" CommandArgument='<%# Eval("ModelNumber") %>' CssClass="text-danger" CausesValidation="False" OnClick="btnEditEquipment_OnClick">Edit</asp:LinkButton>
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
    <script>
        var t = $('#EquipmentsTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script>


</asp:Content>