using ApiEmpresas.DTOs.Empresa;
using ApiEmpresas.DTOs.Endereco;
using ApiEmpresas.DTOs.Funcionario;
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
                    opt => opt.MapFrom(src => src.RegimeTributario.ToString()))
                .ForMember(dest => dest.Setores,
                    opt => opt.MapFrom(src =>
                        src.Setores.Select(es => new SetorDTO
                        {
                            Id = es.Setor!.Id,
                            Nome = es.Setor!.Nome,
                            Funcionarios = es.Setor.Funcionarios
                                .Where(fs => fs.EmpresaId == src.Id && fs.Funcionario != null)
                                .Select(fs => new FuncionarioResponseResumidoDTO
                                {
                                    Id = fs.Funcionario!.Id,
                                    Nome = fs.Funcionario!.Nome
                                })
                                .ToList()
                        }).ToList()
                    )
                );


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
            // FUNCIONÁRIO
            // -------------------------------------------------

            // RESPOSTA
            CreateMap<Funcionario, FuncionarioResponseDTO>()
                .ForMember(d => d.Setores, o => o.MapFrom(s => s.FuncionarioSetorVinculo))
                .ForMember(d => d.Habilidades, o => o.MapFrom(s => s.Habilidades.Select(h => h.Habilidade)));

            // CREATE
            CreateMap<CreateFuncionarioDTO, Funcionario>();

            // UPDATE
            CreateMap<UpdateFuncionarioDTO, Funcionario>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Endereco, opt => opt.Ignore());

            CreateMap<Habilidade, HabilidadeResponseDTO>();

            CreateMap<CreateHabilidadeDTO, Habilidade>();

            CreateMap<UpdateHabilidadeDTO, Habilidade>()
                .ForAllMembers(opt =>
                    opt.Condition((src, dest, srcMember) => srcMember != null));

            // FUNCIONARIO SETOR
            CreateMap<FuncionarioSetor, FuncionarioSetorResponseDTO>()
                .ForMember(d => d.EmpresaId, o => o.MapFrom(s => s.EmpresaId))
                .ForMember(d => d.NomeEmpresa, o => o.MapFrom(s => s.Empresa!.Nome))
                .ForMember(d => d.SetorId, o => o.MapFrom(s => s.SetorId))
                .ForMember(d => d.NomeSetor, o => o.MapFrom(s => s.Setor!.Nome))
                .ForMember(d => d.Salario, o => o.MapFrom(s => s.Salario));
        }
    }
}
