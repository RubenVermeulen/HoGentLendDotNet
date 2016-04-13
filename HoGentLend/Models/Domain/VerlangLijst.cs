using System.Collections.Generic;

namespace HoGentLend.Models.Domain
{
    public class VerlangLijst
    {

        public IList<Materiaal> Materials { get; set; }

        public void addMaterial(Materiaal material)
        {
            Materials.Add(material);
        }

        public void removeMaterial(Materiaal material)
        {
            Materials.Remove(material);
        }


    }
}