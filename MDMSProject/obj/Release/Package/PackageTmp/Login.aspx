<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MDMSProject.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login Page</title>
</head>
<body>
<p>This is the Login page</p>
<a href="Login.aspx">Login</a> | <a href="Registration.aspx">Registration</a>
    <form id="form1" runat="server">
    <div>
     <p>Enter Username</p>
            <asp:TextBox ID="usernametextbox" Text="Enter user name here" runat="server" Height="16px" Width="551px" onblur="if(this.value=='')this.value=this.defaultValue;" onfocus="if(this.value==this.defaultValue)this.value='';" />

            <p>Enter Password</p>
            <asp:TextBox ID="userpasswordtextbox" TextMode="Password" Text="Enter password here" runat="server" Height="16px" Width="543px" />
    <asp:Button ID = "submitButton" Text = "Log in" runat="server"  OnClick="submitEventMethod" />

    <p>Display userID from database</p>
    <asp:TextBox ID="userID" Text="poop" runat="server" Height="16px" Width="551px"/>

    </div>
    </form>
</body>
</html>
