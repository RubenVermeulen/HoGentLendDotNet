using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.Domain
{
    public class Firma
    {
        public long Id { get; private set; }

        public string Name { get; private set; }

        public string Email { get; private set; }
    }
}