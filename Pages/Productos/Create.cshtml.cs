using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PruebaTecnica.Models;
using PruebaTecnica.Services;

namespace PruebaTecnica.Pages.Productos
{
    public class CreateModel : PageModel
    {
        private readonly IProductoService _productoService;

        public CreateModel(IProductoService productoService)
        {
            _productoService = productoService;
        }

        [BindProperty]
        public Producto Producto { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _productoService.CrearProductoAsync(Producto);
                TempData["SuccessMessage"] = "Producto creado exitosamente";
                return RedirectToPage("./Index");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al crear el producto");
                return Page();
            }
        }
    }
}