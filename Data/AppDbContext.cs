using ApiEmpresas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiEmpresas.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }


        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Setor> Setores { get; set; }
        public DbSet<EmpresaSetor> EmpresaSetores { get; set; }
        public DbSet<FuncionarioHabilidade> FuncionarioHabilidades { get; set; }
        public DbSet<Habilidade> Habilidades { get; set; }


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
            // Funcionário → Empresa (1:N)
            // --------------------------------------------
            modelBuilder.Entity<Funcionario>()
                .HasOne(f => f.Empresa)
                .WithMany(e => e.Funcionarios)
                .HasForeignKey(f => f.EmpresaId)
                .OnDelete(DeleteBehavior.Restrict);


            // Funcionário ↔ Setor (N:N)
            modelBuilder.Entity<FuncionarioSetor>()
                .HasKey(fs => new { fs.FuncionarioId, fs.SetorId });

            modelBuilder.Entity<FuncionarioSetor>()
                .HasOne(fs => fs.Funcionario)
                .WithMany(f => f.Setores)
                .HasForeignKey(fs => fs.FuncionarioId);

            modelBuilder.Entity<FuncionarioSetor>()
                .HasOne(fs => fs.Setor)
                .WithMany(s => s.Funcionarios)
                .HasForeignKey(fs => fs.SetorId);

            // --------------------------------------------
            // Funcionário ↔ Habilidade (N:N)
            modelBuilder.Entity<FuncionarioHabilidade>()
                .HasKey(fh => new { fh.FuncionarioId, fh.HabilidadeId });

            modelBuilder.Entity<FuncionarioHabilidade>()
                .HasOne(fh => fh.Funcionario)
                .WithMany(f => f.Habilidades)
                .HasForeignKey(fh => fh.FuncionarioId);

            modelBuilder.Entity<FuncionarioHabilidade>()
                .HasOne(fh => fh.Habilidade)
                .WithMany(h => h.Funcionarios)
                .HasForeignKey(fh => fh.HabilidadeId);

        }
    }
}
