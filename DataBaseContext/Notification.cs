namespace DataBaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Notification
    {
        [Key]
        [StringLength(10)]
        public string IdNo { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DateCreat { get; set; }

        [Column(TypeName = "text")]
        public string PersonCreat { get; set; }

        [Column(TypeName = "text")]
        public string Title { get; set; }

        [Column(TypeName = "text")]
        public string Content { get; set; }

        [Column(TypeName = "text")]
        public string FileName { get; set; }
    }
}
