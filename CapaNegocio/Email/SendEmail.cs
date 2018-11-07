//-----------------------------------------------------------------------
// <copyright file="SendEmail.cs" company="COEF">
//    Todos los derechos reservados
// </copyright>
//-----------------------------------------------------------------------

namespace CapaNegocio.Email
{
    using CapaDominio;
    using SendGrid;
    using SendGrid.Helpers.Mail;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="SendEmail" />
    /// </summary>
    public class SendEmail
    {
        private ValidationsCourse validationsCourse = new ValidationsCourse();
        /// <summary>
        /// The SendMassive
        /// </summary>
        public void SendMassive(List<AspNetUsers> users, News news)
        {
            try
            {
                var apiKey = Environment.GetEnvironmentVariable("ClaveCG");
                SendGridClient client = new SendGridClient(apiKey);
                if (news.IdCourse != null)
                {
                    var course = validationsCourse.SearchById(news.IdCourse);
                    var msgTeacher = new SendGridMessage()
                    {
                        From = new EmailAddress("cgonzalezvarela10@gmail.com", "COEF"),
                        Subject = "Novedad",
                        PlainTextContent = "",
                        HtmlContent = "<strong>Esta novedad aplica para acudientes que tengan matriculados hij@s en el curso " + course.Description + " </strong>" +
                        "<br/> <br/> <p>" + news.Description + "</p>"
                    };
                    foreach (var user in users)
                    {
                        msgTeacher.AddTo(new EmailAddress(user.Email, user.FullName));
                    }
                    client.SendEmailAsync(msgTeacher);
                }
                else
                {
                    var msgAdmin = new SendGridMessage()
                    {
                        From = new EmailAddress("cgonzalezvarela10@gmail.com", "COEF"),
                        Subject = "Novedad",
                        PlainTextContent = "",
                        HtmlContent = "<p>" + news.Description + "</p>"
                    };
                    foreach (var user in users)
                    {
                        msgAdmin.AddTo(new EmailAddress(user.Email, user.FullName));
                    }
                    client.SendEmailAsync(msgAdmin);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void PostRegister(AspNetUsers user)
        {
            try
            {
                var apiKey = Environment.GetEnvironmentVariable("ClaveCG");
                SendGridClient client = new SendGridClient(apiKey);
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress("cgonzalezvarela10@gmail.com", "COEF"),
                    Subject = "Registro exitoso",
                    PlainTextContent = "Test cg",
                    HtmlContent =
                    "<p> <strong>A sido registrado con éxito en la plataforma COEF</strong><br/>" +
                    "Ingrese a traves del siguiente enlace <a href='https://admincolegios.azurewebsites.net/'>COEF </a> <br/>" +
                    "Sus credenciales son: <br/>" +
                    "Usuario: " + user.UserName + "<br/>" +
                    "Contraseña: Su documento de identidad" +
                    "</p>"
                };
                msg.AddTo(new EmailAddress(user.Email, user.FullName));
                client.SendEmailAsync(msg);

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
