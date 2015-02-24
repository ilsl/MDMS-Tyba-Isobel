<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="MDMSProject.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registration page</title>
</head>
<body>
<p>This is the registation page</p>
<a href="Login.aspx">Login</a> | <a href="Registration.aspx">Registration</a>
    <form id="form1" runat="server">
    <div>
   <p>Enter First name</p> 
       <asp:TextBox ID = "firstnametextbox" Text = "" runat="server" />
       <p>Enter Last name</p> 
       <asp:TextBox ID = "lastnametextbox" Text = "" runat="server" />
       <p>Enter Email Address</p> 
       <asp:TextBox ID = "emailtextbox" Text = "" runat="server" />
       <p>Automatic generated username</p> 
       <asp:TextBox ID = "usernametextbox" Text = "" runat="server" />
       <p>Enter casename</p> 
       <asp:TextBox ID = "CaseNameTextBox" Text = "" runat="server" />
       <p>Automatic generated password</p> 
       <asp:TextBox ID = "passwordtextbox" Text = "" runat="server" />
       <p>Enter License Type</p> 
       <asp:DropDownList id="drop1" runat="server">
      
    <asp:ListItem>L1</asp:ListItem>
    <asp:ListItem>L2</asp:ListItem>
    <asp:ListItem>L3</asp:ListItem>
    <asp:ListItem>L4</asp:ListItem>
    <asp:ListItem>L5</asp:ListItem>
    </asp:DropDownList>
    <asp:Button ID="RegisterButton" Text="Register New User" runat="server" OnClick="registerEventMethod"/>
<p><asp:label id="Label1" runat="server"/></p>

<p><asp:label id="mess" runat="server"/></p>
    </div>
    </form>
</body>
</html>
