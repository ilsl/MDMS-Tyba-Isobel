<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MDMSProject.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

		<meta charset="utf-8"/>

		<title>
			Login
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

		<div class="col-md-4"></div>

		<div class="col-md-4" style="margin-top: 50px">

			<div class="panel panel-default" style="background: rgba(250, 250, 250, 0.6)">
				<div class="panel-heading">
					<h3 class="panel-title">Login</h3>
  				</div>
  				<div class="panel-body">

  					<form method="post" accept-charset="utf-8" id="login" runat="server">

						<div class="form-group">
							<label for="username"><i class="fa fa-user"></i> Username</label>
                          
							<div class="controls">
								<asp:TextBox ID="usernametextbox" Text="Enter user name here" runat="server" OnFocus="if(this.value==this.defaultValue)this.value='';" OnBlur="if(this.value=='')this.value=this.defaultValue;" class="form-control input-lg" />
							
                            </div>
                           
						</div>
						<div class="form-group">
							<label for="password"><i class="fa fa-lock"></i> Password</label>
                          
							<div class="controls">
								<asp:TextBox ID="userpasswordtextbox" TextMode="Password" Text="Enter password here" runat="server" class="form-control input-lg" />
							</div>
                          
						</div>

						<div class="form-group" style="margin-bottom: 0;">
							<div class="controls">
								<%--<button type="submit" id="submitButton" class="btn btn-lg btn-primary" style="width:100%;">--%>
                                <asp:Button ID = "submitButton" Text = "Log in" runat="server"  OnClick="submitEventMethod" class="btn btn-lg btn-primary" Style="width: 100%;" /> 
                         	</div>
						</div>

					</form>

  				</div>
			</div>
          
		</div><!-- End col-md-4 -->

		<div class="col-md-4"></div>

	</body>
</html>