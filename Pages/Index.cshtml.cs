using Microsoft.AspNetCore.Mvc.RazorPages;
using PruebaTecnica.Models;
using PruebaTecnica.Services;

namespace PruebaTecnica.Pages.Productos
{
    public class IndexModel : PageModel
    {
        private readonly IProductoService _productoService;

        public IndexModel(IProductoService productoService)
        {
            _productoService = productoService;
        }

        public IEnumerable<Producto> Productos { get; set; } = new List<Producto>();

        public async Task OnGetAsync()
        {
            // Usa el procedimiento almacenado (HU-001 - Requisito)
            Productos = await _productoService.ObtenerTodosProductosAsync();
        }
    }
}