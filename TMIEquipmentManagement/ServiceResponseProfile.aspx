<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ServiceResponseProfile.aspx.cs" Inherits="TMIEquipmentManagement.ServiceResponseProfile" %>
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
            Service Response : <asp:Label ID="lblServiceResponseId" runat="server" Text=""></asp:Label>
        </h1>
    </section>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <div class="col-md-12">
        <div class="box box-solid">
            <div class="box-header with-border">
                <i class="fa fa-desktop"></i>

                <h3 class="box-title">Service Response Details</h3>
            </div>
            <!-- /.box-header -->
            <div class="box-body">
                <dl class="dl-horizontal profile-description">
                    <dt>Service Response ID</dt>
                    <dd>
                        <asp:Label ID="lblServiceResponseId2" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Date</dt>
                    <dd>
                        <asp:Label ID="lblDate" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Service Description</dt>
                    <dd>
                        <asp:Label ID="lblServiceDescription" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Covered By Warranty</dt>
                    <dd>
                        <asp:Label ID="lblCoveredByWarranty" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Charges Paid</dt>
                    <dd>
                        <asp:Label ID="lblChargesPaid" runat="server" Text=""></asp:Label>
                    </dd>
                    <dt>Replacement</dt>
                    <dd>
                        <asp:HyperLink ID="hlReplacementItem" runat="server"></asp:HyperLink>
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
                <i class="fa fa-desktop"></i>

                <h3 class="box-title">Service Request Details</h3>
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
                        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
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
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContentPlaceHolder" runat="server">
</asp:Content>
