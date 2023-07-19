namespace MS.GestaoEstoque.Interface
{
    public interface IProcessaEvento
    {
        Task Processa(string mensagem);

    }
}
