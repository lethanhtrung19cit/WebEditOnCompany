namespace DataBaseContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Information
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Information()
        {
            TopicOfLectures = new HashSet<TopicOfLecture>();
        }

        [Key]
        [StringLength(10)]
        public string IdLe { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(20)]
        public string UserName { get; set; }

        [StringLength(30)]
        public string Email { get; set; }

        [StringLength(10)]
        public string Phone { get; set; }

        [StringLength(10)]
        public string IdKhoa { get; set; }

        public virtual Account Account { get; set; }

        public virtual Department Department { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TopicOfLecture> TopicOfLectures { get; set; }
    }
}
