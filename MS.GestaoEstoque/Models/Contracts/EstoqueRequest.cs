using MS.GestaoEstoque.Enums;

namespace MS.GestaoEstoque.Models.Contracts
{
    public class EstoqueRequest
    {
        public string Nome { get; set; }
        public CategoriaEnum Categoria { get; set; }
        public int Quantidade { get; set; }
    }
}
