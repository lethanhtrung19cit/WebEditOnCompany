using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DuAnQLNCKH.Models
{
    public class NotificationModel 
    {
        private SqlConnection con;
        public void connection()
        {
            string constr = @"Data Source=DESKTOP-ECMGDNK\SQLEXPRESS;initial catalog=nckh_dhdn;integrated security=True";
            con = new SqlConnection(constr);

        }
        private DHTDTTDNEntities1 qLNCKHDHTDTD = null;
        public NotificationModel()
        {
            qLNCKHDHTDTD = new DHTDTTDNEntities1();
        }
        
        public List<Notification> listAll()
        {
            var list = qLNCKHDHTDTD.Database.SqlQuery<Notification>("select * from Notification").ToList();
            return list;
        }
        public List<Notification> detailNotification(string IdNo)
        {
            var list = qLNCKHDHTDTD.Database.SqlQuery<Notification>("select * from Notification where IdNo='"+IdNo+"'");
            return list.ToList();
            //using (var context = new DHTDTTDNEntities1())
            //{
            //    var query = from st in context.Notifications
            //                where st.IdNo == IdNo
            //                select st ;

            //    var idNo = query.FirstOrDefault<Notification>();
            //}
            //var list = qLNCKHDHTDTD.Database.SqlQuery<Notification>("select Title, Content, FileName from Notification where IdNo='" + IdNo+"'");
            //return list.ToList();
        }
        public bool AddNoTification(Notification notification, string PersonCreate, string Object)
        {
            
           
            connection();
            SqlCommand com = new SqlCommand("AddNotifi", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@DateCreate", DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy")));
            com.Parameters.AddWithValue("@PersonCreate", PersonCreate);
            com.Parameters.AddWithValue("@Title", notification.Title);          
            com.Parameters.AddWithValue("@Content", notification.Content);
            com.Parameters.AddWithValue("@FileName", notification.FileName);
            com.Parameters.AddWithValue("@Object", Object);
            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {

                return true;

            }
            else
            {

                return false;
            }
        }
    }
}