using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MS.GestaoEstoque.Enums;
using MS.GestaoEstoque.Interface;
using MS.GestaoEstoque.Models;
using MS.GestaoEstoque.Models.Contracts;
using System.Text;

namespace MS.GestaoEstoque.Service
{
    public class EstoqueService : IEstoqueService
    {
        private readonly IEstoqueRepository _repository;
        private readonly IMapper _mapper;
        private readonly IRabbitMqClient _rabbitMqClient;

        public EstoqueService(IEstoqueRepository repository, IMapper mapper, IRabbitMqClient rabbitMqClient)
        {
            _repository = repository;
            _mapper = mapper;
            _rabbitMqClient = rabbitMqClient;
        }

        public async Task<EstoqueResponse> CriarAsync(EstoqueRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (request.Nome.Length < 4) throw new ArgumentNullException("Nome vazio ou inválido!");

            var estoqueEntity = _mapper.Map<Estoque>(request);
            await _repository.AddAsync(estoqueEntity);

            var estoqueResponse = _mapper.Map<EstoqueResponse>(estoqueEntity);
            return estoqueResponse;
        }

        public async Task<IEnumerable<EstoqueResponse>> ObterTodosAsync()
        {
            var entities = await _repository.ListAsync(x => x.Quantidade != 0);
            var response = _mapper.Map<IEnumerable<EstoqueResponse>>(entities);

            return response;
        }

        public async Task<IEnumerable<EstoqueResponse>> ObterPorNomeAsync(string nome)
        {
            var entities = await _repository.ListAsync(x => x.Nome == nome);
            var response = _mapper.Map<IEnumerable<EstoqueResponse>>(entities);

            return response;
        }

        public async Task<IEnumerable<EstoqueResponse>> ObterPorCategoriaAsync(CategoriaEnum categoria)
        {
            var entities = await _repository.ListAsync(x => x.Categoria == categoria);
            var response = _mapper.Map<IEnumerable<EstoqueResponse>>(entities);

            return response;
        }

        public async Task ValidaItensEmEstoque(PedidoDtoRequest pedidoDto)
        {
            var pedido = _mapper.Map<PedidoDtoResponse>(pedidoDto);
            var naoEncontrados = new List<string>();
            var encontrados = new List<Estoque>();

            foreach (var item in pedido.Itens)
            {
                var entity = await _repository.FindAsync(x => x.Nome == item && x.Quantidade > 0);
                if (entity == null)
                {
                    naoEncontrados.Add(item);
                }
                else
                {
                    entity.Quantidade -= 1;
                    encontrados.Add(entity);
                }
            }
            pedido.Itens = naoEncontrados;
            if (naoEncontrados.Count > 0)
            {
                pedido.Mensagem = $"Itens não encontrados no estoque: {string.Join(", ", naoEncontrados)}";

                _rabbitMqClient.EnviaParaMsPedidos(pedido);
                _rabbitMqClient.EnviaParaMsEmails(pedido);
                throw new ArgumentException($"Itens não encontrados no estoque: {string.Join(", ", naoEncontrados)}");
            }
            pedido.Mensagem = "Pedido efetuado com sucesso!";
            _rabbitMqClient.EnviaParaMsEmails(pedido);
            _rabbitMqClient.EnviaParaMsPedidos(pedido);
            await _repository.EditRangeAsync(encontrados);
        }
    }
}