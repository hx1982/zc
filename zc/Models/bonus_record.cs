namespace zc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class bonus_record
    {
        [Key]
        public int bonus_record_id { get; set; }

        public int user_id { get; set; }

        public int bouns_type { get; set; }

        public int bouns_money { get; set; }

        public int? source_id { get; set; }

        public DateTime create_time { get; set; }

        [StringLength(1000)]
        public string bonus_remark { get; set; }

        public bool bouns_is_give { get; set; }

        public virtual user user { get; set; }

        public virtual user user1 { get; set; }
    }
}
