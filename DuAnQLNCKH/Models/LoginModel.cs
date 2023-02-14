using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace DuAnQLNCKH.Models
{
    public class LoginModel
    {
        private DHTDTTDNEntities1 qLNCKHDHTDTD = null;
        Account account = new Account();
        public LoginModel()
        {
            qLNCKHDHTDTD = new DHTDTTDNEntities1();
        }
        
    }
}