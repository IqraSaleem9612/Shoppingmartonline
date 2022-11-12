using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineShopeProject.Models;



namespace OnlineShopeProject.Controllers
{
    public class ProductsController : Controller
    {
        private Model4 db = new Model4();

        // GET: Product
        public ActionResult Index()
        {
            var products = db.Products.OrderByDescending(p =>p.PRODUCT_ID).Include(p => p.Category);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CATEGORY_FID = new SelectList(db.Categories, "CATEGORY_ID", "CATEGORY_NAME");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                product.PRO_PIC.SaveAs(Server.MapPath("~/ProductPicture/"+product.PRO_PIC.FileName));
                product.PRODUCT_PICTURE = "~/ProductPicture/" +product.PRO_PIC.FileName;
                product.PRO_PIC2.SaveAs(Server.MapPath("~/ProductPicture/" + product.PRO_PIC2.FileName));
                product.PRODUCT_PICTURE2 = "~/ProductPicture/" + product.PRO_PIC2.FileName;
                product.PRO_PIC3.SaveAs(Server.MapPath("~/ProductPicture/" + product.PRO_PIC3.FileName));
                product.PRODUCT_PICTURE3 = "~/ProductPicture/" + product.PRO_PIC3.FileName;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CATEGORY_FID = new SelectList(db.Categories, "CATEGORY_ID", "CATEGORY_NAME", product.CATEGORY_FID);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CATEGORY_FID = new SelectList(db.Categories, "CATEGORY_ID", "CATEGORY_NAME", product.CATEGORY_FID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Product product)
        {
            if (ModelState.IsValid)
            {
                if(product.PRO_PIC!=null)
                { 
                product.PRO_PIC.SaveAs(Server.MapPath("~/ProductPicture/" + product.PRO_PIC.FileName));
                product.PRODUCT_PICTURE = "~/ProductPicture/" + product.PRO_PIC.FileName;
                }

                if (product.PRO_PIC2 != null)
                {
                    product.PRO_PIC2.SaveAs(Server.MapPath("~/ProductPicture/" + product.PRO_PIC2.FileName));
                    product.PRODUCT_PICTURE2 = "~/ProductPicture/" + product.PRO_PIC2.FileName;
                }

                if (product.PRO_PIC3 != null)
                {
                    product.PRO_PIC3.SaveAs(Server.MapPath("~/ProductPicture/" + product.PRO_PIC3.FileName));
                    product.PRODUCT_PICTURE3 = "~/ProductPicture/" + product.PRO_PIC3.FileName;
                }

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CATEGORY_FID = new SelectList(db.Categories, "CATEGORY_ID", "CATEGORY_NAME", product.CATEGORY_FID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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
