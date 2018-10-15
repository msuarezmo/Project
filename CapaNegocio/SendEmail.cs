using System;
using System.Net;
using System.Net.Mail;

namespace CapaNegocio
{
   public class SendEmail
    {
        public string SendNews(string email, string titulo, string contenido)
        {

            try
            {
                var senderEmail = new MailAddress("nperilla22@gmail.com", "Demo");
                var receiverMail = new MailAddress(email, "Receiver");
                var password = "nelson1234_";
                var subject = titulo;
                var body = contenido;

                var stmp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)

                };
                using (var mess = new MailMessage(senderEmail, receiverMail)
                {
                    Subject = titulo,
                    Body = body,
                }
                )
                {
                    stmp.Send(mess);
                }
            }
            catch (Exception)
            {

                throw;
            }
          
            return "Enviando.....";
        }

        }
}
