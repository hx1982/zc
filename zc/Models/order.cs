namespace zc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("order")]
    public partial class order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public order()
        {
            order_detail = new HashSet<order_detail>();
        }

        [Key]
        public int order_id { get; set; }

        [Required]
        [StringLength(20)]
        public string order_num { get; set; }

        public int order_type { get; set; }

        public int user_id { get; set; }

        public int package_id { get; set; }

        public int order_cash { get; set; }

        public int order_rep { get; set; }

        public bool is_pay { get; set; }

        public int? pay_type { get; set; }

        [StringLength(100)]
        public string logistics_company { get; set; }

        [StringLength(100)]
        public string logistics_num { get; set; }

        [StringLength(1000)]
        public string order_remark { get; set; }

        public DateTime? create_time { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<order_detail> order_detail { get; set; }

        public virtual package package { get; set; }

        public virtual user user { get; set; }
    }
}
