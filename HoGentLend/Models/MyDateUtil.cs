using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoGentLend.Models
{
    public class MyDateUtil
    {
        public static bool DoesFirstPairOverlapWithSecond(DateTime a1, DateTime a2, DateTime? b1, DateTime? b2)
        {
            if (a1 == null && a2 == null)
            {
                return false;
            }
            if (a1 == null)
            {
                a1 = a2;
            }
            if (a2 == null)
            {
                a2 = a1;
            }
            if (a2 < a1 || b2 < b1)
            {
                return false;
            }

            a1 = a1.AddHours(-a1.Hour).AddMinutes(-a1.Minute).AddSeconds(-a1.Second);
            a2 = a2.AddHours(23 - a2.Hour).AddMinutes(59 - a2.Minute).AddSeconds(59 - a2.Second);

            if (a2 > b1 && a2 < b2)
            {
                return true;
            }
            if (a1 > b1 && a1 < b2)
            {
                return true;
            }
            if (a1 < b1 && a2 > b2)
            {
                return true;
            }
            return false;
        }
    }
}