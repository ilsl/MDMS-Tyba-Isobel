﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoggedIn.aspx.cs" EnableEventValidation="false"
    Inherits="MDMSProject.LoggedIn" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <meta charset="utf-8" />

    <title>Home page</title>

    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta name="author" content="" />
    <meta name="robots" content="index, follow" />
    <meta name="description" content="" />
    <meta name="keywords" content="" />

    <link rel="stylesheet" type="text/css" href="assets/css/bootstrap.css" />
    <link rel="stylesheet" type="text/css" href="assets/css/bootstrap-theme.css" />
    <link rel="stylesheet" type="text/css" href="assets/css/redactor.css" />
    <link rel="stylesheet" type="text/css" href="assets/css/font-awesome.min.css" />
    <link rel="stylesheet" type="text/css" href="assets/css/fileupload.css" />
    <link rel="stylesheet" type="text/css" href="assets/css/fileupload-ui.css" />
    <link rel="stylesheet" type="text/css" href="assets/css/styles.css" />
    <link rel="stylesheet" type="text/css" href="assets/css/gridview.css" />

    <script src="assets/js/jquery.js"></script>
    <script src="assets/js/bootstrap.min.js"></script>
    <script src="assets/js/redactor.min.js"></script>
    <script src="assets/js/fileupload.js"></script>
    <script src="Scripts/jquery-2.1.3.min.js"></script>
    <%--For Signalr--%>
    <script src="Scripts/jquery.signalR-2.2.0.min.js"></script>
    <script src="signalr/hubs"></script>
    <!--Reference the autogenerated SignalR hub script. -->
    <script src="assets/js/fileupload.js"></script>

    <script type="text/javascript">
        //<![CDATA[
        $(document).ready(function () {
            // Starts the content editor in the messages box - look at documentation on http://imperavi.com/redactor/
            $('#content').redactor();
            // This function loads the text from the cases list into the message-viewer
            $('.list-click').on('click', function () {
                that = $(this);
                $('.list-click').removeClass('active-case');
                that.addClass('active-case');
                //$('#message-viewer').html('Add conversation text here');
            });
            $('#sendmessage').on('click', function () {
                $('#message-viewer').append($('#content').val());
            });
            $(function () {
                // Declare a proxy to reference the hub.
                var chat = $.connection.chatHub;
                // Create a function that the hub can call to broadcast messages.
                chat.client.broadcastMessage = function (name, message) {
                    // Html encode display name and message.
                    var encodedName = '<%=session_name()%>'; // Grabbed the user's full name with a getter method.
                    var encodedMsg = $('<div />').text(message).html();
                    var encodeMsg = $('<div />').text(message).text().toString();
                    // Add the message to the page.
                    $('#message-viewer').append('<li><strong>' + encodedName
                    + '</strong>:&nbsp;' + encodeMsg + '</li>');
                };
                // Get the user name and store it to prepend to messages.
                //$('#displayname').val(prompt('Enter your name:', ''));
                // Set initial focus to message input box.
                $('#content').focus();
                // Start the connection.
                $.connection.hub.start().done(function () {
                    $('#sendmessage').on('click', function () {
                        // Call the Send method on the hub.
                        chat.server.send($('#displayname').val(), $('#content').val().replace('<p>', "").replace('</p>', ""));
                        // Clear text box and reset focus for next comment.
                        $('#message-viewer').val('').focus();
                        //$('#content').html('Add conversation text here')
                    });
                });
            });
        });
            //]]>
    </script>
</head>

