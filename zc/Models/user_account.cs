namespace zc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class user_account
    {
        [Key]
        public int account_id { get; set; }

        public int user_id { get; set; }

        public int account1 { get; set; }

        public int account2 { get; set; }

        public int account3 { get; set; }

        public int account4 { get; set; }

        public virtual user user { get; set; }
    }
}
