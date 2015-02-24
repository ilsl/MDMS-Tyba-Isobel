
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace MDMSProject
{
    public class SendMail
    {
        // https://accounts.google.com/DisplayUnlockCaptcha

        public static void Email(String firstname, String lastname, String email, String username, String password)
        {
            try
            {
                // Gmail Address from where you send the mail
                var fromAddress = "badwolf967@gmail.com";
                // any address where the email will be sending
                var toAddress = email;
                //Password of your gmail address
                const string fromPassword = "x/factor/210";
                // Passing the values and make a email formate to display
                string subject = "Login Confirmation";
                string body = "Dear " + firstname + " " + lastname + ",\n\n" + "Here are your login details.\n\n";
                body += "Username: " + username + "\n";
                body += "Password: " + password + "\n\n\nThank you.\nEclipse Legal Systems";

                // smtp settings
                var smtp = new System.Net.Mail.SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential(fromAddress, fromPassword);
                    smtp.Timeout = 20000;
                }
                // Passing values to smtp object
                smtp.Send(fromAddress, toAddress, subject, body);

                // POP UP CONFIRMING EMAIL HAS BEEN SENT TO CLIENT
            }
            catch (Exception)
            {

            }
        }
    }
}

