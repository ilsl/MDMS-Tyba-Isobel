<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="MDMSProject.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

		<meta charset="utf-8"/>

		<title>
			Register
		</title>

		<meta name="viewport" content="width=device-width, initial-scale=1"/>

		<meta name="author" content=""/>
		<meta name="robots" content="index, follow"/>
		<meta name="description" content=""/>
		<meta name="keywords" content=""/>

		<link rel="stylesheet" type="text/css" href="assets/css/bootstrap.css"/>
		<link rel="stylesheet" type="text/css" href="assets/css/bootstrap-theme.css"/>
		<link rel="stylesheet" type="text/css" href="assets/css/redactor.css"/>
		<link rel="stylesheet" type="text/css" href="assets/css/font-awesome.min.css"/>
		<link rel="stylesheet" type="text/css" href="assets/css/styles.css"/>

		<script src="assets/js/jquery.js"></script>
		<script src="assets/js/bootstrap.min.js"></script>
		<script src="assets/js/redactor.min.js"></script>

		<script type="text/javascript">
		    //<![CDATA[

		    $(document).ready(function () {

		        $('#content').redactor();


		    });

		    //]]>
		</script>

		<style>
			html {
				background: url("assets/images/login-bg.jpg");
			}

		</style>

	</head>

	<body>

		<div class="col-md-4"></div><!-- End col-md-4 -->

		<div class="col-md-4"  style="margin-top: 50px">


			<div class="panel panel-default" style="background: rgba(250, 250, 250, 0.6)">
				<div class="panel-heading">
					<h3 class="panel-title">Register</h3>
  				</div>
  				<div class="panel-body">

					<form action="" method="post" accept-charset="utf-8" id="setup" class="form-horizontal" runat="server">

						<div class="control-group">
							<label class="control-label" for="firstnametextbox">First Name</label>
							<div class="controls">

                                <asp:TextBox ID = "firstnametextbox" Text = "" runat="server" class="form-control input-lg" required="required" placeholder="First Name" />
							</div>
						</div>

						<div class="control-group">
							<label class="control-label" for="lastnametextbox">Last Name</label>
							<div class="controls">
                                       <asp:TextBox ID = "lastnametextbox" Text = "" runat="server" class="form-control input-lg" required="required" placeholder="First Name" />
							</div>
						</div>
						<div class="control-group">
							<label class="control-label" for="emailtextbox">Email</label>
							<div class="controls">
                                <asp:TextBox ID = "emailtextbox" Text = "" runat="server" class="form-control input-lg" required="required" placeholder="Email" />
							</div>
						</div>
                        

						<div class="control-group">
							<label class="control-label" for="drop1">Licence</label>
							<div class="controls">
								
                                <asp:DropDownList id="drop1" runat="server" name="drop1" class="form-control input-lg" Cssstyle="margin-bottom:10px;">
      
    <asp:ListItem>L1</asp:ListItem>
    <asp:ListItem>L2</asp:ListItem>
    <asp:ListItem>L3</asp:ListItem>
    <asp:ListItem>L4</asp:ListItem>
    <asp:ListItem>L5</asp:ListItem>
     </asp:DropDownList>
							</div>
						</div>

						<div class="control-group">
							<div class="controls">
                                <asp:Button ID="Button1" Text="Register New User" runat="server" OnClick="registerEventMethod" class="btn btn-lg btn-primary" Style="margin-top: 15px; width: 100%;" />

							</div>
						</div>

					</form>

				</div>
			</div>

		</div><!-- End col-md-4 -->

		<div class="col-md-4"></div><!-- End col-md-4 -->


	</body>
</html>
 <%--   <title>Registration page</title>
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
</html>--%>
