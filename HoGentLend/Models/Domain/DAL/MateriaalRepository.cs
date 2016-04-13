using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;

namespace HoGentLend.Models.DAL
{
    public class MateriaalRepository : Repository<Materiaal, int>, IMateriaalRepository
    {
        private HoGentLendContext ctx;
        //private DbSet<Materiaal> materialen;

        public MateriaalRepository(HoGentLendContext ctx) : base (ctx.Materialen, ctx)
        {
        }

        public IQueryable<Materiaal> FindByFilter(String filter, Groep doelgroep, Groep leergebied)
        {
            filter = filter.ToLower();
            IQueryable<Materiaal> materialen = FindAll().
                Where(m =>
                    (m.Name.ToLower().Contains(filter)) ||
                    (m.ArticleCode.ToLower().Contains(filter)) ||
                    (m.Firma.Email.ToLower().Contains(filter)) ||
                    (m.Firma.Name.ToLower().Contains(filter)) ||
                    (m.Location.ToLower().Contains(filter))
                );

            if(doelgroep != null)
            {
                materialen = materialen.Where(m => m.Doelgroepen.All(dg => dg == doelgroep));
            }
            if(leergebied != null)
            {
                materialen = materialen.Where(m => m.Leergebieden.All(dg => dg == leergebied));
            }
            return materialen;
        }

    }
}