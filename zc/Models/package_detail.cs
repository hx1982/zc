namespace zc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class package_detail
    {
        [Key]
        public int package_detail_id { get; set; }

        public int package_id { get; set; }

        public int goods_id { get; set; }

        public int goods_num { get; set; }

        public virtual good good { get; set; }

        public virtual package package { get; set; }
    }
}
