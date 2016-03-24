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
        private DbSet<Materiaal> materialen;

        public MateriaalRepository(HoGentLendContext ctx) : base (ctx.Materialen, ctx)
        {
        }
    }
}