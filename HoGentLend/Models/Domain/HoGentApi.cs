using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.Domain
{
    public class HoGentApiLookupResult
    {

        public string Faculteit { get; private set; }
        public string LastName { get; private set; }
        public string FirstName { get; private set; }
        public string Email { get; private set; }
        public string Base64Foto { get; private set; }
        public string Type { get; private set; }
    }

    public class HoGentApiLookupProvider
    {
        public HoGentApiLookupResult Lookup(string userId, string password)
        {
            return null;    
        }
    }
}