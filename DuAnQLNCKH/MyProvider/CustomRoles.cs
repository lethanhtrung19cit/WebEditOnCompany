using DuAnQLNCKH.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace DuAnQLNCKH.MyProvider
{
    public class CustomRoles : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            DHTDTTDNEntities1 conn = new DHTDTTDNEntities1();
            string[] data = conn.Accounts.Where(x => x.Email == username).Select(x => x.Access.ToString()).ToArray();
            int lengt = data.Length;
            if (lengt > 0)
            {
                var link = HttpContext.Current.Request.Url.ToString();
                if (link.Contains("Admin"))
                {
                    HttpContext.Current.Session["Acess"] = "0";
                    HttpContext.Current.Session["UserName"] = username;
                }
                else if (link.Contains("myTopicLecture") 
                    || link.Contains("CreateTopicOfLecture") 
                    || link.Contains("ViewCreateTopicOfLecture")
                    || link.Contains("UserLecture"))
                {
                    HttpContext.Current.Session["Acess"] = "2";
                    HttpContext.Current.Session["UserName"] = username;

                }
                else
                {
                    HttpContext.Current.Session["Acess"] = "1";
                    HttpContext.Current.Session["UserName"] = username;                    
                }
                return data;
            }
            return null;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}