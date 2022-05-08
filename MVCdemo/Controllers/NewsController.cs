using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCdemo.Models;
using MvcPaging;
using Newtonsoft.Json;

namespace MVCdemo.Controllers
{
    //// 判斷有無登入
    //[Authorize]
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // 預設分頁資料筆數
        private const int DefaultPageSize = 1;

        // GET: News
        public ActionResult Index(int? page)
        {
            ViewBag.Message = "12345";

            // 判斷是否有分頁參數
            if (!page.HasValue) {
                page = 0;
            }
            else {
                page--;
            }

            var news = db.News.Include(n => n.Catalog).ToList();
            ViewBag.Catalog = news; //動態類似var 前台使用要確定型別

            return View(news.ToPagedList(page.Value, DefaultPageSize));
        }

        // GET: News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: News/Create
        public ActionResult Create()
        {
            ViewBag.CtaegoryId = new SelectList(db.NewsCatalog, "Id", "Subject");
            return View();
        }

        // POST: News/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        //使用Bind()來指定要繫結欄位，不使用就是全部
        //public ActionResult Create([Bind(Include = "Id,Subject,Article,StartDate,EndDate,initDate,CtaegoryId,NewsClass")] News news)
        public ActionResult Create(News news)
        {
            if (ModelState.IsValid)
            {
                //可以在.Add()之前修改或動手腳
                DateTime dateTime = DateTime.Now;
                news.initDate = dateTime;
                //將資料用.Add()加入
                db.News.Add(news);
                //將資料存入資料表
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CtaegoryId = new SelectList(db.NewsCatalog, "Id", "Subject", news.CtaegoryId);
            return View(news);
        }

        // GET: News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            ViewBag.CtaegoryId = new SelectList(db.NewsCatalog, "Id", "Subject", news.CtaegoryId);
            return View(news);
        }

        // POST: News/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Bind(Include="") 決定要收那些資料，不寫就是全收 (*注意型別有沒有允許Null)
        public ActionResult Edit([Bind(Include = "Id,Subject,Article,StartDate,EndDate,initDate,CtaegoryId,NewsClass")] News news)
        {
            if (ModelState.IsValid)
            {
                //跟 news 說目前動作的修改 (自動檢查修改)
                db.Entry(news).State = EntityState.Modified;
                //存入資料庫
                db.SaveChanges();
                //return RedirectToAction("Index");

                //編輯後改回傳給前端看到成功提示
                return Content("Sucess");
            }
            ViewBag.CtaegoryId = new SelectList(db.NewsCatalog, "Id", "Subject", news.CtaegoryId);
            return View(news);
        }

        // GET: News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            News news = db.News.Find(id);
            db.News.Remove(news);
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


        //設定 int id 為要導入的值
        public ActionResult GetJson(int NewsId)
        {
            var news = db.News.Include(n => n.Catalog).Where(x => x.Id == NewsId);
            //.Find() 只能依 ID 找資料，只能找一筆
            var catalog = db.NewsCatalog.Find(1);
            //執行查詢將資料轉成 List<T> 存取
            List<News> listNews = news.ToList();
            //自訂欄位作法
            var data = listNews.Select(x => new {
                ID = x.Id,
                Item = x.Subject,
                NewsType = catalog //可以塞入新的表或任何格式
            }).FirstOrDefault(); //用.FirstOrDefault()只找一筆，就不會出現陣列外框

            string output = JsonConvert.SerializeObject(data);
            return Content(output, "application/json");
        }
    }
}
