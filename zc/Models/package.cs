namespace zc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("package")]
    public partial class package
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public package()
        {
            orders = new HashSet<order>();
            package_detail = new HashSet<package_detail>();
        }

        [Key]
        public int package_id { get; set; }

        [Required]
        [StringLength(200)]
        public string package_name { get; set; }

        public int package_price { get; set; }

        public int package_status { get; set; }

        [Required]
        [StringLength(1000)]
        public string package_remark { get; set; }

        public int oper_id { get; set; }

        public DateTime create_time { get; set; }

        public virtual _operator _operator { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order> orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<package_detail> package_detail { get; set; }
    }
}
