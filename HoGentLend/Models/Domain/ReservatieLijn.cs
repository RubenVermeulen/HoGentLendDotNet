using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.Domain
{
    public class ReservatieLijn
    {

        public long Id { get; private set; }

        public long Amount { get; set; }
        public DateTime IndienMoment { get; set; }
        public DateTime OphaalMoment { get; set; }
        public virtual Materiaal Materiaal { get; set; }
        public virtual Reservatie Reservatie { get; set; }
    }
}