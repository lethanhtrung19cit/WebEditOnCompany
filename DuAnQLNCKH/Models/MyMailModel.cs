using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuAnQLNCKH.Models
{
    public class MyMailModel
    {
        public string From { get; set; }
        [Required(ErrorMessage = "Please enter your email address")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
        public string Email { get; set; }

        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}