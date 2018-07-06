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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int user_id { get; set; }

        public int account1_total { get; set; }

        public int account1_use { get; set; }

        public int account1_balance { get; set; }

        public int account2_total { get; set; }

        public int account2_use { get; set; }

        public int account2_balance { get; set; }

        public int account3_total { get; set; }

        public int account3_use { get; set; }

        public int account3_balance { get; set; }

        public virtual user user { get; set; }
    }
}
