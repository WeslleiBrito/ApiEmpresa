using ApiEmpresas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiEmpresas.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }

        public DbSet<Setor> Setores { get; set; }
        public DbSet<Profissao> Profissoes { get; set; }
        public DbSet<EmpresaSetor> EmpresaSetores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --------------------------------------------
            // HERANÇA TPT
            // --------------------------------------------
            modelBuilder.Entity<Pessoa>().ToTable("Pessoas");
            modelBuilder.Entity<Empresa>().ToTable("Empresas");
            modelBuilder.Entity<Funcionario>().ToTable("Funcionarios");

            // --------------------------------------------
            // ENDERECO como OWNED TYPE
            // --------------------------------------------
            modelBuilder.Entity<Pessoa>().OwnsOne(p => p.Endereco);

            // --------------------------------------------
            // Empresa ↔ Setor (N:N)
            // --------------------------------------------
            modelBuilder.Entity<EmpresaSetor>()
                .HasKey(es => new { es.EmpresaId, es.SetorId });

            modelBuilder.Entity<EmpresaSetor>()
                .HasOne(es => es.Empresa)
                .WithMany(e => e.Setores)
                .HasForeignKey(es => es.EmpresaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmpresaSetor>()
                .HasOne(es => es.Setor)
                .WithMany(s => s.Empresas)
                .HasForeignKey(es => es.SetorId)
                .OnDelete(DeleteBehavior.Cascade);

            // --------------------------------------------
            // Setor → Profissão (1:N)
            // --------------------------------------------
            modelBuilder.Entity<Profissao>()
                .HasOne(p => p.Setor)
                .WithMany(s => s.Profissoes)
                .HasForeignKey(p => p.SetorId)
                .OnDelete(DeleteBehavior.Restrict);

            // --------------------------------------------
            // Profissão → Funcionário (1:N)
            // --------------------------------------------
            modelBuilder.Entity<Funcionario>()
                .HasOne(f => f.Profissao)
                .WithMany(p => p.Funcionarios)
                .HasForeignKey(f => f.ProfissaoId)
                .OnDelete(DeleteBehavior.Restrict);

            // --------------------------------------------
            // Empresa → Funcionário (1:N)
            // --------------------------------------------
            modelBuilder.Entity<Funcionario>()
                .HasOne<Empresa>()
                .WithMany()
                .HasForeignKey(f => f.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
