//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DuAnQLNCKH.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Statement
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Statement()
        {
            this.DetailStatementLes = new HashSet<DetailStatementLe>();
        }
    
        public int IdSt { get; set; }
        public System.DateTime DateRp { get; set; }
        public string Status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DetailStatementLe> DetailStatementLes { get; set; }
    }
}