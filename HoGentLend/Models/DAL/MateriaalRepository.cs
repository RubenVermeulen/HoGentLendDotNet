using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;

namespace HoGentLend.Models.DAL
{
    public class MateriaalRepository : IMateriaalRepository
    {
        private HoGentLendContext ctx;
        private DbSet<Materiaal> materialen;

        public MateriaalRepository(HoGentLendContext ctx)
        {
            this.ctx = ctx;
            this.materialen = ctx.Materialen;
        }
         
        public Materiaal FindBy(string id)
        {
            return materialen.Find(id);
        }

        public IQueryable<Materiaal> FindAll()
        {
            return materialen;
        }

        public void Add(Materiaal entity)
        {
            materialen.Add(entity);
        }

        public void Delete(Materiaal entity)
        {
            materialen.Remove(entity);
        }

        public void SaveChanges()
        {
            ctx.SaveChanges();
        }
    }
}