using HoGentLend.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace HoGentLend.Models.DAL
{
    public class GebruikerRepository : Repository<Gebruiker, int>, IGebruikerRepository
    {
        public GebruikerRepository(HoGentLendContext ctx) : base(ctx.Gebruikers, ctx)
        { }

        public Gebruiker FindByEmail(string email)
        {
            return dbSet.Include(g => g.Reservaties)
                .Include(g => g.WishList.Materials)
                .Include(g => g.Reservaties.Select(r => r.ReservatieLijnen))
                .Include(g => g.Reservaties.Select(r => r.ReservatieLijnen.Select(rl => rl.Materiaal)))
                .FirstOrDefault(g => g.Email == email);
        }
    }
}