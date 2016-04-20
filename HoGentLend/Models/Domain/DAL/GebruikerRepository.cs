using HoGentLend.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace HoGentLend.Models.DAL
{
    public class GebruikerRepository : Repository<Gebruiker, int>, IGebruikerReposSitory
    {
        public GebruikerRepository(HoGentLendContext ctx) : base(ctx.Gebruikers, ctx)
        { }

        public Gebruiker FindByEmail(string email)
        {
            return DbSet.FirstOrDefault(g => g.Email == email);
        }
    }
}