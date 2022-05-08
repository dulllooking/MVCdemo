using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MVCdemo.Models;
using Newtonsoft.Json;

namespace MVCdemo.Controllers
{
    public class MembersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private StringBuilder _sbPermission = new StringBuilder(); // 全域習慣用底線

        // GET: Members
        public ActionResult Index()
        {
            return View(db.Members.ToList());
        }

        // GET: Members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: Members/Create
        public ActionResult Create()
        {
            // 一進來就執行
            List<Permissions> permissions = db.Permissions.ToList(); // 用ToList將資料都先取出來

            var roots = permissions.Where(x => x.ParentId == null);
            _sbPermission.Append("[");

            GetPermissionString(roots);

            _sbPermission.Append("]");

            ViewBag.Data = _sbPermission.ToString();

            return View();
        }

        private void GetPermissionString(IEnumerable<Permissions> nodes)
        {
            // 遞迴做父層後內部再送進去 children 做一樣的事
            foreach (Permissions node in nodes) {
                _sbPermission.Append($"{{\"id\": \"{node.Value}\", \"text\": \"{node.Subject}\"");

                // 結束條件
                if (node.PermissionsCollection.Count > 0) {
                    _sbPermission.Append(", \"children\": [");

                    GetPermissionString(node.PermissionsCollection); //children

                    _sbPermission.Append("]");
                }

                _sbPermission.Append("},");
            }
        }


        // POST: Members/Create
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Account,Password,Name,Permissions")] Member member)
        {
            ModelState.Remove("Guid"); // 因為不小心加了"必填"所以先拿掉，不然下一步會被擋掉
            if (ModelState.IsValid) // 驗證欄位限制
            {
                member.Guid = Guid.NewGuid();
                member.PasswordSalt = Utility.CreateSalt();
                member.Password = Utility.GenerateHashWithSalt(member.Password, member.PasswordSalt);
                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(member);
        }

        // GET: Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Member member = db.Members.Find(id);

            if (member == null)
            {
                return HttpNotFound();
            }

            // 一進來就執行
            List<Permissions> permissions = db.Permissions.ToList(); // 用ToList將資料都先取出來

            var roots = permissions.Where(x => x.ParentId == null);
            _sbPermission.Append("[");

            GetPermissionString(roots);

            _sbPermission.Append("]");

            ViewBag.Data = _sbPermission.ToString();

            ViewBag.TreeData = member.Permissions;

            return View(member);
        }

        // POST: Members/Edit/5
        // 若要免於大量指派 (overposting) 攻擊，請啟用您要繫結的特定屬性，
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Account,Password,PasswordSalt,Name,Guid")] Member member)
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: ViewLogins/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: ViewLogins/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "Id,Account,Password")] ViewLogin viewLogin)
        {
            if (ModelState.IsValid) {
                Member member = db.Members.FirstOrDefault(x => x.Account == viewLogin.Account);
                if (member != null) {
                    string password = Utility.GenerateHashWithSalt(viewLogin.Password, member.PasswordSalt);
                    if (password != member.Password) {
                        ViewBag.Message = "登入失敗";
                    }
                    else {
                        string userData = JsonConvert.SerializeObject(member);
                        Utility.SetAuthenTicket(userData, viewLogin.Account);
                        return RedirectToAction("Index", "News");
                    }
                }
                else {
                    ViewBag.Message = "登入失敗";
                }
            }

            return View(viewLogin);
        }

        // JQuery 練習
        // GET: Members/CheckAccount/account
        public ActionResult CheckAccount(string account)
        {
            Member member = db.Members.FirstOrDefault(x => x.Account == account);

            if (member == null) {
                return Content("此帳號可以使用");
            }

            return Content("**此帳號已有人使用**");
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
