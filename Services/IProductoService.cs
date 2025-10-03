using PruebaTecnica.Models;

namespace PruebaTecnica.Services
{
    public interface IProductoService
    {
        Task<IEnumerable<Producto>> ObtenerTodosProductosAsync();
        Task<Producto?> ObtenerProductoPorIdAsync(int id);
        Task<Producto> CrearProductoAsync(Producto producto);
        Task<Producto> EditarProductoAsync(Producto producto);
        Task<bool> EliminarProductoAsync(int id);
        Task<bool> ActualizarPrecioProductoAsync(int id, decimal precioBase, decimal? precioConDescuento);
    }
}