﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.IO;

namespace MDMSProject
{
    public partial class LoggedIn : System.Web.UI.Page
    {
        MySql.Data.MySqlClient.MySqlConnection conn;
        MySql.Data.MySqlClient.MySqlCommand cmd;
        MySql.Data.MySqlClient.MySqlDataReader reader;
        String queryStr;
        static String uID, cID, case_name, name;
        protected HtmlInputFile fillMyFile;

        protected void Page_Load(object sender, EventArgs e)
        {
            name = (String)Session["uname"];
            uID = (String)Session["user_id"];

            if (name == null)
            {
                Response.BufferOutput = true;
                Response.Redirect("Login.aspx", false);
            }

            // userID used to query the DB to return usersCurrentChats, variable cannot be called from ASPX, therefore parameter given from here. Works correctly, as in only displays cases relevant to user.
            usersCurrentChats_SqlDataSource.SelectParameters.Add("uID", uID.ToString());
            usersCurrentChats_SqlDataSource.SelectCommand = "SELECT allcases.case_name, client.first_name, client.last_name FROM allcases, client, mapcases WHERE mapcases.user_id = @uID AND allcases.case_id = mapcases.case_id AND mapcases.user_id = client.user_id ";

            // GridView populated by DataTable which is populated by MySQL. No DataSource required.
            conn = new MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString());
            cmd = new MySqlCommand("SELECT case_name FROM project.allcases;", conn);
            conn.Open();
            DataTable dataTable = new DataTable();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);

            da.Fill(dataTable);

            allCases_GridView.DataSource = dataTable;
            allCases_GridView.DataBind();
        }

        public String session_name()
        {
            return name;
        }

        // This method runs when the page loads, iterating through the IF statement for every row in grid.
        // WHAT SHOULD HAPPEN WHEN MOUSE HOVERED OVER THE ROWS/CASES.
        protected void usersCurrentChats_OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.cursor='pointer';this.style.textDecoration='italics'; this.originalcolor=this.style.backgroundColor; this.style.backgroundColor='#E8E8E8 '";
                e.Row.Attributes["onmouseout"] = "this.style.textDecoration='none';this.style.backgroundColor=this.originalcolor";
                e.Row.Attributes["onclick"] = ClientScript.GetPostBackClientHyperlink(this.usersCurrentChats, "Select$" + e.Row.RowIndex);
            }
        }

        public void displayConversations(String user_ID_for_conversations, String case_ID_for_conversations)
        {
            caseMessages_SqlDataSource.SelectParameters.Remove(caseMessages_SqlDataSource.SelectParameters["user_ID_for_conversations"]);
            caseMessages_SqlDataSource.SelectParameters.Remove(caseMessages_SqlDataSource.SelectParameters["case_ID_for_conversations"]);
            caseMessages_SqlDataSource.SelectParameters.Add("user_ID_for_conversations", user_ID_for_conversations.ToString());
            caseMessages_SqlDataSource.SelectParameters.Add("case_ID_for_conversations", case_ID_for_conversations.ToString());
            caseMessages_SqlDataSource.SelectCommand = "SELECT messages.alphauser_id, messages.case_id, messages.messages_text FROM mapcases, messages WHERE messages.case_id = @case_ID_for_conversations AND messages.case_id = mapcases.case_id AND messages.alphauser_id = @user_ID_for_conversations";

        }

        // WHAT SHOULD HAPPEN WHEN ROW/CHAT IS CLICKED.
        protected void usersCurrentChats_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in usersCurrentChats.Rows)
            {
                if (row.RowIndex == usersCurrentChats.SelectedIndex)
                {
                    // Changes row colour when clicked.
                    row.BackColor = ColorTranslator.FromHtml("#4DB8B8");
                    row.ToolTip = string.Empty;

                    // Grab the case name which is clicked, meaning user is working on this case now.
                    case_name = Convert.ToString(usersCurrentChats.SelectedRow.Cells[0].Text);
                    cID = getUserIDcaseID().ElementAt(1);
                    //Isobel return the conversation table.
                    displayConversations(uID, cID);

                }
                else
                {
                    // OMG this is so important! This basically removes the highlight on the current row and onto the next row. Without this, all rows would become highlighted. With this, one a single row remains highlighted. Do not remove.
                    row.BackColor = ColorTranslator.FromHtml("#FFFFFF");
                    row.ToolTip = "Click to select";
                }
            }
        }


        protected void logoutEventMethod(object sender, EventArgs e)
        {
            Session["uname"] = null;
            Session["user_id"] = null;
            Session.Abandon();
            Response.BufferOutput = true;
            Response.Redirect("Login.aspx", false);
        }

        // This is used for saving the messages to the database.
        // The messages need to have correct userIDs - used to establish who sent them - and correct caseIdDs - used to establish which case the message belongs to.
        public String[] getUserIDcaseID()
        {
            // Array is tidier than multiple strings, make use of indexes.
            String[] IDs = new String[2];
            IDs[0] = uID; // Populated with the Session method during Page_Load - no need to query DB for this.

            String connString = System.Configuration.ConfigurationManager.ConnectionStrings["projectConnectionString"].ToString();
            conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
            conn.Open();

            queryStr = "";

            // Use case_name to obtain case ID from the allcases table.
            queryStr = "SELECT case_id FROM project.allcases WHERE case_name=?cname";
            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?cname", case_name);

            reader = cmd.ExecuteReader();
            while (reader.HasRows && reader.Read())
            {
                // Grab caseID.
                IDs[1] = reader.GetString(reader.GetOrdinal("case_id"));
            }
            reader.Close();
            // Return the array, therefore method can be called and assigned to varaible due to return - efficient.
            return IDs;
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            String connString = System.Configuration.ConfigurationManager.ConnectionStrings["projectConnectionString"].ToString();
            conn = new MySql.Data.MySqlClient.MySqlConnection(connString);

            string filename = Path.GetFileName(FileUpload.PostedFile.FileName);
            string contentType = FileUpload.PostedFile.ContentType;

            Stream fs = FileUpload.PostedFile.InputStream;
            BinaryReader br = new BinaryReader(fs);
            byte[] bytes = br.ReadBytes((Int32)fs.Length);

            queryStr = "INSERT INTO `project`.`documents` (`document_type`, `document_name`, `document_contents`, `betauser_id`, `betacase_id`) VALUES (@Type, @FileName, @File, @uname, @cname)";
            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("@Type", contentType);
            cmd.Parameters.AddWithValue("@FileName", filename);
            cmd.Parameters.AddWithValue("@File", bytes);
            cmd.Parameters.AddWithValue("@uname", uID);
            cmd.Parameters.AddWithValue("@cname", cID);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = "File Uploaded Successfully";
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = ex.ToString();
            }
        }
    }
}