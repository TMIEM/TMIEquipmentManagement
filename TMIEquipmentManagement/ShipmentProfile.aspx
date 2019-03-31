<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ShipmentProfile.aspx.cs" Inherits="TMIEquipmentManagement.ShipmentProfile" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="./bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script src="https://www.gstatic.com/charts/loader.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/1.3.5/jspdf.min.js"></script>
    
    <script type="text/javascript">
        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(function() {
            drawEquipmentsChart();
            drawSparePartsChart();
        });
        function drawEquipmentsChart() {
            var btnSave = document.getElementById('save-pdf-equipments');
            var options = {
                title: 'Service Requests For Equipments (By Model Number) in Shipment',
                hAxis: {title: 'Date',  titleTextStyle: {color: '#333'},
                    slantedText:true, slantedTextAngle:80},
                vAxis: {minValue: 0},
                explorer: { 
                    actions: ['dragToZoom', 'rightClickToReset'],
                    axis: 'horizontal',
                    keepInBounds: true,
                    maxZoomIn: 4.0},
                colors: ['#D44E41'],
                
            };
            $.ajax({
                type: "POST",
                url: "ShipmentProfile.aspx/GetEquipmentsChartData",
                data: JSON.stringify({ shipmentPoNumber: $("#HeaderContentPlaceHolder_lblShipmentPoNumber").text()}),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(r) {
                    for (var key in r.d) {
                        if (key == 0) continue;
                        console.log(key);
                        r.d[key][0] = new Date(Date.parse(r.d[key][0]));
                    }

                    if (r.d.length === 1) {
                        r.d[1] = ['No Data Available', 0, 'No Data Available'];

                        options = {
                            // set annotation for -- No Data Copy
                            annotations: {
                                // remove annotation stem and push to middle of chart
                                stem: {
                                    color: 'transparent',
                                    length: 120
                                },
                                textStyle: {
                                    color: '#9E9E9E',
                                    fontSize: 18
                                }
                            },
                            legend: {position: 'none'},
                            vAxis: {
                                viewWindow: {
                                    min: 0,
                                    max: 30
                                }
                            },
                        };
                    }

                    var data = google.visualization.arrayToDataTable(r.d);
                    var chart = new google.visualization.ScatterChart($("#chart")[0]);
                    google.visualization.events.addListener(chart, 'ready', function () {
                        btnSave.disabled = false;
                    });

                    btnSave.addEventListener('click', function () {
                        var doc = new jsPDF('landscape');
                        doc.addImage(chart.getImageURI(), 0, 0);
                        doc.save('EquipmentsChart.pdf');
                    }, false)
                    chart.draw(data, options);
                },
                failure: function (r) {
                    alert("Failed to draw graph");
                },
                error: function (r) {
                    alert("Failed to draw graph");
                }
            });
        }


        function drawSparePartsChart() {
            var btnSave = document.getElementById('save-pdf-spareparts');
            var options = {
                title: 'Service Requests For Spare Parts (By Model Number) in Shipment',
                hAxis: {title: 'Date',  titleTextStyle: {color: '#333'},
                    slantedText:true, slantedTextAngle:80},
                vAxis: {minValue: 0},
                explorer: { 
                    actions: ['dragToZoom', 'rightClickToReset'],
                    axis: 'horizontal',
                    keepInBounds: true,
                    maxZoomIn: 4.0},
                colors: ['#D44E41'],
                
            };
            $.ajax({
                type: "POST",
                url: "ShipmentProfile.aspx/GetSparePartsChartData",
                data: JSON.stringify({ shipmentPoNumber: $("#HeaderContentPlaceHolder_lblShipmentPoNumber").text()}),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(r) {
                    for (var key in r.d) {
                        if (key == 0) continue;
                        console.log(key);
                        r.d[key][0] = new Date(Date.parse(r.d[key][0]));
                    }
                    if (r.d.length === 1) {
                        r.d[1] = ['No Data Available', 0, 'No Data Available'];

                         options = {
                            // set annotation for -- No Data Copy
                            annotations: {
                                // remove annotation stem and push to middle of chart
                                stem: {
                                    color: 'transparent',
                                    length: 120
                                },
                                textStyle: {
                                    color: '#9E9E9E',
                                    fontSize: 18
                                }
                            },
                            legend: {position: 'none'},
                            vAxis: {
                                viewWindow: {
                                    min: 0,
                                    max: 30
                                }
                            },
                        };
                    }

                    var data = google.visualization.arrayToDataTable(r.d);
                    var chart = new google.visualization.ScatterChart($("#chart2")[0]);
                    google.visualization.events.addListener(chart, 'ready', function () {
                        btnSave.disabled = false;
                    });

                    btnSave.addEventListener('click', function () {
                        var doc = new jsPDF('landscape');
                        doc.addImage(chart.getImageURI(), -10, 0);
                        doc.save('SparePartsChart.pdf');
                    }, false)

                    chart.draw(data, options);
                },
                failure: function (r) {
                    alert("Failed to draw graph");
                },
                error: function (r) {
                    alert("Failed to draw graph");
                }
            });
        }

        $(window).resize(function(){
            drawEquipmentsChart();
            drawSparePartsChart();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContentPlaceHolder" runat="server">
    <section class="content-header">
        <h1>
            Shipment: <asp:Label ID="lblShipmentPoNumber" runat="server" Text=""></asp:Label>
        </h1>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
  
    <div class="col-md-12">
        <div class="box box-solid">
            <div class="box-header with-border">
                <i class="fa fa-truck"></i>

                <h3 class="box-title">Shipment Details</h3>
            </div>
            <!-- /.box-header -->
            <div class="box-body">
                <dl class="dl-horizontal">
                    <dt>PO Number</dt>
                    <dd>
                        <asp:Label ID="lblDetailsShipmentPoNumber" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Supplier Invoice Number</dt>
                    <dd>
                        <asp:Label ID="lblDetailsInvoiceNumber" runat="server" Text="Label"></asp:Label>
                    </dd>
                    <dt>Date of Arrival</dt>
                    <dd>
                        <asp:Label ID="lblDetailsDateOfArrival" runat="server" Text="Label"></asp:Label>
                    </dd>
                    <dt>Supplier</dt>
                    <dd>
                        <asp:Label ID="lblDetailsSupplier" runat="server" Text="Label"></asp:Label>
                    </dd>
                </dl>
            </div>
            <!-- /.box-body -->
        </div>
        <!-- /.box -->
    </div>
    

    <div class=" col-md-12">
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
                        No Equipments In Shipment
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="EquipmentItemsTable" class="table table-hover">
                            <thead>
                                <tr>
                                    <th>Model Number</th>
                                    <th>Serial Number</th>
                                    <th>Price</th>
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
                                <a href='../EquipmentItemProfile.aspx?serialnumber=<%# Eval("SerialNumber").ToString() %>' target="_blank">
                                    <%# Eval("SerialNumber").ToString() %>
                                </a>
                            </td>
                            <td>
                                <%# Eval("Price", "{0:n}")%>
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
                <h3 class="box-title">Service Requests For Equipment Items received in shipment</h3>

                <div class="box-tools pull-right">
                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                        <i class="fa fa-minus"></i>
                    </button>
                </div>
                <!-- /.box-tools -->
            </div>
            <!-- /.box-header -->
            <div class="box-body" style="overflow: scroll">
                <input id="save-pdf-equipments" type="button" value="Save as PDF" disabled />

                <div id="chart" style="width: 100%; min-height: 450px;">
                </div>
            </div>
            <!-- /.box-body -->
        </div>
        
    </div>
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
                                <a href='../SparePartItemProfile.aspx?serialnumber=<%# Eval("SerialNumber").ToString() %>' target="_blank">
                                    <%# Eval("SerialNumber").ToString() %>
                                </a>
                            </td>
                            <td>
                                <%# Eval("Price", "{0:n}")%>
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
                <h3 class="box-title">Service Requests For Spare Part Items received in shipment</h3>

                <div class="box-tools pull-right">
                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                        <i class="fa fa-minus"></i>
                    </button>
                </div>
                <!-- /.box-tools -->
            </div>
            <!-- /.box-header -->
            <div class="box-body" style="overflow: scroll">
                <input id="save-pdf-spareparts" type="button" value="Save as PDF" disabled />
                <div id="chart2" style="width: 100%; min-height: 450px;">
                </div>
            </div>
            <!-- /.box-body -->
        </div>
            
    </div>
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
        var t = $('#ConsumableBatchItemsTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script>
</asp:Content>
