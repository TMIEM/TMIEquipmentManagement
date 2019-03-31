<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SparePartItemProfile.aspx.cs" Inherits="TMIEquipmentManagement.SparePartItemProfile" %>
<%@ Import Namespace="EntityLayer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- DataTables -->
    <link rel="stylesheet" href="./bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContentPlaceHolder" runat="server">
    <section class="content-header">
        <h1>
            Spare Part Item : <asp:Label ID="lblSparePartItemSerialNumber" runat="server" Text=""></asp:Label>
        </h1>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="col-md-6">
        <div class="box box-solid">
            <div class="box-header with-border">
                <i class="fa fa-desktop"></i>

                <h3 class="box-title">Spare Part Item Details</h3>
            </div>
            <!-- /.box-header -->
            <div class="box-body">
                <dl class="dl-horizontal profile-description">
                    <dt>Serial Number</dt>
                    <dd>
                        <asp:Label ID="lblSerialNumber" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Price</dt>
                    <dd>
                        <asp:Label ID="lblPrice" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Shipment PO Number</dt>
                    <dd>
                        <asp:Label ID="lblShipmentPoNumber" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Model Number</dt>
                    <dd>
                        <asp:Label ID="lblModelNumber" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Make</dt>
                    <dd>
                        <asp:Label ID="lblMake" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Description</dt>
                    <dd>
                        <asp:Label ID="lblDescription" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Life Span (Months)</dt>
                    <dd>
                        <asp:Label ID="lblLifeSpan" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Is Used </dt>
                    <dd>
                        <asp:Label ID="lblInstalled" runat="server" Text=""></asp:Label>
                    </dd>

                </dl>
            </div>
            <!-- /.box-body -->
        </div>
        <!-- /.box -->
    </div>
    <div class="col-md-6">
        <div runat="server" ID="divInstallationdetails" class="box box-widget widget-user-2">
            <!-- Add the bg color to the header using any of the bg-* classes -->
            <div  class="widget-user-header bg-yellow">
                <!-- /.widget-user-image -->
                <h3 style="margin-left: 0px" class="widget-user-username">Spare Part Usage Details</h3>
            </div>
            <div class="box-footer no-padding">
                <ul class="nav nav-stacked">

                    <li><asp:HyperLink ID="hlEquipment" NavigateUrl="#" runat="server">Attached to Equipment <span class="pull-right badge bg-purple"><asp:Label ID="lblEquipmentSerial" runat="server" Text=""></asp:Label></span></asp:HyperLink></li>                    
                    <li><asp:HyperLink ID="hlUsedInService" NavigateUrl="#" runat="server">Used in Service <span class="pull-right badge bg-purple"><asp:Label ID="lblUsedInService" runat="server" Text=""></asp:Label></span></asp:HyperLink></li>                                        
                    <li><a href="#">Usage Date <span class="pull-right badge bg-blue"><asp:Label ID="lblUsageDate" runat="server" Text=""></asp:Label></span></a></li>                    
                    <li><a href="#">Warranty Period (Months) <span class="pull-right badge bg-aqua"><asp:Label ID="lblWarranty" runat="server" Text=""></asp:Label></span></a></li>                    
                    <li><asp:HyperLink ID="hlRemovedInService" NavigateUrl="#" runat="server">Removed in Service <span class="pull-right badge bg-purple"><asp:Label ID="lblRemovedInService" runat="server" Text=""></asp:Label></span></asp:HyperLink></li>                                                            
                    <li><a href="#">Removal Date  <span class="pull-right badge bg-red"><asp:Label ID="lblRemovalDate" runat="server" Text=""></asp:Label></span></a></li>                    
                    <li>
                        <a href="#">Health Status 
                            <div class="progress pull-right">
                                <div style="width: 150px">
                                    <div runat="server" ID="divHealthBar" class="progress-bar progress-bar-info progress-bar-striped active" role="progressbar"
                                         aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" data-healthpercent="100" style="width: 150px;min-width: 8%">
                                        <asp:Label ID="lblHealthPercent" runat="server" Text="0%"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </a>
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <div class="col-md-12">
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">
                    Service Requests For Item
                </h3>
            </div>
            <!-- /.box-header -->
            <!-- form start -->

            <div class="box-body">
                <asp:ListView ID="lvServiceRequests" runat="server" ItemPlaceholderID="technicianItemPlaceHolder">
                    <EmptyDataTemplate>
                        No Service Requests Added
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="ServiceRequestsTable" class="table table-hover">
                            <thead>
                            <tr>
                                <th>ID</th>
                                <th>Date</th>
                                <th>Current Product Location</th>
                                <th>Type</th>
                                <th>Under Warranty</th>
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
                                <a href='../ServiceRequestProfile.aspx?id=<%# Eval("Id").ToString() %>' target="_blank">
                                    <%# Eval("Id").ToString() %>
                                </a>
                            </td>
                            <td>
                                <%# Eval("Date").ToString() %>
                            </td>
                            <td>
                                <%# Eval("CurrentProductLocation").ToString() %>
                            </td>
                            <td>
                                <%# Eval("Type").ToString() %>
                            </td>
                            <td>
                                <%# Eval("UnderWarranty").ToString() %>
                                
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
    <div class="col-md-12">
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">
                    Service Responses For Services Requests made for item
                </h3>
            </div>
            <!-- /.box-header -->
            <!-- form start -->

            <div class="box-body">
                <asp:ListView ID="lvServiceResponses" runat="server" ItemPlaceholderID="serviceResponseItemPlaceHolder">
                    <EmptyDataTemplate>
                        No Service Responses Received
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
                                <a href='../ServiceRequestProfile.aspx?id=<%# Eval("ServiceRequestId").ToString() %>' target="_blank">
                                    <%# Eval("ServiceRequestId").ToString() %>
                                </a>                                
                            </td>
                            <td>
                                <%# CreateItemLink((ServiceItem) Eval("ReplacementItem"))%>
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
    <script src="/bower_components/datatables.net/js/jquery.dataTables.min.js"></script>
    <script src="/bower_components/datatables.net-bs/js/dataTables.bootstrap.min.js"></script>

    <script>
        
        var t = $('#ServiceRequestsTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
        var t = $('#ServiceResponsesTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script>
</asp:Content>
