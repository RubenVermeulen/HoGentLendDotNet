using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;
using Ninject.Infrastructure.Language;

namespace HoGentLend.ViewModels
{
    public class ReservatieViewModel
    {

        public long Id { get; private set; }

        [DisplayName("Lener")]
        public string Lener { get; set; }

        [DisplayName("Ophaalmoment")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime Ophaalmoment { get; set; }

        [DisplayName("Indienmoment")]
        [DisplayFormat(DataFormatString = "{0:dd-MM/yyyy}")]
        public DateTime Indienmoment { get; set; }

        [DisplayName("Reservatiemoment")]
        [DisplayFormat(DataFormatString = "{0:dd MM yyyy}")]
        public DateTime Reservatiemoment { get; set; }

        [DisplayName("Opgehaald")]
        public bool Opgehaald { get; set; }

        public List<ReservatieLijnViewModel> ReservatieLijnen { get; set; }


        public ReservatieViewModel(Reservatie r)
        {

            Id = r.Id;
            Ophaalmoment = r.Ophaalmoment;
            Indienmoment = r.Indienmoment;
            Reservatiemoment = r.Reservatiemoment;
            Opgehaald = r.Opgehaald;


            ReservatieLijnen = r.ReservatieLijnen?.Select(rl => new ReservatieLijnViewModel(rl)).ToList();

        }

    }
}