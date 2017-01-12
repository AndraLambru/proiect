using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BBusiness;
using BUtilities;

namespace WebApplication1
{
    /// <summary>
    /// Summary description for imgProvider
    /// </summary>
    public class imgProvider : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "image/JPEG";

            string pictureName = context.Request.QueryString["name"];
            try
            {
                context.Response.WriteFile(BConstants.PATH + pictureName);
            }
            catch { }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}