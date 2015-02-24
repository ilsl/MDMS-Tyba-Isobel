using System;
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
            //else
            //{
            //    //userLabel.Text = name;
            //}

           
            // userID used to query the DB to return usersCurrentChats, variable cannot be called from ASPX, therefore parameter given from here.  Works correctly, as in only displays cases relevant to user.
            usersCurrentChats_SqlDataSource.SelectParameters.Add("uID", uID.ToString());
            usersCurrentChats_SqlDataSource.SelectCommand = "SELECT allcases.case_name, client.first_name, client.last_name FROM allcases, client, mapcases WHERE mapcases.user_id = @uID AND allcases.case_id = mapcases.case_id AND mapcases.user_id = client.user_id ";

            // GridView populated by DataTable which is populated by MySQL.  No DataSource required.
            conn = new MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["projectConnectionString"].ToString());
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



        //  This method runs when the page loads, iterating through the IF statement for every row in grid.
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
                }
                else
                {
                    // OMG this is so important!  This basically removes the highlight on the current row and onto the next row.  Without this, all rows would become highlighted.  With this, one a single row remains highlighted. Do not remove.
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

        //protected void button_Click(object sender, EventArgs e)
        //{
        //    WriteToDB();
        //}


        // Writes file to the database
         //HttpPostedFile myFile = fillMyFile.PostedFile;
         //   int nFileLen = myFile.ContentLength;
         //   byte[] myData = new byte[nFileLen];
         //   myFile.InputStream.Read(myData, 0, nFileLen);
        public void WriteToDB(String strName, String strType, ref byte[] Buffer)
        {

            // Create connection

            String connString = System.Configuration.ConfigurationManager.ConnectionStrings["projectConnectionString"].ToString();
            conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
            // Open Connection
            conn.Open();
            queryStr = "INSERT INTO `project`.`documents` (`alphauser_id`, `document_type`, `case_id`, `document_name`, `document_size`) VALUES (?uname, ?ContentType, ?cname, ?FileName, ?ContentLength)";
           
            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?ContentLength", Buffer.Length);
            cmd.Parameters.AddWithValue("?ContentType", strType);
            cmd.Parameters.AddWithValue("?FileName", strName);
            cmd.Parameters.AddWithValue("?InputStream", Buffer);
            cmd.Parameters.AddWithValue("?uname", uID);
            cmd.Parameters.AddWithValue("?cname", cID);
            //method2
            //cmd.Parameters.AddWithValue("?ContentLength", myFile.ContentLength);
            //cmd.Parameters.AddWithValue("?ContentType", myFile.ContentType);
            //cmd.Parameters.AddWithValue("?FileName", myFile.FileName);
            //cmd.Parameters.AddWithValue("?InputStream", myFile.InputStream);
            conn.Close();
            
        }
    }
}