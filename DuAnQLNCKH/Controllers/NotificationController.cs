using DuAnQLNCKH.Models;
using Microsoft.SharePoint.Client;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Attachment = System.Net.Mail.Attachment;

namespace DuAnQLNCKH.Controllers
{
    public class NotificationController : Controller
    {
        //[NotMapped]
        //public IEnumerable<Information> EmailCollection { get; set; }
        //[NotMapped]
        //public string[] SelectedIdArray { get; set; }
        DHTDTTDNEntities1 qLNCKHDHTDTD = new DHTDTTDNEntities1();
        NotificationModel notify = new NotificationModel();
        private SqlConnection con;
        public void connection()
        {
            string constr = @"Data Source=DESKTOP-ECMGDNK\SQLEXPRESS;initial catalog=nckh_dhdn;integrated security=True";
            con = new SqlConnection(constr);

        }
        public void viewbag()
        {
            var list1 = (from c in qLNCKHDHTDTD.Notifications
                         select new FileNotifiModel
                         {
                             DateCreat = c.DateCreat,
                             PersonCreat = c.PersonCreat,
                             Title = c.Title,
                             Content = c.Content,
                             FileName = c.FileName
                         }).ToList();


            ViewBag.listNoti = list1;
            ViewBag.NotificationsLec = qLNCKHDHTDTD.Notifications.OrderByDescending(x => x.DateCreat).Where(a => a.Object == "Giảng viên").ToList();
            ViewBag.NotificationsStu = qLNCKHDHTDTD.Notifications.OrderByDescending(x => x.DateCreat).Where(a => a.Object == "Sinh viên").ToList(); 
           
            List<TopicOfLecture> listProgress = qLNCKHDHTDTD.TopicOfLectures.ToList();


            //ViewBag.listProgress = new MultiSelectList(qLNCKHDHTDTD.TopicOfLectures.Select(x => x.Progress).Distinct().ToList(), "Progress");
            ViewBag.listEmail = qLNCKHDHTDTD.Information.ToList();
             
        }
         
        public ActionResult Index()
        {
            if (Session["Acess"]==null || Session["Acess"].ToString()=="null")                
            {
                return RedirectToAction("Index", "Login");
            }
            selectedEmail ema = new selectedEmail();

           
            ViewBag.NotificationsLec = qLNCKHDHTDTD.Notifications.OrderByDescending(x => x.DateCreat).Where(a=>a.Object=="GV").ToList();
              
            List<TopicOfLecture> listProgress = qLNCKHDHTDTD.TopicOfLectures.ToList();
            //ViewBag.listProgress = new SelectList(new List<SelectListItem> {
            //           new SelectListItem { Value = "0" , Text = "--Tất cả đề tài--" },
            //           new SelectListItem { Value = "1" , Text = "đang thực hiện" },
            //           new SelectListItem { Value = "2" , Text = "quá hạn" },
            //           new SelectListItem { Value = "3" , Text = "đã nghiệm thu" }
            //}, "Value", "Text");
            //ViewBag.listProgress = new MultiSelectList(qLNCKHDHTDTD.TopicOfLectures.Select(x => x.Progress).Distinct().ToList(), "Progress");
            ema.EmailCollection = qLNCKHDHTDTD.Information.ToList();

            
            ViewBag.listEmail = ema.EmailCollection;

            return View();
        }
        public ActionResult NotifiStu()
        {
            if (Session["UserName"]==null)
            {
                Session["UserName"] = "null";
                Session["Acess"] = "null";
            }
            
            selectedEmail ema = new selectedEmail();


            ViewBag.NotificationsStu = qLNCKHDHTDTD.Notifications.OrderByDescending(x => x.DateCreat).Where(a=>a.Object=="SV").ToList();
 
             ema.EmailCollectionStu = qLNCKHDHTDTD.TopicOfStudents.ToList();

            ViewBag.listEmail = ema.EmailCollectionStu;
            ViewBag.listProgress = new SelectList(new List<SelectListItem> {
                       new SelectListItem { Value = "0" , Text = "--Tất cả đề tài--" },
                       new SelectListItem { Value = "1" , Text = "đang thực hiện" },
                       new SelectListItem { Value = "2" , Text = "quá hạn" },
                       new SelectListItem { Value = "3" , Text = "đã nghiệm thu" }
            }, "Value", "Text");
            return View();
        }
        [Authorize(Roles = "1")]
        [HttpPost]
        public ActionResult CreateNotifi(HttpPostedFileBase files, Notification model, string Object)
        {
            model.DateCreat = DateTime.Now;
            model.PersonCreat = Session["UserName"].ToString();
            model.Object = Object;
            if (files != null)
            {
                var Extension = Path.GetExtension(files.FileName);
                var fileName = "filenotifi-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Extension;
                string path = Path.Combine(Server.MapPath("~/File/FileNotification"), fileName);
                model.FileName = Url.Content(Path.Combine("~/File/FileNotification/", fileName));
                 qLNCKHDHTDTD.Notifications.Add(model);
                qLNCKHDHTDTD.SaveChanges();
                files.SaveAs(path);
                
                
                if (Object.Equals("GV"))
                    return Redirect("Index");
                else return Redirect("NotifiStu");
             
            }
            else
            {
                qLNCKHDHTDTD.Notifications.Add(model);
                qLNCKHDHTDTD.SaveChanges();
                 
                if (Object.Equals("GV"))
                    return Redirect("Index");
                else return Redirect("NotifiStu");
            }
          
        }

