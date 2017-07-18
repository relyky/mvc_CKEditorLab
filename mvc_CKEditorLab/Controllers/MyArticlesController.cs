using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using mvc_CKEditorLab.Models;

namespace mvc_CKEditorLab.Controllers
{
    public class MyArticlesController : Controller
    {
        private LocalDBEntities db = new LocalDBEntities();

        // GET: MyArticles
        public ActionResult Index()
        {
            return View(db.MyArticle.ToList());
        }

        // GET: MyArticles/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyArticle myArticle = db.MyArticle.Find(id);
            if (myArticle == null)
            {
                return HttpNotFound();
            }
            return View(myArticle);
        }

        // GET: MyArticles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MyArticles/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Subject,ContentText,IsPublish,CreateDate,UpdateDate")] MyArticle myArticle)
        {
            if (ModelState.IsValid)
            {
                myArticle.Id = Guid.NewGuid();
                myArticle.CreateDate = DateTime.Now;
                myArticle.UpdateDate = DateTime.Now;
                db.MyArticle.Add(myArticle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(myArticle);
        }

        // GET: MyArticles/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyArticle myArticle = db.MyArticle.Find(id);
            if (myArticle == null)
            {
                return HttpNotFound();
            }

            myArticle.ContentText = HttpUtility.HtmlDecode(myArticle.ContentText);
            return View(myArticle);
        }

        // POST: MyArticles/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Subject,ContentText,IsPublish,CreateDate,UpdateDate")] MyArticle myArticle)
        {
            if (ModelState.IsValid)
            {
                myArticle.UpdateDate = DateTime.Now;
                db.Entry(myArticle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(myArticle);
        }

        // GET: MyArticles/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MyArticle myArticle = db.MyArticle.Find(id);
            if (myArticle == null)
            {
                return HttpNotFound();
            }
            return View(myArticle);
        }

        // POST: MyArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            MyArticle myArticle = db.MyArticle.Find(id);
            db.MyArticle.Remove(myArticle);
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
