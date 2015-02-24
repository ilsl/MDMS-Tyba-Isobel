using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
namespace MDMSProject
{
    public class chatHub : Hub
    {
        public void Send(String name, String message)
        {
           // Call the broadcastMessage method to update clients.
            Clients.All.broadcastMessage(name, message);
            // call ths saveToDB message method to save the messages to database
            saveToDB(message);

        }
        String[] ID;
        LoggedIn x = new LoggedIn();
        
        protected void saveToDB(String message)
        {
            MySql.Data.MySqlClient.MySqlConnection conn;
            MySql.Data.MySqlClient.MySqlCommand cmd;
            String queryStr;
            ID = x.getUserIDcaseID(); 
            String connString = System.Configuration.ConfigurationManager.ConnectionStrings["projectConnectionString"].ToString();

            conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
            conn.Open();
            queryStr = "";
            //Inset the message along wiht user_id and case_id into the databases.
            queryStr = "INSERT INTO `project`.`messages` (`alphauser_id`, `case_id`, `messages_text`) VALUES (?x0, ?x1 , ?text)";
            cmd = new MySql.Data.MySqlClient.MySqlCommand(queryStr, conn);
            
            cmd.Parameters.AddWithValue("?x0", ID[0]);
            cmd.Parameters.AddWithValue("?x1", ID[1]);
            cmd.Parameters.AddWithValue("?text", message);

            cmd.ExecuteReader();
            conn.Close();
            
        }
    }
}

