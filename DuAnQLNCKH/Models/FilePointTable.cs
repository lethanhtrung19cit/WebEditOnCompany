using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DuAnQLNCKH.Models
{
    public class FilePointTable
    {
        public int IdP { get; set; }
        public string IdTy { get; set; }
        public string NameP { get; set; }
        public string File1 { get; set; }
        public List<FilePointTable> ListFile { get; set; }
    }
}