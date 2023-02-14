using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using PagedList;
using System.Text;
using System.Threading.Tasks;


namespace DataBaseContext.DataMethod
{
    public class AccountSQL
    {
        DataConnection connect;
        public AccountSQL()
        {
            connect = new DataConnection();
        }
        // thêm tai khoan
        public int InsertAcc(string user, string pass, int access)
        {
            if (FindByUser(user) == true)
            {
                return 0;
            }
            else
            {
                object[] para =
                {
                    new SqlParameter("@userName",user),
                    new SqlParameter("@passWord", pass),
                    new SqlParameter("@access", access),
                };
                int res = connect.Database.ExecuteSqlCommand("Insert_Account @userName,@passWord,@access ", para);
                return res;
            }

        }
        // 
        public Account GetByUser(string user)
        {
            return connect.Accounts.SingleOrDefault(m => m.UserName == user);
        }
        // chi tiet tai khoan
        public Account DetailbyUser(string user)
        {
            return connect.Accounts.Find(user);
        }
        // tim username them mới
        public bool FindByUser(string user, string pass = null)
        {
            var find = connect.Accounts.Where(m => m.UserName == user && m.PassWord == pass).ToList();
            if (find.Count > 0)
                return true;
            else return false;
        }
        // danh sách tài khoản, tìm kiếm
        public IEnumerable<Account> getListAcc(string seach, int page, int pagesize)
        {
            IQueryable<Account> res = connect.Accounts;
            if (!string.IsNullOrEmpty(seach))
            {
                res = res.Where(x => x.UserName.Contains(seach));
            }
            return res.OrderBy(x => x.UserName).ToPagedList(page, pagesize);
        }
        // đăng nhập
        //public bool Login(string user, string pass)
        //{
        //    var res = connect.Accounts.SingleOrDefault(m => m.UserName == user && m.PassWord == pass);
        //    if (res != null)
        //    {
        //        if (res.Status == "0")
        //            return true;
        //        else
        //            return false;
        //    }
        //    return false;


        //}
        // sửa tài khoản
        //public bool UpdateAccount(string username, int access, string status)
        //{
        //    try
        //    {
        //        var acc = connect.Accounts.Find(username);
        //        if (access != 0)
        //            acc.Access = access;
        //        if (status != null)
        //            acc.Status = status;
        //        connect.SaveChanges();
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }
        //}
        // thay đổi mật khẩu
        public bool ChangePassword(string user, string pass)
        {
            try
            {
                var acc = connect.Accounts.Find(user);

                acc.PassWord = pass;
                connect.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
        // lấy thông tin theo session
        //public Information getInfo(string username)
        //{
        //    var res = from i in connect.Information
        //              join d in connect. on i.IdKhoa equals d.IdKhoa
        //              where i.UserName == username
        //              select new Information
        //              {
        //                  IdLe = i.IdLe,
        //                  Name = i.Name,
        //                  Email = i.Email,
        //                  Phone = i.Phone,
        //                  UserName = i.UserName,
        //                  Khoa = d.
        //              };
        //    res.FirstOrDefault();
        //    if (res != null)
        //        return res.FirstOrDefault();
        //    else return null;

        //}
        // lây thông tin để cập nhật
        public Information GetInformation(string username)
        {
            return connect.Information.Where(x => x.UserName == username).FirstOrDefault();
        }
        // cập nhật
        public bool Update(Information information, string username)
        {
            var model = connect.Information.Where(x => x.UserName == username).Single();
            if (model != null)
            {
                if (model.IdLe != information.IdLe || model.Name != information.Name ||
                   model.IdKhoa != information.IdKhoa || model.Email != information.Email || model.Phone != information.Phone)
                {
                    model.IdLe = information.IdLe;
                    model.Name = information.Name;
                    model.IdKhoa = information.IdKhoa;
                    model.Email = information.Email;
                    model.Phone = information.Phone;
                    connect.SaveChanges();
                    var model1 = connect.Information.Where(x => x.UserName == username).Single();
                    return true;
                }
            }


            return false;
        }
    }
}
