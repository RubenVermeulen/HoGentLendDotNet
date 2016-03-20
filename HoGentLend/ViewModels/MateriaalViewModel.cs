using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;

namespace HoGentLend.ViewModels
{
    public class MateriaalViewModel
    {
        public MateriaalViewModel()
        {
        }

        public MateriaalViewModel(Materiaal m)
        {
            Id = m.Id;
            Name = m.Name;
            Description = m.Description;
            ArticleCode = m.ArticleCode;
            Price = m.Price;
            Amount = m.Amount;
            AmountNotAvailable = m.AmountNotAvailable;
            IsLendable = m.IsLendable;
            Location = m.Location;
            DoelGroepen = m.DoelGroepen.OfType<string>().ToList();
            LeerGebieden = m.LeerGebieden.OfType<string>().ToList();
            FirmaName = m.Firma.Name;
            FirmaEmail = m.Firma.Email;
            PhotoBase64 = (m.PhotoBytes != null) ? Convert.ToBase64String(m.PhotoBytes) : null;
        }

        public long Id { get; private set; }

        [DisplayName("Naam")]
        public string Name { get; private set; }

        [DisplayName("Beschrijving")]
        public string Description { get; private set; }

        [DisplayName("Artikelcode")]
        public string ArticleCode { get; private set; }

        [DisplayName("Prijs")]
        public double Price { get; private set; }

        [DisplayName("Aantal")]
        public int Amount { get; private set; }

        [DisplayName("Aantal onbeschikbaar")]
        public int AmountNotAvailable { get; private set; }

        [DisplayName("Uitleenbaar")]
        public bool IsLendable { get; private set; }

        [DisplayName("Locatie")]
        public string Location { get; private set; }

        public List<String> DoelGroepen { get; private set; }

        public List<string> LeerGebieden { get; private set; }

        [DisplayName("Naam firma")]
        public string FirmaName { get; private set; }

        [DisplayName("E-mailadres firma")]
        public string FirmaEmail { get; private set; }

        public string PhotoBase64 { get; private set; }
    }
}