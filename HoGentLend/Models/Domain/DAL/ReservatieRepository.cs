using System;
using System.Collections.Generic;
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
        
    }
}