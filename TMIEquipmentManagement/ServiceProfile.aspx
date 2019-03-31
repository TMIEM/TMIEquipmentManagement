<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ServiceProfile.aspx.cs" Inherits="TMIEquipmentManagement.ServiceProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- DataTables -->
    <link rel="stylesheet" href="./bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContentPlaceHolder" runat="server">
    <section class="content-header">
        <h1>
            Service : <asp:Label ID="lblServiceId" runat="server" Text=""></asp:Label>
        </h1>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="col-md-12">
        <div class="box box-solid">
            <div class="box-header with-border">
                <i class="fa fa-desktop"></i>

                <h3 class="box-title">Service Details</h3>
            </div>
            <!-- /.box-header -->
            <div class="box-body">
                <dl class="dl-horizontal profile-description">
                    <dt>Service ID</dt>
                    <dd>
                        <asp:Label ID="lblServiceId2" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Date</dt>
                    <dd>
                        <asp:Label ID="lblDate" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Problem Description</dt>
                    <dd>
                        <asp:Label ID="lblProblemDescription" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Service Description</dt>
                    <dd>
                        <asp:Label ID="lblServiceDescription" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Special Note</dt>
                    <dd>
                        <asp:Label ID="lblSpecialNote" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Equipment Item</dt>
                    <dd>
                        <asp:HyperLink ID="hlEquipmentItem" runat="server"></asp:HyperLink>
                    </dd>
                    <dt>Technician</dt>
                    <dd>
                        <asp:HyperLink ID="hlTechnician" runat="server"></asp:HyperLink>
                    </dd>
                </dl>
            </div>
            <!-- /.box-body -->
        </div>
        <!-- /.box -->
    </div>
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
                                    <th>Removed In Service</th>
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
                                <a href='../ServiceProfile.aspx?id=<%# Eval("RemovalServiceId").ToString() %>' target="_blank">
                                    <%# ((int) Eval("RemovalServiceId") < 1) ?  "Not Removed" : Eval("RemovalServiceId")%>
                                </a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            <!-- /.box-body -->
        </div>
    </div>
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
                                    <th>Removal Service ID</th>
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
                                <a href='../ConsumableProfile.aspx?modelnumber=<%# Eval("ConsumableBatchModelNumber").ToString()%>' target="_blank">
                                    <%# Eval("ConsumableBatchModelNumber").ToString() %>
                                </a>
                            </td>
                            <td>
                                <%# Eval("QuantityUsed").ToString() %>
                            </td>

                            <td>
                                <a href='../ServiceProfile.aspx?id=<%# Eval("RemovalServiceId").ToString() %>' target="_blank">
                                    <%# ((int) Eval("RemovalServiceId") < 1) ?  "Not Removed" : Eval("RemovalServiceId")%>
                                </a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            <!-- /.box-body -->
        </div>
    </div>
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
                                <a href='../SparePartItemProfile.aspx?serialnumber=<%# Eval("SparePartItemSerialNumber").ToString() %>' target="_blank">
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

                           
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            <!-- /.box-body -->
        </div>
    </div>
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
                                <a href='../ConsumableProfile.aspx?modelnumber=<%# Eval("ConsumableBatchModelNumber").ToString()  %>' target="_blank">
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

                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            <!-- /.box-body -->
        </div>


    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContentPlaceHolder" runat="server">
    <!-- DataTables -->
    <script src="/bower_components/datatables.net/js/jquery.dataTables.min.js"></script>
    <script src="/bower_components/datatables.net-bs/js/dataTables.bootstrap.min.js"></script>

    <script>
        var t = $('#SparePartUsageItemsTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
        var t = $('#ConsumableBatchUsageItemsTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
        var t = $('#SparePartsRemovedItemsTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
        var t = $('#ConsumablesRemovedItemsTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script>
</asp:Content>
