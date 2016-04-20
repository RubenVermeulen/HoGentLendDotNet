using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.Domain
{
    public class Reservatie
    {

        public long Id { get; private set; }

        public virtual Gebruiker Lener { get; set; }
        public DateTime Ophaalmoment { get; set; }
        public DateTime Indienmoment { get; set; }
        public DateTime Reservatiemoment { get; set; }
        public bool Opgehaald { get; set; }
        //ReservatieLijn bestaat nog niet
        //public virtual List<ReservatieLijn> ReservatieLijnen  { get; set; }


    }
}