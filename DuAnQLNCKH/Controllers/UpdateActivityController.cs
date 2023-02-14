using DuAnQLNCKH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace DuAnQLNCKH.Controllers
{
    public class UpdateActivityController : Controller
    {
        // GET: UpdateActivity
        DHTDTTDNEntities1 entities = new DHTDTTDNEntities1();
        List<Extension> extensions = new DHTDTTDNEntities1().Extensions.ToList();
        List<TopicOfLecture> topicOfLectures = new DHTDTTDNEntities1().TopicOfLectures.ToList();
        List<ProgressLe> progressLes = new DHTDTTDNEntities1().ProgressLes.ToList();
        List<Information> information = new DHTDTTDNEntities1().Information.ToList();
        List<PointTable> pointTables = new DHTDTTDNEntities1().PointTables.ToList();
        List<Author> authors = new DHTDTTDNEntities1().Authors.ToList();
         List<TopicOfStudent> topicOfStudents = new DHTDTTDNEntities1().TopicOfStudents.ToList();
         List<Faculty> faculties = new DHTDTTDNEntities1().Faculties.ToList();
         List<Subject> subjects = new DHTDTTDNEntities1().Subjects.ToList();
        public void UpdateToLe(string IdTp, int Status)
        {
             
                 
                entities.Database.ExecuteSqlCommand("set dateformat dmy insert into ProgressLe(IdTp, Date, Progress) values ('" + IdTp+"', '"+DateTime.Now.ToString("dd/MM/yyyy")+"', "+Status+")");
                entities.SaveChanges();
             

        }
        public void UpdateToSt(string IdTp, int Status)
        {
            

                entities.Database.ExecuteSqlCommand("set dateformat dmy insert into ProgressSt(IdTp, Date, Progress) values ('" + IdTp + "', '" + DateTime.Now.ToString("dd/MM/yyyy") + "', " + Status + ")");
                entities.SaveChanges();
             
        }
        public void SendEmail()
        {

        }
        public ActionResult Index()
        {
            //SmtpClient smtp = new SmtpClient("smtp.mailtrap.io", 2525);
            //smtp.EnableSsl = true;
            //smtp.Credentials = new NetworkCredential("securesally@gmail.com", "nktpjwonjgwnayrj");
            // in the beginning of the file
           
            MailAddress to = new MailAddress("trunglethanh343@gmail.com");
            MailAddress from = new MailAddress("trunglethanh6@gmail.com");
            MailMessage message = new MailMessage(from, to);
            message.Subject = "Good morning, Charles";
            message.Body = "Charles, Long time no talk. Would you be up for lunch in Soho on Monday? I'm paying.;";
            
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("trunglethanh6@gmail.com", "xnmiecsavqjivjxt"),
                EnableSsl = true
                // specify whether your host accepts SSL connections
            };
            try
            {
                client.Send(message);
            }
            catch (SmtpException ex)
            {
                //return ex.ToString();
            }
            //try
            //{ 
            //        var senderEmail = new MailAddress("trunglethanh6@gmail.com", "Jamil");
            //        var receiverEmail = new MailAddress("trunglethanh343@gmail.com", "Receiver");
            //        var password = "lethanhtrung18082001";
            //        var sub = "test email";
            //        var body = "This is content";
            //        var smtp = new SmtpClient
            //        {
            //            Host = "smtp.gmail.com",
            //            Port = 587,
            //            EnableSsl = true,
            //            DeliveryMethod = SmtpDeliveryMethod.Network,
            //            UseDefaultCredentials = false,
            //            Credentials = new NetworkCredential(senderEmail.Address, password)
            //        };
            //        using (var mess = new MailMessage(senderEmail, receiverEmail)
            //        {
            //            Subject = "test email",
            //            Body = body
            //        })
            //        {
            //            smtp.Send(mess);
            //        }
            //        return View();

            //}
            //catch (Exception e)
            //{
            //    ViewBag.Error = "Some Error";
            //}
            var topicofLecture = (from t in topicOfLectures
                                      join a in authors on t.IdTp equals a.IdTp
                                      join i in information on a.Email equals i.Email
                                       join e in extensions on t.IdTp equals e.IdTp
                                      join pr in progressLes on t.IdTp equals pr.IdTp
                                      where i.Email==Session["UserName"].ToString()
                                      select new TopicOfLectureView
                                      {
                                           topicOfLecture = t,
                                          information = i,
                                          extension = e,
                                          progressLe=pr


                                      }).ToList();
                ViewBag.TopicOfLecture = topicofLecture;

                ViewBag.listProgress = new SelectList(new List<SelectListItem> {
                           new SelectListItem { Value = "1" , Text = "đang thực hiện" },
                           new SelectListItem { Value = "2" , Text = "quá hạn" } 
                }, "Value", "Text");

            return View();
       }
        //public ActionResult Student()
        //{
        //    List<TopicOfStudent> topicOfStudents = entities.TopicOfStudents.ToList();
        //      List<PointTable> pointTables = entities.PointTables.ToList();

        //    var topicofLecture = (from t in topicOfStudents 
        //                          join pr in progressLes on t.IdTp equals pr.IdTp
                                   
        //                          select new TopicOfLectureView
        //                          {
        //                              pointTable = p,
        //                              topicOfStudent = t,                                       
        //                              progressSt = pr


        //                          }).ToList();
        //    ViewBag.TopicOfStudent = topicofLecture;
        //    ViewBag.listProgress = new SelectList(new List<SelectListItem> {
        //                   new SelectListItem { Value = "1" , Text = "đang thực hiện" },
        //                   new SelectListItem { Value = "2" , Text = "quá hạn" }
        //        }, "Value", "Text");
        //    return View();
        //}
        public ActionResult getListMember(string IdTp)
        {
 
            entities.Configuration.ProxyCreationEnabled = false;

            var listJournal = entities.Authors.Where(x => x.IdTp == IdTp && x.Grade == 1).ToList();
            return Json(listJournal, JsonRequestBehavior.AllowGet);
            

        }
        public void accept(string IdTp)
        {
            var topicofLecture = (from t in topicOfLectures
                                  join a in authors on t.IdTp equals a.IdTp
                                  join i in information on a.Email equals i.Email
                                   where t.IdTp == IdTp
                                  select new TopicOfLectureView
                                  {
                                       topicOfLecture = t,
                                      information = i, 
                                      author = a
                                  }).ToList();
            ViewBag.detail = topicofLecture;
        }
        public ActionResult detailAcceptance(string IdTp)
        {
            ViewBag.Status = "";
            accept(IdTp);
            return View();
        }
        public ActionResult UpdateAcceptance()
        {
             
                var topicofLecture = (from t in topicOfLectures
                                      join a in authors on t.IdTp equals a.IdTp
                                      join i in information on a.Email equals i.Email
                                        where t.Status == 3
                                      select new TopicOfLectureView
                                      {
                                           topicOfLecture = t,
                                          information = i,                                           author=a
                                      }).ToList();
                ViewBag.TopicOfLecture = topicofLecture;
                var topicofStudent = (from t in topicOfStudents
                                      join s in subjects on t.IdSu equals s.IdSu
                                     
                                      select new TopicOfLectureView
                                      {
                                           topicOfStudent = t,
                                          subject=s
                                      }).ToList();
                ViewBag.TopicOfStudent = topicofStudent;
            
            return View();
        }
        [HttpPost]
        public ActionResult Acceptance(string IdAu1, string IdTp, string[] IdAu, string[] point, string[] time, string p, string t)
        {
             
            var a1 = entities.Authors.Find(int.Parse(IdAu1));
            //a1.Point = float.Parse(p);
            //a1.Time = int.Parse(t);
            entities.Entry(a1).State = System.Data.Entity.EntityState.Modified;
            entities.SaveChanges();
            for (int i=0;i<IdAu.Length;i++)
            {
                var a = entities.Authors.Find(int.Parse(IdAu[i]));
                //a.Point =float.Parse(point[i]);
                //a.Time =int.Parse(time[i]);
                entities.Entry(a).State = System.Data.Entity.EntityState.Modified;
                entities.SaveChanges();
            }
            ViewBag.Status = "Đã duyệt đề tài";
            var topic = entities.TopicOfLectures.Find(IdTp);
            topic.Status = 4;
             entities.Entry(topic).State = System.Data.Entity.EntityState.Modified;
            entities.SaveChanges();
            accept(IdTp);
            
            return View("detailAcceptance");
            
         }
        public void AcceptanceStu(string IdTp)
        {
            entities.Database.ExecuteSqlCommand("update TopicOfStudent set Status=N'đã nghiệm thu' where IdTp='" + IdTp + "'");
            entities.SaveChanges();
         }
        //public ActionResult ReportToSv()
        //{
             
                
        //        List<TopicOfStudent> topicOfStudents = entities.TopicOfStudents.ToList();
        //        List<ProgressSt> progressSts = entities.ProgressSts.ToList();
        //        List<PointTable> pointTables = entities.PointTables.ToList();

                
        //        var topics = (from t in topicOfStudents
        //                      join p in pointTables on t.IdP equals p.IdP
                               
        //                      where t.Status == 0
        //                      select new TopicOfStudentView
        //                      {

        //                          topicOfStudent = t,                                  
        //                          pointTable = p,
                                  

        //                      }).ToList();
        //        ViewBag.listTopicOfStudent = topics;
        //        var detailProgress = (from t in topicOfStudents

        //                              join pr in progressSts on t.IdTp equals pr.IdTp

        //                              select new TopicOfStudentView
        //                              {

        //                                  topicOfStudent = t,
        //                                  progressSt = pr

        //                              }).ToList();
        //        ViewBag.detailProgressSt = detailProgress;
            
        //    return View();
        //}
        
    }
}