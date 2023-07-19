using AutoMapper;
using MS.GestaoEstoque.Interface;
using MS.GestaoEstoque.Models;
using System.Text.Json;

namespace MS.GestaoEstoque.Events
{
    public class ProcessaEvento : IProcessaEvento
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;
        private IConfiguration _configuration;

        public ProcessaEvento(IMapper mapper, IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _mapper = mapper;
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        public async Task Processa(string mensagem)
        {
            using var scope = _scopeFactory.CreateScope();

            var service = scope.ServiceProvider.GetRequiredService<IEstoqueService>();


            var mensagemResponse = JsonSerializer.Deserialize<PedidoDtoRequest>(mensagem);

            if (mensagemResponse != null)
                await service.ValidaItensEmEstoque(mensagemResponse);
        }
    }
}
