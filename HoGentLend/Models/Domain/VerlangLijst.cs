using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.Domain
{
    public class VerlangLijst
    {

        public IList<Materiaal> materialen { get; set; }

        public void voegMaterialenToe (Materiaal materiaal){

            
                materialen.Add(materiaal);
            
        }

        public void verwijderMaterialen(Materiaal materiaal) {
            materialen.Remove(materiaal);
        }
        

    }
}