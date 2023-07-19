using MS.GestaoEstoque.Enums;
using MS.GestaoEstoque.Models;
using MS.GestaoEstoque.Models.Contracts;

namespace MS.GestaoEstoque.Interface
{
    public interface IEstoqueService
    {
        Task<EstoqueResponse> CriarAsync(EstoqueRequest request);
        Task<IEnumerable<EstoqueResponse>> ObterTodosAsync();
        Task<IEnumerable<EstoqueResponse>> ObterPorNomeAsync(string nome);
        Task<IEnumerable<EstoqueResponse>> ObterPorCategoriaAsync(CategoriaEnum categoria);
        Task ValidaItensEmEstoque(PedidoDtoRequest itensList);
    }
}
