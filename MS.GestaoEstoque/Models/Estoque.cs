using MS.GestaoEstoque.Enums;

namespace MS.GestaoEstoque.Models
{
    public class Estoque
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public CategoriaEnum Categoria { get; set; }
        public int Quantidade { get; set; }
    }
}
