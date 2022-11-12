using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Novo.Models.Domain;

namespace Novo.Infra;

public class GeComuContext : IdentityDbContext<Usuario>
{
    public GeComuContext(DbContextOptions<GeComuContext> options)
        : base(options)
    {
    }

    public DbSet<Item> Acentos { get; set; }
    public DbSet<Ambiente> Ambientes { get; set; }
    public DbSet<Reserva> Reservas { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.ToTable("Usuario");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

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

            //entity.HasOne(d => d.Reserva)
            //    .WithOne(x => x.Ambiente);
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
    }
}
