namespace zc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("menu")]
    public partial class menu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public menu()
        {
            sysroles = new HashSet<sysrole>();
        }

        [Key]
        public int menu_id { get; set; }

        [Required]
        [StringLength(20)]
        public string menu_name { get; set; }

        public int menu_parent_id { get; set; }

        [StringLength(200)]
        public string menu_url { get; set; }

        [StringLength(1000)]
        public string menu_remark { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<sysrole> sysroles { get; set; }
    }
}
