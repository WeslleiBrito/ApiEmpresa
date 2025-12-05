using ApiEmpresas.DTOs.Empresa;
using ApiEmpresas.DTOs.Endereco;
using ApiEmpresas.DTOs.Funcionario;
using ApiEmpresas.DTOs.Profissao;
using ApiEmpresas.DTOs.Setor;
using ApiEmpresas.Models;
using AutoMapper;

namespace ApiEmpresas.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // -------------------------------------------------
            // ENDEREÇO
            // -------------------------------------------------
            CreateMap<Endereco, EnderecoDTO>();
            CreateMap<CreateEnderecoDTO, Endereco>();


            // -------------------------------------------------
            // EMPRESA
            // -------------------------------------------------
            CreateMap<Empresa, EmpresaResponseDTO>()
                .ForMember(dest => dest.RegimeTributario,
                           opt => opt.MapFrom(src => src.RegimeTributario.ToString()));

            CreateMap<CreateEmpresaDTO, Empresa>()
                .ForMember(dest => dest.Setores, opt => opt.Ignore()); // tratado no service

            CreateMap<UpdateEmpresaDTO, Empresa>()
                .ForMember(dest => dest.Setores, opt => opt.Ignore()); // setores é tratado manualmente no service


            // -------------------------------------------------
            // SETOR
            // -------------------------------------------------
            CreateMap<CreateSetorDTO, Setor>();
             CreateMap<UpdateSetorDTO, Setor>();
            CreateMap<Setor, SetorResponseDTO>();

            // EmpresaSetor → SetorDTO (usado por EmpresaResponseDTO)
            CreateMap<EmpresaSetor, SetorDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Setor!.Id))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Setor!.Nome));

            
            // -------------------------------------------------
            // PROFISSÃO
            // -------------------------------------------------
            CreateMap<CreateProfissaoDTO, Profissao>();

            CreateMap<Profissao, ProfissaoResponseDTO>()
                .ForMember(dest => dest.SetorNome,
                           opt => opt.MapFrom(src => src.Setor != null ? src.Setor.Nome : null))
                .ForMember(dest => dest.SetorId,
                           opt => opt.MapFrom(src => src.SetorId));


            // -------------------------------------------------
            // FUNCIONÁRIO
            // -------------------------------------------------

            // RESPOSTA
            CreateMap<Funcionario, FuncionarioResponseDTO>();

            // CREATE
            CreateMap<CreateFuncionarioDTO, Funcionario>();

            // UPDATE
            CreateMap<UpdateFuncionarioDTO, Funcionario>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.EmpresaId, opt => opt.Ignore())
                .ForMember(dest => dest.Endereco, opt => opt.Ignore()); 
            

        }
    }
}
