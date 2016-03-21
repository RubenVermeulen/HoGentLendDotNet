using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.Domain
{
    public class Materiaal
    {
        public long Id { get; private set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string ArticleCode { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public int AmountNotAvailable { get; set; }
        public bool IsLendable { get; set; }
        public string Location { get; set; }
        public List<Groep> DoelGroepen { get; set; }
        public List<Groep> LeerGebieden { get; set; }
        public Firma Firma { get; set; }
        public byte[] PhotoBytes { get; set; }
    }
}