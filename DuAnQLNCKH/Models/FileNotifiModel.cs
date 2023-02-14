using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DuAnQLNCKH.Models
{
    public class FileNotifiModel
    {
        public DateTime DateCreat { get; set; }
        public string PersonCreat { get; set; }
        [Required(ErrorMessage = "Nhập tiêu đề thông báo")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Nhập nội dung thông báo")]
        public string Content { get; set; }
        public string FileName { get; set; }
        public List<FileNotifiModel> ListFile { get; set; }
    }
}