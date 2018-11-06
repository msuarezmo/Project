//-----------------------------------------------------------------------
// <copyright file="SendEmail.cs" company="COEF">
//    Todos los derechos reservados
// </copyright>
//-----------------------------------------------------------------------

namespace CapaNegocio.Email
{
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using System;

    /// <summary>
    /// Defines the <see cref="SendEmail" />
    /// </summary>
    public class SendEmail
    {
        /// <summary>
        /// The SendMassive
        /// </summary>
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
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