<body>
    <!-- Navbar at top of page -->
    <nav class="navbar navbar-default">
        <div class="container-fluid">
            <!-- This is only displayed on mobiles and is the collapsable version of the nav bar -->
            <div class="navbar-header">
                <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                </button>
                <a class="navbar-brand" href="#">Logo/Icon thing</a>
            </div>
            <!-- Link in the nav bar -->
            <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                <ul class="nav navbar-nav">
                    <li class="active"><a href="#">Home</a></li>
                    <li><a href="#">Logout(set my onclick please)</a></li>
                </ul>
            </div>
            <!-- /.navbar-collapse -->
        </div>
        <!-- /.container-fluid -->
    </nav>
    <div class="col-md-4">
        <!-- List group to display the cases -->
        <ul class="list-group">

            <!-- First item is the popup to show all cases -->
            <!-- The popup is triggered by the data-toggel and data-target -->
            <li class="list-group-item text-center" data-toggle="modal" data-target="#myModal"><i
                class="fa fa-plus"></i>Add case</li>

            <!-- First item is the popup to show all cases -->
            <!-- The popup is triggered by the data-toggel and data-target -->
        </ul>
        <form id="Form1" method="POST" enctype="multipart/form-data" runat="server">
            <asp:GridView ID="usersCurrentChats" runat="server" AllowSorting="False" CssClass="mGrid"
                AutoGenerateColumns="True"
                DataSourceID="usersCurrentChats_SqlDataSource" OnRowDataBound="usersCurrentChats_OnRowDataBound" OnSelectedIndexChanged="usersCurrentChats_OnSelectedIndexChanged">
            </asp:GridView>
            <asp:SqlDataSource ID="usersCurrentChats_SqlDataSource" runat="server"
                ConnectionString="<%$ ConnectionStrings:WebAppConnString %>"
                ProviderName="<%$ ConnectionStrings:WebAppConnString.ProviderName %>"></asp:SqlDataSource>
    </div>
    <!-- End col-md-4 -->
    <div class="col-md-8">
        <!-- Tab pannle container -->
        <div role="tabpanel">
            <!-- Nav tabs -->
            <ul class="nav nav-tabs" role="tablist">
                <li role="presentation" class="active"><a href="#message" aria-controls="message" role="tab" data-toggle="tab"><i class="fa fa-comments"></i> Messages</a></li>
                <li role="presentation"><a href="#files" aria-controls="files" role="tab" data-toggle="tab"><i class="fa fa-file"></i> Files</a></li>
            </ul>
            <!-- Tab content panels -->
            <div class="tab-content">
                <!-- Messages container -->
                <div role="tabpanel" class="tab-pane active" id="message">
                    <!-- This contains the actual conversation -->
                    <div id="message-viewer"></div>

                    <asp:GridView ID="caseMessages" runat="server" AllowSorting="False" CssClass="mGrid"
                        AutoGenerateColumns="True"
                        DataSourceID="caseMessages_SqlDataSource">
                    </asp:GridView>
                    <asp:SqlDataSource ID="caseMessages_SqlDataSource" runat="server"
                        ConnectionString="<%$ ConnectionStrings:WebAppConnString %>"
                        ProviderName="<%$ ConnectionStrings:WebAppConnString.ProviderName %>"></asp:SqlDataSource>

                    <!-- This text area is editbalbe, the ID is used to start the text editor -->
                    <textarea id="content" name="content" cols="20" rows="1"></textarea>
                    &nbsp;<!-- Adds text to message-viewer - controlled from javascript function -->
                    <button type="button" id="sendmessage" class="btn btn-lg btn-primary" style="width: 100%;">
                        Send</button>
                </div>
                <!-- End .tab-pane -->

                <!-- Files container -->
                <div role="tabpanel" class="tab-pane" id="files">
                    <div id="file-viewer">
                        <!-- Upload form, look at the documentation on https://github.com/blueimp/jQuery-File-Upload -->
                        <!-- The fileupload-buttonbar contains buttons to add/delete files and start/cancel the upload -->
                        <br />
                        <asp:Label ID="lblMessage" runat="server" Text=""
                            Font-Names="Arial"></asp:Label>
                        <div class="row fileupload-buttonbar">
                            <div class="col-lg-7">
                                <!-- The fileinput-button span is used to style the file input field as button -->
                                <asp:FileUpload ID="FileUpload" runat="server" Text="Select File" CssClass="btn btn-success fileinput-button" />
                                <asp:Button ID="Upload_File_Button" runat="server" Text="Upload" UseSubmitBehaviour="true" CssClass="btn btn-primary start" OnClick="Button1_Click"></asp:Button>

                                <asp:Button ID="Cancel_Upload_Button" runat="server" Text="Cancel" UseResetBehaviour="true" CssClass="btn btn-warning cancel" type="reset" value="Clear"></asp:Button>

                                <asp:Button ID="Delete_File_Button" runat="server" Text="Delete" CssClass="btn btn-danger delete"></asp:Button>
                                <!-- The global file processing state -->
                                <span class="fileupload-process"></span>
                            </div>
                            <!-- The global progress state -->
                            <div class="col-lg-5 fileupload-progress fade">
                                <!-- The global progress bar -->
                                <div class="progress progress-striped active" role="progressbar" aria-valuemin="0" aria-valuemax="100">
                                    <div class="progress-bar progress-bar-success" style="width: 0%;"></div>
                                </div>
                                <!-- The extended global progress state -->
                                <div class="progress-extended">&nbsp;</div>
                            </div>
                        </div>
                    </div>
                    <!-- End #file-viewer -->
                </div>
                <!-- End .tab-pane -->
            </div>
            <!-- End #tab-content -->
        </div>
        <!-- End tabpanel -->
    </div>
    <!-- End .col-md-8 -->
    <!-- Modal - this is the all cases popup Keep ID unique to it can be loaded properly -->
    <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <!-- Modal heading includes the close button and title -->
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="myModalLabel">Modal title</h4>
                </div>
                <!-- Modal body shows all the cases -->
                <div class="modal-body">
                    <asp:GridView ID="allCases_GridView" runat="server" CssClass="mGrid" CellSpacing="10" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" OnRowDataBound="usersCurrentChats_OnRowDataBound" OnSelectedIndexChanged="usersCurrentChats_OnSelectedIndexChanged">
                        <RowStyle BackColor="White" ForeColor="#003399" />
                        <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                        <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                        <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                    </asp:GridView>
                </div>

                </form>
            </div>
        </div>
    </div>
</body>
</html>