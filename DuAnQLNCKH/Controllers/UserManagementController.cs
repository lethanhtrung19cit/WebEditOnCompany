using DuAnQLNCKH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuAnQLNCKH.Controllers
{
    public class UserManagementController : Controller
    {
        // GET: UserManagement
        DHTDTTDNEntities1 dHTDTTDNEntities1 = new DHTDTTDNEntities1();
        public ActionResult Index()
        {
            
            var info = dHTDTTDNEntities1.Emails.ToList();
            ViewBag.listInfo = info;
            return View();
            

        }
        [HttpPost]
        public ActionResult addEmail(string Email, string PassWord, string PassWord1)
        {
            if (PassWord!=PassWord1)
            {
                ModelState.AddModelError("PassWord", "Nhập lại mật khẩu không đúng");
            }
            else
            {
                var e = from i in dHTDTTDNEntities1.Emails select i;
                foreach (var e1 in e)
                {
                    dHTDTTDNEntities1.Emails.Remove(e1);
                }
                Email email = new Email();
                email.NameEmail = Email;
                email.PassWord = PassWord;                
                dHTDTTDNEntities1.Emails.Add(email);
                dHTDTTDNEntities1.SaveChanges();
                
                
                ModelState.AddModelError("resultAddEmail", "Đã sửa lại địa chỉ email");
            }
            var info = dHTDTTDNEntities1.Emails.ToList();
            ViewBag.listInfo = info;
            return View("Index");
        }
        [HttpPost]
        public ActionResult editInfo(Email model, string NewPassWord, string NewPassWord1)
        {
            var pass = dHTDTTDNEntities1.Emails.Where(x => x.NameEmail == model.NameEmail).Select(x => x.PassWord).FirstOrDefault();
            if (model.PassWord!=pass)
            {
                ModelState.AddModelError("OldPassWord", "Mật khẩu cũ không đúng");
            }
            if (NewPassWord!=NewPassWord1)
            {
                ModelState.AddModelError("NewPassWord", "Nhập lại mật khẩu cũ không khớp");
            }
            if (model.PassWord == pass && NewPassWord==NewPassWord1)
            {
                using (var context = new DHTDTTDNEntities1())
                {

                    var data = context.Emails.FirstOrDefault();
                     
                    if (data != null)
                    {
                        data.NameEmail = model.NameEmail;
                        data.PassWord = NewPassWord;

                        context.SaveChanges();
                        ModelState.AddModelError("result", "Đổi mật khẩu thành công");
                    }
                         
                }
                
            }
            var info = dHTDTTDNEntities1.Emails.ToList();
            ViewBag.listInfo = info;
            return View("Index");

        }
        [HttpPost]
        public ActionResult changePassWord1(string OldPassword, string NewPassword, string ConfirmPassword)
        {
            string p = Session["UserName"].ToString();

            var passWord = (from a in dHTDTTDNEntities1.Accounts where a.Email == p && a.Access == 1 select a.PassWord).First();

            if (HashMD5.MD5Hash(OldPassword) == passWord) ViewBag.OldPassword = "";
            else ModelState.AddModelError("OldPassword", "Mật khẩu cũ không đúng");
            if (ConfirmPassword.Equals(NewPassword)) ViewBag.ConfirmPassword = "";
            else ModelState.AddModelError("ConfirmPassword", "Xác nhận lại mật khẩu không đúng");

            if (ViewBag.OldPassword == "" && ViewBag.ConfirmPassword == "")
            {
                int IdAccount = dHTDTTDNEntities1.Accounts.Where(x => x.Email == p && x.Access == 1).Select(x => x.IdAccount).FirstOrDefault();
                var acc = dHTDTTDNEntities1.Accounts.Find(IdAccount);
                acc.PassWord = HashMD5.MD5Hash(NewPassword);
                dHTDTTDNEntities1.SaveChanges();
                ModelState.AddModelError("result", "Đổi mật khẩu thành công");

            }
            return View("changePassWord");
        }
        public ActionResult changePassWord()
        {
            
        
           
            string p = Session["UserName"].ToString();
            using (DHTDTTDNEntities1 db = new DHTDTTDNEntities1())
            {

                var accounts = dHTDTTDNEntities1.Accounts.ToList().Where(a => a.Email == Session["UserName"].ToString());
                //var accounts = dHTDTTDNEntities1.Accounts.Where(a => a.UserName == Session["UserName"].ToString()).ToList();
                //                ViewBag.passWord = dHTDTTDNEntities1.Accounts.Where(a => a.UserName == Session["UserName"].ToString());
                var passWord = (from a in dHTDTTDNEntities1.Accounts where a.Email == p select a.PassWord).First();

                ViewBag.passWord = passWord;
                return View();
            }


        }
       

    }
}