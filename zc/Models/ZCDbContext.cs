namespace zc.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ZCDbContext : DbContext
    {
        public ZCDbContext()
            : base("name=ZCDbContext")
        {

        }

        public virtual DbSet<account_record> account_record { get; set; }
        public virtual DbSet<bonus_record> bonus_record { get; set; }
        public virtual DbSet<cash_record> cash_record { get; set; }
        public virtual DbSet<good> goods { get; set; }
        public virtual DbSet<level> levels { get; set; }
        public virtual DbSet<menu> menus { get; set; }
        public virtual DbSet<_operator> operators { get; set; }
        public virtual DbSet<order> orders { get; set; }
        public virtual DbSet<order_detail> order_detail { get; set; }
        public virtual DbSet<package> packages { get; set; }
        public virtual DbSet<package_detail> package_detail { get; set; }
        public virtual DbSet<sysrole> sysroles { get; set; }
        public virtual DbSet<user> users { get; set; }
        public virtual DbSet<user_account> user_account { get; set; }
        public virtual DbSet<user_bonus> user_bonus { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<good>()
                .Property(e => e.goods_image)
                .IsUnicode(false);

            modelBuilder.Entity<good>()
                .HasMany(e => e.order_detail)
                .WithRequired(e => e.good)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<good>()
                .HasMany(e => e.package_detail)
                .WithRequired(e => e.good)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<level>()
                .Property(e => e.recom_rate1)
                .HasPrecision(4, 3);

            modelBuilder.Entity<level>()
                .Property(e => e.recom_rate2)
                .HasPrecision(4, 3);

            modelBuilder.Entity<level>()
                .Property(e => e.level_image)
                .IsUnicode(false);

            modelBuilder.Entity<level>()
                .HasMany(e => e.users)
                .WithRequired(e => e.level)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<menu>()
                .Property(e => e.menu_url)
                .IsUnicode(false);

            modelBuilder.Entity<menu>()
                .HasMany(e => e.sysroles)
                .WithMany(e => e.menus)
                .Map(m => m.ToTable("sysrole_menu").MapLeftKey("menu_id").MapRightKey("role_id"));

            modelBuilder.Entity<_operator>()
                .Property(e => e.oper_code)
                .IsUnicode(false);

            modelBuilder.Entity<_operator>()
                .Property(e => e.oper_phone)
                .IsUnicode(false);

            modelBuilder.Entity<_operator>()
                .Property(e => e.oper_password)
                .IsUnicode(false);

            modelBuilder.Entity<_operator>()
                .Property(e => e.oper_permission)
                .IsUnicode(false);

            modelBuilder.Entity<_operator>()
                .HasMany(e => e.cash_record)
                .WithOptional(e => e._operator)
                .HasForeignKey(e => e.oper_id1);

            modelBuilder.Entity<_operator>()
                .HasMany(e => e.cash_record1)
                .WithOptional(e => e.operator1)
                .HasForeignKey(e => e.oper_id2);

            modelBuilder.Entity<_operator>()
                .HasMany(e => e.goods)
                .WithRequired(e => e._operator)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<_operator>()
                .HasMany(e => e.packages)
                .WithRequired(e => e._operator)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<_operator>()
                .HasMany(e => e.sysroles)
                .WithMany(e => e.operators)
                .Map(m => m.ToTable("operator_sysrole").MapLeftKey("oper_id").MapRightKey("role_id"));

            modelBuilder.Entity<order>()
                .Property(e => e.order_num)
                .IsUnicode(false);

            modelBuilder.Entity<order>()
                .Property(e => e.logistics_num)
                .IsUnicode(false);

            modelBuilder.Entity<order>()
                .HasMany(e => e.order_detail)
                .WithRequired(e => e.order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<package>()
                .HasMany(e => e.orders)
                .WithRequired(e => e.package)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<package>()
                .HasMany(e => e.package_detail)
                .WithRequired(e => e.package)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<user>()
                .Property(e => e.user_code)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.user_phone)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.id_number)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.login_password)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.second_password)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.account_num)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .HasMany(e => e.account_record)
                .WithRequired(e => e.user)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<user>()
                .HasMany(e => e.bonus_record)
                .WithRequired(e => e.user)
                .HasForeignKey(e => e.user_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<user>()
                .HasMany(e => e.bonus_record1)
                .WithRequired(e => e.user1)
                .HasForeignKey(e => e.source_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<user>()
                .HasMany(e => e.cash_record)
                .WithRequired(e => e.user)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<user>()
                .HasMany(e => e.orders)
                .WithRequired(e => e.user)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<user>()
                .HasMany(e => e.user_account)
                .WithRequired(e => e.user)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<user>()
                .HasMany(e => e.user_bonus)
                .WithRequired(e => e.user)
                .HasForeignKey(e => e.user_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<user>()
                .HasMany(e => e.user_bonus1)
                .WithRequired(e => e.user1)
                .HasForeignKey(e => e.referrer_id1)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<user>()
                .HasMany(e => e.user_bonus2)
                .WithOptional(e => e.user2)
                .HasForeignKey(e => e.referrer_id2);

        }
    }
}
