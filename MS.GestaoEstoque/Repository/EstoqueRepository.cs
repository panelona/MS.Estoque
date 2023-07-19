using Microsoft.EntityFrameworkCore;
using MS.GestaoEstoque.Interface;
using MS.GestaoEstoque.Models;
using System.Linq.Expressions;

namespace MS.GestaoEstoque.Repository
{
    public class EstoqueRepository : IEstoqueRepository
    {
        private readonly IDbContextFactory<TransientDbContextFactory> _context;

        public EstoqueRepository(IDbContextFactory<TransientDbContextFactory> context)
        {
            _context = context;
        }

        public async Task AddAsync(Estoque estoque)
        {
            using (var context = _context.CreateDbContext())
            {
                await context.AddAsync(estoque);
                await context.SaveChangesAsync();
            }
        }

        public async Task EditAsync(Estoque estoque)
        {
            using (var context = _context.CreateDbContext())
            {
                context.Update(estoque);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Estoque> FindAsNoTrackingAsync(Expression<Func<Estoque, bool>> expression)
        {
            using (var context = _context.CreateDbContext())
            {
                return await context.Set<Estoque>().AsNoTracking().FirstOrDefaultAsync(expression);
            }
        }

        public async Task<Estoque> FindAsync(Guid id)
        {
            using (var context = _context.CreateDbContext())
            {
                return await context.Set<Estoque>().FindAsync(id);
            }
        }

        public async Task<Estoque> FindAsync(Expression<Func<Estoque, bool>> expression)
        {
            using (var context = _context.CreateDbContext())
            {
                return await context.Set<Estoque>().FirstOrDefaultAsync(expression);
            }
        }

        public async Task<IEnumerable<Estoque>> ListAsync()
        {
            using (var context = _context.CreateDbContext())
            {
                return await context.Set<Estoque>().ToListAsync();
            }
        }

        public async Task<IEnumerable<Estoque>> ListAsync(Expression<Func<Estoque, bool>> expression)
        {
            using (var context = _context.CreateDbContext())
            {
                return await context.Set<Estoque>().Where(expression).ToListAsync();
            }
        }

        public async Task EditRangeAsync(IEnumerable<Estoque> estoques)
        {
            using (var context = _context.CreateDbContext())
            {
                context.Set<Estoque>().UpdateRange(estoques);
                await context.SaveChangesAsync();
            }
        }

        public async Task RemoveAsync(Estoque estoque)
        {
            using (var context = _context.CreateDbContext())
            {
                context.Set<Estoque>().Remove(estoque);
                await context.SaveChangesAsync();
            }
        }
    }
}
