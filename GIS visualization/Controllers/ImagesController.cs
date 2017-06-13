using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GIS_visualization.Models;
using XmpCore;
using System.IO;

namespace GIS_visualization.Controllers
{
    public class ImagesController : Controller
    {
        private dbContext db = new dbContext();

        // GET: Images

        public ActionResult Index()
        {
            return View(db.Images.ToList());
        }


        // GET: Images/Map
        public ActionResult Map()
        {
            return View(db.Images.ToList());
        }


        // GET: Images/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // GET: Images/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Images/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,src,Author,Date,Type,Dimensions,InventoryMark,Longitude,Latitude")] Image image)
        {
            if (ModelState.IsValid)
            {
                // Using XmpCore NuGet package extracts data from xmp file
                IXmpMeta xmp;
                string xmpPath = String.Format("~\\Images\\{0}.xmp", image.InventoryMark);
                FileStream stream = new FileStream(HttpContext.Server.MapPath(xmpPath), FileMode.Open);
                xmp = XmpMetaFactory.Parse(stream);


                // both latitude and longitude have to be strings and decimal value separated with .
                // so jquery.geo can read them properly
                string latitude = xmp.GetPropertyString("http://ns.adobe.com/exif/1.0/", "GPSLatitude");
                string orientation = latitude.Last().ToString();
                int numberPrefix = orientation == "N" ? 1 : -1;
                latitude = latitude.Substring(0, latitude.Length - 2);
                double degrees = double.Parse(latitude.Substring(0, latitude.IndexOf(",")));
                double minutes = double.Parse(latitude.Substring(latitude.IndexOf(",")+1).Replace(".",","))/60;
                latitude = (numberPrefix*(degrees+minutes)).ToString().Replace(",", ".");
                image.Latitude = latitude;


                string longitude= xmp.GetPropertyString("http://ns.adobe.com/exif/1.0/", "GPSLongitude");
                orientation = longitude.Last().ToString();
                numberPrefix = orientation == "E" ? 1 : -1;
                longitude = longitude.Substring(0, longitude.Length - 2);
                degrees = double.Parse(longitude.Substring(0, longitude.IndexOf(",")));
                minutes = double.Parse(longitude.Substring(longitude.IndexOf(",") + 1).Replace(".", ",")) / 60;
                longitude = (numberPrefix * (degrees + minutes)).ToString().Replace(",", "."); ;
                image.Longitude = longitude;


                // because of localization, years, montsh and days are extracted separately
                string years = xmp.GetPropertyDate("http://ns.adobe.com/xap/1.0/", "CreateDate").Year.ToString();
                string months = xmp.GetPropertyDate("http://ns.adobe.com/xap/1.0/", "CreateDate").Month.ToString();
                string days = xmp.GetPropertyDate("http://ns.adobe.com/xap/1.0/", "CreateDate").Day.ToString();
                image.Date =DateTime.Parse(String.Format("{0}.{1}.{2}.",days,months,years));


                // format xxxx X yyyy px
                string dimensionX = xmp.GetPropertyString("http://ns.adobe.com/exif/1.0/", "PixelXDimension");
                string dimensionY = xmp.GetPropertyString("http://ns.adobe.com/exif/1.0/", "PixelYDimension");
                image.Dimensions = dimensionX + " X " + dimensionY + " px";

                db.Images.Add(image);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(image);
        }

        // GET: Images/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // POST: Images/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,src,Author,Date,Type,Dimensions,InventoryMark,Longitude,Latitude")] Image image)
        {
            if (ModelState.IsValid)
            {                
                db.Entry(image).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(image);
        }

        // GET: Images/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image image = db.Images.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // POST: Images/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Image image = db.Images.Find(id);
            db.Images.Remove(image);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
