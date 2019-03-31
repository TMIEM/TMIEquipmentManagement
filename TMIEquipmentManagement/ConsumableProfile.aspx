<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ConsumableProfile.aspx.cs" Inherits="TMIEquipmentManagement.ConsumableProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- DataTables -->
    <link rel="stylesheet" href="./bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script src="https://www.gstatic.com/charts/loader.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/1.3.5/jspdf.min.js"></script>
    
    <script type="text/javascript">
        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(function() {
            drawChart();
        });
        function drawChart() {
            var btnSave = document.getElementById('save-pdf-graph');
            var modelNumber = $("#HeaderContentPlaceHolder_lblConsumableModelNumber").text();
            var yearSelection = $("#MainContentPlaceHolder_ddlYear option:selected").text();
            var options = {
                title: 'Consumable usages for model:'+modelNumber+' over time',
                hAxis: {title: 'Date',  titleTextStyle: {color: '#333'},
                    slantedText:true, slantedTextAngle:80},
                trendlines: { 0: {} },
                vAxis: {minValue: 0},
                explorer: { 
                    actions: ['dragToZoom', 'rightClickToReset'],
                    axis: 'horizontal',
                    keepInBounds: true,
                    maxZoomIn: 10.0},
                colors: ['#D44E41'],
                
            };
            $.ajax({
                type: "POST",
                url: "ConsumableProfile.aspx/GetUsageChartData",
                data: JSON.stringify({ modelnumber: modelNumber, 
                                year: yearSelection}),
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
                    var chart = new google.visualization.ColumnChart($("#chart")[0]);
                    google.visualization.events.addListener(chart, 'ready', function () {
                        btnSave.disabled = false;
                    });

                    btnSave.addEventListener('click', function () {
                        var doc = new jsPDF('landscape');
                        doc.addImage(chart.getImageURI(), 0, 0);
                        doc.save('Consumable'+modelNumber+'-'+yearSelection+'-Chart.pdf');
                    }, false)
                    chart.draw(data, options);
                },
                failure: function (r) {
                    alert("Failed to draw graph");
                    console.log(r);
                },
                error: function (r) {
                    alert("Failed to draw graph");
                    console.log(r);
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContentPlaceHolder" runat="server">
    <section class="content-header">
        <h1>
            Consumable : <asp:Label ID="lblConsumableModelNumber" runat="server" Text=""></asp:Label>
        </h1>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="col-md-12">
        <div class="box box-solid">
            <div class="box-header with-border">
                <i class="fa fa-cogs"></i>

                <h3 class="box-title">Consumable Details</h3>
            </div>
            <!-- /.box-header -->
            <div class="box-body">
                <dl class="dl-horizontal profile-description">
                    <dt>Model Number</dt>
                    <dd>
                        <asp:Label ID="lblConsumableModelNumber2" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Make</dt>
                    <dd>
                        <asp:Label ID="lblMake" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Description</dt>
                    <dd>
                        <asp:Label ID="lblDescription" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Life Span (Days)</dt>
                    <dd>
                        <asp:Label ID="lblLifeSpan" runat="server" Text=""></asp:Label>
                    </dd>
                </dl>
            </div>
            <!-- /.box-body -->
        </div>
        <!-- /.box -->
    </div>
    <div class="col-md-12">
        <div class="box box-solid">
            <div class="box-header with-border">
                <h3 class="box-title">Consumable Batches Info</h3>
            </div>
            <!-- /.box-header -->
            <div class="box-body">
                <div class="form-group col-md-12">
                    <asp:ListView ID="lvConsumableBatches" runat="server" ItemPlaceholderID="equipmentItemItemPlaceHolder">
                        <EmptyDataTemplate>
                            No Consumable Batches Available In Inventory.
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <table id="ConsumableBatchItemsTable" class="table table-hover">
                                <thead>
                                    <tr>
                                        <th>Shipment PO Number</th>
                                        <th>Model Number</th>
                                        <th>Price per unit</th>
                                        <th>Quantity</th>
                                        <th>Quantity Used</th>
                                        <th>Quantity Remaining</th>
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
                                    <a href='../ShipmentProfile.aspx?ponumber=<%# Eval("ShipmentPoNumber").ToString() %>' target="_blank">
                                        <%# Eval("ShipmentPoNumber").ToString() %>
                                    </a>
                                </td>
                                <td>
                                    <a href='../ConsumableProfile.aspx?modelnumber=<%# Eval("ModelNumber").ToString() %>' target="_blank">
                                        <%# Eval("ModelNumber").ToString() %>
                                    </a>
                                </td>

                                <td>
                                    <%# Eval("Price", "{0:n}")%>
                                </td> 
                                <td>
                                    <%# Eval("Quantity").ToString() %>
                                </td> 
                                <td>
                                    <%# Eval("QuantityUsed").ToString() %>
                                </td> 
                                <td>
                                    <%# Eval("QuantityRemaining").ToString() %>
                                </td> 

                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
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
                                    <th>Service ID</th>
                                    <th>Shipment PO Number</th>
                                    <th>Model Number</th>
                                    <th>Quantity Used</th>
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
                                <a href='../ServiceProfile.aspx?id=<%# Eval("ServiceId").ToString()%>' target="_blank">
                                    <%# Eval("ServiceId").ToString() %>
                                </a>
                            </td>
                            <td>
                                <a href='../ShipmentProfile.aspx?ponumber=<%# Eval("ConsumbaleBatchShipmentPONumber").ToString() %>' target="_blank">
                                    <%# Eval("ConsumbaleBatchShipmentPONumber").ToString() %>
                                </a>
                            </td>
                            <td>
                                <a href='../ConsumableProfile.aspx?modelnumber=<%# Eval("ConsumableBatchModelNumber").ToString() %>' target="_blank">
                                    <%# Eval("ConsumableBatchModelNumber").ToString() %>
                                </a>
                            </td>
                            <td>
                                <%# Eval("QuantityUsed").ToString() %>
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
                <h3 class="box-title">Consumable Usages/Month</h3>

                <div class="box-tools pull-right">
                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                        <i class="fa fa-minus"></i>
                    </button>
                </div>
                <!-- /.box-tools -->
            </div>
            <!-- /.box-header -->
            <div class="box-body" style="overflow: scroll">
                <div class="col-md-12">
                    <div style="text-align: right" class="col-md-3 pull-right">
                        <input id="save-pdf-graph"   CssClass="form-control pull-right" type="button" value="Save as PDF" disabled />
                    </div>
                    <div class="col-md-3">  
                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control" onchange="drawChart()" ></asp:DropDownList>
                    </div>
                </div>
                <br/>
                <br/>
                <br/>
                <div id="chart" style="width: 100%; min-height: 450px;">
                </div>
            </div>
            <!-- /.box-body -->
        </div>
        
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContentPlaceHolder" runat="server">
    <script src="/bower_components/datatables.net/js/jquery.dataTables.min.js"></script>
    <script src="/bower_components/datatables.net-bs/js/dataTables.bootstrap.min.js"></script>
    <script>
        var t = $('#ConsumableBatchItemsTable').DataTable({
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
    </script>
</asp:Content>
