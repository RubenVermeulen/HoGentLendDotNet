using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.Domain
{
    public interface IMateriaalRepository : IRepository<Materiaal, int>
    {

        IQueryable<Materiaal> FindByFilter(String filter, Groep doelgroep, Groep leergebied);
    }
}