using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Email
{
    public class SendEmail
    {
        public void SendMassive()
        {
            try
            {
                var apiKey = Environment.GetEnvironmentVariable("ClaveCG");
                SendGridClient client = new SendGridClient(apiKey);
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress("cgonzalezvarela10@gmail.com", "COEF"),
                    Subject = "Novedad",
                    PlainTextContent = "Test cg",
                    HtmlContent = "<strong>Ejemplo de una novedad</strong>"
                };
                msg.AddTo(new EmailAddress("msuarezmo@uninpahu.edu.co", "Maria Suarez"));
                msg.AddTo(new EmailAddress("cgonzalezva@uninpahu.edu.co", "Cristian Gonzalez"));
                msg.AddTo(new EmailAddress("nperillaro@uninpahu.edu.co", "Nelson perilla"));
                client.SendEmailAsync(msg);
                //    var msg = new SendGridMessage();
                //    msg.SetFrom(new EmailAddress("cgonzalezvarela10.com", "SendGrid DX Team"));
                //    var recipients = new List<EmailAddress>
                //{
                //    new EmailAddress("msuarezmo@uninpahu.edu.co", "Maria Suarez"),
                //    new EmailAddress("cgonzalezva@uninpahu.edu.co", "Cristian Gonzalez"),
                //    new EmailAddress("nperillaro@uninpahu.edu.co", "Nelson perilla")
                //};
                //    msg.AddTos(recipients);
                //    msg.SetSubject("Testing the SendGrid C# Library");
                //    msg.AddContent(MimeType.Text, "Hello World plain text!");
                //    msg.AddContent(MimeType.Html, "<p>Hello World!</p>");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
