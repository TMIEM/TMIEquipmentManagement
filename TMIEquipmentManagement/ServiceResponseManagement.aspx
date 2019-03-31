<%@ Page Title="Service Responses" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ServiceResponseManagement.aspx.cs" Inherits="TMIEquipmentManagement.ServiceResponseManagement" MaintainScrollPositionOnPostback="true" %>
<%@ Import Namespace="EntityLayer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../../bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css">
    <link rel="stylesheet" href="./dist/css/base/jquery-ui-1.9.2.custom.min.css">
    <style>
        .gone {
            visibility: hidden;
            display: none;
        }
    </style>
    <script type="text/javascript">
        function switchServiceItemTypeTab(itemType) {
            switch (itemType) {
            case "SparePart":
                console.log(itemType);
                document.getElementById("sparePartTabLink").click();
                console.log(itemType);
                break;
            case "Equipment":
                console.log(itemType);
                document.getElementById("equipmentTabLink").click();
                console.log(itemType);
                break;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContentPlaceHolder" runat="server">
    <section class="content-header">
        <h1>
            Service Response Management
            <small>View, Add and Update Service Responses</small>
        </h1>
        <ol class="breadcrumb">
            <li>
                <a href="index.aspx"><i class="fa fa-dashboard"></i> Dashboard</a>
            </li>
            <li class="active"><i class="fa fa-wrench"></i> Service Response Management</li>
        </ol>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="col-md-12">
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label ID="lblAddEditHeading" runat="server" Text="Add Service Response"></asp:Label>
                </h3>
            </div>
            <!-- /.box-header -->
            <!-- form start -->

            <div class="box-body">
                <asp:HiddenField ID="hfEditingServiceResponseId" Value="" runat="server"/>
                <div class="row">
                    <div class="form-group col-md-12">
                        <!--input fields (left side)-->
                        <div class="col-md-6">
                            <div class="row">
                                <div class="form-group col-md-12">
                                    <label>Service Request ID (Choose from Service Requests table)</label>
                                    <asp:TextBox runat="server" CssClass="form-control"
                                                 ID="txtServiceRequestId"
                                                 MaxLength="254"
                                                 Enabled="False"
                                                 placeholder="Enter Service Request Id">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                                ID="rfvServiceRequestId"
                                                                ControlToValidate="txtServiceRequestId"
                                                                ErrorMessage="Service Request ID is required">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-md-12">
                                    <label>Date</label>
                                    <asp:TextBox runat="server" CssClass="form-control"
                                                 ID="txtServiceResponseDate"
                                                 MaxLength="254"
                                                 placeholder="Enter Service Response Date">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                                ID="rfvServiceResponseDate"
                                                                ControlToValidate="txtServiceResponseDate"
                                                                ErrorMessage="Service Response date is required">
                                    </asp:RequiredFieldValidator>
                                    <asp:CompareValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                          ID="cvServiceResponseDate"
                                                          ControlToValidate="txtServiceResponseDate"
                                                          Operator="DataTypeCheck"
                                                          Type="Date"
                                                          ErrorMessage="Please enter a valid date">
                                    </asp:CompareValidator>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-md-12">
                                    <label>Service Description</label>
                                    <asp:TextBox runat="server" CssClass="form-control"
                                                 ID="txtServiceDescription"
                                                 MaxLength="5000"
                                                 Columns="20"
                                                 Rows="8"
                                                 TextMode="MultiLine"
                                                 placeholder="Enter a description of the service performed on the item">
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
                                    <label>Covered By Warranty</label>
                                    <asp:DropDownList ID="DropDownListCoveredByWarranty" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                        <asp:ListItem Selected="True" Text="No" Value="No"></asp:ListItem>
                                    </asp:DropDownList> 
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-md-12">
                                    <label>Charges Paid</label>
                                    <asp:TextBox runat="server" CssClass="form-control"
                                                 ID="txtChargesPaid"
                                                 TextMode="Number"
                                                 MaxLength="49"
                                                 placeholder="Enter Price (USD)">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                                ID="rfvChargesPaid"
                                                                ControlToValidate="txtChargesPaid"
                                                                ErrorMessage="Charges paid is required">
                                    </asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="row">
                                <div class="form-group col-md-12">
                                    <label>Replacement Received</label>
                                    <asp:DropDownList ID="DropDownListReplacementReceived" runat="server" CssClass="form-control" AutoPostBack="True" OnSelectedIndexChanged="DropDownListReplacementReceived_OnSelectedIndexChanged">
                                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                        <asp:ListItem Selected="True" Text="No" Value="No"></asp:ListItem>
                                    </asp:DropDownList> 
                                    <asp:Label ID="lblReplacementReceivedError" runat="server" CssClass="text-danger" Text="" Visible="False"></asp:Label>
                                </div>
                            </div>

                        </div>
                        <!--/.input fields (left side)-->
                        <!--choosing (right side)-->
                        <div class="col-md-6">
                            <div class="box box-primary box-solid">
                                <div class="box-header with-border">
                                    <h3 class="box-title">
                                        View & Search Service Requests
                                    </h3>
                                </div>
                                <!-- /.box-header -->
                                <!-- form start -->

                                <div class="box-body">
                                    <asp:ListView ID="lvServiceRequests" runat="server" ItemPlaceholderID="serviceRequestItemPlaceHolder">
                                        <EmptyDataTemplate>
                                            No Service Requests Added
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="ServiceRequestsTable" class="table table-hover">
                                                <thead>
                                                <tr>
                                                    <th>ID</th>
                                                    <th>Date</th>
                                                    <th>Type</th>
                                                    <th>Service Request Item</th>
                                                    <th>Select Service Request</th>
                                                </tr>
                                                </thead>
                                                <tbody>
                                                <asp:PlaceHolder runat="server" ID="serviceRequestItemPlaceHolder"></asp:PlaceHolder>
                                                </tbody>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td>
                                                    <a href='../ServiceRequestProfile.aspx?id=<%# Eval("Id").ToString() %>' target="_blank">
                                                        <%# Eval("Id").ToString() %>
                                                    </a>
                                                </td>
                                                <td>
                                                    <%# ((DateTime) Eval("Date")).ToShortDateString() %>
                                                </td>
                                                <td>
                                                    <%# Eval("Type").ToString() %>
                                                </td>
                                                <td>
                                                    <%# CreateServiceRequestItemLink((ServiceItem) Eval("ServiceItem"))%>
                                                    
                                                </td>
                                                <td>
                                                    <asp:LinkButton runat="server" CommandArgument='<%# Eval("Id") %>'  CausesValidation="False" OnClick="btnChooseSR_OnClick">Select</asp:LinkButton>
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
                        <!--/.choosing (right side)-->

                    </div>
                </div>
                
                
                <!--Response Item Selection-->
                <div ID="divEquipmentItemsTable" Visible="False" runat="server" class="row">
                    <div class="col-md-12">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    View & Search Equipment Items
                                </h3>
                            </div>
                            <div class="box-body">
                                <asp:ListView ID="lvEquipmentItems" runat="server" ItemPlaceholderID="equipmentItemItemPlaceHolder">
                                    <EmptyDataTemplate>
                                        No Equipments In Inventory.
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="EquipmentItemsTable" class="table table-hover">
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
                                                <%# Eval("Price", "{0:n}")%>
                                            </td>

                                            <td>
                                                <asp:LinkButton runat="server" CommandArgument='<%# Eval("SerialNumber") %>' CssClass="text-primary" CausesValidation="False" OnClick="btnChooseEquipmentItem_OnClick">Choose</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </div>
                            <div class="box-footer">

                            </div>
                        </div>
                    </div>
                </div>
                <div ID="divSparePartItemsTable" Visible="False" runat="server" class="row">
                    <div class="col-md-12">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    View & Search Spare Part Items
                                </h3>
                            </div>
                            <div class="box-body">
                                <asp:ListView ID="lvSparePartItems" runat="server" ItemPlaceholderID="SparePartItemPlaceHolder">
                                    <EmptyDataTemplate>
                                        No Spare Parts Found
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
                            <div class="box-footer">

                            </div>
                        </div>
                    </div>
                </div>

                
                <div ID="divReplacedItemCallout" runat="server" Visible="False" class="row">
                    <div class="col-md-4">
                        <div class="callout callout-info">
                            <h4>Replacement Received Item is:</h4>
                            <p>
                                <asp:Label runat="server" Text="Not Chosen Yet" ID="lblChosenServiceResponseItem"></asp:Label>
                            </p>
                        </div>
                    </div>
                </div>
                <!--/.Response Item Selection-->
            </div>
            <!-- /.box-body -->
            <div class="box-footer">
                <asp:TextBox ID="txtServiceItemType" CssClass="gone" runat="server"></asp:TextBox>

                <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                            ID="rfvServiceItem"
                                            ControlToValidate="txtServiceItemType"
                                            ErrorMessage="Please choose the replacement item received from the table above">
                </asp:RequiredFieldValidator><br/>
                <asp:Button ID="btnSubmit" CssClass="btn btn-primary" OnClick="btnSubmit_OnClick"  runat="server" Text="Add ServiceResponse"/>
                <asp:Button ID="btnCancelUpdate" CssClass="btn btn-danger" CausesValidation="False" OnClick="btnCancelUpdate_OnClick"  runat="server" Visible="False" Text="Cancel Update"/>
            </div>
        </div>
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">
                    View & Search ServiceResponses
                </h3>
            </div>
            <!-- /.box-header -->
            <!-- form start -->

            <div class="box-body">
                <asp:ListView ID="lvServiceResponses" runat="server" ItemPlaceholderID="serviceResponseItemPlaceHolder">
                    <EmptyDataTemplate>
                        No Service Requests Added
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="ServiceResponsesTable" class="table table-hover">
                            <thead>
                            <tr>
                                <th>ID</th>
                                <th>Date</th>
                                <th>Covered By Warranty</th>
                                <th>Charges Paid</th>
                                <th>Service Request ID</th>
                                <th>Replacement Item</th>
                                <th>Actions</th>
                            </tr>
                            </thead>
                            <tbody>
                            <asp:PlaceHolder runat="server" ID="serviceResponseItemPlaceHolder"></asp:PlaceHolder>
                            </tbody>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <a href='../ServiceResponseProfile.aspx?id=<%# Eval("Id").ToString() %>' target="_blank">
                                    <%# Eval("Id").ToString() %>
                                </a>
                            </td>
                            <td>
                                <%# Eval("Date").ToString() %>
                            </td>
                            <td>
                                <%# Eval("CoveredByWarranty").ToString() %>
                            </td>
                            <td>
                                <%# Eval("ChargesPaid").ToString() %>
                            </td>
                            <td>
                                <%# Eval("ServiceRequestId").ToString() %>
                                
                            </td>
                            <td>
                                <%# CreateItemLink((ServiceItem) Eval("ReplacementItem"))%>
                                
                            </td>
                            <td>
                                <asp:LinkButton runat="server" CommandArgument='<%# Eval("Id") %>' CssClass="text-danger" CausesValidation="False" OnClick="btnEditSR_OnClick">Edit</asp:LinkButton>
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
        setTimeout(function(){
            var itemType = $('#MainContentPlaceHolder_txtServiceItemType').val();
            switch (itemType) {
            case "SparePart":
                console.log(itemType);
                $('#sparePartTabLink')[0].click();
                console.log(itemType);
                break;
            case "Equipment":
                console.log(itemType);
                $('#equipmentTabLink')[0].click();
                console.log(itemType);
                break;
            }
        }, 0);
        
    </script>
    <script>
        var t = $('#EquipmentItemsTable').DataTable({
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
        var t = $('#ServiceResponsesTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script>
    <script>
        var t = $('#ServiceRequestsTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script>
    <script>
        $(function () {
            $("#MainContentPlaceHolder_txtServiceResponseDate").datepicker({
                changeMonth: true,
                changeYear: true
            });
        });
        $(function () {
            $("#MainContentPlaceHolder_txtProblemOccurenceDate").datepicker({
                changeMonth: true,
                changeYear: true
            });
        });
    </script>
</asp:Content>
