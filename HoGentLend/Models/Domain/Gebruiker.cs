using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using HoGentLend.Models.DAL;

namespace HoGentLend.Models.Domain
{
    public abstract class Gebruiker
    {
        public long Id { get; private set; }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }

        public virtual VerlangLijst WishList { get; private set; }
        public virtual List<Reservatie> Reservaties { get; private set; }

        protected Gebruiker()
        {
            // default for entityframework
        }

        public Gebruiker(string firstName, string lastName, string email)
            : this(firstName, lastName, email, new VerlangLijst(), new List<Reservatie>())
        { }

        public Gebruiker(string firstName, string lastName, string email, VerlangLijst wishList, List<Reservatie> reservaties)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.WishList = wishList;
            this.Reservaties = reservaties;
        }

        public abstract bool CanSeeAllMaterials();

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
            List<int> amounts,
            DateTime ophaalDatum, DateTime indienDatum,
            IQueryable<Reservatie> allReservations)
        {
            if (materials == null || amounts == null || materials.Count != amounts.Count)
            {
                throw new ArgumentException("Materialen en hoeveelheden zijn verplicht en er moeten evenveel van elk gegeven worden.");
            }
            Reservatie reservatie = new Reservatie(this, ophaalDatum, indienDatum);
            reservatie.ReservatieLijnen = new List<ReservatieLijn>();
            for (int i = 0; i < materials.Count; i++)
            {
                Materiaal mat = materials[i];
                long amount = amounts[i];
                long availableAmount = GetAmountAvailableForReservation(mat, allReservations, ophaalDatum, indienDatum);
                if (amount > availableAmount)
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

        public Reservatie RemoveReservation(Reservatie reservatie)
        {
            if (reservatie == null)
            {
                throw new ArgumentException("De reservatie is niet beschikbaar of mogelijk al verwijderd.");
            }
            if (!Reservaties.Contains(reservatie))
            {
                throw new ArgumentException("De reservatie is al verwijderd geweest.");
            }
            if (reservatie.Opgehaald)
            {
                throw new ArgumentException("Een reservatie die is al is opgehaald kan niet geannnuleerd worden.");
            }
            Reservaties.Remove(reservatie);
            return reservatie;
        }

        public void RemoveReservationLijn(ReservatieLijn reservatieLijn, ReservatieRepository reservatieRepository)
        {
            if (reservatieLijn == null)
            {
                throw new ArgumentException("De reservatielijn is niet beschikbaar of mogelijk al verwijderd.");
            }
            if (!Reservaties.Contains(reservatieLijn.Reservatie))
            {
                throw new ArgumentException("De reservatielijn is niet beschikbaar.");
            }
            if (!reservatieLijn.Reservatie.ReservatieLijnen.Contains(reservatieLijn))
            {
                throw new ArgumentException("De reservatielijn is al verwijderd geweest.");
            }

            Reservatie r = reservatieLijn.Reservatie;
            //reservatieLijn.Reservatie.ReservatieLijnen.Remove(reservatieLijn);
            reservatieRepository.RemoveReservationLine(reservatieLijn);

            // Verwijder de volledige reservatie wanneer er geen reservatielijnen meer zijn.
            if (r.ReservatieLijnen.Count == 0)
            {
                reservatieRepository.Delete(r);
            }
        }

        private long GetAmountAvailableForReservation(Materiaal mat, IQueryable<Reservatie> allReservations,
            DateTime ophaalDatum, DateTime indienDatum)
        {
            IEnumerable<ReservatieLijn> reservationLinesWithMaterialThatOverlap = allReservations.SelectMany(
                r => r.ReservatieLijnen.Where(rl => rl.Materiaal.Id == mat.Id)
            ).ToList()
            .Where(rl => MyDateUtil.DoesFirstPairOverlapWithSecond(ophaalDatum, indienDatum, rl.OphaalMoment, rl.IndienMoment));

            long amountReserved = FilterReservatieLijnenDieOveruledKunnenWorden(reservationLinesWithMaterialThatOverlap).Select(rl => rl.Amount).Sum();
            long amountAvailable = mat.Amount - mat.AmountNotAvailable - amountReserved;
            return amountAvailable;
        }

        public abstract bool CanSeeMaterial(Materiaal mat);
        public abstract IEnumerable<ReservatieLijn> FilterReservatieLijnenDieOveruledKunnenWorden(IEnumerable<ReservatieLijn> lijnen);


    }
}