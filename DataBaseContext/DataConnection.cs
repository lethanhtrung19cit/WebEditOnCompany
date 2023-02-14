using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DataBaseContext
{
    public partial class DataConnection : DbContext
    {
        public DataConnection()
            : base("name=nckh_dhdnEntities25")
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<DetailedStatement> DetailedStatements { get; set; }
        public virtual DbSet<Information> Information { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<PointTable> PointTables { get; set; }
        public virtual DbSet<Statement> Statements { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TopicOfLecture> TopicOfLectures { get; set; }
        public virtual DbSet<TopicOfStudent> TopicOfStudents { get; set; }
        public virtual DbSet<Type> Types { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .Property(e => e.PassWord)
                .IsUnicode(false);

            modelBuilder.Entity<Department>()
                .Property(e => e.IdKhoa)
                .IsUnicode(false);

            modelBuilder.Entity<Department>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<DetailedStatement>()
                .Property(e => e.NameTopic)
                .IsUnicode(false);

            modelBuilder.Entity<Information>()
                .Property(e => e.IdLe)
                .IsUnicode(false);

            modelBuilder.Entity<Information>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<Information>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Information>()
                .Property(e => e.Phone)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Information>()
                .Property(e => e.IdKhoa)
                .IsUnicode(false);

            modelBuilder.Entity<Notification>()
                .Property(e => e.IdNo)
                .IsUnicode(false);

            modelBuilder.Entity<Notification>()
                .Property(e => e.PersonCreat)
                .IsUnicode(false);

            modelBuilder.Entity<Notification>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Notification>()
                .Property(e => e.Content)
                .IsUnicode(false);

            modelBuilder.Entity<Notification>()
                .Property(e => e.FileName)
                .IsUnicode(false);

            modelBuilder.Entity<PointTable>()
                .Property(e => e.IdP)
                .IsUnicode(false);

            modelBuilder.Entity<PointTable>()
                .Property(e => e.IdTy)
                .IsUnicode(false);

            modelBuilder.Entity<PointTable>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Statement>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<TopicOfLecture>()
                .Property(e => e.IdTp)
                .IsUnicode(false);

            modelBuilder.Entity<TopicOfLecture>()
                .Property(e => e.IdLe)
                .IsUnicode(false);

            modelBuilder.Entity<TopicOfLecture>()
                .Property(e => e.IdP)
                .IsUnicode(false);

            modelBuilder.Entity<TopicOfStudent>()
                .Property(e => e.IdTp)
                .IsUnicode(false);

            modelBuilder.Entity<TopicOfStudent>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<TopicOfStudent>()
                .Property(e => e.NameSt)
                .IsUnicode(false);

            modelBuilder.Entity<TopicOfStudent>()
                .Property(e => e.IdSV)
                .IsUnicode(false);

            modelBuilder.Entity<TopicOfStudent>()
                .Property(e => e.Emmail)
                .IsUnicode(false);

            modelBuilder.Entity<TopicOfStudent>()
                .Property(e => e.IdP)
                .IsUnicode(false);

            modelBuilder.Entity<TopicOfStudent>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<TopicOfStudent>()
                .Property(e => e.Progress)
                .IsUnicode(false);

            modelBuilder.Entity<Type>()
                .Property(e => e.IdTy)
                .IsUnicode(false);

            modelBuilder.Entity<Type>()
                .Property(e => e.Name)
                .IsUnicode(false);
        }
    }
}
