using Microsoft.EntityFrameworkCore;
using MS.GestaoEstoque.Models;

namespace MS.GestaoEstoque.Repository
{
    public class TransientDbContextFactory : DbContext
    {
        public DbSet<Estoque> Estoques { get; set; }
        public TransientDbContextFactory(DbContextOptions<TransientDbContextFactory> options) : base(options)
        {

        }
    }
}
