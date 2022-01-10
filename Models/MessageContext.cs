using Microsoft.EntityFrameworkCore;

namespace TestTask.Models
{
    public class MessageContext : DbContext
    {
        public DbSet<MessageModel> messagesdb { get; set; }

        public MessageContext(DbContextOptions<MessageContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}