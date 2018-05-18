namespace zc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class good
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public good()
        {
            order_detail = new HashSet<order_detail>();
            package_detail = new HashSet<package_detail>();
        }

        [Key]
        public int goods_id { get; set; }

        [Required]
        [StringLength(200)]
        public string goods_name { get; set; }

        [Required]
        [StringLength(10)]
        public string goods_unit { get; set; }

        public int case_price { get; set; }

        public int rep_price { get; set; }

        [StringLength(200)]
        public string goods_image { get; set; }

        public string goods_desc { get; set; }

        [StringLength(1000)]
        public string goods_remark { get; set; }

        public int oper_id { get; set; }

        public DateTime create_time { get; set; }

        public virtual _operator _operator { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order_detail> order_detail { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<package_detail> package_detail { get; set; }
    }
}
