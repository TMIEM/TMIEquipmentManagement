<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="TMIEquipmentManagement.index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .products-list .product-info { margin-left: 10px; }

        .healthlist {
            max-height: 400px;
            overflow-y: scroll;
        }
    </style>

    <script>

        function setHealthBars() {
            var healthBars = document.getElementsByClassName("progress-bar"); // preferred`
            for (var i = 0; i < healthBars.length; i++) {
                var healthBar = healthBars[i];
                var health = healthBar.getAttribute('data-healthpercent');
                var healthFloat = parseFloat(health);
                var progressBarClass = "progress-bar-info";
                if (healthFloat < 70 && healthFloat > 40) {
                    progressBarClass = "progress-bar-warning";
                } else if (healthFloat < 40) {
                    progressBarClass = "progress-bar-danger";
                } else {
                    progressBarClass = "progress-bar-success";
                }

                healthBar.classList.remove('progress-bar-info');
                healthBar.classList.add(progressBarClass);
                healthBar.setAttribute('aria-valuenow', healthFloat);
                healthBar.style.width = healthFloat + '%';
                //$(healthBars[i].id).attr('aria-valuenow', healthFloat).css('width', healthFloat);
            }
        }

        function openModal() {
            $('#removals-modal-danger').modal('show');
        }
    </script>

    <link rel="stylesheet" href="./bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script src="https://www.gstatic.com/charts/loader.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/1.3.5/jspdf.min.js"></script>

    <script type="text/javascript">
        google.load("visualization", "1", { packages: ["calendar", "corechart", "map"] });
        google.setOnLoadCallback(function() {
            drawChart2();
            drawConsumableChart();
            drawEquipmentsChart();
            drawSparePartsChart();

        });


        function drawChart2() {
            var btnSave = document.getElementById('save-servicechart-pdf-graph');
            var options = {
                title: 'Service Calendar',
                explorer: {
                    actions: ['dragToZoom', 'rightClickToReset'],
                    axis: 'horizontal',
                    keepInBounds: true,
                    maxZoomIn: 10.0
                }
            };
            $.ajax({
                type: "POST",
                url: "index.aspx/GetServiceChartData",
                data: '',
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
                            legend: { position: 'none' },
                            vAxis: {
                                viewWindow: {
                                    min: 0,
                                    max: 30
                                }
                            },
                        };
                    }

                    var data = google.visualization.arrayToDataTable(r.d);
                    var chart = new google.visualization.Calendar($("#servicechart")[0]);
                    google.visualization.events.addListener(chart,
                        'ready',
                        function() {
                            btnSave.disabled = false;
                        });

                    btnSave.addEventListener('click',
                        function() {
                            var doc = new jsPDF('landscape');
                            doc.addImage(chart.getImageURI(), 0, 0);
                            doc.save('overall-service-Chart.pdf');
                        },
                        false);
                    chart.draw(data, options);
                },
                failure: function(r) {
                    alert("Failed to draw graph");
                    console.log(r);
                },
                error: function(r) {
                    alert("Failed to draw graph");
                    console.log(r);
                }
            });
        }

        function drawConsumableChart() {
            var btnSave = document.getElementById('save-pdf-graph-consumables');
            var options = {
                title: 'Top 3 Used Consumable Models',

            };
            $.ajax({
                type: "POST",
                url: "index.aspx/GetConsumableChart",
                data: '',
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
                            legend: { position: 'none' },
                            vAxis: {
                                viewWindow: {
                                    min: 0,
                                    max: 30
                                }
                            },
                        };
                    }

                    var data = google.visualization.arrayToDataTable(r.d);
                    var chart = new google.visualization.PieChart($("#consumablechart")[0]);
                    google.visualization.events.addListener(chart,
                        'ready',
                        function() {
                            btnSave.disabled = false;
                        });

                    btnSave.addEventListener('click',
                        function() {
                            var doc = new jsPDF('landscape');
                            doc.addImage(chart.getImageURI(), 0, 0);
                            doc.save('top3-consumable-Chart.pdf');
                        },
                        false)
                    chart.draw(data, options);
                },
                failure: function(r) {
                    alert("Failed to draw graph");
                    console.log(r);
                },
                error: function(r) {
                    alert("Failed to draw graph");
                    console.log(r);
                }
            });
        }


        function drawEquipmentsChart() {
            var btnSave = document.getElementById('save-pdf-graph-equipments');
            var options = {
                title: 'Top 3 Used Equipment Models',

            };
            $.ajax({
                type: "POST",
                url: "index.aspx/GetEquipmentChart",
                data: '',
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
                            legend: { position: 'none' },
                            vAxis: {
                                viewWindow: {
                                    min: 0,
                                    max: 30
                                }
                            },
                        };
                    }

                    var data = google.visualization.arrayToDataTable(r.d);
                    var chart = new google.visualization.PieChart($("#equipmentchart")[0]);
                    google.visualization.events.addListener(chart,
                        'ready',
                        function() {
                            btnSave.disabled = false;
                        });

                    btnSave.addEventListener('click',
                        function() {
                            var doc = new jsPDF('landscape');
                            doc.addImage(chart.getImageURI(), 0, 0);
                            doc.save('top3-equipment-Chart.pdf');
                        },
                        false)
                    chart.draw(data, options);
                },
                failure: function(r) {
                    alert("Failed to draw graph");
                    console.log(r);
                },
                error: function(r) {
                    alert("Failed to draw graph");
                    console.log(r);
                }
            });
        }


        function drawSparePartsChart() {
            var btnSave = document.getElementById('save-pdf-graph-spareparts');
            var options = {
                title: 'Top 3 Used Spare Part Models',

            };
            $.ajax({
                type: "POST",
                url: "index.aspx/GetSparePartChart",
                data: '',
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
                            legend: { position: 'none' },
                            vAxis: {
                                viewWindow: {
                                    min: 0,
                                    max: 30
                                }
                            },
                        };
                    }

                    var data = google.visualization.arrayToDataTable(r.d);
                    var chart = new google.visualization.PieChart($("#sparepartchart")[0]);
                    google.visualization.events.addListener(chart,
                        'ready',
                        function() {
                            btnSave.disabled = false;
                        });

                    btnSave.addEventListener('click',
                        function() {
                            var doc = new jsPDF('landscape');
                            doc.addImage(chart.getImageURI(), 0, 0);
                            doc.save('top3-sparepart-Chart.pdf');
                        },
                        false)
                    chart.draw(data, options);
                },
                failure: function(r) {
                    alert("Failed to draw graph");
                    console.log(r);
                },
                error: function(r) {
                    alert("Failed to draw graph");
                    console.log(r);
                }
            });
        }

        function drawMap() {
            var btnSave = document.getElementById('save-pdf-graph-spareparts');
            var options = {
                title: 'Top 3 Used Spare Part Models',

            };
            $.ajax({
                type: "POST",
                url: "index.aspx/GetCustomerLocationData",
                data: '',
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
                            legend: { position: 'none' },
                            vAxis: {
                                viewWindow: {
                                    min: 0,
                                    max: 30
                                }
                            },
                        };
                    }

                    var data = google.visualization.arrayToDataTable(r.d);
                    var chart = new google.visualization.Map($("#customerlocationchart")[0]);
                    google.visualization.events.addListener(chart,
                        'ready',
                        function() {
                            btnSave.disabled = false;
                        });


                    chart.draw(data, options);
                },
                failure: function(r) {
                    alert("Failed to draw graph");
                    console.log(r);
                },
                error: function(r) {
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
            Dashboard
            <small>Control panel</small>
        </h1>
        <ol class="breadcrumb">
            <li>
                <a href="#"><i class="fa fa-dashboard"></i> Home</a>
            </li>
            <li class="active">Dashboard</li>
        </ol>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<section class="content">
<div class="row">
    <!-- ./col -->
    <div class="col-lg-4 col-xs-6">
        <!-- small box -->
        <div class="small-box bg-green">
            <div class="inner">
                <h3>
                    <asp:Label ID="lblGoodHealth" runat="server" Text="0"></asp:Label>
                </h3>

                <p>Items Having Good Health</p>
            </div>
            <div class="icon">
                <i class="ion ion-android-happy"></i>
            </div>
            <a href="#" class="small-box-footer"></a>
        </div>
    </div>
    <!-- ./col -->
    <div class="col-lg-4 col-xs-6">
        <!-- small box -->
        <div class="small-box bg-yellow">
            <div class="inner">
                <h3>
                    <asp:Label ID="lblOkayHealth" runat="server" Text="0"></asp:Label>
                </h3>

                <p>Items Having Okay Health</p>
            </div>
            <div class="icon">
                <i class="ion ion-thumbsup"></i>
            </div>
            <a href="#" class="small-box-footer"></a>
        </div>
    </div>
    <!-- ./col -->
    <div class="col-lg-4 col-xs-6">
        <!-- small box -->
        <div class="small-box bg-red">
            <div class="inner">
                <h3>
                    <asp:Label ID="lblLowHealth" runat="server" Text="0"></asp:Label>
                </h3>

                <p>Items Having Low Health</p>
            </div>
            <div class="icon">
                <i class="ion ion-android-sad"></i>
            </div>
            <a href="#" class="small-box-footer"></a>
        </div>
    </div>
    <!-- ./col -->
</div>
<div class="row">
    <div class="col-md-4">
        <div class="box box-primary box-solid ">
            <div class="box-header with-border">
                <h3 class="box-title">Equipment Item Health Levels</h3>

                <div class="box-tools pull-right">
                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                        <i class="fa fa-minus"></i>
                    </button>
                    <button type="button" class="btn btn-box-tool" data-widget="remove">
                        <i class="fa fa-times"></i>
                    </button>
                </div>
            </div>
            <!-- /.box-header -->
            <div class="box-body healthlist">
                <asp:ListView ID="lvEquipmentHealths" runat="server" ItemPlaceholderID="equipmentHealthPlaceHolder">
                    <EmptyDataTemplate>
                        No Spare Parts Found
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <ul class="products-list product-list-in-box">
                            <asp:PlaceHolder runat="server" ID="equipmentHealthPlaceHolder"></asp:PlaceHolder>
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li class="item">

                            <div class="product-info">

                                <a class="product-title" href='../EquipmentItemProfile.aspx?serialnumber=<%# Eval("EquipmentInstallation.EquipmentItemSerialNumber").ToString() %>' target="_blank">
                                    <%# Eval("EquipmentInstallation.EquipmentItemSerialNumber").ToString() %>
                                    <span class="label label-warning pull-right">Expiry <%# ((DateTime) Eval("ExpiryDate")).ToShortDateString() %></span>

                                </a>
                                <br/>
                                <a class="product-title" href='../EquipmentProfile.aspx?modelnumber=<%# Eval("EquipmentInstallation.EquipmentItem.EquipmentModelNumber").ToString() %>' target="_blank">
                                    <%# Eval("EquipmentInstallation.EquipmentItem.EquipmentModelNumber").ToString() %>

                                </a>


                                <div style="margin-top: 10px" class="progress product-description ">
                                    <div >
                                        <div id="<%# Eval("EquipmentInstallation.EquipmentItemSerialNumber").ToString() %>%>" class="progress-bar progress-bar-striped active progress-bar-success" role="progressbar"
                                             aria-valuenow="0" aria-valuemin="0" aria-valuemax="100"
                                             data-healthpercent="<%# Eval("HealthPercentage").ToString() %>" style="min-width: 8%; width: 8%;">
                                            <span><%# Eval("HealthPercentage").ToString() %>%</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </li>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            <!-- /.box-body -->
            <div class="box-footer text-center">
            </div>
            <!-- /.box-footer -->
        </div>
    </div>

    <div class="col-md-4">
        <div class="box box-primary box-solid">
            <div class="box-header with-border">
                <h3 class="box-title">Spare Part Item Health Levels</h3>

                <div class="box-tools pull-right">
                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                        <i class="fa fa-minus"></i>
                    </button>
                    <button type="button" class="btn btn-box-tool" data-widget="remove">
                        <i class="fa fa-times"></i>
                    </button>
                </div>
            </div>
            <!-- /.box-header -->
            <div class="box-body healthlist">
                <asp:ListView ID="lvSparePartsHealth" runat="server" ItemPlaceholderID="sparePartHealthPlaceHolder">
                    <EmptyDataTemplate>
                        No Spare Parts Found
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <ul class="products-list product-list-in-box">
                            <asp:PlaceHolder runat="server" ID="sparePartHealthPlaceHolder"></asp:PlaceHolder>
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li class="item">

                            <div class="product-info">

                                <a class="product-title" href='../SparePartItemProfile.aspx?serialnumber=<%# Eval("SparePartUsage.SparePartItemSerialNumber").ToString() %>' target="_blank">
                                    <%# Eval("SparePartUsage.SparePartItemSerialNumber").ToString() %>
                                    <span class="label label-warning pull-right">Expiry <%# ((DateTime) Eval("ExpiryDate")).ToShortDateString() %></span>

                                </a>
                                <br/>
                                <a class="product-title" href='../SparePartProfile.aspx?modelnumber=<%# Eval("SparePartUsage.SparePartItem.SparePartModelNumber").ToString() %>' target="_blank">
                                    <%# Eval("SparePartUsage.SparePartItem.SparePartModelNumber").ToString() %>

                                </a>
                                <div style="margin-top: 10px" class="progress product-description ">
                                    <div >
                                        <div id="<%# Eval("SparePartUsage.SparePartItemSerialNumber").ToString() %>%>" class="progress-bar progress-bar-striped active progress-bar-success" role="progressbar"
                                             aria-valuenow="0" aria-valuemin="0" aria-valuemax="100"
                                             data-healthpercent="<%# Eval("HealthPercentage").ToString() %>" style="min-width: 8%; width: 8%;">
                                            <span><%# Eval("HealthPercentage").ToString() %>%</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </li>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            <!-- /.box-body -->
            <div class="box-footer text-center">
            </div>
            <!-- /.box-footer -->
        </div>
    </div>
    <div class="col-md-4">
        <div class="box box-primary box-solid">
            <div class="box-header with-border">
                <h3 class="box-title">Consumables Health Levels</h3>

                <div class="box-tools pull-right">
                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                        <i class="fa fa-minus"></i>
                    </button>
                    <button type="button" class="btn btn-box-tool" data-widget="remove">
                        <i class="fa fa-times"></i>
                    </button>
                </div>
            </div>
            <!-- /.box-header -->
            <div class="box-body healthlist">
                <asp:ListView ID="lvConsumableHealths" runat="server" ItemPlaceholderID="consumableHealthPlaceHolder">
                    <EmptyDataTemplate>
                        No Consumable Usages Found
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <ul class="products-list product-list-in-box">
                            <asp:PlaceHolder runat="server" ID="consumableHealthPlaceHolder"></asp:PlaceHolder>
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li class="item">

                            <div class="product-info">

                                <a class="product-title" href='../EquipmentItemProfile.aspx?serialnumber=<%# Eval("ConsumableBatchUsage.Service.InstalledEquipmentSerialNumber").ToString() %>' target="_blank">
                                    <%# Eval("ConsumableBatchUsage.Service.InstalledEquipmentSerialNumber").ToString() %>
                                    <span class="label label-warning pull-right">Expiry <%# ((DateTime) Eval("ExpiryDate")).ToShortDateString() %></span>

                                </a>
                                <br/>
                                <a class="product-title" href='../ConsumableProfile.aspx?modelnumber=<%# Eval("ConsumableBatchUsage.ConsumableBatchModelNumber").ToString() %>' target="_blank">
                                    <%# Eval("ConsumableBatchUsage.ConsumableBatchModelNumber").ToString() %>

                                </a>
                                <div style="margin-top: 10px" class="progress product-description ">
                                    <div >
                                        <div class="progress-bar progress-bar-striped active progress-bar-success" role="progressbar"
                                             aria-valuenow="0" aria-valuemin="0" aria-valuemax="100"
                                             data-healthpercent="<%# Eval("HealthPercentage").ToString() %>" style="min-width: 8%; width: 8%;">
                                            <span><%# Eval("HealthPercentage").ToString() %>%</span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </li>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            <!-- /.box-body -->
            <div class="box-footer text-center">
            </div>
            <!-- /.box-footer -->
        </div>
    </div>
</div>

<div class="row">
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
</div>
<div class="row">
    <div class="col-md-12">
        <div class="box box-solid">
            <div class="box-header with-border">
                <h3 class="box-title">All Spare Part Items of Model</h3>
            </div>
            <!-- /.box-header -->
            <div class="box-body">
                <div class="form-group col-md-12">
                    <asp:ListView ID="lvAllSpareParts" runat="server" ItemPlaceholderID="equipmentItemItemPlaceHolder">
                        <EmptyDataTemplate>
                            No Spare Parts Available In Inventory.
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <table id="AllSparePartItemsTable" class="table table-hover">
                                <thead>
                                    <tr>
                                        <th>Serial Number</th>
                                        <th>Model Number</th>
                                        <th>Price</th>
                                        <th>Shipment</th>
                                        <th>Is used/installed</th>
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
                                    <a href='../SparePartItemProfile.aspx?serialnumber=<%# Eval("SerialNumber").ToString() %>' target="_blank">
                                        <%# Eval("SerialNumber").ToString() %>
                                    </a>
                                </td>
                                <td>
                                    <a href='../SparePartProfile.aspx?modelnumber=<%# Eval("ModelNumber").ToString() %>' target="_blank">
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
</div>
<div class="row">
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
</div>
<div class="row">
    <div style="max-height: 700px; overflow: scroll" class="col-md-12">
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">Overall Service Chart</h3>

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
                        <input id="save-servicechart-pdf-graph" CssClass="form-control pull-right" type="button" value="Save as PDF" disabled/>
                    </div>
                    <div class="col-md-3">
                    </div>
                </div>
                <br/>
                <br/>
                <br/>
                <div id="servicechart" style="width: 100%; min-height: 450px;">
                </div>
                <br/>
                <br/>
                <br/>
            </div>
            <!-- /.box-body -->
        </div>
    </div>
</div>

<div class="row">
    <div class="col-md-12">
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">Top 3 Equipment (Models) Installed</h3>

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
                        <input id="save-pdf-graph-equipments" CssClass="form-control pull-right" type="button" value="Save as PDF" disabled/>
                    </div>
                    <div class="col-md-3">
                    </div>
                </div>
                <br/>
                <br/>
                <br/>
                <div id="equipmentchart" style="width: 100%; min-height: 450px;">
                </div>
            </div>
            <!-- /.box-body -->
        </div>

    </div>
</div>



<div class="row">
    <div class="col-md-12">
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">Top 3 Consumables (Models) Used</h3>

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
                        <input id="save-pdf-graph-consumables" CssClass="form-control pull-right" type="button" value="Save as PDF" disabled/>
                    </div>
                    <div class="col-md-3">
                    </div>
                </div>
                <br/>
                <br/>
                <br/>
                <div id="consumablechart" style="width: 100%; min-height: 450px;">
                </div>
            </div>
            <!-- /.box-body -->
        </div>

    </div>
</div>


<div class="row">
    <div class="col-md-12">
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">Top 3 Spare Part (Models) Used</h3>

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
                        <input id="save-pdf-graph-spareparts" CssClass="form-control pull-right" type="button" value="Save as PDF" disabled/>
                    </div>
                    <div class="col-md-3">
                    </div>
                </div>
                <br/>
                <br/>
                <br/>
                <div id="sparepartchart" style="width: 100%; min-height: 450px;">
                </div>
            </div>
            <!-- /.box-body -->
        </div>

    </div>
</div>

<%-- <div class="row"> --%>
<%--     <div class="col-md-12"> --%>
<%--         <div class="box box-primary"> --%>
<%--             <div class="box-header with-border"> --%>
<%--                 <h3 class="box-title">Customer Locations</h3> --%>
<%-- --%>
<%--                 <div class="box-tools pull-right"> --%>
<%--                     <button type="button" class="btn btn-box-tool" data-widget="collapse"> --%>
<%--                         <i class="fa fa-minus"></i> --%>
<%--                     </button> --%>
<%--                 </div> --%>
<%--                 <!-- /.box-tools --> --%>
<%--             </div> --%>
<%--             <!-- /.box-header --> --%>
<%--             <div class="box-body" style="overflow: scroll"> --%>
<%--                 <div class="col-md-12"> --%>
<%--                     <div style="text-align: right" class="col-md-3 pull-right"> --%>
<%--                         <input  CssClass="form-control pull-right" type="button" value="Load Map" onclick="drawMap();" /> --%>
<%--                     </div> --%>
<%--                     <div class="col-md-3">   --%>
<%--                     </div> --%>
<%--                 </div> --%>
<%--                 <br/> --%>
<%--                 <br/> --%>
<%--                 <br/> --%>
<%--                 <div id="customerlocationchart" style="width: 100%; min-height: 450px;"> --%>
<%--                 </div> --%>
<%--             </div> --%>
<%--             <!-- /.box-body --> --%>
<%--         </div> --%>
<%--          --%>
<%--     </div> --%>
<%-- </div> --%>


</section>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContentPlaceHolder" runat="server">
    <script src="/bower_components/datatables.net/js/jquery.dataTables.min.js"></script>
    <script src="/bower_components/datatables.net-bs/js/dataTables.bootstrap.min.js"></script>
    <script>
        var t = $('#AllEquipmentItemsTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
        var t = $('#AllSparePartItemsTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
        var t = $('#ConsumableBatchItemsTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script>
</asp:Content>