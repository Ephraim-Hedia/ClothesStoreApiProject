using System.Net;
using System.Net.Mail;

namespace Store.Services.Helper.Email
{
    public class EmailSetting
    {
        public static void SendEmail(TempEmail email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;

            client.Credentials = new NetworkCredential("ephraimhedia2@gmail.com", "hsgdftfrontnehnj");
            client.Send("ephraimhedia2@gmail.com", email.To, email.Title, email.Body);

        }
    }
}
