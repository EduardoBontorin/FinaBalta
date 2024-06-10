using Fina.Api.Data.Mappings;
using Fina.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Fina.Api.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Transaction> Transactions { get; set; } = null!;//TODO: Estudar null not 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Mapear todas as classes que possuem mapping. Não precisa criar uma por uma
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }

}
