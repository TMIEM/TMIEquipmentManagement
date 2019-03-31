<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ServiceRequestProfile.aspx.cs" Inherits="TMIEquipmentManagement.ServiceRequestProfile" %>
<%@ Import Namespace="EntityLayer" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- DataTables -->
    <link rel="stylesheet" href="./bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css">
    <style>
        @media (min-width: 768px) {
            .dl-horizontal dt {
                width: 300px;
            }
            .dl-horizontal dd {
                margin-left: 320px;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContentPlaceHolder" runat="server">
    <section class="content-header">
        <h1>
            Service Request : <asp:Label ID="lblServiceRequestId" runat="server" Text=""></asp:Label>
        </h1>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="col-md-12">
        <div class="box box-solid">
            <div class="box-header with-border">
                <i class="fa fa-desktop"></i>

                <h3 class="box-title">Equipment Item Details</h3>
            </div>
            <!-- /.box-header -->
            <div class="box-body">
                <dl class="dl-horizontal profile-description">
                    <dt>Service Request ID</dt>
                    <dd>
                        <asp:Label ID="lblServiceRequestId2" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt><asp:Label ID="lblServiceItemType" runat="server" Text=""></asp:Label></dt>
                    <dd>
                        <asp:HyperLink ID="hlServiceItem" runat="server"></asp:HyperLink>
                    </dd>
                    <dt>Date</dt>
                    <dd>
                        <asp:Label ID="lblDate" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Current Product Location</dt>
                    <dd>
                        <asp:Label ID="lblProductLocation" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Request Type</dt>
                    <dd>
                        <asp:Label ID="lblType" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Under Warranty</dt>
                    <dd>
                        <asp:Label ID="lblUnderWarranty" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Problem Details</dt>
                    <dd>
                        <asp:Label ID="lblProblemDetails" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Problem Occurrence Date</dt>
                    <dd>
                        <asp:Label ID="lblProblemDate" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Problem Frequency Details</dt>
                    <dd>
                        <asp:Label ID="lblProblemFrequency" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Problem Reproduction Instructions</dt>
                    <dd>
                        <asp:Label ID="lblProblemReproduction" runat="server" Text=""></asp:Label>
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
                <h3 class="box-title">
                    Responses for Service Request
                </h3>
            </div>
            <!-- /.box-header -->
            <!-- form start -->

            <div class="box-body">
                <asp:ListView ID="lvServiceResponses" runat="server" ItemPlaceholderID="serviceResponseItemPlaceHolder">
                    <EmptyDataTemplate>
                        No Responses Received
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
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContentPlaceHolder" runat="server">
    <script src="/bower_components/datatables.net/js/jquery.dataTables.min.js"></script>
    <script src="/bower_components/datatables.net-bs/js/dataTables.bootstrap.min.js"></script>
    <script>
        var t = $('#ServiceResponsesTable').DataTable({
            "scrollX": true,
            "scrollCollapse": true,
            "fixedColumns": true,
            "autoWidth": false
        });
        t.columns.adjust();
    </script>
</asp:Content>
