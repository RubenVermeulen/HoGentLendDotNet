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
        public DateTime? Ophaalmoment { get; set; }
        public DateTime? Indienmoment { get; set; }
        public DateTime? Reservatiemoment { get; set; }
        public bool Opgehaald { get; set; }
        public virtual List<ReservatieLijn> ReservatieLijnen  { get; set; }

        private Reservatie()
        {
            ReservatieLijnen = new List<ReservatieLijn>();
        }

        public Reservatie(Gebruiker lener,
            DateTime ophaalMoment,
            DateTime indienMoment
            ) : this()
        {
            if (lener == null)
            {
                throw new ArgumentNullException("Een lener is verplicht.");
            }
            if (ophaalMoment == null)
            {
                throw new ArgumentNullException("Een ophaalmoment is verplicht.");
            }
            if(indienMoment == null)
            {
                throw new ArgumentNullException("Een indienmoment is verplicht.");
            }
            this.Lener = lener;
            this.Ophaalmoment = ophaalMoment;
            this.Indienmoment = indienMoment;

            this.Opgehaald = false;
            this.Reservatiemoment = DateTime.Now;
        }

        internal void AddReservationLine(Materiaal materiaal, long amount, DateTime ophaalDatum, DateTime indienDatum)
        {
            ReservatieLijn reservatieLijn = new ReservatieLijn(amount, indienDatum, ophaalDatum, materiaal, this);
            ReservatieLijnen.Add(reservatieLijn);
        }
    }
}