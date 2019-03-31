<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestTran.aspx.cs" Inherits="TMIEquipmentManagement.TestTran" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<form id="form1" runat="server">
    <div>
        <asp:Button ID="Button1" runat="server" Text="Test" OnClick="Button1_OnClick"/>
        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        <br/>
        <br/>
        <asp:Button ID="Button2" runat="server" Text="Test" OnClick="Button2_OnClick"/>
        <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
        <br/>
        <asp:Button ID="Button3" runat="server" Text="Test" OnClick="Button3_OnClick"/>
        <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
        <br/>
        <asp:Button ID="Button4" runat="server" Text="Test" OnClick="Button4_OnClick"/>
        <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label>

    </div>
</form>
</body>
</html>