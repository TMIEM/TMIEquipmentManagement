<%@ Page Title="Consumable Management" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ConsumableManagement.aspx.cs" Inherits="TMIConsumableManagement.ConsumableManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- DataTables -->
    <link rel="stylesheet" href="../../bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContentPlaceHolder" runat="server">
    <section class="content-header">
        <h1>
            Consumable Management
            <small>View, Add and Update Consumables</small>
        </h1>
        <ol class="breadcrumb">
            <li>
                <a href="index.aspx"><i class="fa fa-dashboard"></i> Dashboard</a>
            </li>
            <li class="active"><i class="fa fa-user"></i> Consumable Management</li>
        </ol>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="col-md-12">
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label ID="lblAddEditHeading" runat="server" Text="Add Consumable"></asp:Label>
                </h3>
            </div>
            <!-- /.box-header -->
            <!-- form start -->

            <div class="box-body">
                <asp:HiddenField ID="hfEditingConsumableModel" Value="" runat="server"/>

                <!-- left side input fields -->
                <div class="col-md-6">
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label>
                                Model Number <br/>
                                <asp:Label ID="lblModelNumberWarning" runat="server" ForeColor="Orange" Text="Please enter with care, model number cannot be updated once consumable is added to the system."></asp:Label>
                            </label>
                            <asp:TextBox runat="server" CssClass="form-control"
                                         ID="txtConsumableModel"
                                         MaxLength="49"
                                         placeholder="Enter Consumable Model Number">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                        ID="rfvConsumableModel"
                                                        ControlToValidate="txtConsumableModel"
                                                        ErrorMessage="Consumable model number is required">
                            </asp:RequiredFieldValidator>
                            <asp:Label ID="lblDuplicateModelNumberAlert" runat="server" Visible="False" CssClass="text-danger"
                                       Text="A consumable with the model number entered exists in the system, please enter a different model number or update the existing item">
                            </asp:Label>

                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label>Make</label>
                            <asp:TextBox runat="server" CssClass="form-control"
                                         ID="txtConsumableMake"
                                         MaxLength="254"
                                         placeholder="Enter Consumable Make">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                        ID="rfvConsumableMake"
                                                        ControlToValidate="txtConsumableMake"
                                                        ErrorMessage="Consumable make is required">
                            </asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group col-md-12">
                            <label>Life Span (Days)</label>
                            <asp:TextBox runat="server" CssClass="form-control"
                                         ID="txtConsumableLifeSpan"
                                         TextMode="Number"
                                         placeholder="Enter Consumable life span in days"
                                         Min="1">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                        ID="rfvConsumableLifeSpan"
                                                        ControlToValidate="txtConsumableLifeSpan"
                                                        ErrorMessage="Consumable life span is required">
                            </asp:RequiredFieldValidator> 
                            <asp:RangeValidator runat="server" ControlToValidate="txtConsumableLifeSpan"
                                                CssClass="text-danger"
                                                ErrorMessage="Minimum life span should be greater than or equal to 1 day"
                                                MinimumValue="1"
                                                MaximumValue="99999999999"/>
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
                                         ID="txtConsumableDescription"
                                         MaxLength="1999"
                                         TextMode="MultiLine"
                                         Columns="50"
                                         Rows="18"
                                         placeholder="Enter Consumable Description">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                        ID="rfvConsumableDescription"
                                                        ControlToValidate="txtConsumableDescription"
                                                        ErrorMessage="Consumable description is required">
                            </asp:RequiredFieldValidator>
                           
                        </div>
                    </div>
                </div>
                <!-- /.right side input fields-->


            </div>
            <!-- /.box-body -->
            <div class="box-footer">
                <asp:Button ID="btnSubmit" CssClass="btn btn-primary" OnClick="btnSubmit_OnClick" runat="server" Text="Add Consumable"/>
                <asp:Button ID="btnCancelUpdate" CssClass="btn btn-danger" CausesValidation="False" OnClick="btnCancelUpdate_OnClick" runat="server" Visible="False" Text="Cancel Update"/>
            </div>
        </div>
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">
                    View & Search Consumables
                </h3>
            </div>
            <!-- /.box-header -->
            <!-- form start -->

            <div class="box-body">
                <asp:ListView ID="lvConsumables" runat="server" ItemPlaceholderID="consumableItemPlaceHolder">
                    <EmptyDataTemplate>
                        No Consumables Added
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="ConsumablesTable" class="table table-hover">
                            <thead>
                            <tr>
                                <th>Model Number</th>
                                <th>Make</th>
                                <th>Description</th>
                                <th>Life Span</th>
                                <th>Actions</th>
                            </tr>
                            </thead>
                            <tbody>
                            <asp:PlaceHolder runat="server" ID="consumableItemPlaceHolder"></asp:PlaceHolder>
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
                                <asp:LinkButton runat="server" CommandArgument='<%# Eval("ModelNumber") %>' CssClass="text-danger" CausesValidation="False" OnClick="btnEditConsumable_OnClick">Edit</asp:LinkButton>
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
        var t = $('#ConsumablesTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script>


</asp:Content>