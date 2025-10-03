using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PruebaTecnica.Models;
using PruebaTecnica.Services;

namespace PruebaTecnica.Pages.Productos
{
    public class EditModel : PageModel
    {
        private readonly IProductoService _productoService;

        public EditModel(IProductoService productoService)
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _productoService.EditarProductoAsync(Producto);
                TempData["SuccessMessage"] = "Producto actualizado exitosamente";
                return RedirectToPage("Index");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al actualizar el producto");
                return Page();
            }
        }
    }
}