//-----------------------------------------------------------------------
// <copyright file="ValidationsNews.cs" company="COEF">
//    Todos los derechos reservados
// </copyright>
//-----------------------------------------------------------------------

namespace CapaNegocio.Validations
{
    using CapaDominio;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;

    /// <summary>
    /// Defines the <see cref="ValidationsNews" />
    /// </summary>
    public class ValidationsNews : Datamodel
    {
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
