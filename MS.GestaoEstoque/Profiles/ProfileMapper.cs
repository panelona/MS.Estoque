using AutoMapper;
using MS.GestaoEstoque.Models;
using MS.GestaoEstoque.Models.Contracts;

namespace MS.GestaoEstoque.Profiles
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        { 
            CreateMap<PedidoDtoRequest, PedidoDtoResponse>().ReverseMap();
            CreateMap<EstoqueRequest, Estoque>().ReverseMap();
            CreateMap<EstoqueRequest, EstoqueResponse>().ReverseMap();
            CreateMap<EstoqueResponse, Estoque>().ReverseMap();
        }
    }
}
