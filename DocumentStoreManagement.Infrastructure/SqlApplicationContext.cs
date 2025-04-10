using DocumentStoreManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentStoreManagement.Infrastructure
{
    /// <summary>
    /// Sql Application Db Context
    /// </summary>
    /// <param name="options"></param>
    public class SqlApplicationContext(DbContextOptions options) : DbContext(options)
    {
        /// <summary>
        /// Student Table
        /// </summary>
        public DbSet<Student> Students { get; set; }

        /// <summary>
        /// Document Table
        /// </summary>
        public DbSet<Document> Documents { get; set; }

        /// <summary>
        /// Book Table
        /// </summary>
        public DbSet<Book> Books { get; set; }

        /// <summary>
        /// Magazine Table
        /// </summary>
        public DbSet<Magazine> Magazines { get; set; }

        /// <summary>
        /// Newspaper Table
        /// </summary>
        public DbSet<Newspaper> Newspapers { get; set; }

        /// <summary>
        /// Order Table
        /// </summary>
        public DbSet<Order> Orders { get; set; }

        /// <summary>
        /// OrderDetail Table
        /// </summary>
        public DbSet<OrderDetail> OrderDetails { get; set; }
    }
}
