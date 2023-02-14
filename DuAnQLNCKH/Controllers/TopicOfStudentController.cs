using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using DuAnQLNCKH.Models;
using ExcelDataReader;

namespace DuAnQLNCKH.Controllers
{
    public class TopicOfStudentController : Controller
    {
        
        DHTDTTDNEntities1 qLNCKHDHTDTD = new DHTDTTDNEntities1();
        TopicOfStudentModel dtsv = new TopicOfStudentModel();
        List<Subject> subjects = new DHTDTTDNEntities1().Subjects.ToList();
        List<TopicOfStudent> topicOfStudents = new DHTDTTDNEntities1().TopicOfStudents.ToList();
        List<PointTable> pointTables = new DHTDTTDNEntities1().PointTables.ToList();
        List<Faculty> faculties = new DHTDTTDNEntities1().Faculties.ToList();
        public ActionResult Index()
        {
            
            
            var topicEnd = (from t in topicOfStudents
                             join s in subjects on t.IdSu equals s.IdSu  
                            select new TopicOfStudentView
                            {

                                topicOfStudent = t,
                                subject = s

                            }).OrderBy(x => x.topicOfStudent.DateSt).ToList();
            ViewBag.topicEnd = topicEnd;


            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcelData(HttpPostedFileBase upload, byte type)
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
                        TopicOfStudent topic = new TopicOfStudent();
                         
                        topic.Name = dt_.Rows[row_][0].ToString();
                        string s = dt_.Rows[row_][1].ToString();
                        string subject = qLNCKHDHTDTD.Subjects.Where(x => x.NameCu.Contains(s)).Select(x=>x.IdSu).FirstOrDefault().ToString();
                        topic.IdSu = subject;
                        topic.IdTp = dtsv.IdTp();
                        topic.Type = type;
                        topic.NameSt = dt_.Rows[row_][2].ToString();
                        topic.LectureIntruc = dt_.Rows[row_][3].ToString();
                        if (dt_.Rows[row_][4].ToString()!="")
                            topic.DateSt = DateTime.Parse(dt_.Rows[row_][4].ToString());
                        if (dt_.Rows[row_][5].ToString()!="")
                            topic.Point = float.Parse(dt_.Rows[row_][5].ToString());
                        qLNCKHDHTDTD.TopicOfStudents.Add(topic);
                        qLNCKHDHTDTD.SaveChanges();
                     }
                          
