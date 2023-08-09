

using Microsoft.EntityFrameworkCore;

namespace Task2.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Todo> Todos { get; set; }
        public DbSet<User> Users { get; set; }


    }
}
