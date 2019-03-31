<%@ Page Title="404" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="404.aspx.cs" Inherits="TMIEquipmentManagement._404" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeaderContentPlaceHolder" runat="server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <!-- Content Header (Page header) -->
    <section class="content-header">
        <h1>
            404 Error Page
        </h1>
        <ol class="breadcrumb">
            <li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li>
            <li class="active">404 error</li>
        </ol>
    </section>

    <!-- Main content -->
    <section class="content">
        <h2 style="text-align: center; font-size: 100px; font-weight: 300"  class="headline text-yellow"> 404</h2>
        <div class="error-page">
            <!-- /.error-content -->
            <div style="margin: 0px" class="error-content">
                <h3 ><i class="fa fa-warning text-yellow"></i> Oops! Page not found.</h3>
                <p>
                    We could not find the page you were looking for.
                    Meanwhile, you may <a href="../../index.html">return to dashboard</a>
                </p>
            </div>
        </div>
        
        <!-- /.error-page -->
    </section>
    <!-- /.content -->
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ScriptsContentPlaceHolder" runat="server">
</asp:Content>
