using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoGentLend.Models.Domain
{
    public class VerlangLijst
    {

        public IList<Materiaal> materials { get; set; }

        public void voegMaterialenToe (Materiaal material){

            
                materials.Add(material);
            
        }

        public void verwijderMaterialen(Materiaal material) {
            materials.Remove(material);
        }
        

    }
}