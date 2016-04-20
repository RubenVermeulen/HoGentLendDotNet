using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;

namespace HoGentLend.ViewModels
{
    public class ReservatieLijnViewModel
    {

        public MateriaalViewModel Materiaal { get; set; }
        public ReservatieLijnViewModel(ReservatieLijn r)
        {


            Materiaal = new MateriaalViewModel(r.Materiaal);

        }

    }
}