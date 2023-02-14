namespace DataBaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TopicOfStudent")]
    public partial class TopicOfStudent
    {
        [Key]
        [StringLength(10)]
        public string IdTp { get; set; }

        [Column(TypeName = "text")]
        public string Name { get; set; }

        [Column(TypeName = "text")]
        public string NameSt { get; set; }

        [StringLength(12)]
        public string IdSV { get; set; }

        [StringLength(30)]
        public string Emmail { get; set; }

        [StringLength(10)]
        public string IdP { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateSt { get; set; }

        public int? Times { get; set; }

        public double? Expense { get; set; }

        [Column(TypeName = "text")]
        public string Status { get; set; }

        [Column(TypeName = "text")]
        public string Progress { get; set; }

        public virtual PointTable PointTable { get; set; }
    }
}
