using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using BBusiness;
using BUtilities;
using System.IO;
using System.Text;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ModelBags model = new ModelBags()
            {
                listBags=Operations.Read(0,10000)
            };
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult SaveBag(string name, string description, string price)
        {
            string result = "";
            int id = 0;
            try
            {
                float p = (float)Convert.ToDouble(price);
                result = Operations.WriteBag(name, description, p,ref id) ? "" : "database error";
            }
            catch (Exception exc)
            {
                result = "DataBase Error!";
            }
            return Json(new {result=result, id=id.ToString()}, JsonRequestBehavior.AllowGet);
        }

        
        [HttpPost]
        public JsonResult SavePic()
        {
            string result = "";
            int id = 0;
            try
            {
                // collect FormData (excepting BLOBs!
                string guid = Request["guid"];
                int id_bag = int.Parse(Request["id_bag"]);
                int rank = int.Parse(Request["rank"]);
               
                // write BLOB passes / image-content
                string originalFileNAme = Request.Files[0].FileName;
                string newFileName = guid + (new FileInfo(originalFileNAme)).Extension;
                
                byte[] imageContent = new byte[(int)Request.Files[0].InputStream.Length];
                Request.Files[0].InputStream.Read(imageContent, 0, imageContent.Length);
                string newName = BConstants.PATH + newFileName;
                System.IO.File.WriteAllBytes(newName, imageContent);

                // write in database
                Operations.WritePic(id_bag, newFileName, rank, ref id);
            }
            catch (Exception exc)
            {
                result = "DataBase Error!";
            }
            return Json(new { result = result, id = id.ToString() }, JsonRequestBehavior.AllowGet);
        }
    }
}