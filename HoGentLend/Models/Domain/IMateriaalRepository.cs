using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HoGentLend.ViewModels;

namespace HoGentLend.Models.Domain
{
    public interface IMateriaalRepository : IRepository<Materiaal, int>
    {

        IEnumerable<MateriaalViewModel> FindByFilter(String filter, int doelgroepId, int leergebiedId);
    }
}