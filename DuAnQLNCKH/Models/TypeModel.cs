using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DuAnQLNCKH.Models
{
    public class TypeModel
    {
        private SqlConnection con;
        DHTDTTDNEntities1 qLNCKHDHTDTD = new DHTDTTDNEntities1();

        public void connection()
        {
            string constr = @"Data Source=DESKTOP-ECMGDNK\SQLEXPRESS;initial catalog=nckh_dhdn;integrated security=True";
            con = new SqlConnection(constr);

        }
        public string IdTy()
        {
            connection();
            con.Open();
            string sql = string.Format("declare cur_IdTy cursor for select count(IdTy) from Type open cur_IdTy declare @count int fetch next from cur_IdTy into @count if @count=0 begin insert into Type(IdTy) values ('1') select IdTy='T1' from Type delete from Type where IdTy=1 ;end; else begin select IdTy='T'+CAST(@count+1 as varchar(10)) from Type ;fetch next from cur_IdTy into @count ;end; close cur_IdTy deallocate cur_IdTy");
            SqlCommand a = new SqlCommand(sql, con);
            String a1 = (String)a.ExecuteScalar();
            con.Close();
            return a1;
        }
        
        public List<ChildDetail> listChild(int IdType)
        {

            return (from d in qLNCKHDHTDTD.DetailType1
                    join c in qLNCKHDHTDTD.ChildDetails on d.IdDetail equals c.IdDetail
                     where d.IdType == IdType
                     
                     select c
                    ).ToList();
                     
        }
        public List<ChildDetail> listHours(int IdDetail)
        {

            return qLNCKHDHTDTD.ChildDetails.Where(x => x.IdDetail == IdDetail).ToList();
                     
        }
        public List<DetailType1> listDetail(int IdType)
        {
            return qLNCKHDHTDTD.DetailType1.Where(x => x.IdType == IdType).ToList();
        }
    }
}