namespace zc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class account_record
    {
        [Key]
        public int acc_record_id { get; set; }

        public int user_id { get; set; }

        public int acc_type { get; set; }

        public int cons_type { get; set; }

        public int acc_record_type { get; set; }

        public int cons_value { get; set; }

        public int? acc_balance { get; set; }

        public int? oper_id { get; set; }

        [StringLength(1000)]
        public string acc_remark { get; set; }

        [StringLength(1000)]
        public string acc_re_remark1 { get; set; }

        [StringLength(1000)]
        public string acc_re_remark2 { get; set; }

        public DateTime acc_record_time { get; set; }

        public virtual _operator _operator { get; set; }

        public virtual user user { get; set; }
    }
}
