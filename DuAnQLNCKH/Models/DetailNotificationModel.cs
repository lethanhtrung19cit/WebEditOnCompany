using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuAnQLNCKH.Models
{
    public class DetailNotificationModel
    {
        private DHTDTTDNEntities1 qLNCKHDHTDTD = null;
        public DetailNotificationModel()
        {
            qLNCKHDHTDTD = new DHTDTTDNEntities1();
        }
        public List<Notification> detailNotification(string IdNo)
        {
            var list = qLNCKHDHTDTD.Database.SqlQuery<Notification>("select Title, Content, FileName from Notification where IdNo='01'");
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

    }
}