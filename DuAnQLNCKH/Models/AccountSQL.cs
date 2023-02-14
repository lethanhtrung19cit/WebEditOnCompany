using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuAnQLNCKH.Models
{
    public class AccountSQL
    {
        DHTDTTDNEntities1 connect;
        public AccountSQL()
        {
            connect = new DHTDTTDNEntities1();
        }
        // thêm tai khoan
        public void InsertAcc(string user, string pass, int access, int Position, string Name)
        {
            Account account = new Account();
            account.Email = user;
            account.PassWord = pass;
            account.Access = access;
            account.Status = 0;
            var e = connect.Information.Where(x => x.Email == user).Select(x => x.Email).ToList();
            if (e.Count==0)
            {
                Information information = new Information();
                information.Email = user;
                information.IdPo = Position;
                information.NameLe = Name;
                 
                connect.Information.Add(information);
            }
            connect.Accounts.Add(account);
             connect.SaveChanges();
        
        }
        
        public Account GetByUser(string user)
        {
            return  connect.Accounts.SingleOrDefault(m => m.Email == user);
        }
        // chi tiet tai khoan
        public Account DetailbyUser(int ID)
        {
            return connect.Accounts.Find(ID);
        }
        // tim username them mới
        public bool FindByUser(string user, int Access)
        {
            var find = connect.Accounts.Where(m => m.Email == user && m.Access==Access).ToList();
            if (find.Count > 0)
                return true;
            else return false;
        }
        // danh sách tài khoản, tìm kiếm
        //public IEnumerable<Account> getListAcc(string seach, int page, int pagesize)
        //{
        //    IQueryable<Account> res = connect.Accounts;
        //    if (!string.IsNullOrEmpty(seach))
        //    {
        //        res = res.Where(x => x.Email.Contains(seach));
        //    }
        //    return res.OrderBy(x => x.Email).ToPagedList(page, pagesize);
        //}
        public IEnumerable<Account> getListAcc(string seach, int page, int pagesize)
        {
            var list = connect.Accounts.Where(x=>x.Status==0 || x.Status==1).ToList();

            if (!string.IsNullOrEmpty(seach))
            {
                list = list.Where(x => x.Email.Contains(seach)).ToList();
            }
            return list.OrderBy(x => x.Email).ToPagedList(page, pagesize);
        }
        //sửa tài khoản
        public bool UpdateAccount(string username, int access, int status)
        {
            try
            {
                var acc = connect.Accounts.Where(x=>x.Email==username && x.Access==access).FirstOrDefault();                                                  
                acc.Status = status;
                connect.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
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
            return connect.Information.Where(x => x.Email == username).FirstOrDefault();
        }
        // cập nhật
        //public bool Update(Information information, string username)
        //{
        //    var model = connect.Information.Where(x => x.UserName == username).Single();
        //    if (model != null)
        //    {
        //        if (model.IdLe != information.IdLe || model.Name != information.Name ||
        //           model.IdKhoa != information.IdKhoa || model.Email != information.Email || model.Phone != information.Phone)
        //        {
        //            model.IdLe = information.IdLe;
        //            model.Name = information.Name;
        //            model.IdKhoa = information.IdKhoa;
        //            model.Email = information.Email;
        //            model.Phone = information.Phone;
        //            connect.SaveChanges();
        //            var model1 = connect.Information.Where(x => x.UserName == username).Single();
        //            return true;
        //        }
        //    }


        //    return false;
        //}
    }
}