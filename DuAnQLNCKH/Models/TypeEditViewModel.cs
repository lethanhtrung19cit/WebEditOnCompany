using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DuAnQLNCKH.Models
{
    public class TypeEditViewModel
    {
       

        [Required]
        [Display(Name = "Type")]
        public string SelectedType { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }

        [Required]
        [Display(Name = "DetailType")]
        public string SelectedDetailType { get; set; }
        public IEnumerable<SelectListItem> DetailTypes { get; set; }
    }
}