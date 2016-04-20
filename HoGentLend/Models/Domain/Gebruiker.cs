using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Collections.Generic;

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

        public void AddReservation(List<Materiaal> materials, List<long> amounts, DateTime ophaalDatum, DateTime indienDatum)
        {
            if (materials == null || amounts == null || materials.Count != amounts.Count)
            {
                throw new ArgumentException("Materialen en hoeveelheden zijn verplicht en er moeten evenveel van elk gegeven worden.");
            }
            Reservatie reservatie = new Reservatie(this, ophaalDatum, indienDatum);
            for(int i = 0; i < materials.Count; i++)
            {
                reservatie.AddReservationLine(materials[i], amounts[i], ophaalDatum, indienDatum);
            }
            Reservaties.Add(reservatie);
        }
    }
}