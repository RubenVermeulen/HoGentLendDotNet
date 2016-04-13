using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.Domain.HoGentApi
{
    public class OfflineHoGentApiLookupProvider : IHoGentApiLookupProvider
    {
        public HoGentApiLookupResult Lookup(string userId, string unhashedPassword)
        {
            if (userId == "offtest" && unhashedPassword == "offtest")
            {
                return new HoGentApiLookupResult()
                {
                    Faculteit = "FBO",
                    FirstName = "Offline",
                    LastName  = "Student",
                    Email = "offline.student@hogent.be",
                    Type = "student"
                };
            }
            return new HoGentApiLookupResult();
        }
    }
}