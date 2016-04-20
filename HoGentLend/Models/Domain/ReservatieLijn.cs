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
        public DateTime? IndienMoment { get; set; }
        public DateTime? OphaalMoment { get; set; }
        public virtual Materiaal Materiaal { get; set; }
        public virtual Reservatie Reservatie { get; set; }
        public long ReservatieId { get; set; }

        private ReservatieLijn()
        {

        }

        public ReservatieLijn(long amount, DateTime ophaalMoment, DateTime indienMoment, Materiaal mat, Reservatie r) : this()
        {
            if (mat == null)
            {
                throw new ArgumentNullException("Een materiaal is verplicht.");
            }
            if (ophaalMoment == null)
            {
                throw new ArgumentNullException("Een ophaalmoment is verplicht.");
            }
            if (indienMoment == null)
            {
                throw new ArgumentNullException("Een indienmoment is verplicht.");
            }
            if (amount < 0)
            {
                throw new ArgumentException("De reservatie hoeveelheid moet groter zijn dan 0.");
            }
            this.Amount = amount;
            this.IndienMoment = indienMoment;
            this.OphaalMoment = ophaalMoment;
            this.Materiaal = mat;
            this.Reservatie = r;
        }
    }
}