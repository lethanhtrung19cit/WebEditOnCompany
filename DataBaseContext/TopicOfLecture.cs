namespace DataBaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TopicOfLecture")]
    public partial class TopicOfLecture
    {
        [Key]
        [StringLength(10)]
        public string IdTp { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(10)]
        public string IdLe { get; set; }

        [StringLength(10)]
        public string IdP { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateSt { get; set; }

        public int? Times { get; set; }

        public double? Expense { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(50)]
        public string Progress { get; set; }

        public virtual Information Information { get; set; }

        public virtual PointTable PointTable { get; set; }
    }
}
