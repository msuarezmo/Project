using CapaDominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio.Validations
{
    public class ValidationsAssistances : Datamodel
    {
        public IEnumerable<Assistances> GetAllAsistences()
        {

            try
            {
                return db.Assistances;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Assistances SearchById(int? id)
        {
            try
            {
                return db.Assistances.Find(id);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool SaveAssistances(List<Assistances> assistances)
        {
            try
            {
                foreach (Assistances assistance in assistances)
                {
                    SaveAssistance(assistance);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void SaveAssistance(Assistances assistance)
        {
            try
            {
                db.Assistances.Add(assistance);
                db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
