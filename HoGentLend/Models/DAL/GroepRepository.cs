using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;

namespace HoGentLend.Models.DAL
{
    public class GroepRepository : Repository<Groep, string>, IGroepRepository
    {

        public GroepRepository(HoGentLendContext ctx) : base(ctx.Groepen, ctx)
        {}

        public IQueryable<Groep> findAllDoelGroepen()
        {
            return FindAll().Where(g => ! g.IsLeerGebied);
        }

        public IQueryable<Groep> findAllLeerGebieden()
        {
            return FindAll().Where(g => g.IsLeerGebied);
        }
 
    }
}