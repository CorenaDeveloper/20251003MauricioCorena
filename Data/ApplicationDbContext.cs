using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Models;

namespace PruebaTecnica.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Producto> Productos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de la entidad Producto
            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Descripcion).HasMaxLength(1000);
                entity.Property(e => e.PrecioBase).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.PrecioConDescuento).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Imagen).HasMaxLength(500);
                entity.Property(e => e.FechaCreacion).HasDefaultValueSql("GETDATE()");
            });

            // Crear el procedimiento almacenado (HU-001 - Requisito)
            modelBuilder.Entity<Producto>()
                .ToTable(tb => tb.HasTrigger("TR_ActualizarFechaModificacion"));
        }
    }
}