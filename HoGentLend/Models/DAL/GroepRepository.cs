using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using HoGentLend.Models.Domain;

namespace HoGentLend.Models.DAL
{
    public class GroepRepository : IGroepRepository
    {
        private HoGentLendContext ctx;
        private DbSet<Groep> groepen;

        public GroepRepository(HoGentLendContext ctx)
        {
            this.ctx = ctx;
            this.groepen = ctx.Groepen;
        }

        public Groep FindBy(string id)
        {
            return groepen.Find(id);
        }

        public IQueryable<Groep> FindAll()
        {
            return groepen;
        }

        public IQueryable<Groep> findAllDoelGroepen()
        {
            return groepen.Where(g => ! g.IsLeerGebied);
        }

        public IQueryable<Groep> findAllLeerGebieden()
        {
            return groepen.Where(g => g.IsLeerGebied);
        }

        public void Add(Groep entity)
        {
            groepen.Add(entity);
        }

        public void Delete(Groep entity)
        {
            groepen.Remove(entity);
        }

        public void SaveChanges()
        {
            ctx.SaveChanges();
        }
 
    }
}