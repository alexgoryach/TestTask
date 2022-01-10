using Microsoft.EntityFrameworkCore;
using TestTask.ViewModels;

namespace TestTask.Models
{
    public class FileContext : DbContext
    {
        public DbSet<FileModel> filesdb { get; set; }

        public FileContext(DbContextOptions<FileContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}