using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PruebaTecnica.Models;
using PruebaTecnica.Services;

namespace PruebaTecnica.Pages.Productos
{
    public class EditModel : PageModel
    {
        private readonly IProductoService _productoService;
        private readonly IWebHostEnvironment _environment;

        public EditModel(IProductoService productoService, IWebHostEnvironment environment)
        {
            _productoService = productoService;
            _environment = environment;
        }

        [BindProperty]
        public Producto Producto { get; set; } = new();

        [BindProperty]
        public IFormFile? ImagenArchivo { get; set; }

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
                var imagenAnterior = Producto.Imagen;

                // Procesar nueva imagen si se subió
                if (ImagenArchivo != null && ImagenArchivo.Length > 0)
                {
                    // Validar tamaño (máx 5MB)
                    if (ImagenArchivo.Length > 5 * 1024 * 1024)
                    {
                        ModelState.AddModelError("ImagenArchivo", "La imagen no puede exceder 5MB");
                        return Page();
                    }

                    // Validar extensión
                    var extension = Path.GetExtension(ImagenArchivo.FileName).ToLowerInvariant();
                    var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                    if (!extensionesPermitidas.Contains(extension))
                    {
                        ModelState.AddModelError("ImagenArchivo", "Solo se permiten imágenes JPG, PNG o GIF");
                        return Page();
                    }

                    // Crear nombre único para la imagen
                    var nombreArchivo = $"{Guid.NewGuid()}{extension}";

                    // Ruta donde se guardará la imagen
                    var rutaCarpeta = Path.Combine(_environment.WebRootPath, "images", "productos");

                    // Crear la carpeta si no existe
                    if (!Directory.Exists(rutaCarpeta))
                    {
                        Directory.CreateDirectory(rutaCarpeta);
                    }

                    var rutaCompleta = Path.Combine(rutaCarpeta, nombreArchivo);

                    // Guardar el archivo
                    using (var stream = new FileStream(rutaCompleta, FileMode.Create))
                    {
                        await ImagenArchivo.CopyToAsync(stream);
                    }

                    // Eliminar imagen anterior si existe
                    if (!string.IsNullOrEmpty(imagenAnterior))
                    {
                        var rutaImagenAnterior = Path.Combine(_environment.WebRootPath, imagenAnterior.TrimStart('/'));
                        if (System.IO.File.Exists(rutaImagenAnterior))
                        {
                            System.IO.File.Delete(rutaImagenAnterior);
                        }
                    }

                    // Actualizar la ruta en el modelo
                    Producto.Imagen = $"/images/productos/{nombreArchivo}";
                }

                await _productoService.EditarProductoAsync(Producto);
                TempData["SuccessMessage"] = "Producto actualizado exitosamente";
                return RedirectToPage("../Index");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Ocurrió un error al actualizar el producto: {ex.Message}");
                return Page();
            }
        }
    }
}