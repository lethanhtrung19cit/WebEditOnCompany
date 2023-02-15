 using DuAnQLNCKH.Models;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuAnQLNCKH.Controllers
{
    public class AdminController : Controller
    {
        DHTDTTDNEntities1 qLNCKHDHTDTD = new DHTDTTDNEntities1();
        [Authorize(Roles = "0")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcelData(HttpPostedFileBase upload)
        {

            if (ModelState.IsValid)
            {

                if (upload != null && upload.ContentLength > 0)
                {
                    // ExcelDataReader works with the binary Excel file, so it needs a FileStream
                    // to get started. This is how we avoid dependencies on ACE or Interop:
                    Stream stream = upload.InputStream;

                    // We return the interface, so that
                    IExcelDataReader reader = null;


                    if (upload.FileName.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                    else if (upload.FileName.EndsWith(".xlsx"))
                    {
                        reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    }
                    else
                    {
                        ModelState.AddModelError("File", "This file format is not supported");
                        return View();
                    }

                    DataTable dt_ = new DataTable();

                    dt_ = reader.AsDataSet().Tables[0];


                    for (int row_ = 1; row_ < dt_.Rows.Count; row_++)
                    {
                        Account account = new Account();
                        Information information = new Information();
                        string position= dt_.Rows[row_][1].ToString();
                        string Faculty= dt_.Rows[row_][2].ToString();
                        information.IdPo = qLNCKHDHTDTD.Positions.Where(x => x.NamePo.Contains(position)).Select(x => x.IdPo).FirstOrDefault();
                         account.Email = dt_.Rows[row_][4].ToString();
                        account.PassWord = "827ccb0eea8a706c4c34a16891f84e7b";
                        account.Status = 0;
                        account.Access = 2;
                        information.Email= dt_.Rows[row_][4].ToString();
                        information.NameLe= dt_.Rows[row_][3].ToString();
                        qLNCKHDHTDTD.Accounts.Add(account);
                        qLNCKHDHTDTD.Information.Add(information);
                        qLNCKHDHTDTD.SaveChanges();
                         
                    }

                    return RedirectToAction("FormInsert");

                }
                else
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }
            }
            return View();
        }
        // GET: Admin
        [Authorize(Roles = "0")]
        public ActionResult Index(string seach, int page = 1, int pagesize = 10)
        {
             
            var model = new AccountSQL().getListAcc(seach, page, pagesize);
            ViewBag.seaching = seach;
             return View(model);
             
        }
        [Authorize(Roles = "0")]
        public void Setlist()
        {

            ViewBag.Access = new List<SelectListItem>{
                       new SelectListItem { Value = "0" , Text = "Quản trị viên" },
                       new SelectListItem { Value = "1" , Text = "Phòng Nghiên Cứu Khoa Học" },
                        new SelectListItem { Value = "2" , Text = "Giảng Viên" } 
            };
            ViewBag.Status = new List<SelectListItem> {
                       new SelectListItem { Value = "0" , Text = "Hoat động" },
                       new SelectListItem { Value = "1" , Text = "Vô hiệu Hóa" }
            };

        }
        [Authorize(Roles = "0")]
        public ActionResult FormInsert()
        {
            List<Position> positions = qLNCKHDHTDTD.Positions.ToList();
            ViewBag.listPosition = new SelectList(positions, "IdPo", "NamePo");
             
            Setlist();
            return View();
        }
        [Authorize(Roles = "0")]
        public ActionResult FormEdit(int ID)
        {
            Setlist();
            var model = new AccountSQL().DetailbyUser(ID);
             return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "0")]
        public ActionResult Create(Account model, int Position, string Name)
        {
            model.PassWord = "12345";
            if (ModelState.IsValid)
            {
                var res = new AccountSQL();
                if (res.FindByUser(model.Email, model.Access) == false)
                {
                    res.InsertAcc(model.Email, HashMD5.MD5Hash(model.PassWord), model.Access, Position, Name);
                    ModelState.AddModelError("", "Thêm Tài Khoản Thành Công");
                }
                else
                {
                    ModelState.AddModelError("", "Tài Khoản Đã Tồn Tại");
                } 
            }
            Setlist();
            List<Position> positions = qLNCKHDHTDTD.Positions.ToList();
            ViewBag.listPosition = new SelectList(positions, "IdPo", "NamePo");
            return View("FormInsert");
        }
        [Authorize(Roles = "0")]
        public ActionResult Update(Account model, string email, int Access)
        {
            
            var update = new AccountSQL().UpdateAccount(email, Access, model.Status);
            if (update == true)
                ModelState.AddModelError("", "Cập Nhật Thành Công");
            else ModelState.AddModelError("", "Cập Nhật Lỗi");
            
            model.Email = email;
            model.Access = Access;
            
            Setlist();
            return View("FormEdit", model);
        }
        [Authorize(Roles = "0")]
        public ActionResult Edit(string UserName)
        {
 
            var model = qLNCKHDHTDTD.Accounts.Where(x=>x.Email==UserName).SingleOrDefault();
            
            return View(model);

        }
        [HttpPost]
        [Authorize(Roles = "0")]
        public ActionResult EditAccount(Account account)
        {
 
            return View();

        }
        [Authorize(Roles = "0")]
        public ActionResult DeleteAccount(int IdAccount)
        {
            var acc = qLNCKHDHTDTD.Accounts.Find(IdAccount);
            acc.Status = 2;
            qLNCKHDHTDTD.SaveChanges();
            return Redirect("Index");
            
        }
        [Authorize(Roles = "0")]
        [HttpPost]
        public ActionResult changePassWord1(string OldPassword, string NewPassword, string ConfirmPassword)
        {
            string p = Session["UserName"].ToString();
            var passWord = (from a in qLNCKHDHTDTD.Accounts where a.Email == p && a.Access == 0 select a.PassWord).First();

            if (HashMD5.MD5Hash(OldPassword) == passWord) ViewBag.OldPassword = "";
            else ModelState.AddModelError("OldPassword", "Mật khẩu cũ không đúng");
            if (ConfirmPassword.Equals(NewPassword)) ViewBag.ConfirmPassword = "";
            else ModelState.AddModelError("ConfirmPassword", "Xác nhận lại mật khẩu không đúng");

            if (ViewBag.OldPassword == "" && ViewBag.ConfirmPassword == "")
            {
                int IdAccount = qLNCKHDHTDTD.Accounts.Where(x => x.Email == p && x.Access == 0).Select(x => x.IdAccount).FirstOrDefault();
                var acc = qLNCKHDHTDTD.Accounts.Find(IdAccount);
                acc.PassWord = HashMD5.MD5Hash(NewPassword);
                qLNCKHDHTDTD.SaveChanges();
                ModelState.AddModelError("result", "Đổi mật khẩu thành công");

            }
            return View("changePassWord");
        }
        [Authorize(Roles = "0")]
        public ActionResult changePassWord()
        {
            string p = Session["UserName"].ToString();

            var accounts = qLNCKHDHTDTD.Accounts.ToList().Where(a => a.Email == Session["UserName"].ToString());

            return View();

        }
    }
}