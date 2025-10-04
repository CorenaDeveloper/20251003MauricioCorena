using PruebaTecnica.Models;
using PruebaTecnica.Repositories;

namespace PruebaTecnica.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;

        public ProductoService(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public async Task<IEnumerable<Producto>> ObtenerTodosProductosAsync()
        {
            // Usa el procedimiento almacenado 
            return await _productoRepository.ObtenerProductosConSPAsync();
        }

        public async Task<Producto?> ObtenerProductoPorIdAsync(int id)
        {
            return await _productoRepository.ObtenerPorIdAsync(id);
        }

        public async Task<Producto> CrearProductoAsync(Producto producto)
        {
            // validacion de precio debe ser mayor que cero
            if (producto.PrecioBase <= 0)
                throw new ArgumentException("El precio base debe ser mayor que cero");

            // validacion  precio con descuento debe ser menor al precio base
            if (producto.PrecioConDescuento.HasValue)
            {
                if (producto.PrecioConDescuento.Value <= 0)
                    throw new ArgumentException("El precio con descuento debe ser mayor que cero");

                if (producto.PrecioConDescuento.Value >= producto.PrecioBase)
                    throw new ArgumentException("El precio con descuento debe ser menor al precio base");
            }

            return await _productoRepository.AgregarAsync(producto);
        }

        public async Task<Producto> EditarProductoAsync(Producto producto)
        {
            // Validaciones similares a CrearProductoAsync
            if (producto.PrecioBase <= 0)
                throw new ArgumentException("El precio base debe ser mayor que cero");

            if (producto.PrecioConDescuento.HasValue)
            {
                if (producto.PrecioConDescuento.Value <= 0)
                    throw new ArgumentException("El precio con descuento debe ser mayor que cero");

                if (producto.PrecioConDescuento.Value >= producto.PrecioBase)
                    throw new ArgumentException("El precio con descuento debe ser menor al precio base");
            }

            return await _productoRepository.ActualizarAsync(producto);
        }

        public async Task<bool> EliminarProductoAsync(int id)
        {
            return await _productoRepository.EliminarAsync(id);
        }

        public async Task<bool> ActualizarPrecioProductoAsync(int id, decimal precioBase, decimal? precioConDescuento)
        {
            // validacion de precio debe ser mayor que cero (HU-002)
            if (precioBase <= 0)
                throw new ArgumentException("El precio base debe ser mayor que cero");

            if (precioConDescuento.HasValue)
            {
                if (precioConDescuento.Value <= 0)
                    throw new ArgumentException("El precio con descuento debe ser mayor que cero");

                if (precioConDescuento.Value >= precioBase)
                    throw new ArgumentException("El precio con descuento debe ser menor al precio base");
            }

            return await _productoRepository.ActualizarPrecioAsync(id, precioBase, precioConDescuento);
        }
    }
}