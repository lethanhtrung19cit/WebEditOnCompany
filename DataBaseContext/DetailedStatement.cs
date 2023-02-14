namespace DataBaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DetailedStatement")]
    public partial class DetailedStatement
    {
        [Key]
        public int IdDtST { get; set; }

        public int? IdSt { get; set; }

        [Column(TypeName = "text")]
        public string NameTopic { get; set; }

        public virtual Statement Statement { get; set; }
    }
}
