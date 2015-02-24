using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

namespace MDMSProject
{
    public partial class Login : System.Web.UI.Page
    {
        MySql.Data.MySqlClient.MySqlConnection conn;
        MySql.Data.MySqlClient.MySqlCommand cmd;
        MySql.Data.MySqlClient.MySqlDataReader reader;
        String querystr;
        String name;
        String user_id;


        protected void Page_Load(object sender, EventArgs e)
        {
        }
             protected void submitEventMethod(object sender, EventArgs e)
             {
                 LoginWithPasswordHashFunction();
              
                 //if (checkAgainstWhiteList(usernametextbox.Text) == true &&
                 //    checkAgainstWhiteList(userpasswordtextbox.Text) == true)
                 //{
                 //    DoSQLQuery();
                 //}
                 //else
                 //{
                 //    userpasswordtextbox.Text = "Does not pass White List test";
                 //}

        }

            

             private void LoginWithPasswordHashFunction()
             {
                 List<String> salthashList = null;
                 List<String> namesList = null;
                 try
                 {
                     String connString = System.Configuration.ConfigurationManager.ConnectionStrings["projectConnectionString"].ToString();

                     conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
                     conn.Open();
                     querystr = "SELECT `slowHashSalt`, `first_name`, `last_name`, `user_id` FROM `project`.`client` WHERE username=?uname";

                     cmd = new MySql.Data.MySqlClient.MySqlCommand(querystr, conn);
                     cmd.Parameters.AddWithValue("?uname", usernametextbox.Text);
                     reader = cmd.ExecuteReader();

                     while (reader.HasRows && reader.Read())
                     {
                         if (salthashList == null)
                         {
                             salthashList = new List<String>();
                             namesList = new List<String>();
                         }
                         String saltHashes = reader.GetString(reader.GetOrdinal("slowHashSalt"));
                         salthashList.Add(saltHashes);

                         String fullname = reader.GetString(reader.GetOrdinal("first_name")) + " " + reader.GetString(reader.GetOrdinal("last_name"));
                         namesList.Add(fullname);

                         user_id=reader.GetString(reader.GetOrdinal("user_id"));
                       
                     }
                     reader.Close();
                     //check to see whether the results from the query are empty
                     if (salthashList != null)
                     {
                         for (int i = 0; i < salthashList.Count; i++)
                         {
                             querystr = "";
                             bool ValidUser = PasswordHash.ValidatePassword(userpasswordtextbox.Text, salthashList[i]);
                             if (ValidUser == true)
                             {
                        
                                 Session["uname"] = namesList[i];
                                 Session["user_id"] = user_id;
                                 Response.BufferOutput = true;
                                 Response.Redirect("LoggedIn.aspx", false);
                             }
                             else
                             {
                                 userpasswordtextbox.Text = "User not authenticated";
                             }
                         }
                     }
                 }
                 catch (Exception ex)
                 {
                     userpasswordtextbox.Text = ex.ToString();
                 }
             }
             private bool checkAgainstWhiteList(string userInput)
             {
                 var regExpression = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9]*$");
                 if (regExpression.IsMatch(userInput))
                 {
                     return true;
                 }
                 else
                 {
                     return false;
                 }
             }
             private void DoSQLQuery()
             {
                 try
                 {
                     String connString = System.Configuration.ConfigurationManager.ConnectionStrings["projectConnectionString"].ToString();
                     conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
                     conn.Open();
                     querystr = "";
                     //querystr = "SELECT * FROM project.client WHERE user_password= '" + userpasswordtextbox.Text + "' AND user_id= '" + Useridtextbox.Text + "'";
                     //cmd = new MySql.Data.MySqlClient.MySqlCommand(querystr, conn);

                     querystr = "SELECT * FROM project.client WHERE user_password=?pword  AND username=?uname";
                     cmd = new MySql.Data.MySqlClient.MySqlCommand(querystr, conn);
                     cmd.Parameters.AddWithValue("?uname", usernametextbox.Text);
                     cmd.Parameters.AddWithValue("?pword", userpasswordtextbox.Text);

                     reader = cmd.ExecuteReader();
                     name = "";
                     while (reader.HasRows && reader.Read())
                     {
                         name = reader.GetString(reader.GetOrdinal("first_name")) + " " +
                              reader.GetString(reader.GetOrdinal("last_name"));

                     }

                     if (reader.HasRows)
                     {
                         Session["uname"] = name;
                         Session["user_id"] = user_id;
                         Response.BufferOutput = true;
                         Response.Redirect("LoggedIn.aspx", false);
                     }

                     else
                     {

                         userpasswordtextbox.Text = "invalid user";
                     }

                     reader.Close();
                     conn.Close();
                 }
                 catch (Exception e)
                 {
                     userpasswordtextbox.Text = e.ToString();
                 }
             }
    }
}