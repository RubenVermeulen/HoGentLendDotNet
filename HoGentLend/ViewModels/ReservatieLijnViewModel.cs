using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;

namespace HoGentLend.ViewModels
{
    public class ReservatieLijnViewModel
    {
        public long Id { get; private set; }

        [DisplayName("Hoeveelheid")]
        public long Amount { get; set; }

        [DisplayName("Indiendmoment")]
        public DateTime? IndienMoment { get; set; }

        [DisplayName("Ophaalmoment")]
        public DateTime? OphaalMoment { get; set; }

        [DisplayName("Materiaal")]
        public MateriaalViewModel Materiaal { get; set; }

        public ReservatieLijnViewModel(ReservatieLijn r)
        {
            Amount = r.Amount;
            IndienMoment = r.IndienMoment;
            OphaalMoment = r.OphaalMoment;
            Materiaal = new MateriaalViewModel(r.Materiaal);

            Materiaal = new MateriaalViewModel(r.Materiaal);

        }

        
    }
}