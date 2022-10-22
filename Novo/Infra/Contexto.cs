using Microsoft.EntityFrameworkCore;
using Novo.Models.Domain;

namespace Novo.Infra
{
    public class Contexto : DbContext
    {
        public Contexto()
        {

        }

        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
        }

        public DbSet<Item> Acentos { get; set; }
        public DbSet<Ambiente> Ambientes { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>(entity =>
            {
                entity.HasKey(i => i.IdItem);
                entity.ToTable("Item");
                entity.Property(i => i.IdItem).ValueGeneratedOnAdd();

                entity.HasOne(i => i.Ambiente)
                .WithMany(a => a.Items)
                .HasForeignKey(i => i.IdAmbiente);
            });


            modelBuilder.Entity<Ambiente>(entity =>
            {
                entity.HasKey(e => e.IdAmbiente);

                entity.ToTable("Ambiente");

                entity.Property(e => e.IdAmbiente).ValueGeneratedOnAdd();

                entity.HasMany(d => d.Items)
                    .WithOne(p => p.Ambiente)
                    .HasForeignKey(p => p.IdAmbiente);
            });

            modelBuilder.Entity<Reserva>(entity =>
            {
                entity.HasKey(e => e.IdReserva);

                entity.ToTable("Reserva");

                entity.Property(e => e.IdReserva).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Ambiente)
                    .WithMany()
                    .HasForeignKey(d => d.IdAmbiente)
                    .HasConstraintName("FK_Reserva_Ambiente");

                entity.HasOne(d => d.Usuario)
                    .WithMany()
                    .HasForeignKey(d => d.IdUsuario)
                    .HasConstraintName("FK_Reserva_Usuario");

                entity.HasOne(d => d.Item)
                    .WithMany()
                    .HasForeignKey(d => d.IdItem)
                    .HasConstraintName("FK_Reserva_Item"); //TODO - Criar FK no banco de dados.
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.IdUsuario);

                entity.ToTable("Usuario");

                entity.Property(e => e.IdUsuario).ValueGeneratedOnAdd();
            });
        }
    }
}