                     return RedirectToAction("listTopicStu");

                }
                else
                {
                    ModelState.AddModelError("File", "Please Upload Your file");
                }
            }
            return View();
        }
        public ActionResult ViewInputTopicStu()
        {
            ViewBag.listSu = new SelectList(qLNCKHDHTDTD.Subjects.ToList(), "IdSu", "NameCu");
            ViewBag.listTopicStudent = qLNCKHDHTDTD.TopicOfStudents.ToList();
            ViewBag.listTypeTopic = new List<SelectListItem>{
                       new SelectListItem { Value = "0" , Text = "Công trình nghiên cứu khoa học" },
                       new SelectListItem { Value = "1" , Text = "Khóa luận tốt nghiệp" } 
            };
             return View();
        }    
        public ActionResult InputTopicStu(TopicOfStudent topic)
        {
            topic.IdTp= dtsv.IdTp();
            qLNCKHDHTDTD.TopicOfStudents.Add(topic);
            qLNCKHDHTDTD.SaveChanges();
            return Redirect("ViewInputTopicStu");
        }
        public ActionResult listTopicStu()
        {             
            ViewBag.listTopicNCKH = qLNCKHDHTDTD.TopicOfStudents.Where(x=>x.Type==0).ToList();
            var a = qLNCKHDHTDTD.TopicOfStudents.Where(x=>x.Type==0).ToList();
            ViewBag.listTopicKLTN = qLNCKHDHTDTD.TopicOfStudents.Where(x=>x.Type==1).ToList();
            var b = qLNCKHDHTDTD.TopicOfStudents.Where(x=>x.Type==1).ToList();
            return View();
        }
        
      
        public ActionResult getTypeList(int IdTy)
        {
            qLNCKHDHTDTD.Configuration.ProxyCreationEnabled = false;
            List<PointTable> DetailList = qLNCKHDHTDTD.PointTables.Where(x => x.IdP == IdTy).ToList();
            return Json(DetailList, JsonRequestBehavior.AllowGet);
        }
        public ActionResult viewRegister()
        {
             ViewBag.listtype = new SelectList(pointTables, "IdP", "NameP");
            List<Subject> faculties = qLNCKHDHTDTD.Subjects.ToList();
            ViewBag.listSubject = new SelectList(faculties, "IdSu", "NameCu");
            return View();
        }
       
        [HttpPost]
        public JsonResult Delete(string IdTp)
        {
            bool a = qLNCKHDHTDTD.Database.ExecuteSqlCommand("delete from TopicOfStudent where IdTp='" + IdTp + "'") > 0;

            return Json(new
            {
                IdTp = IdTp,
                a = a
            }, JsonRequestBehavior.AllowGet);
        }



        //public ActionResult Register(HttpPostedFileBase FileDemo1, TopicOfStudent topicOfStudent)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        string id = dtsv.IdTp();

        //        TopicOfStudentModel topic = new TopicOfStudentModel();
        //        if (FileDemo1 != null)
        //        {
        //            var Extension = Path.GetExtension(FileDemo1.FileName);
        //            var fileName = "fileTopic-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Extension;
        //            string path = Path.Combine(Server.MapPath("~/File/FileTopic"), fileName);
        //            topicOfStudent.FileDemo = Url.Content(Path.Combine("~/File/FileTopic/", fileName));
        //            if (topic.AddTopicStudent(topicOfStudent, id))
        //            {
        //                FileDemo1.SaveAs(path);
        //                ViewBag.Message = "Employee details added successfully";
        //                ViewBag.listtype = new SelectList(pointTables, "IdP", "NameP");
        //                List<Subject> faculties1 = qLNCKHDHTDTD.Subjects.ToList();
        //                ViewBag.listSubject = new SelectList(faculties1, "IdSu", "NameCu");
        //                return View("viewRegister");
        //            }
        //        }
        //        else
        //        {

        //            qLNCKHDHTDTD.Database.ExecuteSqlCommand("set dateformat dmy Insert into TopicOfStudent(IdTp, Name, NameSt, IdSV, IdP, DateSt, Times, Expense, Status, CountAuthor, IdFa, LectureIntruc) values ('" + id + "', N'" + topicOfStudent.Name + "', N'" + topicOfStudent.NameSt + "', '" + id + "', " + topicOfStudent.IdSV + "'," + topicOfStudent.IdP + ", '" + topicOfStudent.DateSt + "', " + topicOfStudent.Times + ", " + topicOfStudent.Expense + ", 0, " + topicOfStudent.CountAuthor + "', '" + topicOfStudent.IdSu + "', N'" + topicOfStudent.LectureIntruc + "')");
        //            ViewBag.Message = "Employee details added successfully";
        //            ViewBag.listtype = new SelectList(pointTables, "IdP", "NameP");
        //            List<Subject> faculties1 = qLNCKHDHTDTD.Subjects.ToList();
        //            ViewBag.listSubject = new SelectList(faculties1, "IdSu", "NameCu");
        //            return View("viewRegister");
        //        }
        //    }
        //    ModelState.Clear();

        //    ViewBag.listtype = new SelectList(pointTables, "IdP", "NameP");
        //    List<Subject> faculties = qLNCKHDHTDTD.Subjects.ToList();
        //    ViewBag.listSubject = new SelectList(faculties, "IdSu", "NameCu");
        //    return View("viewRegister");
        //}
        //public ActionResult chuaduyet()
        //{

        //    using (DHTDTTDNEntities1 db = new DHTDTTDNEntities1())
        //    {
        //        List<TopicOfStudent> topicOfStudents = db.TopicOfStudents.ToList();
        //        List<PointTable> pointTables = db.PointTables.ToList();
        //        var topics = (from t in topicOfStudents
        //                      join p in pointTables on t.IdP equals p.IdP

        //                      where t.Status == 0
        //                      select new TopicOfStudentView
        //                      {

        //                          topicOfStudent = t,

        //                          pointTable = p

        //                      }).ToList();
        //        ViewBag.listextension = topics;
        //    }
        //    return View();
        //}
        //[HttpPost]
        //[Route("/TopicOfStudent/xetduyetsv")]
        //public void xetduyetsv(string IdTp,string NameTo, string Email)
        //{


        //    using (DHTDTTDNEntities1 entities = new DHTDTTDNEntities1())
        //    {
        //        entities.Database.ExecuteSqlCommand("update TopicOfStudent set Status=N'đã duyệt', Progress=N'đang thực hiện' where IdTp='" + IdTp + "'");
        //        entities.SaveChanges();
        //        string from1 = qLNCKHDHTDTD.Emails.Select(x => x.NameEmail).FirstOrDefault();
        //        string pass = qLNCKHDHTDTD.Emails.Where(x => x.NameEmail == from1).Select(x => x.PassWord).FirstOrDefault();

        //        string s = Session["UserName"].ToString();

        //        // By using a Message with no constructors, you can define your To recipients below
        //        using (MailMessage mail = new MailMessage())
        //        {
        //            // Define your senders
        //            mail.From = new MailAddress(from1);
        //            mail.Body = "Thông báo đề tài " + NameTo + " đã được xét duyệt";
        //            mail.Subject = "Thông báo sinh viên xét duyệt đề tài";

        //            mail.To.Add(Email);

        //            mail.IsBodyHtml = true;
        //            SmtpClient smtp = new SmtpClient();
        //            smtp.Host = "smtp.gmail.com";
        //            smtp.EnableSsl = true;
        //            NetworkCredential networkCredential = new NetworkCredential(from1, pass);
        //            smtp.UseDefaultCredentials = true;
        //            smtp.Credentials = networkCredential;
        //            smtp.Port = 587;
        //            smtp.Send(mail);
        //        }
        //    }

        //}       
    }
}