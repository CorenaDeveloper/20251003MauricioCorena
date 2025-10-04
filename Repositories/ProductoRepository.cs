using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Data;
using PruebaTecnica.Models;

namespace PruebaTecnica.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener todos los productos 
        public async Task<IEnumerable<Producto>> ObtenerTodosAsync()
        {
            return await _context.Productos
                .AsNoTracking()
                .OrderByDescending(p => p.FechaCreacion)
                .ToListAsync();
        }

        // Obtener productos usando el procedimiento almacenado 
        public async Task<IEnumerable<Producto>> ObtenerProductosConSPAsync()
        {
            return await _context.Productos
                .FromSqlRaw("EXEC sp_ObtenerProductos")
                .AsNoTracking()
                .ToListAsync();
        }

        // Obtener producto por ID
        public async Task<Producto?> ObtenerPorIdAsync(int id)
        {
            return await _context.Productos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Agregar nuevo producto 
        public async Task<Producto> AgregarAsync(Producto producto)
        {
            producto.FechaCreacion = DateTime.Now;
            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        // Actualizar producto
        public async Task<Producto> ActualizarAsync(Producto producto)
        {
            producto.FechaModificacion = DateTime.Now;
            _context.Productos.Update(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        // Eliminar producto
        public async Task<bool> EliminarAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return false;

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }

        // Actualizar precio
        public async Task<bool> ActualizarPrecioAsync(int id, decimal precioBase, decimal? precioConDescuento)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return false;

            producto.PrecioBase = precioBase;
            producto.PrecioConDescuento = precioConDescuento;
            producto.FechaModificacion = DateTime.Now;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}