using PruebaTecnica.Models;

namespace PruebaTecnica.Repositories
{
    public interface IProductoRepository
    {
        // CRUD básico (HU-001)
        Task<IEnumerable<Producto>> ObtenerTodosAsync();
        Task<Producto?> ObtenerPorIdAsync(int id);
        Task<Producto> AgregarAsync(Producto producto);
        Task<Producto> ActualizarAsync(Producto producto);
        Task<bool> EliminarAsync(int id);

        // Usando procedimiento almacenado (Requisito del documento)
        Task<IEnumerable<Producto>> ObtenerProductosConSPAsync();

        // HU-002: Actualizar precio
        Task<bool> ActualizarPrecioAsync(int id, decimal precioBase, decimal? precioConDescuento);
    }
}