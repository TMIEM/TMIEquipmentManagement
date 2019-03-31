<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ChartTest.aspx.cs" Inherits="TMIEquipmentManagement.ChartTest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
    <script type="text/javascript">
        google.load("visualization", "1", { packages: ["corechart","calendar" ]});
        google.setOnLoadCallback(drawChart);
        function drawChart() {
            var dataTable = new google.visualization.DataTable();
            dataTable.addColumn({ type: 'date', id: 'Date' });
            dataTable.addColumn({ type: 'number', id: 'Won/Loss' });
            dataTable.addRows([
                [ new Date(2012, 3, 13), 37032 ],
                [ new Date(2012, 3, 14), 38024 ],
                [ new Date(2012, 3, 15), 38024 ],
                [ new Date(2012, 3, 16), 38108 ],
                [ new Date(2012, 3, 17), 38229 ],
                // Many rows omitted for brevity.
                [ new Date(2013, 9, 4), 38177 ],
                [ new Date(2013, 9, 5), 38705 ],
                [ new Date(2013, 9, 12), 38210 ],
                [ new Date(2013, 9, 13), 38029 ],
                [ new Date(2013, 9, 19), 38823 ],
                [ new Date(2013, 9, 23), 38345 ],
                [ new Date(2013, 9, 24), 38436 ],
                [ new Date(2013, 9, 30), 38447 ]
            ]);

            var chart = new google.visualization.Calendar(document.getElementById('calendar_basic'));

            var options = {
                title: "Red Sox Attendance",
                height: 350,
            };

            chart.draw(dataTable, options);
        }
    </script>
    <script type="text/javascript">
        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(drawChart);
        function drawChart() {
            var options = {
                title: 'Service Requests For Equipments in Shipment',
                hAxis: {title: 'Date',  titleTextStyle: {color: '#333'},
                    slantedText:true, slantedTextAngle:80},
                vAxis: {minValue: 0},
                explorer: { 
                    actions: ['dragToZoom', 'rightClickToReset'],
                    axis: 'horizontal',
                    keepInBounds: true,
                    maxZoomIn: 4.0},
                colors: ['#D44E41'],
                trendlines: { 0: {} }
                
            };
            $.ajax({
                type: "POST",
                url: "ChartTest.aspx/GetChartData2",
                data: '{}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(r) {
                    for (var key in r.d) {
                        if (key == 0) continue;
                        console.log(key);
                        r.d[key][0] = new Date(Date.parse(r.d[key][0]));
                    }
                    var data = google.visualization.arrayToDataTable(r.d);
                    var formatter = new google.visualization.DateFormat({pattern: 'MMMM, yyyy'});
                    formatter.format(data, 0);
                    var chart = new google.visualization.ColumnChart($("#chart2")[0]);
                    chart.draw(data, options);
                },
                failure: function (r) {
                    alert(r.d);
                },
                error: function (r) {
                    alert(r.d);
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContentPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div id="chart" style="width: 700px; height: 500px;">
    </div><
    <div id="chart2" style="width: 700px; height: 500px;">
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContentPlaceHolder" runat="server">
</asp:Content>
