using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;

namespace HoGentLend.Models.Domain
{
    public class Gebruiker
    {
        public long Id { get; private set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsLector { get; set; }

        public VerlangLijst WishList { get; set; }


        public bool DoShowAllMaterials()
        {
            return IsLector;
        }

        public bool CanSeeMaterial(Materiaal mat)
        {
            return IsLector || mat.IsLendable;
        }
    }
}