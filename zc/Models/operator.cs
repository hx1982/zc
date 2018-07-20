namespace zc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("operator")]
    public partial class _operator
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public _operator()
        {
            account_record = new HashSet<account_record>();
            cash_record = new HashSet<cash_record>();
            cash_record1 = new HashSet<cash_record>();
           // users = new HashSet<user>();
            sysroles = new HashSet<sysrole>();
        }

        [Key]
        public int oper_id { get; set; }

        [Required]
        [StringLength(20)]
        public string oper_code { get; set; }

        [Required]
        [StringLength(100)]
        public string oper_name { get; set; }

        [Required]
        [StringLength(20)]
        public string oper_phone { get; set; }

        [Required]
        [StringLength(50)]
        public string oper_password { get; set; }

        [Required]
        [StringLength(100)]
        public string oper_department { get; set; }

        [Required]
        [StringLength(200)]
        public string oper_permission { get; set; }

        [StringLength(1000)]
        public string oper_remark { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<account_record> account_record { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cash_record> cash_record { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cash_record> cash_record1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<user> users { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sysrole> sysroles { get; set; }
    }
}
