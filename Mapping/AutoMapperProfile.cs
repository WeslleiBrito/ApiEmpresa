using ApiEmpresas.DTOs.Empresa;
using ApiEmpresas.DTOs.Endereco;
using ApiEmpresas.DTOs.Funcionario;
using ApiEmpresas.DTOs.Profissao;
using ApiEmpresas.DTOs.Setor;
using ApiEmpresas.DTOs.Habilidade;
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


            CreateMap<PatchEmpresaDTO, Empresa>()
                .ForAllMembers(opt => opt.Condition(
                    (src, dest, srcMember) => srcMember != null
            ));

            CreateMap<PatchEnderecoDTO, Endereco>()
                .ForAllMembers(opt => opt.Condition(
                    (src, dest, srcMember) => srcMember != null
            ));

            // -------------------------------------------------
            // SETOR
            // -------------------------------------------------
            CreateMap<CreateSetorDTO, Setor>();
            CreateMap<UpdateSetorDTO, Setor>();
            CreateMap<Setor, SetorResponseDTO>();
            CreateMap<Setor, SetorDTO>();

            // EmpresaSetor → SetorDTO (usado por EmpresaResponseDTO)
            CreateMap<EmpresaSetor, SetorDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Setor!.Id))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Setor!.Nome));


            // -------------------------------------------------
            // PROFISSÃO
            // -------------------------------------------------
            CreateMap<CreateProfissaoDTO, Profissao>();
            CreateMap<Profissao, ProfissaoResponseDTO>();

            // -------------------------------------------------
            // FUNCIONÁRIO
            // -------------------------------------------------

            // RESPOSTA
            CreateMap<Funcionario, FuncionarioResponseDTO>()
                .ForMember(dest => dest.Empresa,
                    opt => opt.MapFrom(src => src.Empresa != null
                        ? new Dictionary<string, object>
                        {
                            { "Id", src.Empresa.Id },
                            { "Nome", src.Empresa.Nome }
                        } : null
                    )
                )

                .ForMember(dest => dest.Profissao,
                    opt => opt.MapFrom(src => src.Profissao != null
                        ? new Dictionary<string, object>
                        {
                            { "Id", src.Profissao.Id },
                            { "Nome", src.Profissao.Nome }
                        } : null
                    )
                )

                .ForMember(dest => dest.Setores,
                    opt => opt.MapFrom(src => src.Setores != null
                        ? src.Setores
                            .Where(fs => fs.Setor != null) // Filtra se o setor for nulo
                            .Select(fs => new Dictionary<string, object>
                            {
                                { "Id", fs.Setor!.Id },
                                { "Nome", fs.Setor!.Nome }
                            }).ToList()
                        : new List<Dictionary<string, object>>()
                    )
                );

            // CREATE
            CreateMap<CreateFuncionarioDTO, Funcionario>();

            // UPDATE
            CreateMap<UpdateFuncionarioDTO, Funcionario>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForPath(dest => dest.Empresa.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Endereco, opt => opt.Ignore());

            CreateMap<Habilidade, HabilidadeResponseDTO>();

            CreateMap<CreateHabilidadeDTO, Habilidade>();

            CreateMap<UpdateHabilidadeDTO, Habilidade>()
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null));

        }
    }
}
