using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDMSProject
{
    public class InputVaildation
    {

        //MySql.Data.MySqlClient.MySqlConnection conn;
        //MySql.Data.MySqlClient.MySqlCommand cmd;
        //MySql.Data.MySqlClient.MySqlDataReader reader;
        //String queryStr;


        public static bool validatePassword(String p1, String p2)
        {
            bool pass = true;
            if (p1.Equals(p2) == false)
            {
                pass = false;
            }
            return pass;
        }
        public static bool ValidateName(String input)
        {
            bool pass = true;
            var positiveIntRegex = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9]*$");
            if (positiveIntRegex.IsMatch(input)== false)
            {
                pass=false;
            }
            if (input.Trim().Length < 1)
            {
                pass = false;
            }
            return pass;
      }

        public static bool doesCaseExist(String casenumber)
        {
            bool registration = false;
            MySql.Data.MySqlClient.MySqlConnection conn;
            MySql.Data.MySqlClient.MySqlCommand cmd;
            MySql.Data.MySqlClient.MySqlDataReader reader;
            String queryStr;

            String connString = System.Configuration.ConfigurationManager.ConnectionStrings["projectConnectionString"].ToString();

            conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
            conn.Open();
            queryStr = "";

            queryStr = "SELECT case_id FROM project.allcases WHERE case_name=?cname";
            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            cmd.Parameters.AddWithValue("?cname", casenumber);

            reader = cmd.ExecuteReader();
            // int case_exists  = cmd.ExecuteNonQuery();

            if (reader.HasRows)
            {
                registration = true;

            }
            else
            {
                registration = false;
            }

            conn.Close();
            return registration;

        }


        public static bool ValidateUserInput(String input)
        {

            bool pass = true;
            var positiveIntRegex = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9]*$");


            if (positiveIntRegex.IsMatch(input) == false)
            {
                pass = false;

            }

            if (input.Length < 7 || input.Length > 15)
            {
                pass = false;

            }

            return pass;

        }

        public static bool ValidateEmail(String email)
        {

            bool pass = true;

            int index1 = email.IndexOf("@");

            int index2 = email.LastIndexOf("@");

            int num = email.Split('@').Length - 1;

            if (email.Split('@').Length - 1 > 1)
            {
                pass = false;
            }

            if (index1 != index2)
            {
                pass = false;
            }

            if (email.Trim() == "")
            {
                pass = false;
            }

            return pass;


        }
    }
}