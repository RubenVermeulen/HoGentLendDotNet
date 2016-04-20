using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;
using System.Linq;
using System.Web;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;

namespace HoGentLend.Models.DAL
{
    public class ReservatieRepository : Repository<Reservatie, int>, IReservatieRepository
    {

        private HoGentLendContext ctx;

        public ReservatieRepository(HoGentLendContext ctx) : base(ctx.Reservaties, ctx)
        {
            
        }

        public override IQueryable<Reservatie> FindAll()
        {
            return base.FindAll().Include(r => r.ReservatieLijnen);
        }

        public override Reservatie FindBy(int id)
        {
            return dbSet.Include(r => r.ReservatieLijnen).Include(r => r.Lener).Include(r => r.ReservatieLijnen.Select(rl => rl.Materiaal))
              .SingleOrDefault(x => x.Id == id);
        }
    }
}