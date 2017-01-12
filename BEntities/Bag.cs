using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BUtilities;

namespace BEntities
{
    public class Bag
    {  
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public float price { get; set; }
        public List<Pic> pictures { get; set; }

        public Pic BigPic {
            get
            {
                if (pictures == null || pictures.Count == 0)
                    return null;
                Pic p = pictures.Where(r => r.rank == 0).FirstOrDefault();
                if (p != null)
                    return p;
                return pictures[0];
            }
        }
        public string descriptionShort
        {
            get
            {
                if (description != null)
                    return description.Substring(0, description.Length>BConstants.NO_CHARS? BConstants.NO_CHARS:description.Length);
            
                return "";
            }
        } 
  
    }


    
}
