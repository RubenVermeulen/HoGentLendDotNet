using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;
using HoGentLend.ViewModels;

namespace HoGentLend.Models.DAL
{
    public class MateriaalRepository : Repository<Materiaal, int>, IMateriaalRepository
    {
        private HoGentLendContext ctx;
        //private DbSet<Materiaal> materialen;

        public MateriaalRepository(HoGentLendContext ctx) : base(ctx.Materialen, ctx)
        {
        }

        public IEnumerable<MateriaalViewModel> FindByFilter(String filter, int doelgroepId, int leergebiedId)
        {
            IQueryable<Materiaal> materialen;
            IEnumerable<MateriaalViewModel> materialenvm = null;

            if (String.IsNullOrEmpty(filter) && doelgroepId == 0 && leergebiedId == 0)
            {
                materialen = FindAll();
            }

            else
            {
                filter = filter.ToLower();
                materialen = FindAll().
                    Where(m =>
                        (m.Name.ToLower().Contains(filter)) ||
                        (m.ArticleCode.ToLower().Contains(filter)) ||
                        (m.Firma.Email.ToLower().Contains(filter)) ||
                        (m.Firma.Name.ToLower().Contains(filter)) ||
                        (m.Location.ToLower().Contains(filter))
                    );

                if (doelgroepId != 0)
                {
                    materialen = materialen.Where(m => m.Doelgroepen.Any(d => d.Id == doelgroepId));
                }
                if (leergebiedId != 0)
                {
                    materialen = materialen.Where(m => m.Leergebieden.Any(d => d.Id == leergebiedId));
                }
            }

            return materialen.Include(m => m.Firma)
               .Include(m => m.Doelgroepen)
               .Include(m => m.Leergebieden)
               .OrderBy(m => m.Name)
               .ToList()
               .Select(m => new MateriaalViewModel(m));
        }

    }
}