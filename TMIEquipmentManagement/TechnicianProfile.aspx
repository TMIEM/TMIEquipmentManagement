<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="TechnicianProfile.aspx.cs" Inherits="TMIEquipmentManagement.TechnicianProfile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="./bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script src="https://www.gstatic.com/charts/loader.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/1.3.5/jspdf.min.js"></script>
    
    <script type="text/javascript">
        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(function() {
            drawChart();
            drawChart2();
        });
        function drawChart() {
            var btnSave = document.getElementById('save-pdf-graph');
            var technicianId = $("#HeaderContentPlaceHolder_lblTechnicianId").text();
            var yearSelection = $("#MainContentPlaceHolder_ddlYear option:selected").text();
            var options = {
                title: 'Services performed by technician:'+technicianId+' over time',
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
                url: "TechnicianProfile.aspx/GetServicePerformedChartData",
                data: JSON.stringify({ technicianid: technicianId, 
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
                        doc.save('Technician'+technicianId+'-'+yearSelection+'-Services-Chart.pdf');
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


        function drawChart2() {
            var btnSave = document.getElementById('save-pdf-graph2');
            var technicianId = $("#HeaderContentPlaceHolder_lblTechnicianId").text();
            var options = {
                title: 'Services Performed Percentage For Technician '+technicianId+' out of total services performed',
                
            };
            $.ajax({
                type: "POST",
                url: "TechnicianProfile.aspx/GetServiceShareChartData",
                data: JSON.stringify({ technicianid: technicianId}),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(r) {
                    

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
                    var chart = new google.visualization.PieChart($("#installationsharechart")[0]);
                    google.visualization.events.addListener(chart, 'ready', function () {
                        btnSave.disabled = false;
                    });

                    btnSave.addEventListener('click', function () {
                        var doc = new jsPDF('landscape');
                        doc.addImage(chart.getImageURI(), 0, 0);
                        doc.save('Technician'+technicianId+'-share-Chart.pdf');
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
            Technician : <asp:Label ID="lblTechnicianId" runat="server" Text=""></asp:Label>
        </h1>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="col-md-12">
        <div class="box box-solid">
            <div class="box-header with-border">
                <i class="fa fa-desktop"></i>

                <h3 class="box-title">Technician Details</h3>
            </div>
            <!-- /.box-header -->
            <div class="box-body">
                <dl class="dl-horizontal profile-description">
                    <dt>Technician ID</dt>
                    <dd>
                        <asp:Label ID="lblTechnicianId2" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Name</dt>
                    <dd>
                        <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Contact Number</dt>
                    <dd>
                        <asp:Label ID="lblContactNumber" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Email</dt>
                    <dd>
                        <asp:Label ID="lblEmail" runat="server" Text=""></asp:Label>
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
                <h3 class="box-title">Services Performed
                </h3>
            </div>
            <!-- /.box-header -->
            <!-- form start -->

            <div class="box-body">
                <asp:ListView ID="lvServices" runat="server" ItemPlaceholderID="serviceItemPlaceHolder">
                    <EmptyDataTemplate>
                        No Services Performed
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="ServicesTable" class="table table-hover">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Date</th>
                                    <th>Equipment Serial</th>
                                    <th>Equipment Model</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder runat="server" ID="serviceItemPlaceHolder"></asp:PlaceHolder>
                            </tbody>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <a href='../ServiceProfile.aspx?id=<%# Eval("Id").ToString() %>' target="_blank">
                                    <%# Eval("Id").ToString() %>
                                </a>
                            </td>
                            <td>
                                <%# Convert.ToDateTime(Eval("Date")).ToShortDateString()%>
                            </td>
                            <td>
                                <a href='../EquipmentItemProfile.aspx?serialnumber=<%# Eval("InstalledEquipmentSerialNumber").ToString() %>' target="_blank">
                                    <%# Eval("InstalledEquipmentSerialNumber").ToString() %>
                                </a>
                            </td>
                            <td>
                                <a href='../EquipmentProfile.aspx?modelnumber=<%# Eval("EquipmentInstallation.EquipmentItem.EquipmentModelNumber").ToString() %>' target="_blank">
                                    <%# Eval("EquipmentInstallation.EquipmentItem.EquipmentModelNumber").ToString() %>
                                </a>
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
                <h3 class="box-title">Services Performed/Month</h3>

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
    <div class="col-md-12">
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">Technician Equipment Service Percentage</h3>

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
                        <input id="save-pdf-graph2"   CssClass="form-control pull-right" type="button" value="Save as PDF" disabled />
                    </div>
                    <div class="col-md-3">  
                    </div>
                </div>
                <br/>
                <br/>
                <br/>
                <div id="installationsharechart" style="width: 100%; min-height: 450px;">
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
        var t = $('#ServicesTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script>
</asp:Content>
