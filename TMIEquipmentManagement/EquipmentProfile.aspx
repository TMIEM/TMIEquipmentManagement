<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="EquipmentProfile.aspx.cs" Inherits="TMIEquipmentManagement.EquipmentProfile" %>
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
            drawEquipmentsChart();
        });
        function drawEquipmentsChart() {
            var btnSave = document.getElementById('save-pdf-graph');
            var modelNumber = $("#HeaderContentPlaceHolder_lblEquipmentModelNumber").text();
            var yearSelection = $("#MainContentPlaceHolder_ddlYear option:selected").text();
            var options = {
                title: 'Equipment Installations for model:'+modelNumber+' over time',
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
                url: "EquipmentProfile.aspx/GetEquipmentUsageChartData",
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
                        doc.save('EquipmentsChart.pdf');
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
            Equipment : <asp:Label ID="lblEquipmentModelNumber" runat="server" Text=""></asp:Label>
        </h1>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="col-md-12">
        <div class="box box-solid">
            <div class="box-header with-border">
                <i class="fa fa-truck"></i>

                <h3 class="box-title">Equipment Details</h3>
            </div>
            <!-- /.box-header -->
            <div class="box-body">
                <dl class="dl-horizontal profile-description">
                    <dt>Model Number</dt>
                    <dd>
                        <asp:Label ID="lblEquipmentModelNumber2" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Make</dt>
                    <dd>
                        <asp:Label ID="lblMake" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Description</dt>
                    <dd>
                        <asp:Label ID="lblDescription" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Version</dt>
                    <dd>
                        <asp:Label ID="lblVersion" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Software Version</dt>
                    <dd>
                        <asp:Label ID="lblSoftwareVersion" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Minimum Service Period (Months)</dt>
                    <dd>
                        <asp:Label ID="lblMinServicePeriod" runat="server" Text=""></asp:Label>
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
                <h3 class="box-title">Equipment items in Stock</h3>
            </div>
            <!-- /.box-header -->
            <div class="box-body">
                <div class="form-group col-md-12">
                    <asp:ListView ID="lvAvailableEquipmentItems" runat="server" ItemPlaceholderID="equipmentItemItemPlaceHolder">
                        <EmptyDataTemplate>
                            No Equipments Available In Inventory.
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <table id="AvailableEquipmentItemsTable" class="table table-hover">
                                <thead>
                                    <tr>
                                        <th>Serial Number</th>
                                        <th>Model Number</th>
                                        <th>Price</th>
                                        <th>Shipment</th>
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
                                    <a href='../EquipmentProfile.aspx?modelnumber=<%# Eval("ModelNumber").ToString() %>' target="_blank">
                                        <%# Eval("ModelNumber").ToString() %>
                                    </a>
                                </td>

                                <td>
                                    <%# Eval("Price", "{0:n}")%>
                                </td> 
                                <td>
                                    <a href='../ShipmentProfile.aspx?ponumber=<%# Eval("ShipmentPoNumber").ToString() %>' target="_blank">
                                        <%# Eval("ShipmentPoNumber").ToString() %>
                                    </a>
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
        <div class="box box-solid">
            <div class="box-header with-border">
                <h3 class="box-title">All Equipment Items From Model</h3>
            </div>
            <!-- /.box-header -->
            <div class="box-body">
                <div class="form-group col-md-12">
                    <asp:ListView ID="lvAllEquipmentItems" runat="server" ItemPlaceholderID="equipmentItemItemPlaceHolder">
                        <EmptyDataTemplate>
                            No Equipments Available In Inventory.
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <table id="AllEquipmentItemsTable" class="table table-hover">
                                <thead>
                                    <tr>
                                        <th>Serial Number</th>
                                        <th>Model Number</th>
                                        <th>Price</th>
                                        <th>Shipment</th>
                                        <th>Used/Installed</th>
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
                                    <a href='../EquipmentProfile.aspx?modelnumber=<%# Eval("ModelNumber").ToString() %>' target="_blank">
                                        <%# Eval("ModelNumber").ToString() %>
                                    </a>
                                </td>

                                <td>
                                    <%# Eval("Price", "{0:n}")%>
                                </td> 
                                <td>
                                    <a href='../ShipmentProfile.aspx?ponumber=<%# Eval("ShipmentPoNumber").ToString() %>' target="_blank">
                                        <%# Eval("ShipmentPoNumber").ToString() %>
                                    </a>
                                </td>
                                <td>
                                    <%# Eval("IsInstalled").ToString() %>
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
                <h3 class="box-title">Equipment Installations/Month</h3>

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
                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control" onchange="drawEquipmentsChart()" ></asp:DropDownList>
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
        var t = $('#AvailableEquipmentItemsTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script>
    <script>
        var t = $('#AllEquipmentItemsTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script>
    

</asp:Content>
