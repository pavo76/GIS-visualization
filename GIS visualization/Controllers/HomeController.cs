using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using XmpCore;

namespace GIS_visualization.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //IXmpMeta xmp;
            //FileStream stream =new FileStream(HttpContext.Server.MapPath("~\\Images\\IPU-F-20954_PM.xmp"),FileMode.Open);
            //xmp = XmpMetaFactory.Parse(stream);
            //string properties="";
            //foreach (var property in xmp.Properties)
            //{

            //    properties += property.Namespace + " | " + property.Path + " | " + property.Value + " ;\n ";
            //}
            //ViewBag.properties = properties;
            return View();
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
    }
}