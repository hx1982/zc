namespace zc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class order_detail
    {
        [Key]
        public int order_detail_id { get; set; }

        public int order_id { get; set; }

        public int goods_id { get; set; }

        public int goods_num { get; set; }

        public int cash_price { get; set; }

        public int rep_price { get; set; }
        
        public virtual good good { get; set; }

        public virtual order order { get; set; }
    }
}
