using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace HoGentLend.Models.Domain
{
    public class Gebruiker
    {
        public long Id { get; private set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsLector { get; set; }

        public virtual VerlangLijst WishList { get; set; }
        public virtual List<Reservatie> Reservaties { get; set; }

        public Gebruiker()
        {
            Reservaties = new List<Reservatie>();
        }

        public bool DoShowAllMaterials()
        {
            return IsLector;
        }

        public bool CanSeeMaterial(Materiaal mat)
        {
            return IsLector || mat.IsLendable;
        }

        public void AddToWishList(Materiaal mat)
        {
            if (mat == null || !CanSeeMaterial(mat))
            {
                throw new ArgumentException("Het materiaal dat u wenste toe te voegen aan uw verlanglijst is niet beschikbaar.");
            }
            WishList.AddMaterial(mat);
        }

        public void RemoveFromWishList(Materiaal mat)
        {
            if (mat == null)
            {
                throw new ArgumentException("Het materiaal dat u wenste te verwijderen uit uw verlanglijst is niet beschikbaar.");
            }
            WishList.RemoveMaterial(mat);
        }

        public void AddReservation(List<Materiaal> materials,
            List<long> amounts,
            DateTime ophaalDatum, DateTime indienDatum,
            IQueryable<Reservatie> allReservations)
        {
            if (materials == null || amounts == null || materials.Count != amounts.Count)
            {
                throw new ArgumentException("Materialen en hoeveelheden zijn verplicht en er moeten evenveel van elk gegeven worden.");
            }
            Reservatie reservatie = new Reservatie(this, ophaalDatum, indienDatum);
            for (int i = 0; i < materials.Count; i++)
            {
                Materiaal mat = materials[i];
                long amount = amounts[i];
                long availableAmount = GetAmountAvailableForReservation(mat, allReservations, ophaalDatum, indienDatum);
                if (availableAmount > amount)
                {
                    throw new ArgumentException(string.Format("Het materiaal {0} heeft nog maar {1} exemplaren beschikbaar.", mat.Name, availableAmount));
                }
                reservatie.AddReservationLine(materials[i], amounts[i], ophaalDatum, indienDatum);
            }
            if (reservatie.ReservatieLijnen.Count == 0)
            {
                throw new ArgumentException("Een reservatie moet minstens één materiaal bevatten.");
            }
            Reservaties.Add(reservatie);
        }

        internal void RemoveReservation(int reservatieId)
        {
            throw new NotImplementedException();
        }

        private long GetAmountAvailableForReservation(Materiaal mat, IQueryable<Reservatie> allReservations,
            DateTime ophaalDatum, DateTime indienDatum)
        {
            IEnumerable<ReservatieLijn> reservationLinesWithMaterialThatOverlap = allReservations.SelectMany(
                r => r.ReservatieLijnen.Where(rl => rl.Materiaal.Id == mat.Id)
            ).ToList()
            .Where(rl => MyDateUtil.DoesFirstPairOverlapWithSecond(ophaalDatum, indienDatum, rl.OphaalMoment, rl.IndienMoment));

            long amountReserved;
            if (IsLector)
            {
                amountReserved = reservationLinesWithMaterialThatOverlap.Where(rl => rl.Reservatie.Lener.IsLector).Select(rl => rl.Amount).Sum();
            }
            else
            {
                amountReserved = reservationLinesWithMaterialThatOverlap.Select(rl => rl.Amount).Sum();
            }
            long amountAvailable = mat.Amount - mat.AmountNotAvailable - amountReserved;
            return amountAvailable;
        }

    }
}