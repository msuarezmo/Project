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
        public int? ValidateSchedule(Schedule schedule)
        {
            try
            {
                var val = (from x in db.Schedule
                           where x.IdDay == schedule.IdDay
                           && schedule.HourFrom >= x.HourFrom
                           && schedule.HourFrom >=x.HourTo
                           //&& schedule.HourTo <= x.HourTo
                           && schedule.IdCourse == x.IdCourse
                           && schedule.IdTeacher == x.IdTeacher
                           select x).ToList();

                //var val = db.Schedule.Where(x => x.IdDay == schedule.IdDay).ToList();
                
                if (val.Count == 0)
                {
                    return 1;
                }
                else{
                    return 0;
                }
                //foreach (var item in val)
                //{
                //    bool hour = ValidateHour(item, schedule);
                //    if (hour)
                //    {
                //        return 1;
                //        //bool? teacher = ValidateTeacher(item, schedule);
                //        //if (teacher == true)
                //        //{
                //        //    return 1;
                //        //}
                //        //else
                //        //{
                //        //    return 2;
                //        //}
                //    }
                //    else
                //    {
                //        return 0;
                //    }

                //}
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private bool ValidateHour(Schedule item, Schedule origin)
        {
            try
            {
                if (origin.HourFrom > item.HourFrom && origin.HourFrom <= item.HourTo && origin.HourTo >= item.HourFrom && origin.HourTo <= item.HourTo)
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
        private bool? ValidateTeacher(Schedule item, Schedule origin)
        {
            try
            {
                bool hour = ValidateHour(item, origin);
                if (!hour)
                {
                    return null;
                }
                if (hour && item.IdTeacher == origin.IdTeacher)
                {
                    return false;
                }
                if (hour && item.IdTeacher != origin.IdTeacher)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
