namespace zc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("user")]
    public partial class user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public user()
        {
            account_record = new HashSet<account_record>();
            bonus_record = new HashSet<bonus_record>();
            bonus_record1 = new HashSet<bonus_record>();
            cash_record = new HashSet<cash_record>();
        }

        [Key]
        public int user_id { get; set; }

        [Required]
        [StringLength(10)]
        public string user_code { get; set; }

        [Required]
        [StringLength(50)]
        public string user_name { get; set; }

        [Required]
        [StringLength(20)]
        public string user_phone { get; set; }

        [Required]
        [StringLength(20)]
        public string id_number { get; set; }

        [Required]
        [StringLength(50)]
        public string login_password { get; set; }

        [Required]
        [StringLength(50)]
        public string second_password { get; set; }

        public int referrer_id { get; set; }

        public int level_id { get; set; }

        [StringLength(50)]
        public string province { get; set; }

        [StringLength(50)]
        public string city { get; set; }

        [StringLength(50)]
        public string area { get; set; }

        [StringLength(200)]
        public string address { get; set; }

        [StringLength(50)]
        public string bank_name { get; set; }

        [StringLength(50)]
        public string account_num { get; set; }

        [StringLength(50)]
        public string wallet_adder { get; set; }

        public int user_status { get; set; }

        public int reg_money { get; set; }

        public DateTime register_time { get; set; }

        public DateTime? activate_time { get; set; }

        public int? activate_id { get; set; }

        [StringLength(1000)]
        public string user_remark { get; set; }

        [StringLength(1000)]
        public string user_remark1 { get; set; }

        [StringLength(1000)]
        public string user_remark2 { get; set; }

        [StringLength(1000)]
        public string user_remark3 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<account_record> account_record { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<bonus_record> bonus_record { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<bonus_record> bonus_record1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<cash_record> cash_record { get; set; }

        public virtual level level { get; set; }

        public virtual _operator _operator { get; set; }

        public virtual user_account user_account { get; set; }

        [ForeignKey("referrer_id")]
        public virtual user referrer { get; set; }
    }
}
