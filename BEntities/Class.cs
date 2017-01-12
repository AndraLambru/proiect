using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BEntities
{
    public class Class
    {
        public int id { get; set; }
        public string code { get; set; }

        public List<Student> Students { get; set; }
    }
}
