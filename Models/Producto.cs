using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaTecnica.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(200)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es requerido")]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero")]
        [Display(Name = "Precio Base")]
        public decimal PrecioBase { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio con descuento debe ser mayor que cero")]
        [Display(Name = "Precio con Descuento")]
        public decimal? PrecioConDescuento { get; set; }

        [StringLength(500)]
        public string? Imagen { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaModificacion { get; set; }

        [NotMapped]
        public decimal PrecioActual => PrecioConDescuento ?? PrecioBase;

        //Validando que el precio con descuento sea menor que el precio base
        public bool ValidarPrecioDescuento()
        {
            if (PrecioConDescuento.HasValue)
            {
                return PrecioConDescuento.Value < PrecioBase;
            }
            return true;
        }
    }
}