using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EF_Assign3
{
    public class DataContext : DbContext
    {
        public DbSet<BlogType> BlogTypes { get; set; }
        public DbSet<PostType> PostTypes { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }

        public string DbPath { get; set; }

        public DataContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "EF_Assign3.db");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");



    }

}
