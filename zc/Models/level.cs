namespace zc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("level")]
    public partial class level
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public level()
        {
            users = new HashSet<user>();
        }

        [Key]
        public int level_id { get; set; }

        [Required]
        [StringLength(50)]
        public string level_name { get; set; }

        public int level_money { get; set; }

        public int level_money1 { get; set; }

        public decimal recom_rate1 { get; set; }

        public decimal recom_rate2 { get; set; }

        [StringLength(200)]
        public string level_image { get; set; }

        [StringLength(1000)]
        public string level_remark { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<user> users { get; set; }
    }
}
