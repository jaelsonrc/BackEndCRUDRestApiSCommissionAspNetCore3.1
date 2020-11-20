using Microsoft.EntityFrameworkCore;
using ServiceCommission.Models;
using ServiceCommission.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceCommission.Repositories
{
    public class RepositoryContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Commission> Commissions { get; set; }

        public RepositoryContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();

        }


        public RepositoryContext() 
        {
            Database.EnsureCreated();

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseInMemoryDatabase("DbMemory");
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<User>().HasIndex(b => b.Login).IsUnique();
            modelBuilder.Entity<Commission>().HasKey(x => x.Id);
            base.OnModelCreating(modelBuilder);
        }



        public static RepositoryContext Build()
        {
            return new RepositoryContext();
        }
    }
}