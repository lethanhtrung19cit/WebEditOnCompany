using DuAnQLNCKH.Models;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuAnQLNCKH.Controllers
{
    public class UpdatePointController : Controller
    {
        // GET: UpdatePoint
        DHTDTTDNEntities1 qLNCKHDHTDTD = new DHTDTTDNEntities1();
        public void ListInit()
        {
            ViewBag.listGrade = new List<SelectListItem>{
                       new SelectListItem { Value = "0" , Text = "Không đạt" },
                       new SelectListItem { Value = "1" , Text = "Đạt" },
                        new SelectListItem { Value = "2" , Text = "Khá" },
                         new SelectListItem { Value = "3" , Text = "Xuất sắc" }
            };
            ViewBag.listType = qLNCKHDHTDTD.Types.ToList();
        }
        [Authorize(Roles = "1")]
        public ActionResult Index()
        {
            ListInit();
            return View();
        }
        [Authorize(Roles = "1")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExcelData(HttpPostedFileBase upload, int IdType)
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
                        ListInit();
                        ModelState.AddModelError("File" + IdType, "Vui lòng chọn file đúng định dạng excel");
                        return View("Index");
                    }

                    DataTable dt_ = new DataTable();

                    dt_ = reader.AsDataSet().Tables[0];

                    TopicOfLectureModel dtsv = new TopicOfLectureModel();

                    for (int row_ = 1; row_ < dt_.Rows.Count; row_++)
                    {
                        TopicOfLecture topic = new TopicOfLecture();
                        PointTable point = new PointTable();
                        Author author = new Author();
                        string nameTopic = dt_.Rows[row_][1].ToString();
                        string Faculty = dt_.Rows[row_][2].ToString();
                        string listAuthor = dt_.Rows[row_][3].ToString();
                        string listEmail = dt_.Rows[row_][4].ToString();
                        string listHour = dt_.Rows[row_][9].ToString();
                        string dateCreate = dt_.Rows[row_][5].ToString();
                        if (string.IsNullOrEmpty(nameTopic) || string.IsNullOrEmpty(Faculty) || string.IsNullOrEmpty(listAuthor)
                            || string.IsNullOrEmpty(listEmail) || string.IsNullOrEmpty(listHour))
                        {
                            ListInit();
                            ModelState.AddModelError("File" + IdType, "Vui lòng nhập đầy đủ thông tin");
                            return View("Index");
                        }
                        topic.Name = nameTopic;
                        var IDTP = dtsv.IdTp();
                        topic.IdTp = IDTP;
                        author.IdTp = IDTP;
                        point.IdTp = IDTP;
                        topic.IdFa = qLNCKHDHTDTD.Faculties.Where(x => x.Name.Contains(Faculty)).Select(x => x.IdFa).FirstOrDefault().ToString();
                        //DateTime parsedDate;
                        var dataFinal = dateCreate.Split(' ');
                        try
                        {
                            topic.DateSt = DateTime.Parse(dataFinal[0]); 
                        }
                        catch (Exception e)
                        {
                            ListInit();
                            ModelState.AddModelError("File" + IdType, "Định dạng ngày tháng không hợp lệ!");
                            ModelState.AddModelError("File1" + IdType, dataFinal[0]);
                            return View("Index");
                        }
                        //var result = DateTime.TryParseExact(dataFinal[0], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);
                        // ModelState.AddModelError("File" + IdType, result.ToString()+"\n"+parsedDate.ToString());
                        //ListInit(); 
                        //return View("Index");
                        //if (DateTime.TryParseExact(dateCreate, "dd'/'MM'/'yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out parsedDate))
                        //{
                        //    topic.DateSt = parsedDate;
                        //}
                        //else
                        //{
                        //    ListInit();
                        //    ModelState.AddModelError("File" + IdType, "Định dạng ngày tháng không hợp lệ!");
                        //    return View("Index");
                        //}
                        topic.Times = int.Parse(dt_.Rows[row_][6].ToString());
                        topic.IdType = IdType;
                        topic.Expense = double.Parse(dt_.Rows[row_][7].ToString());

                        string g = dt_.Rows[row_][8].ToString();
                        if (g != null)
                        {
                            byte grade;
                            switch (g)
                            {
                                case "Xuất sắc":
                                    grade = 3;
                                    break;
                                case "Khá":
                                    grade = 2;
                                    break;
                                case "Đạt":
                                    grade = 1;
                                    break;
                                default:
                                    grade = 0;
                                    break;
                            }
                            point.Grade = grade;
                            point.Status = 1;
                            topic.Status = 4;
                        }
                        else
                        {
                            point.Status = 0;
                            topic.Status = 3;
                        }
                        List<string> h = dt_.Rows[row_][9].ToString().Split(new string[] { "\n" }, StringSplitOptions.None).ToList();
                        h = h.Where(x => x != "").ToList();

                        topic.CountAuthor = h.Count;
                        qLNCKHDHTDTD.TopicOfLectures.Add(topic);
                        qLNCKHDHTDTD.SaveChanges();

                        List<string> au = dt_.Rows[row_][3].ToString().Split(new string[] { "\n" }, StringSplitOptions.None).ToList();

                        List<string> email = dt_.Rows[row_][4].ToString().Split(new string[] { "\n" }, StringSplitOptions.None).ToList();
                        au = au.Where(x => x != "").ToList();
                        email = email.Where(x => x != "").ToList();
                        author.NameAu = au[0];
                        author.Email = email[0];
                        author.Grade = 0;
                        author.IsDeleted = false;
                        qLNCKHDHTDTD.Authors.Add(author);
                        qLNCKHDHTDTD.SaveChanges();
                        if (au.Count > 1)
                        {
                            for (int i = 1; i < au.Count; i++)
                            {
                                author.NameAu = au[i];
                                author.Email = email[i];
                                author.Grade = 1;
                                author.IsDeleted = false;
                                qLNCKHDHTDTD.Authors.Add(author);
                                qLNCKHDHTDTD.SaveChanges();
                            }
                        }

                        point.IdAu = qLNCKHDHTDTD.Authors.Where(x => x.IdTp == IDTP && x.Grade == 0).Select(x => x.IdAu).FirstOrDefault();
                        point.Hours = int.Parse(h[0]);
                        point.IsDeleted = false;
                        qLNCKHDHTDTD.PointTables.Add(point);
                        qLNCKHDHTDTD.SaveChanges();
                        if (au.Count > 1)
                        {
                            for (int i = 1; i < au.Count; i++)
                            {
                                var e = email[i].ToString();
                                point.IdAu = qLNCKHDHTDTD.Authors.Where(x => x.IdTp == IDTP && x.Grade == 1 && x.Email == e).Select(x => x.IdAu).FirstOrDefault();
                                point.Hours = int.Parse(h[i]);
                                point.IsDeleted = false;
                                qLNCKHDHTDTD.PointTables.Add(point);
                                qLNCKHDHTDTD.SaveChanges();
                            }
                        }

                    }
                    return RedirectToAction("Index");

                }
                else
                {
                    ListInit();
                    ModelState.AddModelError("File" + IdType, "Vui lòng chọn file trước khi upload");
                    return View("Index");
                }
            }
            return View();
        }
        public ActionResult TypeTopic()
        {
            var type = qLNCKHDHTDTD.Types.ToList();
            var detail = qLNCKHDHTDTD.DetailType1.ToList();
            var child = qLNCKHDHTDTD.ChildDetails.ToList();
            ViewBag.listTypeTopic = type;
            ViewBag.listDetail = detail;
            ViewBag.listType = new SelectList(type, "IdType", "NameType");
            ViewBag.listDetailType = (from t in type
                                      join d in detail on t.IdType equals d.IdType
                                      from c in child
                                      select new TopicOfLectureView
                                      {
                                          type = t,
                                          DetailType = d,
                                          ChildDetail = c
                                      }).ToList();
            return View();
        }
        public ActionResult addType(string nameType, byte Level)
        {
            Models.Type t = new Models.Type();
            t.NameType = nameType;
            t.Level = Level;
            qLNCKHDHTDTD.Types.Add(t);
            qLNCKHDHTDTD.SaveChanges();
            return Redirect("TypeTopic");
        }
        [Authorize(Roles = "1")]
        public ActionResult Approve(string IdTp, byte Grade)
        {

            var result = qLNCKHDHTDTD.PointTables.Where(x => x.IdTp == IdTp).ToList();
            for (int i = 0; i < result.Count; i++)
            {
                result[i].Grade = Grade;
                result[i].Status = 4;
            }
            qLNCKHDHTDTD.SaveChanges();

            var t = qLNCKHDHTDTD.TopicOfLectures.Where(x => x.IdTp == IdTp).FirstOrDefault();
            t.Status = 4;
            qLNCKHDHTDTD.SaveChanges();
            return Redirect("Index");
        }
        [Authorize(Roles = "1")]
        public void DisApprove(string IdTp)
        {
            var result = qLNCKHDHTDTD.TopicOfLectures.SingleOrDefault(x => x.IdTp == IdTp);
            result.Status = 2;
            qLNCKHDHTDTD.SaveChanges();
        }
        [Authorize(Roles = "1")]
        public void updateHour(int IdP, int Hours)
        {
            var p = qLNCKHDHTDTD.PointTables.Find(IdP);
            p.IdP = IdP;
            p.Hours = Hours;
            qLNCKHDHTDTD.Entry(p).State = System.Data.Entity.EntityState.Modified;
            qLNCKHDHTDTD.SaveChanges();
        }
        public ActionResult addDetailType(string[] nameDetail, int IdType, string[] nameChild, int[] Hours, byte level)
        {
            DetailType1 detailType = new DetailType1();
            ChildDetail childDetail = new ChildDetail();
            detailType.IdType = IdType;

            if (level == 0)
            {
                for (int d = 0; d < nameDetail.Length; d++)
                {

                    detailType.NameDetail = nameDetail[d];
                    detailType.Hours = Hours[d];
                    qLNCKHDHTDTD.DetailType1.Add(detailType);
                    qLNCKHDHTDTD.SaveChanges();
                }

            }
            else
            {
                int j = 0;
                int count = 0;
                for (int d = 0; d < nameDetail.Length; d++)
                {
                    detailType.NameDetail = nameDetail[d];
                    detailType.Hours = 0;
                    qLNCKHDHTDTD.Entry(detailType).State = System.Data.Entity.EntityState.Added;
                    qLNCKHDHTDTD.SaveChanges();
                    qLNCKHDHTDTD.Entry(detailType).State = System.Data.Entity.EntityState.Detached;

                    string nameD = nameDetail[d];
                    var IdDetail = qLNCKHDHTDTD.DetailType1.Where(x => x.NameDetail == nameD).Select(x => x.IdDetail).First();
                    childDetail.IdDetail = int.Parse(IdDetail.ToString());
                    for (int i = j; i < level + count; i++)
                    {
                        j++;
                        childDetail.NameChild = nameChild[i];
                        childDetail.Hours = Hours[i];
                        qLNCKHDHTDTD.ChildDetails.Add(childDetail);
                        qLNCKHDHTDTD.SaveChanges();
                    }
                    count += level;

                }


            }
            return Redirect("TypeTopic");
        }
        public ActionResult getLevel(int IdType)
        {
            qLNCKHDHTDTD.Configuration.ProxyCreationEnabled = false;
            int Level = int.Parse(qLNCKHDHTDTD.Types.Where(x => x.IdType == IdType).Select(x => x.Level).FirstOrDefault().ToString());
            return Json(Level, JsonRequestBehavior.AllowGet);
        }
    }
}