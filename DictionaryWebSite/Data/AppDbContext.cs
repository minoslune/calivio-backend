using DictionaryWebSite.Models_DTOs;
using Microsoft.EntityFrameworkCore;

namespace DictionaryWebSite.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Word> Words { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
