using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using DuAnQLNCKH.Models;


namespace DuAnQLNCKH.Controllers
{
    public class StatisticController : Controller
    {
        // GET: Statistic
        DHTDTTDNEntities1 dHTDTTDNEntities1 = new DHTDTTDNEntities1();
        List<Author> authors= new DHTDTTDNEntities1().Authors.ToList();
        List<TopicOfLecture> topicOfLectures = new DHTDTTDNEntities1().TopicOfLectures.ToList();
        List<Subject> subjects = new DHTDTTDNEntities1().Subjects.ToList();
        List<PointTable> pointTables = new DHTDTTDNEntities1().PointTables.ToList();
        List<Models.Type> types = new DHTDTTDNEntities1().Types.ToList();
        List<Faculty> faculties = new DHTDTTDNEntities1().Faculties.ToList();
        List<Information> information = new DHTDTTDNEntities1().Information.ToList();
        List<TopicOfStudent> topicOfStudents = new DHTDTTDNEntities1().TopicOfStudents.ToList();
        List<ProgressLe> progressLes = new DHTDTTDNEntities1().ProgressLes.ToList();
        [Authorize(Roles = "1")]
        public ActionResult Index()
        {
           
            viewbag();
           
            return View();
             
        }
        public ActionResult getStudentList(string IdSu)
        {
            
            dHTDTTDNEntities1.Configuration.ProxyCreationEnabled = false;
            if (IdSu=="")
            {
                List<TopicOfStudent> DetailList1 = dHTDTTDNEntities1.TopicOfStudents.ToList();
                return Json(DetailList1, JsonRequestBehavior.AllowGet);
            }
            List<TopicOfStudent> DetailList = dHTDTTDNEntities1.TopicOfStudents.Where(x => x.IdSu == IdSu).ToList();
            return Json(DetailList, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "1")]
        public void ExportExcel1(List<TopicLectureStatisticModel> list)
        {
            Session["listEx1"] = list;
        }
        [Authorize(Roles = "1")]
        public void ExportExcel2(List<TopicStudenStatisticModel> list1)
        {
            Session["listEx2"] = list1;
        }
           
        public void viewbag()
        {
             
               var topicOfLecture = (from t in topicOfLectures
                                        join ty in types on t.IdType equals ty.IdType
                                        join f in faculties on t.IdFa equals f.IdFa
                                        where t.Status==4
                                      select new TopicOfLectureView
                                      {
                                          
                                          topicOfLecture = t,                                        
                                          type=ty,
                                          faculty=f
                                      }).ToList();
                ViewBag.listTopicOfLecture = topicOfLecture;
                var topicOfStudent1 = (from t in topicOfStudents                                       
                                        
                                        join s in subjects  on t.IdSu equals s.IdSu
                                        select new TopicOfStudentView
                                       {

                                           topicOfStudent = t,
                                           subject=s,                                           
 
                                       }).ToList();
                ViewBag.listtopicOfStudents = topicOfStudent1;
                
                ViewBag.listNameStu = new SelectList(topicOfStudents, "IdSV", "NameSt");

            ViewBag.listtype = new SelectList(types, "IdType", "NameType");
             ViewBag.listFacul = new SelectList(faculties, "IdFa", "Name");
             ViewBag.subjects1 = new SelectList(subjects, "IdSu", "NameCu");
             ViewBag.listNameLe = new SelectList(information, "Email", "NameLe");
            
         }

        public ActionResult getTypeList(int IdTy)
        {
            dHTDTTDNEntities1.Configuration.ProxyCreationEnabled = false;
            List<PointTable> DetailList = dHTDTTDNEntities1.PointTables.Where(x => x.IdP == IdTy).ToList();
            return Json(DetailList, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "1")]
        public ActionResult SearchLecture(string name)
        {
            List<TopicOfLecture> listTopicOfLecture = dHTDTTDNEntities1.TopicOfLectures.Where(x=>x.Name.Contains(name)).ToList();
            List<TopicOfStudent> listTopicOfStudent = dHTDTTDNEntities1.TopicOfStudents.ToList();
            ViewBag.listTopicOfStudent = listTopicOfStudent;
            ViewBag.listTopicOfLecture = listTopicOfLecture;
            return View("Index");
        }
      
        public string IdP1;
     
        [Authorize(Roles = "1")]
        [HttpPost]
        public ActionResult ExportExcel()
        {
            var gv = new GridView();

            gv.DataSource = Session["listEx1"];
            gv.DataBind();
            
            gv.HeaderRow.Cells[0].Text = "Tên công trình";
            gv.HeaderRow.Cells[1].Text = "Nhóm tác giả";
            gv.HeaderRow.Cells[2].Text = "Đơn vị chủ trì";
             
            gv.HeaderRow.Cells[3].Text = "Loại công trình";           
            gv.HeaderRow.Cells[4].Text = "Ngày bắt đầu";
             
            gv.HeaderRow.Cells[5].Text = "Kết thúc";
            gv.HeaderRow.Cells[6].Text = "Kinh phí";
            gv.HeaderRow.Cells[7].Text = "Xếp loại"; 
            gv.HeaderRow.Cells[8].Text = "Giờ";

            for (var i = 0; i < gv.Rows.Count; i++)
            {
                
                gv.Rows[i].Cells[1].Text = gv.Rows[i].Cells[1].Text.Replace("\n", "<br/>");
                gv.Rows[i].Cells[8].Text = gv.Rows[i].Cells[8].Text.Replace("\n", "<br/>");
              
            }
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=ThongKeCongTrinh.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            
            viewbag();
            return View("Index");
        }
        [Authorize(Roles = "1")]
        [HttpPost]
        public ActionResult ExportExcelStu()
        {
            var gv = new GridView();

            gv.DataSource = Session["listEx2"];
            gv.DataBind();
          
            gv.HeaderRow.Cells[0].Text = "Tên công trình";
            gv.HeaderRow.Cells[1].Text = "Tên sinh viên";
            gv.HeaderRow.Cells[2].Text = "Chuyên sâu";
             gv.HeaderRow.Cells[3].Text = "GV hướng dẫn";
            gv.HeaderRow.Cells[4].Text = "Ngày bắt đầu";                   
            gv.HeaderRow.Cells[5].Text = "Giờ";
            
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=ThongKeCongTrihSV.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();
            viewbag();
            return View("Index");
        } 
        public string idp()
        {

            return ViewBag.IdP;
        }
    }
}