        public ActionResult sendEmail(string Body, string Subject, string Progress, selectedEmail emp)
        {
            viewbag();
            var list1 = (from c in qLNCKHDHTDTD.Notifications
                         select new FileNotifiModel
                         {
                             DateCreat = c.DateCreat,
                             PersonCreat = c.PersonCreat,
                             Title = c.Title,
                             Content = c.Content,
                             FileName = c.FileName
                         }).ToList();
            //var list1 = demo.Database.SqlQuery<tblFileDetail>("select * from tblFileDetails").ToList();

            // model.ListFile = list1;
            ViewBag.listNoti = list1;
            ViewBag.NotificationsLec = qLNCKHDHTDTD.Notifications.OrderByDescending(x => x.DateCreat).Where(a=>a.Object=="Giảng viên").ToList();
            ViewBag.NotificationsStu = qLNCKHDHTDTD.Notifications.OrderByDescending(x => x.DateCreat).Where(a => a.Object == "Sinh viên").ToList();

            var data = qLNCKHDHTDTD.Emails.FirstOrDefault();

            string from1 = data.NameEmail;

             using (MailMessage mail = new MailMessage())
            {
                 mail.From = new MailAddress(from1);
                mail.Body = Body;
                mail.Subject = Subject;
                 
                qLNCKHDHTDTD.SaveChanges();
                List<Information> information1 = new List<Information>();
               foreach (var i in emp.SelectedIdArray)
                {
                    mail.To.Add(i);

                }
              
                mail.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential networkCredential = new NetworkCredential(from1, data.PassWord);
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = networkCredential;
                smtp.Port = 587;
                smtp.Send(mail);
               

                return View("Index");
            }
            
        }
        public ActionResult getTypeList(int Progress)
        {

            List<TopicOfLecture> topicOfLectures = qLNCKHDHTDTD.TopicOfLectures.ToList();
            List<ProgressLe> progressLes = qLNCKHDHTDTD.ProgressLes.ToList();

            List<Information> informations = qLNCKHDHTDTD.Information.ToList();
            List<Author> authors = qLNCKHDHTDTD.Authors.ToList();
            
            qLNCKHDHTDTD.Configuration.ProxyCreationEnabled = false;
            if (Progress==0)
            {
                var listJournal = (from t in topicOfLectures
                                   join a in authors on t.IdTp equals a.IdTp
                                   join i in informations on a.Email equals i.Email                                    
                                   select new
                                   {                                        
                                       i.Email
                                   }).Distinct().ToList();
                return Json(listJournal, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var listJournal = (from t in topicOfLectures
                                   join a in authors on t.IdTp equals a.IdTp
                                   join i in informations on a.Email equals i.Email
                                   join p in progressLes on t.IdTp equals p.IdTp
                                   where p.Progress == Progress && new TopicOfLectureModel().dateLec(t.IdTp)==p.Date
                                   select new
                                   {
                                       i.Email
                                   }).Distinct().ToList();
                return Json(listJournal, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        //public ActionResult getTypeList1(int Progress)
        //{
        //    List<TopicOfStudent> topicOfStudents = qLNCKHDHTDTD.TopicOfStudents.ToList();

        //     List<ProgressSt> progressSts = qLNCKHDHTDTD.ProgressSts.ToList();
        //    qLNCKHDHTDTD.Configuration.ProxyCreationEnabled = false;
             
        //    if (Progress==0)
        //    {
        //        var listJournal = (from t in topicOfStudents
                                   
        //                           select new
        //                           {
        //                               t.IdSV,
        //                               t.Email

        //                           }).Distinct().ToList();
        //        return Json(listJournal, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        var listJournal = (from t in topicOfStudents
        //                           join p in progressSts on t.IdTp equals p.IdTp
        //                           where p.Progress==Progress
        //                           select new
        //                           {
        //                               t.IdSV,
        //                               t.Email

        //                           }).Distinct().ToList();
        //        return Json(listJournal, JsonRequestBehavior.AllowGet);
        //    }
             
        //}

        public ActionResult DownloadFile(string filePath)
        {
            string fullName = Server.MapPath("~" + filePath);

            byte[] fileBytes = GetFile(fullName);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filePath);
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }
         public ActionResult DetailNotification(string IdNo)
        {
            if (Session["Acess"] == null || Session["Acess"].ToString() == "null")
            {
                return RedirectToAction("Index", "Login");
            }
            var model = notify.detailNotification(IdNo);
            return View(model);

        }
        // hien thi thong bao va chi tiet thong bao
   
        public ActionResult DetailNo(long id)
        {
            // return connect.Notifications.Where(x => x.IdNo == id).ToList();
            ViewBag.Notifications = qLNCKHDHTDTD.Notifications.Where(x => x.IdNo == id).ToList();
            ViewBag.Detail = qLNCKHDHTDTD.Notifications.Where(x => x.IdNo == id).ToList(); 
            return View();
        }
      
    }
}