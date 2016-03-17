using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.Domain
{
    public interface IGebruikerRepsitory : IRepository<Gebruiker, string>
    {
    }
}