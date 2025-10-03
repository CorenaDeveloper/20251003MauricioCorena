using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PruebaTecnica.Models;
using PruebaTecnica.Services;

namespace PruebaTecnica.Pages.Productos
{
    public class DeleteModel : PageModel
    {
        private readonly IProductoService _productoService;

        public DeleteModel(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [BindProperty]
        public Producto Producto { get; set; } = new();

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

            Producto = producto;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Producto.Id == 0)
            {
                return NotFound();
            }

            try
            {
                var resultado = await _productoService.EliminarProductoAsync(Producto.Id);
                if (resultado)
                {
                    TempData["SuccessMessage"] = "Producto eliminado exitosamente";
                    return RedirectToPage("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo eliminar el producto";
                    return RedirectToPage("./Index");
                }
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al eliminar el producto";
                return RedirectToPage("Index");
            }
        }
    }
}