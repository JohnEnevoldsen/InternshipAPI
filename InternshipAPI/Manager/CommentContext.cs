using InternshipAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InternshipAPI.Manager
{
    public class CommentContext : DbContext
    {
        public CommentContext(DbContextOptions<CommentContext> options) : base(options)
        {

        }

        public DbSet<Comment> Activity { get; set; }
    }
}
