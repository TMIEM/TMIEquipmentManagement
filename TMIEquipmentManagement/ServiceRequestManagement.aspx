<%@ Page Title="Service Requests" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ServiceRequestManagement.aspx.cs" Inherits="TMIEquipmentManagement.ServiceRequestManagement" MaintainScrollPositionOnPostback="true" %>
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
            Service Request Management
            <small>View, Add and Update Service Requests</small>
        </h1>
        <ol class="breadcrumb">
            <li>
                <a href="index.aspx"><i class="fa fa-dashboard"></i> Dashboard</a>
            </li>
            <li class="active"><i class="fa fa-wrench"></i> Service Request Management</li>
        </ol>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="col-md-12">
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">
                    <asp:Label ID="lblAddEditHeading" runat="server" Text="Add Service Request"></asp:Label>
                </h3>
            </div>
            <!-- /.box-header -->
            <!-- form start -->

            <div class="box-body">
                <asp:HiddenField ID="hfEditingServiceRequestId" Value="" runat="server"/>
                <div class="row">
                    <div class="form-group col-md-6">
                        <label>Date</label>
                        <asp:TextBox runat="server" CssClass="form-control"
                                     ID="txtServiceRequestDate"
                                     MaxLength="254"
                                     placeholder="Enter Service Request Date">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                    ID="rfvServiceRequestDate"
                                                    ControlToValidate="txtServiceRequestDate"
                                                    ErrorMessage="Service Request date is required">
                        </asp:RequiredFieldValidator>
                        <asp:CompareValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                              ID="cvServiceRequestDate"
                                              ControlToValidate="txtServiceRequestDate"
                                              Operator="DataTypeCheck"
                                              Type="Date"
                                              ErrorMessage="Please enter a valid date">
                        </asp:CompareValidator>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Current Product Location</label>
                        <asp:TextBox runat="server" CssClass="form-control"
                                     ID="txtServiceRequestCurrentProductLocation"
                                     MaxLength="300"
                                     placeholder="Enter the current location of the product">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                    ID="rfvServiceRequestCurrentProductLocation"
                                                    ControlToValidate="txtServiceRequestCurrentProductLocation"
                                                    ErrorMessage="Current Product Location is required">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-md-6">
                        <label>Request Type</label>
                        <asp:TextBox runat="server" CssClass="form-control"
                                     ID="txtServiceRequestType"
                                     MaxLength="254"
                                     placeholder="Enter the type of service being requested">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                    ID="rfvServiceRequestType"
                                                    ControlToValidate="txtServiceRequestType"
                                                    ErrorMessage="Service Request type is required">
                        </asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group col-md-6">
                        <label>Under Warranty</label>
                        <asp:DropDownList ID="DropDownListUnderWarranty" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                            <asp:ListItem Text="No" Value="No"></asp:ListItem>
                        </asp:DropDownList> 
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-md-12">
                        <label>Problem Occurence Date</label>
                        <asp:TextBox runat="server" CssClass="form-control"
                                     ID="txtProblemOccurenceDate"
                                     MaxLength="254"
                                     placeholder="Enter the date on which the problem occured">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                    ID="rfvProblemOccurenceDate"
                                                    ControlToValidate="txtProblemOccurenceDate"
                                                    ErrorMessage="Problem occurence date is required">
                        </asp:RequiredFieldValidator>
                        <asp:CompareValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                              ID="cvProblemOccurenceDate"
                                              ControlToValidate="txtProblemOccurenceDate"
                                              Operator="DataTypeCheck"
                                              Type="Date"
                                              ErrorMessage="Please enter a valid date">
                        </asp:CompareValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-md-12">
                        <label>Problem Details</label>
                        <asp:TextBox runat="server" CssClass="form-control"
                                     ID="txtProblemDetails"
                                     MaxLength="8000"
                                     Columns="20"
                                     Rows="8"
                                     TextMode="MultiLine"
                                     placeholder="Enter a description of the problem in the item">
                        </asp:TextBox>
                        <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                                    ID="rfvtxtProblemDetails"
                                                    ControlToValidate="txtProblemDetails"
                                                    ErrorMessage="Problem details are required">
                        </asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-md-12">
                        <label>Problem Frequency Details</label>
                        <asp:TextBox runat="server" CssClass="form-control"
                                     ID="txtProblemFrequencyDetails"
                                     MaxLength="5000"
                                     Columns="20"
                                     Rows="8"
                                     TextMode="MultiLine"
                                     placeholder="Enter a description of the frequency of the problem in the item, ex: every day at 12 PM">
                        </asp:TextBox>
                       
                    </div>
                </div>
                <div class="row">
                    <div class="form-group col-md-12">
                        <label>Problem Reproduction Instructions</label>
                        <asp:TextBox runat="server" CssClass="form-control"
                                     ID="txtProblemReprodcutionInstructions"
                                     MaxLength="8000"
                                     Columns="20"
                                     Rows="8"
                                     TextMode="MultiLine"
                                     placeholder="Enter a description on how to recreate the problem in the item">
                        </asp:TextBox>
                       
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                      <!-- Custom Tabs (Pulled to the right) -->
                      <div class="nav-tabs-custom">
                        <ul class="nav nav-tabs pull-right">
                          <li class="active"><a href="#tab_1-1" id="equipmentTabLink" data-toggle="tab" >Equipment</a></li>
                          <li class=""><a href="#tab_2-2" id="sparePartTabLink" data-toggle="tab" >Spare Part</a></li>
                          <li class="pull-left header"><i class="fa fa-cogs"></i> Choose Service Request Item</li>
                        </ul>
                        <div class="tab-content">
                          <div class="tab-pane fade in active" id="tab_1-1">
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
                          <!-- /.tab-pane -->
                          <div class="tab-pane" id="tab_2-2">
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
                          <!-- /.tab-pane -->
                          <!-- /.tab-pane -->
                        </div>
                        <!-- /.tab-content -->
                      </div>
                      <!-- nav-tabs-custom -->
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <div class="callout callout-info">
                            <h4>Service Request Item is:</h4>

                            <p>
                                <asp:Label runat="server" Text="Not Chosen Yet" ID="lblChosenServiceRequestItem"></asp:Label>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
            <!-- /.box-body -->
            <div class="box-footer">
                <asp:TextBox ID="txtServiceItemType" CssClass="gone" runat="server"></asp:TextBox>

                <asp:RequiredFieldValidator CssClass="text-danger" Display="Dynamic" runat="server"
                                            ID="rfvServiceItem"
                                            ControlToValidate="txtServiceItemType"
                                            ErrorMessage="Please choose the item that the service is being requested for">
                </asp:RequiredFieldValidator><br/>
                <asp:Button ID="btnSubmit" CssClass="btn btn-primary" OnClick="btnSubmit_OnClick"  runat="server" Text="Add ServiceRequest"/>
                <asp:Button ID="btnCancelUpdate" CssClass="btn btn-danger" CausesValidation="False" OnClick="btnCancelUpdate_OnClick"  runat="server" Visible="False" Text="Cancel Update"/>
            </div>
        </div>
        <div class="box box-primary">
            <div class="box-header with-border">
                <h3 class="box-title">
                    View & Search ServiceRequests
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
                                <th>Service Request Item</th>
                                <th>Actions</th>
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
                            <td>
                                <%# CreateItemLink((ServiceItem) Eval("ServiceItem"))%>
                                
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
            $("#MainContentPlaceHolder_txtServiceRequestDate").datepicker({
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
