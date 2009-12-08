<%@ Page Language="C#" AutoEventWireup="true" Async="true" CodeBehind="Default.aspx.cs" Inherits="SBRobots._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Status: <asp:Label ID="lbStatus" runat="server" Text="Status"></asp:Label>
        <br/>
        <asp:Button ID="btStart" runat="server" Text="Start" onclick="btStart_Click" />
        <br/>
        <asp:Button ID="btStop" runat="server" Text="Stop" onclick="btStop_Click" />
        <br/>
    </div>
    </form>
</body>
</html>
