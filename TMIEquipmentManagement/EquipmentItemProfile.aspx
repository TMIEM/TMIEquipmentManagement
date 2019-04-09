<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="EquipmentItemProfile.aspx.cs" Inherits="TMIEquipmentManagement.EquipmentItemProfile" %>
<%@ Import Namespace="EntityLayer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<!-- DataTables -->
	<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
	<link rel="stylesheet" href="./bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css">
	<link rel="stylesheet" href="./dist/css/base/jquery-ui-1.9.2.custom.min.css">
	<script type="text/javascript" src="https://www.google.com/jsapi"></script>
	<script src="https://www.gstatic.com/charts/loader.js"></script>
	<script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/1.3.5/jspdf.min.js"></script>
	
	<script type="text/javascript">
		google.load("visualization", "1", { packages: ["calendar", "corechart"] });
		google.setOnLoadCallback(function() {
			drawChart2();
		});
		

		function drawChart2() {
			var btnSave = document.getElementById('save-pdf-graph2');
			var serialnumber = $("#HeaderContentPlaceHolder_lblEquipmentItemSerialNumber").text();
			var options = {
				title: 'Service Calendar for Equipment: '+serialnumber+'',
				explorer: { 
					actions: ['dragToZoom', 'rightClickToReset'],
					axis: 'horizontal',
					keepInBounds: true,
					maxZoomIn: 10.0}
			};
			$.ajax({
				type: "POST",
				url: "EquipmentItemProfile.aspx/GetServiceChartData",
				data: JSON.stringify({ equipmentserialnumber: serialnumber}),
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: function(r) {
					for (var key in r.d) {
						if (key == 0) continue;
						console.log(key);
						r.d[key][0] = new Date(Date.parse(r.d[key][0]));
					}

					if (r.d.length === 1) {
						r.d[1] = ['No Data Available', 0];

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
						$("#servicechart").html("No Data Available");
						return;
					}

					var data = google.visualization.arrayToDataTable(r.d);
					var chart = new google.visualization.Calendar($("#servicechart")[0]);
					google.visualization.events.addListener(chart, 'ready', function () {
						btnSave.disabled = false;
					});

					btnSave.addEventListener('click',
						function() {
							var doc = new jsPDF('landscape');
							doc.addImage(chart.getImageURI(), 0, 0);
							doc.save('EquipmentItem-' + serialnumber + '-service-Chart.pdf');
						},
						false);
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
			Equipment Item : <asp:Label ID="lblEquipmentItemSerialNumber" runat="server" Text=""></asp:Label>
		</h1>
	</section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<div class="col-md-6">
		<div class="box box-solid">
			<div class="box-header with-border">
				<i class="fa fa-desktop"></i>

				<h3 class="box-title">Equipment Item Details</h3>
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
					<dt>Version</dt>
					<dd>
						<asp:Label ID="lblVersion" runat="server" Text=""></asp:Label>
					</dd>
					<dt>Software Version</dt>
					<dd>
						<asp:Label ID="lblSoftwareVersion" runat="server" Text=""></asp:Label>
					</dd>
					<dt>Min Service Period </dt>
					<dd>
						<asp:Label ID="lblMinServicePeriod" runat="server" Text=""></asp:Label> (Months)
					</dd>
					<dt>Is Installed </dt>
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
				<h3 style="margin-left: 0px" class="widget-user-username">Equipment Installation Details</h3>
			</div>
			<div  class="widget-user-header">
				<!-- /.widget-user-image -->
				<label style="margin-left: 0px" >Commission Equipment</label>
				<asp:TextBox runat="server" CssClass="form-control"
							 ID="txtCommissioningDate"
							 MaxLength="254"
							 placeholder="Enter Commissioning Date">
				</asp:TextBox>

				<asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
											ID="rfvCommissioningDate"
											ControlToValidate="txtCommissioningDate"
											ErrorMessage="Date is required to commission">
				</asp:RequiredFieldValidator>
				<asp:CompareValidator CssClass="text-danger" Display="Dynamic" runat="server"
									  ID="cvEquipmentInstallationDate"
									  ControlToValidate="txtCommissioningDate"
									  Operator="DataTypeCheck"
									  Type="Date"
									  ErrorMessage="Please enter a valid installation date">
				</asp:CompareValidator>
				<br/>
				<asp:Button ID="btnCommissionEquipment" CssClass="btn btn-success" OnClick="btnCommissionEquipment_OnClick" runat="server" Text="Commission" />
			</div>
			<div class="box-footer no-padding">
				<ul class="nav nav-stacked">
					<li><asp:HyperLink ID="hlCustomer" NavigateUrl="#" runat="server">Customer <span class="pull-right badge bg-purple"><asp:Label ID="lblCustomerName" runat="server" Text=""></asp:Label></span></asp:HyperLink></li>
					<li><a href="#">Delvivery Date <span class="pull-right badge bg-blue"><asp:Label ID="lblInstallationDate" runat="server" Text=""></asp:Label></span></a></li>
					<li><a href="#">Commissioning Date <span class="pull-right badge bg-blue"><asp:Label ID="lblCommissioningDate" runat="server" Text=""></asp:Label></span></a></li>
					<li><a href="#">Warranty Period (Months) <span class="pull-right badge bg-aqua"><asp:Label ID="lblWarranty" runat="server" Text=""></asp:Label></span></a></li>
					<li><a href="#">Service Agreement Period (Months) <span class="pull-right badge bg-olive"><asp:Label ID="lblServiceAgreement" runat="server" Text=""></asp:Label></span></a></li>
					<li><a href="#">Invoice ID <span class="pull-right badge bg-red"><asp:Label ID="lblInvoiceId" runat="server" Text=""></asp:Label></span></a></li>
					<li><a href="#">Last Service Date <span class="pull-right badge bg-orange"><asp:Label ID="lblLastServiceDate" runat="server" Text=""></asp:Label></span></a></li>
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
									<th>Technician</th>
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
								<a href='../TechnicianProfile.aspx?id=<%# Eval("TechnicianId").ToString() %>' target="_blank">
									<%# Eval("Technician.Name").ToString() %>
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
									<th>Attached In Service</th>
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
								<a href='../ServiceProfile.aspx?id=<%# Eval("ServiceId").ToString() %>' target="_blank">
									<%# Eval("ServiceId")%>
								</a>
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
									<th>Attached In Service</th>
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
								<a href='../ServiceProfile.aspx?id=<%# Eval("ServiceId").ToString() %>' target="_blank">
									<%# Eval("ServiceId")%>
								</a>
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
								<%# Eval("ServiceRequestId").ToString() %>
								
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
	<div class="col-md-12">
		<div class="box box-primary">
			<div class="box-header with-border">
				<h3 class="box-title">Service Chart</h3>

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
						<input id="save-pdf-graph2" style="display: none"  CssClass="form-control pull-right" type="button" value="Save as PDF" disabled />
					</div>
					<div class="col-md-3">  
					</div>
				</div>
				<br/>
				<br/>
				<br/>
				<div id="servicechart" style="width: 100%; min-height: 450px;">
				</div>
			</div>
			<!-- /.box-body -->
		</div>
			
	</div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContentPlaceHolder" runat="server">
	<script src="http://digitalbush.com/wp-content/uploads/2013/01/jquery.maskedinput-1.3.1.min_.js"></script>

	<script src="http://code.jquery.com/jquery-1.10.2.js"></script>
	<script src="http://code.jquery.com/ui/1.11.0/jquery-ui.js"></script>

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
		var t = $('#SparePartUsageItemsTable').DataTable({
			"scrollX": true,
			"scrollCollapse": true,
			"fixedColumns": true,
			"autoWidth": false
		});
		t.columns.adjust();
	</script>
	
	<script>
		$(function () {
			$("#MainContentPlaceHolder_txtCommissioningDate").datepicker({
				changeMonth: true,
				changeYear: true
			});
		});
	</script>
</asp:Content>
