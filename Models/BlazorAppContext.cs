using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Models
{
    public class BlazorAppContext : DbContext
    {
        public BlazorAppContext (DbContextOptions<BlazorAppContext> options)
            : base(options)
        {
        }
        
        public DbSet<BlazorApp.Models.Movie> Movie { get; set; }
        public DbSet<BlazorApp.Models.Author> Author { get; set; }
    }
}