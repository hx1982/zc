namespace zc.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class user_bonus
    {
        [Key]
        public int bonus_id { get; set; }

        public int user_id { get; set; }

        public int dist_money { get; set; }

        public int dist_balance { get; set; }

        public int dist_number { get; set; }

        public int referrer_id1 { get; set; }

        public int referrer_money1 { get; set; }

        public int referrer_balance1 { get; set; }

        public int referrer_number1 { get; set; }

        public int? referrer_id2 { get; set; }

        public int? referrer_money2 { get; set; }

        public int? referrer_balance2 { get; set; }

        public int? referrer_number2 { get; set; }

        public virtual user user { get; set; }

        public virtual user user1 { get; set; }

        public virtual user user2 { get; set; }
    }
}
