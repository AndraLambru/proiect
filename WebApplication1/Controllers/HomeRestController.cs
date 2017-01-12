using BBusiness;
using BEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class HomeRestController : ApiController
    {
        // GET: api/HomeRest
        public List<Class> Get()
        {
            return Operations2.Get();
        }

        // GET: api/HomeRest/5
        public string Get(int id, int tid=-1, string op=null, string name=null, int classid=0)
        {
            if ( (tid != 0 && tid != 1) || string.IsNullOrEmpty(op))
                return "Error";

            switch (op.ToUpper())
            {
                case "DELETE":
                    return Operations2.Delete(id, tid) ? "" : "Error";
                case "ADD":
                    return Operations2.Add(tid,name, classid).ToString();
                case "MODIFY":
                    return Operations2.Modify(id, tid, name).ToString();
                default:
                    break;
            }
            return "";
        }

        
        
    }
}
