namespace zc.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ZCDBContext : DbContext
    {
        public ZCDBContext()
            : base("name=ZCDBContext")
        {
        }

        public virtual DbSet<account_record> account_record { get; set; }
        public virtual DbSet<bonus_record> bonus_record { get; set; }
        public virtual DbSet<cash_record> cash_record { get; set; }
        public virtual DbSet<level> levels { get; set; }
        public virtual DbSet<menu> menus { get; set; }
        public virtual DbSet<_operator> operators { get; set; }
        public virtual DbSet<sysrole> sysroles { get; set; }
        public virtual DbSet<user> users { get; set; }
        public virtual DbSet<user_account> user_account { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
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
                .HasMany(e => e.users)
                .WithOptional(e => e._operator)
                .HasForeignKey(e => e.activate_id);

            modelBuilder.Entity<_operator>()
                .HasMany(e => e.sysroles)
                .WithMany(e => e.operators)
                .Map(m => m.ToTable("operator_sysrole").MapLeftKey("oper_id").MapRightKey("role_id"));

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
                .Property(e => e.wallet_adder)
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
                .WithOptional(e => e.user1)
                .HasForeignKey(e => e.source_id);

            modelBuilder.Entity<user>()
                .HasMany(e => e.cash_record)
                .WithRequired(e => e.user)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<user>()
                .HasOptional(e => e.user_account)
                .WithRequired(e => e.user);

        }
    }
}
