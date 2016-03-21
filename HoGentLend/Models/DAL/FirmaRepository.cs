using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;

namespace HoGentLend.Models.DAL
{
    public class FirmaRepository : IFirmaRepository
    {
        private HoGentLendContext ctx;
        private DbSet<Firma> firmas;

        public FirmaRepository(HoGentLendContext ctx)
        {
            this.ctx = ctx;
            this.firmas = ctx.Firmas;
        } 

        public Firma FindBy(string id)
        {
            return firmas.Find(id);
        }

        public IQueryable<Firma> FindAll()
        {
            return firmas;
        }

        public void Add(Firma entity)
        {
            firmas.Add(entity);
        }

        public void Delete(Firma entity)
        {
            firmas.Remove(entity);
        }

        public void SaveChanges()
        {
            ctx.SaveChanges();
        }
    }
}