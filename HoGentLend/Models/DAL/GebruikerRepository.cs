using HoGentLend.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.DAL
{
    public class GebruikerRepository : Repository<Gebruiker, int>, IGebruikerRepository
    {
        public GebruikerRepository(HoGentLendContext ctx) : base(ctx.Gebruikers, ctx)
        { }

    }
}