using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DuAnQLNCKH.Models
{
    public class TopicOfStudentModel
    {

        //string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        private SqlConnection con;
        private DHTDTTDNEntities1 qLNCKHDHTDTD = null;
        public TopicOfStudentModel()
        {
            qLNCKHDHTDTD = new DHTDTTDNEntities1();
        }
         
        public string IdTp()
        {
            string IdTp = null;            
            List<string> ID = qLNCKHDHTDTD.TopicOfStudents.Select(x => x.IdTp.Substring(5, x.IdTp.Length - 5)).ToList();
            if (ID.Count==0)
            {
                IdTp = "DTSV01";
            }
            else
            {
                List<int> IDINT = ID.OrderByDescending(x => int.Parse(x)).Select(x => int.Parse(x)).ToList();
                IdTp = "DTSV0" + (IDINT[0] + 1);
            }           
            return IdTp;
            
        }
        public List<TopicOfStudent> listchuaduyet()
        {
            var list = qLNCKHDHTDTD.Database.SqlQuery<TopicOfStudent>("select * from TopicOfStudent where Status like N'chưa duyệt'").ToList();
            return list;
        }
        public List<TopicOfStudent> listAll()
        {
            var list = qLNCKHDHTDTD.Database.SqlQuery<TopicOfStudent>("select * from TopicOfStudent where Status=N'đã duyệt'").ToList();
            return list;
        }
         
        public TopicOfStudent ViewDetail(string IdTp)
        {
            return qLNCKHDHTDTD.TopicOfStudents.Find(IdTp);
        }
        public bool xoa(string IdTp)
        {
            try
            {
                var topic = qLNCKHDHTDTD.TopicOfStudents.Find(IdTp);
                qLNCKHDHTDTD.TopicOfStudents.Remove(topic);

                qLNCKHDHTDTD.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

         
        }
        public bool xetduyet1(string IdTp)
        {
            try
            {


                //detai.TrangThai = dtgv1.TrangThai;                 
                qLNCKHDHTDTD.Database.ExecuteSqlCommand("update TopicOfStudents set Status=N'đã duyệt' where IdTp=@IdTp",
                     new SqlParameter("@IdTp", IdTp)
                    );

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

         
    }
}