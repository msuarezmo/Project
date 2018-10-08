using CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class Validations
    {
        private colegioEntities db = new colegioEntities();
        public int ValidateSchedule(Schedule schedule)
        {
            try
            {
                var test = db.Schedule.Where(x => x.IdDay == schedule.IdDay).ToList();
                foreach (var item in test)
                {
                    bool hour = ValidateHour(item, schedule);
                    bool teacher = ValidateHour(item, schedule);
                    if (teacher && hour)
                    {
                        return 1;
                    }
                }
                return 0;
            }
            catch (Exception)
            {

                return 0;
            }
        }
        private bool ValidateHour(Schedule item, Schedule origin)
        {
            try
            {
                if (origin.HourFrom > item.HourFrom && origin.HourTo < item.HourTo)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private bool ValidateTeacher(Schedule item, Schedule origin)
        {
            try
            {
                bool hour = ValidateHour(item, origin);
                if (hour && item.IdTeacher == origin.IdTeacher )
                {
                    return false;
                }
                
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
