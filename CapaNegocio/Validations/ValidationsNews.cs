//-----------------------------------------------------------------------
// <copyright file="ValidationsNews.cs" company="COEF">
//    Todos los derechos reservados
// </copyright>
//-----------------------------------------------------------------------

namespace CapaNegocio.Validations
{
    using CapaDominio;
    using CapaNegocio.Email;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="ValidationsNews" />
    /// </summary>
    public class ValidationsNews : Datamodel
    {
        private ValidationStudents validationStudents = new ValidationStudents();
        private ValidationsUser validationsUser = new ValidationsUser();
        private SendEmail sendEmail = new SendEmail();
        /// <summary>
        /// The createNews
        /// </summary>
        /// <param name="news">The news<see cref="News"/></param>
        /// <returns>The <see cref="bool?"/></returns>
        public bool? CreateNews(News news)
        {
            try
            {
                db.News.Add(news);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Elimina novedad
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteNews(int id)
        {
            try
            {
                News news = SearchById(id);
                db.News.Remove(news);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// The EditNews
        /// </summary>
        /// <param name="news">The news<see cref="News"/></param>
        /// <returns>The <see cref="bool?"/></returns>
        public bool? EditNews(News news)
        {
            try
            {
                var query = db.News;
                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool? SendNew(News news)
        {
            try
            {
                var fecha = DateTime.UtcNow.AddHours(-5);
                //es Docente
                if (news.IdCourse != null)
                {
                    var students = validationStudents.GetAllStudents().Where(x => x.CourseId == news.IdCourse).ToList();
                    if (students.Count() > 0)
                    {
                        var users = validationsUser.GetAllParents().Where(z => students.Any(x => x.ParentId == z.Id)).ToList();
                        if (users.Count() > 0)
                        {
                            sendEmail.SendMassive(users, news);
                            news.LastSend = fecha;
                            db.Entry(news).State = EntityState.Modified;
                            db.SaveChanges();
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }

                }
                //Es admin
                else
                {
                    var users = validationsUser.GetAllParents().ToList();
                    if (users.Count() > 0)
                    {
                        sendEmail.SendMassive(users, news);
                        news.LastSend = fecha;
                        db.Entry(news).State = EntityState.Modified;
                        db.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Devuelve todas las novedades
        /// </summary>
        /// <returns>The <see cref="IEnumerable{News}"/></returns>
        public IEnumerable<News> GetAllNews()
        {
            return db.News;
        }

        /// <summary>
        /// Busca novedad por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public News SearchById(int? id)
        {
            try
            {
                return db.News.Find(id);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
