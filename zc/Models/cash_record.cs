namespace zc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class cash_record
    {
        [Key]
        public int cash_record_id { get; set; }

        public int user_id { get; set; }

        public int cash_type { get; set; }

        public int cash_money { get; set; }

        public int cash_status { get; set; }

        public DateTime cash_time1 { get; set; }

        public int? oper_id1 { get; set; }

        public DateTime? cash_time2 { get; set; }

        [StringLength(1000)]
        public string cash_remark1 { get; set; }

        public int? oper_id2 { get; set; }

        public DateTime? cash_time3 { get; set; }

        public virtual _operator _operator { get; set; }

        public virtual _operator operator1 { get; set; }

        public virtual user user { get; set; }
    }
}
