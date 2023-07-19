using MS.GestaoEstoque.Models;
using System.Linq.Expressions;

namespace MS.GestaoEstoque.Interface
{
    public interface IEstoqueRepository
    {
        Task<Estoque> FindAsync(Guid id);
        Task<Estoque> FindAsync(Expression<Func<Estoque, bool>> estoque);
        Task<Estoque> FindAsNoTrackingAsync(Expression<Func<Estoque, bool>> estoque);
        Task<IEnumerable<Estoque>> ListAsync();
        Task<IEnumerable<Estoque>> ListAsync(Expression<Func<Estoque, bool>> estoque);
        Task AddAsync(Estoque estoque);
        Task EditRangeAsync(IEnumerable<Estoque> estoques);
        Task RemoveAsync(Estoque estoque);
        Task EditAsync(Estoque estoque);
    }
}
