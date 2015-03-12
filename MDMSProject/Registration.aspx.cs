using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace MDMSProject
{
    public partial class Registration : System.Web.UI.Page
    {
        MySql.Data.MySqlClient.MySqlConnection conn;
        MySql.Data.MySqlClient.MySqlCommand cmd;
        MySql.Data.MySqlClient.MySqlDataReader reader;
        String queryStr;
        static String generateUsername, password;
        static String userIDa, caseIDa;

        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void registerEventMethod(object sender, EventArgs e)
        {
            //registeruser();
            registerUserWithSlowHash();
        }

        private void registerUserWithSlowHash()
        {
            bool methodStatus = true;

            if (InputVaildation.ValidateName(firstnametextbox.Text) == false)
            {
                methodStatus = false;
            }

            if (InputVaildation.ValidateName(lastnametextbox.Text) == false)
            {
                methodStatus = false;
            }

            if (InputVaildation.ValidateEmail(emailtextbox.Text) == false)
            {
                methodStatus = false;
            }
            if (methodStatus == true)
            {

                String connString = System.Configuration.ConfigurationManager.ConnectionStrings["projectConnectionString"].ToString();

                conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
                conn.Open();
                queryStr = "";

                generateUsername = firstnametextbox.Text.Substring(0, 1) + lastnametextbox.Text;

                queryStr = "INSERT INTO `project`.`client` ( `first_name`, `last_name`,`license_type` , `email`, `username`, `slowHashSalt`)" +
                "VALUES(?firstname, ?lastname, ?license, ?email, ?uname, ?slowHashSalt)";


                cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
                cmd.Parameters.AddWithValue("?firstname", firstnametextbox.Text);
                cmd.Parameters.AddWithValue("?lastname", lastnametextbox.Text);
                cmd.Parameters.AddWithValue("?license", drop1.Text);
                cmd.Parameters.AddWithValue("?email", emailtextbox.Text);
                cmd.Parameters.AddWithValue("?uname", generateUsername);

                password = generatePassword();
                

                String saltHashReturned = PasswordHash.CreateHash(password);
                int commaIndex = saltHashReturned.IndexOf(":");
                String extractedString = saltHashReturned.Substring(0, commaIndex);

                commaIndex = saltHashReturned.IndexOf(":");
                extractedString = saltHashReturned.Substring(commaIndex + 1);
                commaIndex = extractedString.IndexOf(":");
                String salt = extractedString.Substring(0, commaIndex);

                commaIndex = extractedString.IndexOf(":");
                extractedString = extractedString.Substring(commaIndex + 1);
                String hash = extractedString;
                // from the first : to the second : is the salt
                // from the second : to the end is the hash
                cmd.Parameters.AddWithValue("?slowHashSalt", saltHashReturned);
                cmd.ExecuteReader();
                conn.Close();
               // getUserID();
                SendMail.Email(firstnametextbox.Text, lastnametextbox.Text, emailtextbox.Text, generateUsername, password);
                  ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Registration successful.  Client has been emailed with their login details.')", true);

                //Response.BufferOutput = true;
                //Response.Redirect("Login.aspx", false);

            }

            }
        
        // This is the original way to register a user without any form of encryption.
        //private void registeruser()
        //{
        // String connString = System.Configuration.ConfigurationManager.ConnectionStrings["projectConnectionString"].ToString();
        // conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
        // conn.Open();
        // querystr = "";
        // querystr = "INSERT INTO `project`.`client` ( `first_name`, `last_name`, `license_type`, `email`)" + "VALUES ( '" + firstnametextbox.Text + "', '" + lastnametextbox.Text + "', '" + drop1.Text + "', '" + emailtextbox.Text + "')";
        // cmd = new MySql.Data.MySqlClient.MySqlCommand(querystr, conn);
        // cmd.ExecuteReader();
        // conn.Close();
        //}
        private Int32 generateRandomBits()
        {

            RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
            byte[] randomBytes = new byte[32];
            random.GetBytes(randomBytes);
            Int32 randomInt = BitConverter.ToInt32(randomBytes, 0);
            return randomInt;
        }

        private String generatePassword()
        {

            StringBuilder builder = new StringBuilder();
            int len = 10;
            String PossibleChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] possibleChars = PossibleChars.ToCharArray();


            for (int i = 0; i < len; i++)
            {
                // Get our cryptographically random 32-bit integer & use as seed in Random class
                // NOTE: random value generated PER ITERATION, meaning that the System.Random class
                // is re-instantiated every iteration with a new, crytographically random numeric seed.
                int randInt32 = generateRandomBits();
                Random r = new Random(randInt32);

                int nextInt = r.Next(possibleChars.Length);
         
                char c = possibleChars[nextInt];
                builder.Append(c);
            }
            // Set the text box "Text" property using final, constructed string
           // passwordtextbox.Text = builder.ToString();
            return builder.ToString();

        }

        // This method is used to grab the user and case ID to map the cases.  No longer needed because case number is no longer entered when registering a user  However, will be used again when opening a NEW CHAT and mapping the case and then mapping the messages.
        private void getUserID()
        {
            String connString = System.Configuration.ConfigurationManager.ConnectionStrings["WebAppConnString"].ToString();
            conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
            conn.Open();
            queryStr = "";

            queryStr = "SELECT user_id FROM project.client WHERE username=?uname";
            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?uname", generateUsername);

            reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                userIDa = reader.GetString(reader.GetOrdinal("user_id"));
            }

            reader.Close();

            queryStr = "SELECT case_id FROM project.allcases WHERE case_name=?cname";
            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?cname", "INPUT CASE REF");

            reader = cmd.ExecuteReader();

            while (reader.HasRows && reader.Read())
            {
                caseIDa = reader.GetString(reader.GetOrdinal("case_id"));
            }

            reader.Close();

            queryStr = "INSERT INTO project.mapcases (user_id, case_id) VALUES (?userIDa, ?caseIDa )";
            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?userIDa", userIDa);
            cmd.Parameters.AddWithValue("?caseIDa", caseIDa);
            cmd.ExecuteReader();
            conn.Close();
        }

    }

}