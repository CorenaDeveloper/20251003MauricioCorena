using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PruebaTecnica.Services;
using System.ComponentModel.DataAnnotations;

namespace PruebaTecnica.Pages.Productos
{
    public class EditPriceModel : PageModel
    {
        private readonly IProductoService _productoService;

        public EditPriceModel(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [BindProperty]
        public int ProductoId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "El precio base es requerido")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero")]
        public decimal PrecioBase { get; set; }

        [BindProperty]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio con descuento debe ser mayor que cero")]
        public decimal? PrecioConDescuento { get; set; }

        public string ProductoNombre { get; set; } = string.Empty;
        public decimal PrecioActual { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var producto = await _productoService.ObtenerProductoPorIdAsync(id.Value);
            if (producto == null)
            {
                return NotFound();
            }

            ProductoId = producto.Id;
            ProductoNombre = producto.Nombre;
            PrecioBase = producto.PrecioBase;
            PrecioConDescuento = producto.PrecioConDescuento;
            PrecioActual = producto.PrecioActual;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Recargar el nombre del producto para mostrar en caso de error
                var producto = await _productoService.ObtenerProductoPorIdAsync(ProductoId);
                if (producto != null)
                {
                    ProductoNombre = producto.Nombre;
                    PrecioActual = producto.PrecioActual;
                }
                return Page();
            }

            try
            {
                var resultado = await _productoService.ActualizarPrecioProductoAsync(ProductoId, PrecioBase, PrecioConDescuento);
                if (resultado)
                {
                    TempData["SuccessMessage"] = "Precio actualizado exitosamente";
                    return RedirectToPage("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No se pudo actualizar el precio");
                    return Page();
                }
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var producto = await _productoService.ObtenerProductoPorIdAsync(ProductoId);
                if (producto != null)
                {
                    ProductoNombre = producto.Nombre;
                    PrecioActual = producto.PrecioActual;
                }
                return Page();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al actualizar el precio");
                return Page();
            }
        }
    }
}