﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoGentLend.Models.Domain
{
    interface IGroepRepository : IRepository<Groep, string>
    {
        IQueryable<Groep> findAllDoelGroepen();
        IQueryable<Groep> findAllLeerGebieden();

    }
}